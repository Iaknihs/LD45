using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    Renderer renderer;
    public float destroyTime = 1f;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        renderer = GetComponent<Renderer>();
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        Color old = renderer.material.color;
        renderer.material.color = new Color(old.r, old.g, old.b, Mathf.Max(old.a - Time.deltaTime/destroyTime, 0.0f));
    }

    private void OnDestroy()
    {
        Debug.Log(Time.time - startTime);
    }
}
