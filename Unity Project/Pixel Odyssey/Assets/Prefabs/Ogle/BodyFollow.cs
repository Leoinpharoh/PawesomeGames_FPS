using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFollow : MonoBehaviour
{
    public Transform cameraTransform;
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial rotation of the object
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (cameraTransform != null)
        {
            Vector3 directionToCamera = cameraTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            targetRotation *= Quaternion.Euler(0, -90, -90); // Apply the corrective rotation

            // Convert target rotation relative to the initial rotation
            Quaternion relativeRotation = Quaternion.Inverse(initialRotation) * targetRotation;

            // Clamp the Y rotation
            relativeRotation = ClampRotation(relativeRotation);

            // Convert back to world space rotation
            targetRotation = initialRotation * relativeRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }

    Quaternion ClampRotation(Quaternion q)
    {
        q.ToAngleAxis(out float angle, out Vector3 axis);
        angle = NormalizeAngle(angle);

        // Clamp the angle
        angle = Mathf.Clamp(angle, -60, 60);

        // Return the new quaternion
        return Quaternion.AngleAxis(angle, axis);
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 360) angle -= 360;
        while (angle < 0) angle += 360;
        if (angle > 180) angle -= 360;
        return angle;
    }
}
