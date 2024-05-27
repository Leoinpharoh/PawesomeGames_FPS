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
            targetLight = GetComponent<Light>();
        }
    }

    void Update()
    {
        // Increment lerpTime based on colorChangeSpeed and time elapsed
        lerpTime += Time.deltaTime * colorChangeSpeed;

        // Ping-pong the lerpTime to create a continuous transition between startColor and endColor
        float t = Mathf.PingPong(lerpTime, 1.0f);

        // Update the light color
        targetLight.color = Color.Lerp(startColor, endColor, t);
    }
}
