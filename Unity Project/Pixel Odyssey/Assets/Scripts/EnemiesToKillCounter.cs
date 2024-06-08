using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemiesToKillCounter : MonoBehaviour
{
    public List<GameObject> objectsToCheck;
    public int enemiesToDestroy;
    public int enemiesDestroyed;
    

    void Start()
    {

    }

    void Update()
    {
      // Continuously check if all objects are destroyed
        CheckObjects();
        if (objectsToCheck == null)
        {
            Debug.Log("List is Empty");
        }
        if(objectsToCheck != null)
        {
            GameManager.Instance.objectiveEnemiesKilledCount = enemiesDestroyed;
            GameManager.Instance.objectiveEnemiesToKillCount = enemiesToDestroy;
            GameManager.Instance.updateEnemiesToKill();
            Debug.Log("List is Loaded");

        }
    }
    private void CheckObjects()
    {
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj != null)
            {
                enemiesToDestroy += 1;
                break;
            }
            if(obj == null)
            {
                enemiesDestroyed += 1;
            }

        }
    }
}
