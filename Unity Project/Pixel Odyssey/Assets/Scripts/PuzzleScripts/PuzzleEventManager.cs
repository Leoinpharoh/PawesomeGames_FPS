//PuzzleEventManager

using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleEventManager : MonoBehaviour
{
    private Dictionary<string, Action> eventDictionary = new Dictionary<string, Action> ();
    private static PuzzleEventManager puzzleEventManager;

    public static PuzzleEventManager Instance
    {
        get
        {
            if (!puzzleEventManager)
            {
                puzzleEventManager = FindObjectOfType<PuzzleEventManager> ();
                //if (!puzzleEventManager)
                  //  Debug.Log("There needs to be one active PuzzleEventManager script on a GameObject in your scene.");
                //else
                    puzzleEventManager.Init();
            }
            return puzzleEventManager;
        }
    }

    private void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action>();    //creating our dictionary to hold all our puzzle actions
        }
    }

    public static void StartListening(string eventName, Action listener)
    {
        Action thisEvent;

        if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action listener)
    {
        if (puzzleEventManager != null) return;

        Action thisEvent;
        if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            Instance.eventDictionary.TryGetValue (eventName, out thisEvent);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        Action thisEvent;
        if(Instance.eventDictionary.TryGetValue(eventName,out thisEvent))
            thisEvent.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
