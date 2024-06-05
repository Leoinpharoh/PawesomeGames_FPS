using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CenterObjectLogger : MonoBehaviour
{
    public Animator animator;

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
                // Check the name of the object that was hit
                string hitObjectName = hit.collider.gameObject.name;
                Debug.Log("Object at center of screen: " + hitObjectName);

                if (hitObjectName == "Scene Selection Screen" && !GameManager.Instance.isPaused)
                {
                    GameManager.Instance.sceneSelectMain();
                }
                else if (hitObjectName == "Options Screen" && !GameManager.Instance.isPaused)
                {
                    GameManager.Instance.optionsMain();
                }
                else if (hitObjectName == "Play Screen" && !GameManager.Instance.isPaused)
                {
                    animator.enabled = true;
                    animator.SetTrigger("Play");
                    StartCoroutine(LoadLevel());
                }
            }
            else
            {
                Debug.Log("No object at center of screen.");
            }
        }
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(6);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Player Hub", LoadSceneMode.Single);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene loadedScene = SceneManager.GetSceneByName("Player Hub"); // Get the loaded scene by name
        SceneManager.SetActiveScene(loadedScene);
    }
}
