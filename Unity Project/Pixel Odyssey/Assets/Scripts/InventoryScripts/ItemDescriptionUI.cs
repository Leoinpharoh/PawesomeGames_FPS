//ItemDescriptionUI

using UnityEngine;
using TMPro;

public class ItemDescriptionUI : MonoBehaviour
{
    public GameObject descriptionPanel; //panel that the text will show up in
    public TextMeshProUGUI descriptionText;

    public void Start()
    {
        if (descriptionPanel != null && descriptionText != null)    //if there is a description panel
            descriptionText.text = ""; //initially empty
    }

    public void UpdateDescription(string description)
    {
        descriptionText.text = description; //set the new text as the text to appear
    }

    public void ClearDescription()
    {
        descriptionText.text = "";  //set text as empty
    }
}