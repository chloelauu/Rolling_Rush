using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public float range = 5f;   // how far it slides each way
    public float speed = 2f;   // how fast

    Rigidbody rb;
    Vector3 start;
    float offset;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        start = transform.position;
        offset = Random.value * 10f;   // so sliders don't all move in sync
    }

    void FixedUpdate()
    {
        float x = Mathf.Sin((Time.time + offset) * speed) * range;
        rb.MovePosition(start + new Vector3(x, 0, 0));
    }
}