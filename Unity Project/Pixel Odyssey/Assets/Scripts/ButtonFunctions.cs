using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //DEBUG TOOLS NOT IN FINAL PRODUCT

    public void sceneIntro()
    {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("scene1"));
        GameManager.Instance.statePaused();
    }
    public void sceneOne()
    {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scene 1 - Dustin"));
        GameManager.Instance.statePaused();
    }
    public void sceneTwo()
    {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scene 2 - Michael"));
        GameManager.Instance.statePaused();
    }
    public void sceneThree()
    {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scene 3 - Conner"));
        GameManager.Instance.statePaused();
    }
    public void sceneFour()
    {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scene 4 - Leo"));
        GameManager.Instance.statePaused();
    }
    public void sceneFive()
    {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Scene 5 - Demetrice"));
        GameManager.Instance.statePaused();
    }
    public void sceneSix()
    {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("scene6"));
        GameManager.Instance.statePaused();
    }
    public void sceneFinBoss()
    {
 
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Final Boss"));
        GameManager.Instance.statePaused();
    }
    public void sceneCedits()
    {

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Credits"));
        GameManager.Instance.statePaused();
    }
 

}
