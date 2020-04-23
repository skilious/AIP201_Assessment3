using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float heightFromGround;
    public float amplitude = 1.0f;

    float distanceFromGround = 1.0f;
    public Vector3 tempPos;

    public float maxSpeed;
    public float speed;
    public float acceleration;
    public float rotationSpeed;
    float rotation;
    float previousRotation;
    float steeringRotation;

    public bool aiEnabled;
    void Start()
    {
        tempPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!aiEnabled)
        {
            Vector3 current_direction = transform.eulerAngles;
            //Hover formula
            tempPos.y = Mathf.Sin(Time.realtimeSinceStartup * heightFromGround) * amplitude + distanceFromGround;

            //steering
            rotation = Input.GetAxis("Horizontal") * 30.0f;
            steeringRotation = Input.GetAxis("Horizontal") * rotationSpeed;

            current_direction.y += rotationSpeed * steeringRotation * Time.fixedDeltaTime;
            current_direction.z = Mathf.Lerp(previousRotation, -rotation, 0.1f);//-rotation * rotationSpeed * Time.fixedDeltaTime;
            previousRotation = current_direction.z;
            transform.eulerAngles = current_direction;
            //Not yet useful as it interferes with different rotations (Will implement this for better results on rotation later).
            //transform.rotation = Quaternion.Slerp(transform.rotation, tiltRotation, Time.fixedDeltaTime * rotationSmoothness);

            //Velocity forward and backward.
            if (Input.GetAxis("Vertical") > 0.9f)
            {
                speed = acceleration + Mathf.Clamp(speed, 0, maxSpeed - acceleration);
            }
            //Decelerates overtime
            else
            {
                speed = -0.1f + Mathf.Clamp(speed, acceleration, maxSpeed);
            }
            //Reverse (Depends on how slow you want it w/ acceleration).
            if (Input.GetAxis("Vertical") < -0.9f)
            {
                //speed -= acceleration + Mathf.Clamp(speed, acceleration, maxSpeed);
                speed = -0.5f + Mathf.Clamp(speed, acceleration, maxSpeed); //switched for faster deceleration & slightly more realistic movement, fixing camera jump
            }

            //Finalise position as tempPos.
            tempPos += transform.rotation * Vector3.forward * speed * Time.fixedDeltaTime;
            transform.position = tempPos;
        }

        if(aiEnabled)
        {
            Vector3 current_direction = transform.eulerAngles;
            //Hover formula
            tempPos.y = Mathf.Sin(Time.realtimeSinceStartup * heightFromGround) * amplitude + distanceFromGround;

            current_direction.y += rotationSpeed * steeringRotation * Time.fixedDeltaTime;
            current_direction.z = Mathf.Lerp(previousRotation, -rotation, 0.1f);
            previousRotation = current_direction.z;
            transform.eulerAngles = current_direction;
        }

    }
}
//Tai's shit here.
