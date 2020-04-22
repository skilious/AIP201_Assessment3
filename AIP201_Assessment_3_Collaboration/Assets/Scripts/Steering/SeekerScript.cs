using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SeekerState
{
    seek, flee
}


public class SeekerScript : MonoBehaviour
{
    /* 
        -  REFACTORING
        pass in list of nodes to target
        steer towards each node -- no decelleration needed
    */

    public GameObject TARGET;
    public float Max_Velocity = 0.1f, Max_Steering, Seeker_Mass, FleeDistance, SeekDistance, Slowing_Radius;

    Vector3 Velocity, Steering, Desired_Velocity;
    Vector3 FuturePosition;

    public SeekerState seekerstate = SeekerState.seek;
    public bool HUNTING = false;
    public bool NOT_MOVING = false;
    public GameObject FuturePositionRep;
    SeekerScript TargetScript;
    void Start()
    {
        if (TARGET.GetComponent<SeekerScript>().NOT_MOVING == false)
        {
            Debug.Log("Found Something to chase!");
            TargetScript = TARGET.GetComponent<SeekerScript>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!NOT_MOVING)
        {
            float distance = Vector3.Distance(transform.position, TARGET.transform.position);
            if (seekerstate == SeekerState.seek)
            {
                //SEEK EXTREMEE
                // YEET TO TTHE TARGET!
                //Velocity = (TARGET.position - Seeker.position).normalized * Max_Velocity;

                //SEEK W/ STEERING FORCES
                Desired_Velocity = (TARGET.transform.position - transform.position).normalized * Max_Velocity;

                if (HUNTING)
                {
                    if (TargetScript.Velocity.magnitude != 0)
                    {
                        float T = distance / TargetScript.Velocity.magnitude;
                        ////Debug.Log("T = " + T);
                        FuturePosition = new Vector3(TARGET.transform.position.x + TargetScript.Velocity.x * T, TARGET.transform.position.y + TargetScript.Velocity.y * T, TARGET.transform.position.z + TargetScript.Velocity.z * T);
                        Desired_Velocity = (FuturePosition - transform.position).normalized * Max_Velocity;
                        //confirm our aiming -- aim for the gameobject
                        FuturePositionRep.transform.position = FuturePosition;       
                    }
                }
                else
                {
                    if (distance < Slowing_Radius)
                    {
                        Desired_Velocity = Desired_Velocity * (distance / Slowing_Radius);
                    }
                }

                Steering = Desired_Velocity - Velocity; //figure difference between target and current
                Steering = Steering / Seeker_Mass; //account for intertia

                Steering = Vector3.ClampMagnitude(Steering, Max_Steering);

                Velocity = Vector3.ClampMagnitude(Velocity + Steering, Max_Velocity);
                if (distance < FleeDistance)
                {
                    seekerstate = SeekerState.flee;
                }

            }
            else if (seekerstate == SeekerState.flee)
            {
                Desired_Velocity = (transform.position - TARGET.transform.position).normalized * Max_Velocity;

                Steering = Desired_Velocity - Velocity; //figure difference between target and current
                Steering = Steering / Seeker_Mass; //account for intertia

                Steering = Vector3.ClampMagnitude(Steering, Max_Steering);

                Velocity = Vector3.ClampMagnitude(Velocity + Steering, Max_Velocity);

                if (distance > SeekDistance)
                {
                    seekerstate = SeekerState.seek;
                }
            }


            Debug.DrawRay(transform.position, (Desired_Velocity).normalized * 2, Color.blue, 0.1f);
            Debug.DrawRay(transform.position, Velocity, Color.red, 0.1f);
            transform.position = transform.position + (Velocity * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmos()
    {
        if (HUNTING)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(FuturePositionRep.transform.position, Vector3.one * 0.8f);
        }
    }
}
