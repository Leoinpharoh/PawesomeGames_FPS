using UnityEngine;

public class LightStrobe : MonoBehaviour
{
    [SerializeField] private Light targetLight;       // The light whose color you want to change
    [SerializeField] private Color startColor = Color.white; // The starting color
    [SerializeField] private Color endColor = Color.red;     // The ending color
    [SerializeField] private float colorChangeSpeed;  // The speed of the color change

    private float lerpTime = 0.0f;

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>(); // Get the light component if it is not assigned
        }
    }

    void Update()
    {
        lerpTime += Time.deltaTime * colorChangeSpeed; // Increment the lerp time
        float t = Mathf.PingPong(lerpTime, 1.0f); // Calculate the t value for the lerp
        targetLight.color = Color.Lerp(startColor, endColor, t); // Lerp the color of the light
    }
}
