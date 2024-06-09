using System.Collections.Generic;
using UnityEngine;

public class EnemeiesToKillManager : MonoBehaviour
{
    public List<GameObject> objectsToCheck; // List of objects to check

    void Update()
    {
        // Continuously check if all objects are destroyed
        CheckObjects();
    }
    private void CheckObjects()
    {
        bool allDestroyed = true;
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj != null)
            {
                allDestroyed = false;
                break;
            }
        }

    }
    private void CheckKills()
    {
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj.Equals(null))
            {
            //    currentlyDeadEnemies++;
            //    GameManager.Instance.objectiveEnemiesKilledCount = currentlyDeadEnemies;
            //    GameManager.Instance.updateEnemiesToKill();
            //    break;
            }
        }
    }
}
