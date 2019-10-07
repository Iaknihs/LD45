using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBall : MonoBehaviour
{

    public GameObject TailPrefab;
    public float speed = 1;
    private Vector3 velocity;
    private Renderer renderer;
    private CharacterController controller;
    private Vector3 spawnPos;

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

        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), -1.0f, 0);
        velocity.Normalize();
        controller.Move(velocity * speed * Time.deltaTime);
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "kill")
        {
            transform.position = spawnPos;
            GetComponent<AudioSource>().Play();
        }
        if (hit.gameObject.tag == "freedom")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene03");
        }
    }
}
