using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string nextLevelName; // The name of the next level
    [SerializeField] private Image levelFlashImage; // Reference to the Image for fading
    [SerializeField] private float fadeDuration = 1.0f; // Duration of the fade

    [SerializeField] public enum Unlockables { Shotgun, AssaultRifle, Rpg, ToolBelt, OverShield};
    public Unlockables unlockables;

    PlayerManager playerManager;

    
    private void Start()
    {
        if (levelFlashImage == null)
        {
            
        }
        else
        {
            Color color = levelFlashImage.color; // Get the color of the level flash image
            color.a = 0; // Set the alpha value to 0
            levelFlashImage.color = color; // Set the color of the level flash image
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            if(unlockables == Unlockables.Shotgun)
            {
                //unlock Shotgun
                PlayerPrefs.SetInt("ShotgunUnlocked", 1);
                GameManager.Instance.ActivatePlayerOS();
                //playerManager.osCheck();
                PlayerPrefs.Save();
                StartCoroutine(NextLevelCoroutine()); // Start the coroutine to load the next level
            }
            if (unlockables == Unlockables.AssaultRifle)
            {
                PlayerPrefs.SetInt("AssaultRifleUnlocked", 1);
                GameManager.Instance.ActivatePlayerOS();
                //playerManager.osCheck();
                PlayerPrefs.Save();
                StartCoroutine(NextLevelCoroutine()); // Start the coroutine to load the next level
            }
            if (unlockables == Unlockables.Rpg)
            {
                PlayerPrefs.SetInt("RPGUnlocked", 1);
                GameManager.Instance.ActivatePlayerOS();
                //playerManager.osCheck();
                PlayerPrefs.Save();
                StartCoroutine(NextLevelCoroutine()); // Start the coroutine to load the next level
            }
            if (unlockables == Unlockables.ToolBelt)
            {
                PlayerPrefs.SetInt("PotionBeltUnlocked", 1);
                GameManager.Instance.ActivatePlayerOS();
                //playerManager.osCheck();
                PlayerPrefs.Save();
                StartCoroutine(NextLevelCoroutine()); // Start the coroutine to load the next level
            }
            if (unlockables == Unlockables.OverShield)
            {
                PlayerPrefs.SetInt("OvershieldUnlocked", 1);
                GameManager.Instance.ActivatePlayerOS();
                //playerManager.osCheck();
                PlayerPrefs.Save();
                //unlock Shotgun
                StartCoroutine(NextLevelCoroutine()); // Start the coroutine to load the next level
            }

        }
    }

    private IEnumerator NextLevelCoroutine()
    {
        
        levelFlashImage.gameObject.SetActive(true); // Enable the level flash image
       
        yield return StartCoroutine(FadeToWhite()); // Fade to white

        yield return SceneManager.LoadSceneAsync(nextLevelName, LoadSceneMode.Single); // Load the next level

        Scene loadedScene = SceneManager.GetSceneByName(nextLevelName); // Get the loaded scene by name

        if (loadedScene.IsValid()) // Check if the scene is valid
        {
            SceneManager.SetActiveScene(loadedScene); // Set the scene as active
        }

        // Fade from white
        yield return StartCoroutine(FadeFromWhite()); // Fade from white
        levelFlashImage.gameObject.SetActive(false); // Disable the level flash image
        GameManager.Instance.statePaused(); // Pause the game

    }

    private IEnumerator FadeToWhite() // Fade the level flash image to white
    {
        float elapsedTime = 0f; // Elapsed time since the coroutine started
        Color color = levelFlashImage.color; // The color of the level flash image

        while (elapsedTime < fadeDuration) // While the elapsed time is less than the fade duration
        {
            elapsedTime += Time.deltaTime; // Increment the elapsed time by the time since the last frame
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Calculate the alpha value based on the elapsed time
            levelFlashImage.color = color; // Set the color of the level flash image
            yield return null; // Wait for the next frame
        }
    }

    private IEnumerator FadeFromWhite() // Fade the level flash image from white
    {
        float elapsedTime = 0f; // Elapsed time since the coroutine started
        Color color = levelFlashImage.color; // The color of the level flash image

        while (elapsedTime < fadeDuration) // While the elapsed time is less than the fade duration
        {
            elapsedTime += Time.deltaTime; // Increment the elapsed time by the time since the last frame
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration); // Calculate the alpha value based on the elapsed time
            levelFlashImage.color = color; // Set the color of the level flash image
            yield return null; // Wait for the next frame
        }
    }
}
