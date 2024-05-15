using UnityEngine;
using UnityEngine.UI;

public class MinimapIcon : MonoBehaviour
{
    public Transform playerTransform;  // Reference to the player's transform
    public RectTransform minimapRectTransform;  // Reference to the RectTransform of the minimap
    public RectTransform iconRectTransform;  // Reference to the RectTransform of the player icon
    public float mapScale = 1.0f;  // Adjust based on your map scale

    void Update()
    {

        Vector3 playerPosition = playerTransform.position; // Get the player's position

        
        float minimapWidth = minimapRectTransform.rect.width; // Get the width of the minimap
        float minimapHeight = minimapRectTransform.rect.height; // Get the height of the minimap

        float posX = (playerPosition.x / mapScale); // Calculate the X position of the player icon
        float posY = (playerPosition.z / mapScale); // Calculate the Y position of the player icon

        iconRectTransform.localPosition = new Vector3(posX, posY, 0); // Set the position of the player icon

        float playerRotationY = playerTransform.eulerAngles.y; // Get the player's Y rotation
        iconRectTransform.localRotation = Quaternion.Euler(0, 0, -playerRotationY); // Set the rotation of the player icon
    }
}