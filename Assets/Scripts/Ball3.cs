using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball3 : MonoBehaviour
{

    public GameObject TailPrefab;
    public float speed = 1;
    public float jumpSpeed = 1;
    public float gravity = 2f;
    public float max_fall_speed = -3f;
    private Vector3 velocity;
    private Renderer renderer;
    private CharacterController controller;
    private Vector3 spawnPos;
    private bool justJumped;

    private bool isGrounded;
    private Controls controls;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        controller = GetComponent<CharacterController>();
        StartCoroutine("SpawnTailObject");
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity.x = (controls.right - controls.left) * speed * Time.fixedDeltaTime;
        if (controller.isGrounded)
        {
            
            velocity.y = 0.0f;
            if (controls.up - controls.down > 0)
            {
                if (!justJumped)
                {
                    GetComponent<AudioSource>().Play();
                    velocity.y = jumpSpeed;
                    justJumped = true;
                }
            }
            else
                justJumped = false;
        }
        velocity.y = Mathf.Max(velocity.y - gravity * Time.fixedDeltaTime, max_fall_speed);
        controller.Move(velocity);
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
            yield return new WaitForSeconds(.12f);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "dark")
        {
            StartCoroutine("DarkenCamera");
            // UnityEngine.Camera.main.GetComponent<UnityEngine.Camera>().backgroundColor = Color.black;
            Destroy(hit.gameObject);
        }
    }

    IEnumerator DarkenCamera()
    {
        for (; ; )
        {
            UnityEngine.Camera cam = UnityEngine.Camera.main.GetComponent<UnityEngine.Camera>();
            cam.backgroundColor = new Color(
                Mathf.Max(0, cam.backgroundColor.r - (0.01f)),
                Mathf.Max(0, cam.backgroundColor.g - (0.01f)),
                Mathf.Max(0, cam.backgroundColor.b - (0.01f))
            );
            transform.Find("Light").GetComponent<Light>().intensity -= 0.2f;
            if (Mathf.Max(cam.backgroundColor.r, cam.backgroundColor.g, cam.backgroundColor.b) == 0.0)
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene04");
            yield return new WaitForSeconds(.1f);
        }
    }

    private void Update()
    {
        controls.right = Mathf.Max((int)Input.GetAxisRaw("Horizontal"), 0);
        controls.left = Mathf.Abs(Mathf.Min((int)Input.GetAxisRaw("Horizontal"), 0));
        controls.up = Mathf.Max((int)Input.GetAxisRaw("Vertical"), 0);
        controls.down = Mathf.Abs(Mathf.Min((int)Input.GetAxisRaw("Vertical"), 0));

        if (Input.GetKeyDown(KeyCode.Space))
            controls.up = 1;
    }

    private struct Controls
    {
        public int up, down, left, right;
    }
}
