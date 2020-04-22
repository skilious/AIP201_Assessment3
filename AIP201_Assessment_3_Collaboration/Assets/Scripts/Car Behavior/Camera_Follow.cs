using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public float smoothSpeed;
    public Transform player;
    public float distanceUp;
    public float distanceAway;
    Hover car;
    float velocityMultiplier;
    void Start()
    {
        car = player.GetComponent<Hover>();
        velocityMultiplier = car.speed;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //update our Velocity Multiplier
        velocityMultiplier = car.speed;

        //setting up car position to be offset from the player's car.
        Vector3 targetPosition = player.position + Vector3.up * distanceUp - player.forward * (distanceAway + velocityMultiplier/5f); //by adding in the velocity multiplier we're adding an offest as the
        //car accelerates

        //Position delay behind player's car.
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        //It's position equal to smoothedPosition

        //Looking at player's car.
        transform.LookAt(player); //looks @ anchor point, hence the camera jitter
    }
}
//Its not as good to be honest, very glitchy. If someone can fix this. *thumbs up*
//By John Mac - Third Person Control: Camera Follow 
//Link:https://www.youtube.com/watch?v=PO5_aqapZXY

    //Currently working on this: Tai.