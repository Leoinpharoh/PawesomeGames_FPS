using UnityEngine;

public class CameraController : MonoBehaviour
{
    public DisplayInventory inventoryDisplay;

    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;
    //[SerializeField] private GameObject inventoryScreen;

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
        bool isInventoryOpen = inventoryDisplay.isActiveAndEnabled;

        if (!isInventoryOpen)   //if inventory is not open camera can move
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
