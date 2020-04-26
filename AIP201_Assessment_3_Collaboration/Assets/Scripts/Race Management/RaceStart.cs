using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// the only reason i have this scrpit is to stop the game when you open it so the person can start it when they want to,
// and I didnt know where else to place it so a put it here so if you want to add some starting mechanism then it can
// go here.

public class RaceStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }
}
