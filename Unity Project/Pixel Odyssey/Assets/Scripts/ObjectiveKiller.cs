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
                    GameManager.Instance.objectives[i] = string.Empty;
                    for (int j = 0; j < GameManager.Instance.objectives.Count; j++)
                {
                        if(GameManager.Instance.objectives[j] == string.Empty)
                        {
                            if(j == 2)
                            {
                                GameManager.Instance.updateGameObjective();
                                return;
                            }
                            else
                            {
                                int k = j + 1;
                                GameManager.Instance.objectives[j] = GameManager.Instance.objectives[k];
                                GameManager.Instance.objectives[k] = string.Empty;
                            }
                        }
                }
                    Debug.Log(GameManager.Instance.objectives[i]);
                    Debug.Log(objectiveToKillString);
                }
            }
            GameManager.Instance.updateGameObjective();
        }

    }
}
