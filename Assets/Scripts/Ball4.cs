using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball4 : MonoBehaviour
{

    public GameObject TailPrefab;
    public float speed = 1;
    public bool canFly = false;
    public float gravity = 2f;
    public float max_fall_speed = -1f;
    private Vector3 velocity;
    private Renderer renderer;
    private CharacterController controller;
    private Vector3 spawnPos;
    private bool justJumped;
    private bool movable = true;
    private GameObject lights;
    private Animator anim;

    private bool isGrounded;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        controller = GetComponent<CharacterController>();
        StartCoroutine("SpawnTailObject");
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movable)
        {
            if (!canFly){
                velocity.x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
                velocity.y = Mathf.Max(velocity.y - gravity * Time.deltaTime, max_fall_speed);
            }
            else
            {
                velocity.x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
                velocity.y = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
            }
        }
        else
        {
            velocity = Vector3.zero;
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("lights_powerup"))
            {
                movable = true;
                StartCoroutine("Rainbow");
                speed *= 2.5f;
                canFly = true;
                RenderSettings.ambientIntensity = 1;
                RenderSettings.ambientLight = Color.white;
                Destroy(lights);
                GameObject.Find("Camera").GetComponent<Camera1>()._offset = Vector3.zero;
                GetComponent<AudioSource>().Play();
            }
        }
        controller.Move(velocity);
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
            yield return new WaitForSeconds(.12f);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "rainbow")
        {
            Destroy(hit.gameObject);
            lights = GameObject.Find("Lights");
            anim = lights.GetComponent<Animator>();
            anim.SetBool("started", true);
            movable = false;
        }
        else if (hit.gameObject.tag == "rainbow2")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
        }
    }
}
