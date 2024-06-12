using System.Collections.Generic;
using UnityEngine;

public class EnemeiesToKillManager : MonoBehaviour
{
    int enemiesToDestroy;
    public List<GameObject> objectsToCheck; // List of objects to check

    void Start()
    {
        // Continuously check if all objects are destroyed
        CheckObjects();
    }
    private void CheckObjects()
    {
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj != null)
            {
                enemiesToDestroy++;

            }
        }
        GameManager.Instance.objectiveEnemiesToKillCount = enemiesToDestroy;
        GameManager.Instance.updateEnemiesToKill();

    }
}
