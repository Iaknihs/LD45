using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip powerupSound;

    public GameObject TailPrefab;
    public enum BallState
    {
        auto,
        slowdown,
        freeroam,
        cutscene,
        exit
    }
    BallState state = BallState.auto;

    public float initSpeed = 1.0f;
    private float speed;

    public GameObject cam;
    private Renderer renderer;
    private CharacterController controller;
    private Vector3 spawnPos;
    public Vector3 velocity;
    private bool isColliding;
    public Material rainbowMat;

    private Controls controls;

    public int trigger = 0;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        controller = GetComponent<CharacterController>();
        speed = initSpeed;
        spawnPos = transform.position;
        rainbowMat.SetFloat("_isActive", 0.0f);
        PongSpawn();
    }


    void PongSpawn()
    {
        transform.position = spawnPos;
        velocity = new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f, 1.0f), 0.0f);
        velocity.Normalize();
    }


    private void Update()
    {
        controls.right = Mathf.Max((int)Input.GetAxisRaw("Horizontal"), 0);
        controls.left = Mathf.Abs(Mathf.Min((int)Input.GetAxisRaw("Horizontal"), 0));
        controls.down = Mathf.Max((int)Input.GetAxisRaw("Vertical"), 0);
        controls.up = Mathf.Abs(Mathf.Min((int)Input.GetAxisRaw("Vertical"), 0));
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        isColliding = false;

        if (state == BallState.auto)
        {
            if (Manager.instance.state == Manager.GameState.freeroam)
            {
                state = BallState.slowdown;
            }
        }
        else if(state == BallState.slowdown)
        {
            if (speed != 0.0f)
            {
                speed -= 0.01f;
                if (Mathf.Abs(speed) < 0.01f)
                    speed = 0.0f;
            }
            else
            {
                StartCoroutine("SpawnTailObject");
                cam.GetComponent<Camera>().doFollow = true;
                state = BallState.freeroam;
                velocity = Vector3.zero;
                speed = initSpeed;
            }
        }
        else if(state == BallState.freeroam)
        {
            velocity = new Vector3(controls.right - controls.left, controls.down - controls.up, 0); 
            velocity.Normalize();
        }
        else if(state == BallState.cutscene)
        {
            velocity = Vector3.zero;
            if (Manager.instance.state == Manager.GameState.freeroam)
            {
                state = BallState.freeroam;
                if (trigger == 1)
                {
                    spawnPos = transform.position;
                    StartCoroutine("Rainbow");
                    BGMManager.instance.PlayBGM1();
                }
                else if (trigger == 2)
                {
                    spawnPos = transform.position;
                    rainbowMat.SetFloat("_isActive", 1.0f);
                    BGMManager.instance.PlayBGM2();
                }
                else if (trigger == 3)
                {
                    state = BallState.exit;
                }
            }
        }
        else if(state == BallState.exit)
        {
            velocity = GameObject.Find("Portal").transform.position - transform.position;
            velocity.Normalize();
        }

        UpdateMovement();
    }


    private void UpdateMovement()
    {
        //if (state != BallState.freeroam)
        //    transform.Translate(velocity * speed);
        //else
        controller.Move(velocity * speed * Time.fixedDeltaTime);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isColliding) return;
        isColliding = true;

        if (!GetComponent<AudioSource>().isPlaying)
        {
            switch (hit.gameObject.tag)
            {
                case "kill": GetComponent<AudioSource>().PlayOneShot(deathSound); break;
                case "rainbow":
                case "rainbow2": GetComponent<AudioSource>().PlayOneShot(powerupSound); break;
                default: GetComponent<AudioSource>().PlayOneShot(jumpSound); break;
            }
        }

        if (state == BallState.auto || state == BallState.slowdown)
        {
            if (hit.gameObject.tag == "wall")
            {
                string dir = hit.gameObject.GetComponent<Wall>().reflectDirection;

                velocity = new Vector3(
                    Mathf.Abs(velocity.x) * (dir == "right" ? -1 : dir == "left" ? 1 : Mathf.Sign(velocity.x)),
                    Mathf.Abs(velocity.y) * (dir == "down" ? -1 : dir == "up" ? 1 : Mathf.Sign(velocity.y)),
                    velocity.z);
                if (Mathf.Abs(velocity.x) < 0.1)
                    velocity.x = velocity.y;
                velocity.Normalize();
            }
            else if (hit.gameObject.tag == "paddle")
            {
                velocity = transform.position - hit.gameObject.transform.position;
                if (Mathf.Abs(velocity.y) < 0.1)
                    velocity.y = velocity.x;
                velocity.Normalize();
            }
        }
        if (state != BallState.cutscene && state != BallState.exit)
        {
            if (hit.gameObject.tag == "kill")
            {
                PongSpawn();
            }
            else if (hit.gameObject.tag == "rainbow")
            {
                Manager.instance.state = Manager.GameState.cutsceneA;
                state = BallState.cutscene;
                hit.collider.enabled = false;
            }
            else if (hit.gameObject.tag == "rainbow2")
            {
                Manager.instance.state = Manager.GameState.cutsceneB;
                state = BallState.cutscene;
                hit.collider.enabled = false;
            }
            else if(hit.gameObject.tag == "freedom")
            {
                Manager.instance.state = Manager.GameState.cutsceneC;
                state = BallState.cutscene;
                Destroy(hit.gameObject);
            }
        }
        if (state == BallState.exit)
        {
            if (hit.gameObject.tag == "freedom"){
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene02");
            }
        }
    }


    IEnumerator Rainbow()
    {
        for (; ; )
        {
            float H, S, V;
            Color.RGBToHSV(renderer.material.color, out H, out S, out V);
            H += 0.01f;
            S = 0.9f;
            V = 0.9f;
            renderer.material.color = Color.HSVToRGB(H, S, V);

            yield return new WaitForSeconds(.01f);
        }
    }


    IEnumerator SpawnTailObject()
    {
        for (; ; )
        {
            if (velocity != Vector3.zero)
            {
                GameObject thingy = Instantiate(TailPrefab, transform.position, Quaternion.identity);
                thingy.GetComponent<Renderer>().material.color = renderer.material.color;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    private struct Controls
    {
        public int up, down, left, right;
    }
}
