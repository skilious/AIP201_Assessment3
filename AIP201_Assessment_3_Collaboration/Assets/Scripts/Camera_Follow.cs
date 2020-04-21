using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public float smoothSpeed;
    public Transform player;
    public float distanceUp;
    public float distanceAway;
    // Update is called once per frame
    void LateUpdate()
    {
        //setting up car position to be offset from the player's car.
        Vector3 targetPosition = player.position + Vector3.up * distanceUp - player.forward * distanceAway;
        //Position delay behind player's car.
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        //It's position equal to smoothedPosition

        //Looking at player's car.
        transform.LookAt(player);
    }
}
//Its not as good to be honest, very glitchy. If someone can fix this. *thumbs up*
//By John Mac - Third Person Control: Camera Follow 
//Link:https://www.youtube.com/watch?v=PO5_aqapZXY

    //Currently working on this: Tai.