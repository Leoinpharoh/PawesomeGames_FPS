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
                }
                else
                {
                    Debug.Log("Objectives Loaded");
                }
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
}
