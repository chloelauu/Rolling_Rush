using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sweeper : MonoBehaviour
{
    public float speed = 90f;   // degrees per second

    Rigidbody rb;
    float angle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        angle = Random.Range(0f, 180f);          // random starting angle
        if (Random.value < 0.5f) speed = -speed;  // random direction
    }

    void FixedUpdate()
    {
        angle += speed * Time.fixedDeltaTime;
        rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
    }
}