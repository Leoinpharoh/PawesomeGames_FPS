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
        transformPosition();

        if (cameraTransform != null)
        {
            Vector3 directionToCamera = cameraTransform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
            targetRotation *= Quaternion.Euler(0, 0, 0); // Apply the corrective rotation

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
        angle = Mathf.Clamp(angle, 0, 0);

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

    //update robots transform as he moves
    public void transformPosition()
    {

    }

}

    //void Update()
    //{
    //    if (cameraTransform != null)
    //    {
    //        //calc direction to camera
    //        Vector3 directionToCamera = cameraTransform.position - transform.position;

    //        //Limit pitch angle to prevent direct up or down
    //        float angle = Vector3.Angle(Vector3.up, directionToCamera);
    //        float maxPitchAngle = 88f;

    //        if (angle > maxPitchAngle)
    //        {
    //            directionToCamera = Vector3.RotateTowards(transform.forward, directionToCamera, Mathf.Deg2Rad * (angle - maxPitchAngle), 0);
    //        }
    //        // Convert target rotation relative to the initial rotation
    //        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

    //        // smooth interpolation towards the target roation
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
    //    }
    //}

