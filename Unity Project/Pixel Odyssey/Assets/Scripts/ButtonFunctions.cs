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

    public void sceneIntro(Button button)
    {
        
        Debug.Log("Button/Scene Name: " + button.name); // Log the button's name, which is expected to be the scene's name


        StartCoroutine(LoadAndSetActiveScene(button.name)); // Load the scene asynchronously and then set it as active
    }

    private IEnumerator LoadAndSetActiveScene(string sceneName)
    {
        
        if (!SceneManager.GetSceneByName(sceneName).isLoaded) // Check if the scene is already loaded; if not, load it
        {
            Debug.Log("Loading scene: " + sceneName);
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        
        Scene loadedScene = SceneManager.GetSceneByName(sceneName); // Get the loaded scene by name
        if (loadedScene.IsValid())
        {
            Debug.Log("Setting active scene: " + sceneName);
            SceneManager.SetActiveScene(loadedScene);
        }
        else
        {
            Debug.LogError("Failed to load the scene: " + sceneName);
        }
    }


}
