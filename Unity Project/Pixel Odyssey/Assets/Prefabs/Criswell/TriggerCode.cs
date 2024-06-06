using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerCode : MonoBehaviour
{
    public UnityEvent performCode;
    void OnTriggerEnter()
    {
        triggerCode();
    }


    void triggerCode()
    {
        // Define what happens when Enter is pressed at the end of the last message
        if (performCode != null)
        {
            performCode.Invoke();
        }

    }
}
