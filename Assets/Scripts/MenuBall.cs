using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBall : MonoBehaviour
{
    public RectTransform slope;                            // drag TrackLine here
    public float gravity = 1500f;
    public float minRollSpeed = 150f;
    public float dropHeight = 350f;                        // how high above the line it starts
    public float resetY = -500f;                           // restart once it falls below this

    [Header("Random")]
    public Vector2 bounceRange = new Vector2(0.2f, 0.5f);  // bounciness: 0 = none, 1 = super bouncy
    public Vector2 waitRange = new Vector2(0.5f, 2.5f);    // seconds to wait before next drop
    public Vector2 dropRange = new Vector2(0.05f, 0.5f);   // where it lands: 0 = start of line, 1 = end

    RectTransform rt;
    float radius, spin, speed, length, bounce, waitTimer;
    Vector2 vel, start, dir, normal;
    bool rolling;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        radius = rt.rect.width / 2f;

        // work out the two ends of the slope line
        float angle = slope.localEulerAngles.z * Mathf.Deg2Rad;
        Vector2 lineDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        Vector2 centre = slope.localPosition;
        float half = slope.rect.width / 2f;
        Vector2 a = centre - lineDir * half;
        Vector2 b = centre + lineDir * half;
        if (a.x > b.x) { Vector2 t = a; a = b; b = t; }

        start = a;
        length = Vector2.Distance(a, b);
        dir = (b - a).normalized;               // along the line, left to right
        normal = new Vector2(-dir.y, dir.x);    // points up, away from the line

        ResetBall();
    }

    void Update()
    {
        // waiting between drops
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f) ResetBall();
            return;
        }

        float dt = Time.deltaTime;
        Vector2 pos = rt.localPosition;
        Vector2 oldPos = pos;

        if (rolling)
        {
            // speed up going downhill
            speed = Mathf.Max(speed + gravity * -dir.y * dt, minRollSpeed);
            float t = Vector2.Dot(pos - start, dir) + speed * dt;

            if (t > length)          // reached the end, so fall off
            {
                rolling = false;
                vel = dir * speed;
            }
            pos = start + dir * t + normal * radius;
        }
        else
        {
            // falling
            vel.y -= gravity * dt;
            pos += vel * dt;

            float t = Vector2.Dot(pos - start, dir);       // how far along the line
            float dist = Vector2.Dot(pos - start, normal); // how high above the line
            float into = Vector2.Dot(vel, normal);         // moving towards the line?

            if (t >= 0f && t <= length && dist < radius && dist > -radius && into < 0f)
            {
                pos += normal * (radius - dist);           // sit on top of the line
                Vector2 along = vel - normal * into;

                if (-into > 200f) vel = along - normal * into * bounce;   // bounce
                else { rolling = true; speed = Vector2.Dot(vel, dir); }   // start rolling
            }

            if (pos.y < resetY)      // off screen, so wait a random time
            {
                waitTimer = Random.Range(waitRange.x, waitRange.y);
                return;
            }
        }

        rt.localPosition = pos;

        // spin to match how far it moved
        spin -= (pos.x - oldPos.x) / radius * Mathf.Rad2Deg;
        rt.localEulerAngles = new Vector3(0, 0, spin);
    }

    void ResetBall()
    {
        // random landing spot along the line (never closer than one ball-width to the start)
        float along = Mathf.Max(Random.Range(dropRange.x, dropRange.y) * length, radius * 1.5f);

        rt.localPosition = start + dir * along + normal * dropHeight;
        vel = Vector2.zero;
        rolling = false;
        speed = 0f;
        bounce = Random.Range(bounceRange.x, bounceRange.y);
    }
}