using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    public void resume()
    {
        GameManager.Instance.stateUnPaused();
    }    

    public void restart() 
    {
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.stateUnPaused();
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
        
        Debug.Log("Button/Scene Name: " + button.name); // Log the button's name, which is expected to be the scene's name


        StartCoroutine(LoadAndSetActiveScene(button.name)); // Load the scene asynchronously and then set it as active
    }

    private IEnumerator LoadAndSetActiveScene(string sceneName) // Load the scene asynchronously and then set it as active
    {
        
        if (!SceneManager.GetSceneByName(sceneName).isLoaded) // Check if the scene is already loaded; if not, load it
        {
            Debug.Log("Loading scene: " + sceneName); // Log the scene that is being loaded
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single); // Load the scene asynchronously and wait for it to finish loading 
        }

        
        Scene loadedScene = SceneManager.GetSceneByName(sceneName); // Get the loaded scene by name
        if (loadedScene.IsValid()) // Check if the scene is valid
        {
            Debug.Log("Setting active scene: " + sceneName); // Log the scene that is being set as active
            SceneManager.SetActiveScene(loadedScene); // Set the scene as active
        }
        else
        {
            Debug.LogError("Failed to load the scene: " + sceneName); // Log an error message if the scene failed to load
        }
    }


}
