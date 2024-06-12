using System.Collections.Generic;
using UnityEngine;

public class EnemeiesCurrentlyDeadManager : MonoBehaviour
{
    public List<GameObject> objectsToCheck; // List of objects to check
    int currentlyDeadEnemies;
    void Update()
    {
        // Continuously check if all objects are destroyed
        CheckKills();
    }
    private void CheckKills()
    {
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj.Equals(null))
            {
                objectsToCheck.Remove(obj);
                currentlyDeadEnemies++;
                GameManager.Instance.objectiveEnemiesKilledCount = currentlyDeadEnemies;
                GameManager.Instance.updateEnemiesToKill();
                break;
            }
        }
    }
}
