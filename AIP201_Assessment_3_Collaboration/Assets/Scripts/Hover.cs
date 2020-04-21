using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float horizontalSpeed;
    public float verticalSpeed;
    public float amplitude = 1.0f;

    float distanceFromGround = 2.0f;
    public Vector3 tempPos;

    public float acceleration;
    public float velocity;
    void Start()
    {
        tempPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tempPos.x += horizontalSpeed;
        tempPos.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude + distanceFromGround;
        transform.position = tempPos;
    }
}
