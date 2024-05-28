using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] public string objective;

    bool needsObjective;

    // Start is called before the first frame update
    void Start()
    {
        needsObjective = GameManager.Instance.needsObjective;
    }

    // Update is called once per frame
    void Update()
    {

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
                    return;
                    //for (int j = 0; j < GameManager.Instance.objectives.Count; j++)
                    //{
                    //    if (GameManager.Instance.objectives[j] == string.Empty)
                    //    {
                    //        int k = j + 1;
                    //        GameManager.Instance.objectives[j] = GameManager.Instance.objectives[k];
                    //        GameManager.Instance.objectives[k] = string.Empty;

                    //    }
                    //}
                }
                else
                {
                    Debug.Log("Objectives Loaded");
                }
            }
        }

    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") && needsObjective)
    //    {
    //        Debug.Log("objective zone");
    //        //add string to objectives list
    //        GameManager.Instance.objectives.Add(objective);
    //        GameManager.Instance.updateGameObjective();
    //    }

    //}
    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
}
