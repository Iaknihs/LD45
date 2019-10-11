using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public float speed = 10f;
    public Transform follow;
    public bool doFollow;

    // Start is called before the first frame update
    void Start()
    {
        doFollow = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (doFollow)
        {
            Vector2 offset = Vector2.MoveTowards(transform.position, follow.position, speed * Time.deltaTime);
            transform.position = new Vector3(offset.x, offset.y, transform.position.z);
        }
    }
}
