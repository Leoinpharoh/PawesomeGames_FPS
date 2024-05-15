using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    [SerializeField] private GameObject inventoryScreen;

    float rotationX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
        void Update()
        {
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

            if(invertY)
            {
                rotationX += mouseY;
            }
            else
            {
                rotationX -= mouseY;
            }

            rotationX = Mathf.Clamp(rotationX, -60f, 60f);

            transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            transform.parent.Rotate(Vector3.up * mouseX);
        }
    /*void Update()
    {
        bool isInventoryOpen = inventoryScreen.activeSelf;

        if (!isInventoryOpen)
        {
            UpdateCameraRotation();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }*/

    private void UpdateCameraRotation()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
        {
            rotationX += mouseY;
        }
        else
        {
            rotationX -= mouseY;
        }

        rotationX = Mathf.Clamp(rotationX, -60f, 60f);

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
