using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float LapCounter;
    public LapSystem System;
    public LayerMask checkpoint;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CheckPoint"))
        {
            LapCounter += System.IndexAmnt;
            //Debug.Log("Trigger!: " + LapCounter);
        }
    }
}
