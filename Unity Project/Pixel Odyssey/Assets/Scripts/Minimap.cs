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
        // Calculate the player's position relative to the minimap
        Vector3 playerPosition = playerTransform.position;

        // Adjust the position based on the scale of the minimap
        float minimapWidth = minimapRectTransform.rect.width;
        float minimapHeight = minimapRectTransform.rect.height;

        float posX = (playerPosition.x / mapScale);
        float posY = (playerPosition.z / mapScale);

        // Set the local position of the player icon
        iconRectTransform.localPosition = new Vector3(posX, posY, 0);

        // Update the player icon's rotation
        float playerRotationY = playerTransform.eulerAngles.y;
        iconRectTransform.localRotation = Quaternion.Euler(0, 0, -playerRotationY);
    }
}