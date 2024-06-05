using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadFollow : MonoBehaviour
{
    public Transform cameraTransform;

    void Update()
    {
        if (cameraTransform != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraTransform.position - transform.position);
            targetRotation *= Quaternion.Euler(90, 0, 45); // Apply the corrective rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

}
