using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] public string objective;
    [SerializeField] public bool enemiesToKill;
    public List<GameObject> objectsToCheck;
    public int enemiesToDestroy;
    public int currentlyDeadEnemies = 0;
    bool needsObjective;

    // Start is called before the first frame update
    void Start()
    {
 
        needsObjective = GameManager.Instance.needsObjective;
        CheckObjects();
    }

    // Update is called once per frame
    void Update()
    {
        CheckKills();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger");

            //add string to objectives list
            for (int i = 0; i < GameManager.Instance.objectives.Count; i++)
            {
                if (GameManager.Instance.objectives[i] == "")
                {
                    GameManager.Instance.objectives[i] = objective;
                    GameManager.Instance.updateGameObjective();
                    Debug.Log("Objectives Loaded");
                    break;
                }
                else
                {
                    Debug.Log("Objectives not Loaded");
                }
            }
            if (enemiesToKill)
            {
                Debug.Log("enemiesToKill entered");
                if (objectsToCheck == null)
                {
                    Debug.Log("List is Empty");
                }
                if (objectsToCheck != null)
                {
                    GameManager.Instance.objectiveEnemiesKilledCount = currentlyDeadEnemies;
                    GameManager.Instance.objectiveEnemiesToKillCount = enemiesToDestroy;
                    Debug.Log("List is Loaded");
                    GameManager.Instance.updateEnemiesToKill();
                }
            }
            else
            {
                Debug.Log("No Enemies");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
    private void CheckObjects()
    {
        bool allDestroyed = true;
        foreach (GameObject obj in objectsToCheck)
        {
            if (obj != null)
            {
                allDestroyed = false;
                enemiesToDestroy++;
                
            }
        }
        GameManager.Instance.objectiveEnemiesToKillCount = enemiesToDestroy;
        GameManager.Instance.updateEnemiesToKill();

    }

    private void CheckKills()
    {
        Debug.Log("Checking Kills");
        foreach (GameObject obj in objectsToCheck)
        {
            Debug.Log("Checking Kills Deeper xD" + obj);
            if (obj.Equals(null))
            {
                Debug.Log("Checking Kills Deeper the third! xD" + currentlyDeadEnemies);
                objectsToCheck.Remove(obj);
                currentlyDeadEnemies++;
                GameManager.Instance.objectiveEnemiesKilledCount = currentlyDeadEnemies;
                GameManager.Instance.updateEnemiesToKill();
                break;
            }
        }
    }
}
