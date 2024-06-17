//PickUpMessage

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickUpMessage : MonoBehaviour
{
    public GameObject pickUpPanel;
    public TextMeshProUGUI pickUpText;

    private GameObject tempActiveObject;
    // Start is called before the first frame update
    void Start()
    {
        HidePanel();
        tempActiveObject = new GameObject("TempCoroutineHandler");
        DontDestroyOnLoad(tempActiveObject);
        tempActiveObject.AddComponent<MonoBehaviourHelper>();
    }

    public void ShowPickUpPanel(string message)
    {
        pickUpText.text = message;
        pickUpPanel.SetActive(true);

        tempActiveObject.GetComponent<MonoBehaviourHelper>().StartCoroutine(HideAfterDelay(3f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HidePanel();
    }

    public void HidePanel()
    {
        pickUpPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if(tempActiveObject != null)
            Destroy(tempActiveObject);
    }

    private class MonoBehaviourHelper : MonoBehaviour { }
}
