using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour
{
    public TMP_Text displayText;  // Assign your Text component here in the inspector
    public string messageToDisplay = "Hello, World! This is the message.";
    public float typingSpeed = 0.1f;  // Time between each letter

    void OnTriggerEnter()
    {
        StartCoroutine(TypeMessage());
    }

    IEnumerator TypeMessage()
    {
        displayText.text = "";  // Clear existing text
        foreach (char letter in messageToDisplay.ToCharArray())
        {
            displayText.text += letter;  // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed);  // Wait before adding the next letter
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));  // Wait until Enter key is pressed
        displayText.text = "";  // Optionally clear the text or do other actions
    }
}
