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
        if (other.CompareTag("Player") && needsObjective)
        {
            Debug.Log("objective zone");
            //add string to objectives list
            GameManager.Instance.objectives.Add(objective);
            GameManager.Instance.updateGameObjective();
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        gameObject.SetActive(false);
    }
}
