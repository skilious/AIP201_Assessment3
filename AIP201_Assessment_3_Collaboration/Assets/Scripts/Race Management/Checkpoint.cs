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
        foreach (CarController car in sys.cars)
        {
            //Check distance between the car's first place & "THIS" checkpoint's position.
            Vector3 raycastDir = car.transform.position - transform.position;
            //Raycast from its position to distance and if within 5.0 distance, it'll trigger only if they're both correct values 1=1.
            if (Physics.Raycast(transform.position, raycastDir, 5.0f, vehicle) && car.NextCheckpoint == checkpointNumber)
            {
                //Next checkpoint!
                //sys.checkpoint_++;
                car.NextCheckpoint++;
                car.LapCounter += sys.IndexAmnt;
                //If the last checkpoint has gone through, it'll reset.
                if (car.NextCheckpoint >= sys.Checkpoints.Count)
                {
                    
                    //sys.cars[0].laps++;
                    car.laps++;
                    car.LapCounter = car.laps;
                    car.NextCheckpoint = 0;
                }
                //Checking if this whole thing works w/ raycast.
            }
        }
    }

    public void SetCheckpointNumber(int PositionInTrack)
    {
        checkpointNumber = PositionInTrack; //holds the position of this checkpoint in relation to the lap
    }
}