using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{

    public GameObject[] waypoints;
    public int current = 0;
    float rotSpeed;
    public float speed;
    float wPradius = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(waypoints[current].transform.position, transform.position) < wPradius)
        {
            current++;
            if (current >= waypoints.Length)
                current = 0;
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, Time.deltaTime * speed);
    }
}
