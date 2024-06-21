using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OSUnlocked : MonoBehaviour
{
    PlayerManager playerManager;
    SaveSystem saveSystem;

    public List<GameObject> objectsToCheck; // List of objects to check
    void Start()
    {   
        
    }

    void Update()
    {
        if (objectsToCheck.Count > 0)
        {
            CheckObjects();
        }
    }
    private void CheckObjects()
    {
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj == null)
            {
                //objectsToCheck.Remove(obj);
                saveSystem.playerData.OvershieldUnlocked = true;
                saveSystem.SavePlayer();
                GameManager.Instance.ActivatePlayerOS();
            }
        }
    }
}
