using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LapSystem : MonoBehaviour
{
    public List<GameObject> Racers;             //operator provided list of cars that are racing
    public GameObject CheckPointManager;        //GO for object checkpoint management system
    public Text lap, position;                  //UI elements for Player's GUI
    [HideInInspector]
    public List<Checkpoint> Checkpoints;               //automatically populated list of checkpoints
    [HideInInspector]
    public List<CarController> cars = new List<CarController>();    //automatically populated list of car checkpoint scripts
    [HideInInspector]
    public float IndexAmnt;                     //amount to index the lap count float, solved by 1/<number of checkpoints>

    float PreviousLapCounter = 0.0f;            //hold for previous leader
    int PositionInRace = 0;                     //global for player's car position
    float PlayerLapCounter = 0.0f;              //global for lap + fraction
    public int checkpoint_;                     //current cehckpoint

    public int totalLaps; // max amount of laps to race so we can stop the race
    public GameObject endScreen; // screen to display when game is over

    void Start()
    {
        List<GameObject> CheckpointHold = CheckPointManager.GetComponent<FindPath>().checkpoints;   //gather the list of checkpoints from our AI's checkpoint list
        
       
        for (int i = 0; i < Racers.Count; i++)
        {
            if(!Racers[i].GetComponent<CarController>())
            {
                Debug.LogError("Failed Car Controller Add on " + Racers[i].name + " count of: " + i); //error check, make sure we have a checkpoint controller on the cars
                Racers.RemoveAt(i);
                i--;
            }
            else
            {
                cars.Add(Racers[i].GetComponent<CarController>());              //add the checkpoint controller from the car to the list of checkpoint controllers so we can index through them
            }
        }
        
        for(int i = 0; i < CheckpointHold.Count; i++) //for every cehcpoint
        {
            Checkpoints.Add(CheckpointHold[i].GetComponent<Checkpoint>());      //add the held checkpoint in the last spot of out checkpoint
            Checkpoints[i].checkpointNumber = i;                                //set the checkpoint number to the position in the list
        }

        IndexAmnt = 1.0f / Checkpoints.Count;                                   //solve the fraction for what each checkpoint is worth
        Debug.Log("Indexing " + IndexAmnt + " per checkpoint");                 //stupid check
        Debug.Log("Current racers participants: " + Racers.Count);              //idiot check
        Debug.Log("Current car controllers found: " + cars.Count);              //sanity check
        
    }

    // Update is called once per frame
    void Update()
    {
        cars.Sort((s1, s2) => s1.LapCounter.CompareTo(s2.LapCounter));                                      //sort the list of cars by who has more laps
        if(cars[cars.Count - 1].LapCounter > PreviousLapCounter)
        {
            PreviousLapCounter = cars[cars.Count - 1].LapCounter;
            Debug.Log("Lap progression; leader is " + cars[cars.Count -1].name + " at lap: " + cars[cars.Count - 1].LapCounter);     //show who the leader is
        }

        CarController CurrentPosition;
        CurrentPosition = cars.Find(o => o.CompareTag("Player"));                                           //find the player's car 
        PlayerLapCounter = CurrentPosition.LapCounter;                                                      //store the car's lap info
        int PlayerCar = cars.Count - cars.FindIndex(o => o.CompareTag("Player"));                           //find the index of the players car in the sorted list 
        //Debug.Log("player is: " + PlayerCar);
        PositionInRace = PlayerCar;                                                                         //store the player's position for display to screen


        if (PlayerLapCounter >= totalLaps || PreviousLapCounter >= totalLaps) // simulate the end of the game when the total laps have been raced
            ///previous lap counter should always be equal to the player lap counter if player is in lead -- this if statement seems superflous
        {
            endScreen.SetActive(true);          // awaken the end screen
            Time.timeScale = 0.0f;              // slow down the game
        }
    }



    void OnGUI()
    {
        //display the player's info on screen
        lap.text = PlayerLapCounter.ToString();
        position.text = PositionInRace.ToString();
    }

}
