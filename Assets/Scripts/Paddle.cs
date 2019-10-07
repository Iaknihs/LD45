using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    public Transform follow;
    public string axis;
    public float speed = 10.1f;

    private Rigidbody rb;
    private bool colliding;
    private int lastDir;
    private int collDir;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {

        float fPos = (axis == "x") ? follow.position.x : (axis == "y") ? follow.position.y : follow.position.z;
        float pPos = (axis == "x") ? transform.position.x : (axis == "y") ? transform.position.y : transform.position.z;

        int dir = fPos == pPos ? 0 : fPos > pPos ? 1 : -1;

        if ((!colliding || collDir != dir) && dir != 0)
        {
            lastDir = dir;

            Vector3 velocity = new Vector3
            {
                x = (axis == "x") ? 1 : 0,
                y = (axis == "y") ? 1 : 0,
                z = (axis == "z") ? 1 : 0
            };

            // rb.velocity = velocity;
            transform.Translate(velocity * (speed * dir * Time.deltaTime), Space.World);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            colliding = true;
            collDir = lastDir;
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            colliding = false;
        }
    }

}
