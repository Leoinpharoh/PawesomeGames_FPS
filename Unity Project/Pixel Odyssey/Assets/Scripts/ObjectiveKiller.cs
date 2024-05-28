using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveKiller : MonoBehaviour
{
    [SerializeField] ObjectiveTrigger objectiveToKill;
    string objectiveToKillString;
    void Start()
    {
        objectiveToKillString = objectiveToKill.objective.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //add string to objectives list
            for(int i  = 0; i < GameManager.Instance.objectives.Count; i++)
            {
                if (objectiveToKillString == GameManager.Instance.objectives[i])
                {
                    GameManager.Instance.objectives[i].Replace(objectiveToKillString, "Aquiring...");
                    Debug.Log(GameManager.Instance.objectives[i]);
                    Debug.Log(objectiveToKillString);
                }
            }
            GameManager.Instance.updateGameObjective();
        }

    }
}
