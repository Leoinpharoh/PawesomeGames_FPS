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
        if (performCode != null)
        {
            performCode.Invoke();
        }

    }
}
