using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarController : MonoBehaviour
{
    public LapSystem System;
    public LayerMask layerCheck;
    public int checkpoints;
    public float LapCounter;
    public int laps;
    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, 0.5f, layerCheck))
        {
            LapCounter += System.IndexAmnt;
            //Debug.Log("Trigger!: " + LapCounter);
            //Debug.Log("Triggered: " + LapCounter);
        }
    }
}
