using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Reaction
{
    seek, flee, arrived, avoidCollision
}


public class PathSeeker : MonoBehaviour
{
    /* 
        Using a list of nodes created by the pathfinder script, this simple piece of code moves a charecter along the path to the desrired destination

    */
    public GameObject A_Manager;
    public GameObject Hunter;
    public float Max_Velocity = 0.1f, Max_Steering, Seeker_Mass, FleeDistance, SeekDistance, Slowing_Radius;
    
    Node NextNode;

    Vector3 Velocity, Steering, Desired_Velocity;
    Vector3 WanderlustDir;


    public Reaction seekerstate = Reaction.seek;

    List<Node> FollowMe;
    //List<Node> Unwalkable;
    public LayerMask AvoidMe;
    
    

    void Start()
    {
        FollowMe = new List<Node>();
        CollectPathNodeList();
        WanderlustDir = Vector3.back;
    }

    void CollectPathNodeList()
    {
        FollowMe = new List<Node>();
        FollowMe = A_Manager.GetComponent<FindPath>().path;
    }

    void TargetSwitch()
    {
        if (FollowMe.Count >= 1)
        {
            NextNode = FollowMe[0];
            FollowMe.RemoveAt(0);
            //Debug.Log("Tracking node. " + FollowMe.Count + " remaining");
        }
        else
        {
            seekerstate = Reaction.arrived;
        }

    }
    

// Update is called once per frame
void FixedUpdate() // changed from update() to fixedupdate() so i could use Time.timescale
    {
        if (Hunter != null)
        {
            float PersuerDistance = Vector3.Distance(transform.position, Hunter.transform.position);
            if (PersuerDistance < FleeDistance)
            {
                seekerstate = Reaction.flee;
            }
        }
        
        if (seekerstate == Reaction.seek)
        {
            CollectPathNodeList();
            TargetSwitch();
            Desired_Velocity = (NextNode.NodeWorldPos - transform.position).normalized * Max_Velocity;

            Steering = Desired_Velocity - Velocity; //figure difference between target and current
            Steering = Steering / Seeker_Mass; //account for intertia
            Steering = Vector3.ClampMagnitude(Steering, Max_Steering);

            Velocity = Vector3.ClampMagnitude(Velocity + Steering, Max_Velocity);
            transform.forward = Velocity.normalized;
            //if the hunter gets too close, run away and abandon the target

            if (Hunter != null)
            {
                float PersuerDistance = Vector3.Distance(transform.position, Hunter.transform.position);
                if (PersuerDistance < FleeDistance)
                {
                    seekerstate = Reaction.flee;
                }
            }
        }

        else if (seekerstate == Reaction.flee)
        {
            Desired_Velocity = (transform.position - Hunter.transform.position).normalized * Max_Velocity;

            Steering = Desired_Velocity - Velocity; //figure difference between target and current
            Steering = Steering / Seeker_Mass; //account for intertia

            Steering = Vector3.ClampMagnitude(Steering, Max_Steering);

            Velocity = Vector3.ClampMagnitude(Velocity + Steering, Max_Velocity);
            if (Hunter != null)
            {
                float PersuerDistance = Vector3.Distance(transform.position, Hunter.transform.position);
                if (PersuerDistance > FleeDistance * 2)
                {
                    seekerstate = Reaction.seek;
                }
            }

            //re fetch node path list when escape achieved, so we work towards the target again
        }
        else if (seekerstate == Reaction.arrived)
        {
            StartCoroutine(Wanderlust());
            Desired_Velocity = WanderlustDir * Max_Velocity;
            if (FollowMe.Count > 1)
            {
                float distance = Vector3.Distance(NextNode.NodeWorldPos, transform.position);
                if (distance > SeekDistance)
                {
                    seekerstate = Reaction.seek;
                }
            }
            else
            {
                CollectPathNodeList();
            }
        }
        

        
        Debug.DrawRay(transform.position, (Desired_Velocity).normalized * 2, Color.blue, 0.1f);  //point straight at where we want to go next
        Debug.DrawRay(transform.position, Velocity, Color.red, 0.1f); //Velocity direction for where we're going now

        transform.Rotate(transform.up, Vector3.SignedAngle(transform.forward, Velocity, Vector3.up) * Time.deltaTime);
        transform.position = transform.position + (transform.forward * Time.fixedDeltaTime * Velocity.magnitude); //only move forwards 

    }

    IEnumerator Wanderlust()
    {
        WanderlustDir = Random.insideUnitCircle;
        yield return new WaitForSeconds(6.0f);
    }
}
