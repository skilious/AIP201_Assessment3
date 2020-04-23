using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LapSystem : MonoBehaviour
{
    public List<GameObject> Racers;
    public GameObject CheckPointManager;
    public Text lap, position; 
    [HideInInspector]
    List<GameObject> Checkpoints;
    [HideInInspector]
    public List<CarController> cars = new List<CarController>();
    [HideInInspector]
    public float IndexAmnt;

    float PreviousLapCounter = 0.0f;
    int PositionInRace = 0;
    float PlayerLapCounter = 0.0f;
    public int checkpoint_;
    
    void Start()
    {
        Checkpoints = CheckPointManager.GetComponent<FindPath>().checkpoints;
        IndexAmnt = 1.0f / Checkpoints.Count;
        Debug.Log("Indexing " + IndexAmnt + " per checkpoint");
        for (int i = 0; i < Racers.Count; i++)
        {
            if(!Racers[i].GetComponent<CarController>())
            {
                Debug.LogError("Failed Car Controller Add on " + Racers[i].name + " count of: " + i);
                Racers.RemoveAt(i);
                i--;
            }
            else
            {
                cars.Add(Racers[i].GetComponent<CarController>());
            }
        }
        
       
        Debug.Log("Current racers participants: " + Racers.Count);
        Debug.Log("Current car controllers found: " + cars.Count);
        
    }

    // Update is called once per frame
    void Update()
    {
        cars.Sort((s1, s2) => s1.LapCounter.CompareTo(s2.LapCounter));
        if(cars[0].LapCounter > PreviousLapCounter)
        {
            PreviousLapCounter = cars[0].LapCounter;
            Debug.Log("Lap progression; leader is " + cars[0].name + " at lap: " + cars[0].LapCounter);
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
