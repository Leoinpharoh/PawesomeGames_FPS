using System.Collections;
using UnityEngine;
using TMPro;

public class Subtitles : MonoBehaviour
{
    public TMP_Text displayText;  // Main text component for subtitles
    public TMP_Text fadeText;     // Separate TMP text component for continuous fading
    public string messageToDisplay = "Hello, World! This is the message.";
    public float typingSpeed = 0.1f;  // Time between each letter
    public float fadeDuration = 1.0f;  // Duration for each fade in and fade out cycle

    private bool keepFading = true;  // Flag to control the fading loop

    void OnTriggerEnter()
    {
        displayText.text = "";  // Optionally clear the main text at the start
        StartCoroutine(TypeMessage());
        StartCoroutine(FadeInOutLoop(fadeText));
    }

    IEnumerator TypeMessage()
    {
        foreach (char letter in messageToDisplay.ToCharArray())
        {
            displayText.text += letter;  // Add one letter at a time
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // If Enter is pressed, skip to the end of the message
                displayText.text = messageToDisplay;
                break;
            }
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait until Enter key is pressed to clear the text and stop fading
        // This loop ensures that Enter must be pressed even if it's pressed early
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }

        keepFading = false;  // Stop the fading loop
        fadeText.text = "";  // Clear the fading text
        displayText.text = "";  // Clear the main text
    }

    IEnumerator FadeInOutLoop(TMP_Text text)
    {
        while (keepFading)
        {
            // Fade in
            yield return FadeTextToAlpha(text, 1.0f);
            // Fade out
            yield return FadeTextToAlpha(text, 0.0f);
        }
    }

    IEnumerator FadeTextToAlpha(TMP_Text text, float targetAlpha)
    {
        float startAlpha = text.color.a;
        float timer = 0.0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, targetAlpha);  // Ensure target alpha is set
    }
}
