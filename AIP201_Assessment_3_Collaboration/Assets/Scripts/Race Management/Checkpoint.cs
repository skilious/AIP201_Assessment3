using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber;
    public LapSystem sys;
    public LayerMask vehicle;

    void Update()
    {
        //Check distance between the car's first place & "THIS" checkpoint's position.
        Vector3 raycastDir = sys.cars[0].transform.position - transform.position;
        //Raycast from its position to distance and if within 5.0 distance, it'll trigger only if they're both correct values 1=1.
        if (Physics.Raycast(transform.position, raycastDir, 5.0f, vehicle) && sys.checkpoint_ == checkpointNumber)
        {
            //Next checkpoint!
            sys.checkpoint_++;
            //If the last checkpoint has gone through, it'll reset.
            if (sys.checkpoint_ >= 4)
            {
                sys.cars[0].laps++;
                sys.checkpoint_ = 0;
            }
            //Checking if this whole thing works w/ raycast.
            Debug.Log("Works!");

        }
    }
}