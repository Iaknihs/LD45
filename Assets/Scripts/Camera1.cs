using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera1 : MonoBehaviour
{

    public float speed = 10f;
    public Transform follow;
    public bool doFollow;
    public Vector3 _offset = Vector3.zero;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (doFollow)
        {
            Vector2 offset = Vector2.MoveTowards(transform.position, follow.position + _offset, speed * Time.deltaTime);
            transform.position = new Vector3(offset.x, offset.y, transform.position.z);
        }
    }
}
