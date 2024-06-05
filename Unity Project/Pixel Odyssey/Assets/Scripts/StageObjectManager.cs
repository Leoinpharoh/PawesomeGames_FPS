using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StageObjectManager : MonoBehaviour
{
    public GameObject objectToDestroy; // Reference to the NextLevelBeam GameObject in the scene
    public List<GameObject> objectsToCheck; // List of objects to check

    void Start()
    {
        // Initially deactivate the nextLevelBeam
        if (objectToDestroy != null)
        {
            objectToDestroy.SetActive(true);

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

        if (allDestroyed && objectToDestroy != null)
        {
           GameObject.Destroy(objectToDestroy);
        }
    }
}
