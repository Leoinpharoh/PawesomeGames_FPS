using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //add string to objectives list
            for (int i = 0; i < GameManager.Instance.objectives.Count; i++)
            {
                if (GameManager.Instance.objectives[i] == "")
                {
                    GameManager.Instance.objectives[i] = objective;
                    GameManager.Instance.updateGameObjective();
                    break;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
}
