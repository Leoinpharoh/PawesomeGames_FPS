using UnityEngine;

public class CenterObjectLogger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the center of the screen
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // Log the name of the object that was hit
                Debug.Log("Object at center of screen: " + hit.collider.gameObject.name);
                if(hit.collider.gameObject.name == "Scene Selection Screen" && !GameManager.Instance.isPaused)
                {
                    GameManager.Instance.sceneSelectMain();
                }
            }
            else
            {
                Debug.Log("No object at center of screen.");
            }
        }
    }
}