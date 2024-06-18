using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{

    PlayerManager playerManager;

    public Button targetButton;
    public void resume()
    {
        GameManager.Instance.stateUnPaused();
    }

    public void back()
    {
        GameManager.Instance.stateUnPaused();
    }

    public void restart() 
    {
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //playerManager.LoadPlayer();
        GameManager.Instance.stateUnPaused();
    }
    public void HUB()
    {
        //reload scene
        SceneManager.LoadScene("Player Hub");
        GameManager.Instance.stateUnPaused();
    }

    public void options()
    {
        if (GameManager.Instance.menuActive != null)
        {
            GameManager.Instance.menuActive.SetActive(false); // Deactivate the current active menu
        }

        GameManager.Instance.menuOptions.SetActive(true); // Activate the options menu
        GameManager.Instance.menuActive = GameManager.Instance.menuOptions; // Set the active menu to the options menu
    }
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public void sceneSwitch(Button button) // Load the scene that the button is named after
    {
        StartCoroutine(LoadAndSetActiveScene(button.name)); // Load the scene asynchronously and then set it as active

        //make sure scene is not stuck and allow immediate player movement and function
        GameManager.Instance.stateUnPaused();
    }

    private IEnumerator LoadAndSetActiveScene(string sceneName) // Load the scene asynchronously and then set it as active
    {
        
        if (!SceneManager.GetSceneByName(sceneName).isLoaded) // Check if the scene is already loaded; if not, load it
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single); // Load the scene asynchronously and wait for it to finish loading 
        }

        
        Scene loadedScene = SceneManager.GetSceneByName(sceneName); // Get the loaded scene by name

        if (loadedScene.IsValid()) // Check if the scene is valid
        {
            SceneManager.SetActiveScene(loadedScene); // Set the scene as active
        }
        GameManager.Instance.statePaused();
    }


}
