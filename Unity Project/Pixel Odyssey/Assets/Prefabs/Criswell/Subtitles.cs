using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Subtitles : MonoBehaviour
{
    public TMP_Text displayText;  // Main text component for subtitles
    public TMP_Text fadeText;     // Separate TMP text component for continuous fading
    public string[] messagesToDisplay = new string[] { "Hello, World!", "Welcome to the game!", "Enjoy your adventure!" };
    public float typingSpeed = 0.1f;
    public float fadeDuration = 1.0f;

    private bool keepFading = true;
    private int currentMessageIndex = 0;
    private bool skipToEnd = false;
    public UnityEvent onSubtitlesComplete;

    void OnTriggerEnter()
    {
        StartSubtitles();
    }

    public void StartSubtitles()
    {
        StopAllCoroutines();  // Stop all existing coroutines to handle new trigger
        keepFading = true;
        currentMessageIndex = 0;
        StartCoroutine(ControlSubtitles());
    }

    IEnumerator ControlSubtitles()
    {
        StartCoroutine(FadeInOutLoop(fadeText));
        while (currentMessageIndex < messagesToDisplay.Length)
        {
            skipToEnd = false;
            yield return StartCoroutine(TypeMessage(messagesToDisplay[currentMessageIndex]));

            if (!skipToEnd)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
            }

            if (currentMessageIndex == messagesToDisplay.Length - 1)
            {
                TriggerSomethingElse();  // Call another function or trigger an event here
            }

            currentMessageIndex++;
        }

        keepFading = false;
        fadeText.text = "";
        displayText.text = "";
    }

    IEnumerator TypeMessage(string message)
    {
        displayText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            if (skipToEnd)
            {
                displayText.text = message;
                break;
            }
            else
            {
                displayText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                skipToEnd = true;
            }
        }
    }

    IEnumerator FadeInOutLoop(TMP_Text text)
    {
        while (keepFading)
        {
            yield return FadeTextToAlpha(text, 1.0f);
            yield return FadeTextToAlpha(text, 0.0f);
        }
    }

    IEnumerator FadeTextToAlpha(TMP_Text text, float targetAlpha)
    {
        float startAlpha = text.color.a; // Store the initial alpha value of the text
        float timer = 0.0f; // Initialize a timer to 0

        while (timer < fadeDuration) // Loop until the timer exceeds the duration of the fade
        {
            timer += Time.deltaTime; // Increment the timer by the time elapsed since last frame
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration); // Calculate the new alpha value
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha); // Update the text color with new alpha value
            yield return null; // Wait until the next frame before continuing the loop
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, targetAlpha); // Ensure the target alpha is set at the end
    }

    void TriggerSomethingElse()
    {
        // Define what happens when Enter is pressed at the end of the last message
        if (onSubtitlesComplete != null)
        {
            onSubtitlesComplete.Invoke();
        }
            
    }
}
