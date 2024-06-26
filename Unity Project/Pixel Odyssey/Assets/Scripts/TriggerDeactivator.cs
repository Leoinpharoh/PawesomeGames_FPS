using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDeactivator : MonoBehaviour
{
    [SerializeField] GameObject objectToDeactivate;
    private void OnEnable() // Triggers when player enters the collider
    {
        objectToDeactivate.SetActive(false);
    }
}
