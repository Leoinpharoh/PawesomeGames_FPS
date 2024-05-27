using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] string objective;
    [SerializeField] string objective2;
    [SerializeField] string objective3;
    bool needObjective;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.updateGameObjective("Loading...please wait...", "", "");
        needObjective = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If no objective provide one
        if(needObjective)
        {
            GameManager.Instance.updateGameObjective(objective, objective2, objective3);
            //feed objective to UI
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            needObjective = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
