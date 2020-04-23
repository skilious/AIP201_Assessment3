using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LapSystem : MonoBehaviour
{

    public GameObject CheckPointManager;
    public List<GameObject> Racers;
    public Text lap, position;

    [HideInInspector]
    public List<GameObject> checkpoints;
    [HideInInspector]
    public List<CarController> cars = new List<CarController>();
    [HideInInspector]
    public float IndexAmnt;

    float PreviousLapCounter = 0.0f;
    int PositionInRace = 0;
    float PlayerLapCounter = 0.0f;

    public int checkpoint_ = 0;
    void Start()
    {
        //Checkpoints is equal to Findpath's nodes.
        checkpoints = CheckPointManager.GetComponent<FindPath>().checkpoints;
        
        //Index is 1 divide by the amount of checkpoints.
        IndexAmnt = 1.0f / checkpoints.Count;
        Debug.Log("Indexing " + IndexAmnt + " per checkpoint");

        //Check if car controller exists for each racer
        for (int i = 0; i < Racers.Count; i++)
        {
            if (!Racers[i].GetComponent<CarController>())
            {
                Debug.LogError("Failed Car Controller Add on " + Racers[i].name + " count of: " + i);
                Racers.RemoveAt(i);
                i--;
            }
            //Otherwise, add the non-existing script (For some reason) to Racer[i].
            else
            {
                cars.Add(Racers[i].GetComponent<CarController>());
            }
        }

        Debug.Log("Current racers participants: " + Racers.Count);
        Debug.Log("Current car controllers found: " + cars.Count);

    }

    void Update()
    {
        cars.Sort((s1, s2) => s1.LapCounter.CompareTo(s2.LapCounter));
        //cars.Sort((s1, s2) => s1.checkpoints.CompareTo(s2.checkpoints));
        if (cars[0].LapCounter > PreviousLapCounter)
        {
            PreviousLapCounter = cars[0].LapCounter;
            Debug.Log("Lap progression; leader is " + cars[0].name + " at lap: " + cars[0].LapCounter);
            
        }

        if(cars[0].LapCounter > cars[1].LapCounter && cars[0].checkpoints > cars[1].checkpoints)
        {
            Debug.Log("Current position: " + cars[0].name);
        }
        CarController CurrentPosition;
        CurrentPosition = cars.Find(o => o.CompareTag("Player"));
        PlayerLapCounter = CurrentPosition.LapCounter;

        int PlayerCar = cars.FindIndex(o => o.CompareTag("Player"));
        PositionInRace = PlayerCar;
    }


    void OnGUI()
    {
        lap.text = PlayerLapCounter.ToString();
        position.text = (PositionInRace + 1).ToString();
    }
}
