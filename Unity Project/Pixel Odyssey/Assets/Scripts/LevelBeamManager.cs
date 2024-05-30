using System.Collections.Generic;
using UnityEngine;

public class LevelBeamManager : MonoBehaviour
{
    public GameObject nextLevelBeam; // Reference to the NextLevelBeam GameObject in the scene
    public List<GameObject> objectsToCheck; // List of objects to check

    void Start()
    {
        // Initially deactivate the nextLevelBeam
        if (nextLevelBeam != null)
        {
            nextLevelBeam.SetActive(false);
        }
    }

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

        if (allDestroyed && nextLevelBeam != null)
        {
            nextLevelBeam.SetActive(true);
        }
    }
}
