//SimpleConsole

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleConsole : MonoBehaviour
{
    public GameObject consoleTextReference;
    public int maxLines = 10;
    private Queue<string> lines = new Queue<string>();  //stores log mesages

    public static SimpleConsole Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Log(string message)
    {
        Debug.Log(message); 

        lines.Enqueue(message);
        if (lines.Count > maxLines) //removes oldest lines after reaching max lines
        {
            lines.Dequeue();
        }

        UpdateConsoleText();
    }

    private void UpdateConsoleText()
    {
        consoleTextReference.GetComponent<TextMeshPro>().text = string.Join("\n", lines);
    }
}
