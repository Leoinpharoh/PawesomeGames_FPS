using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OSUnlocked : MonoBehaviour
{
    PlayerManager playerManager;

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
                Debug.Log("Boss dead");
                //objectsToCheck.Remove(obj);
                PlayerPrefs.SetInt("OvershieldUnlocked", 0);
                PlayerPrefs.Save();
                GameManager.Instance.playerOSToggle.SetActive(false);
            }
        }
    }
}
