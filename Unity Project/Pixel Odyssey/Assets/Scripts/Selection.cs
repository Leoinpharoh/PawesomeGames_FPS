using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CenterObjectLogger : MonoBehaviour
{
    public Animator animator;
    public SaveSystem saveSystem;
    public bool ShotgunUnlocked = false;
    public bool AssaultRifleUnlocked = false;
    public bool RPGUnlocked = false;
    public bool MeleeUnlocked = false;
    public bool OvershieldUnlocked = false;
    public bool PotionbeltUnlocked = false;
    public bool TutorialComplete = false;

    public int PythonAmmo = 0;
    public int ShotgunAmmo = 0;
    public int AssaultRifleAmmo = 0;
    public int RPGAmmo = 0;

    public int HealthMax = 100;
    public int OvershieldMax = 0;

    public int OvershieldPotions = 0;
    public int HealthPotions = 0;

    public int Currency = 0;

    private bool isLoading = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string hitObjectName = hit.collider.gameObject.name;
                Debug.Log("Object at center of screen: " + hitObjectName);

                if (hitObjectName == "Load Screen")
                {
                    HandleLoadScreenClick();
                }
                else if (hitObjectName == "Options Screen")
                {
                    HandleOptionsScreenClick();
                }
                else if (hitObjectName == "Play Screen")
                {
                    HandlePlayScreenClick();
                }
            }
            else
            {
                Debug.Log("No object at center of screen.");
            }
        }
    }

    private void HandleLoadScreenClick()
    {
        if (!GameManager.Instance.isPaused && GameManager.Instance.tutorialComplete && !isLoading)
        {
            isLoading = true;
            saveSystem.ResetPlayer();
            saveSystem.SavePlayer();
            animator.enabled = true;
            animator.SetTrigger("Play");
            StartCoroutine(LoadLevel("Player Hub"));
        }
        else if (!GameManager.Instance.isPaused && !GameManager.Instance.tutorialComplete)
        {
            Debug.Log("Tutorial not complete");
        }
    }

    private void HandleOptionsScreenClick()
    {
        if (!GameManager.Instance.isPaused)
        {
            GameManager.Instance.optionsMain();
        }
    }

    private void HandlePlayScreenClick()
    {
        if (!GameManager.Instance.isPaused && !isLoading)
        {
            isLoading = true;
            animator.enabled = true;
            animator.SetTrigger("Play");
            StartCoroutine(LoadLevel("Player Hub"));
        }
    }

    IEnumerator LoadLevel(string sceneName)
    {
        yield return new WaitForSeconds(6);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(loadedScene);
    }
}
