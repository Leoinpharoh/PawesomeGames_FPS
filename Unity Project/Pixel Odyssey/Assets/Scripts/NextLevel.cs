using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string nextLevelName; // The name of the next level
    [SerializeField] private Image levelFlashImage; // Reference to the Image for fading
    [SerializeField] private float fadeDuration = 1.0f; // Duration of the fade

    private void Start()
    {
        if (levelFlashImage == null)
        {
            Debug.LogError("LevelFlash Image is not assigned.");
        }
        else
        {
            // Ensure the image is transparent at the start
            Color color = levelFlashImage.color;
            color.a = 0;
            levelFlashImage.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(NextLevelCoroutine());
        }
    }

    private IEnumerator NextLevelCoroutine()
    {
        
        levelFlashImage.gameObject.SetActive(true);
        // Fade to white
        yield return StartCoroutine(FadeToWhite());

        // Load the scene asynchronously
        yield return SceneManager.LoadSceneAsync(nextLevelName, LoadSceneMode.Single);

        Scene loadedScene = SceneManager.GetSceneByName(nextLevelName);

        if (loadedScene.IsValid())
        {
            Debug.Log("Setting active scene: " + nextLevelName);
            SceneManager.SetActiveScene(loadedScene);
        }
        else
        {
            Debug.LogError("Failed to load the scene: " + nextLevelName);
        }

        // Fade from white
        yield return StartCoroutine(FadeFromWhite());
        levelFlashImage.gameObject.SetActive(false);
        GameManager.Instance.statePaused();

    }

    private IEnumerator FadeToWhite()
    {
        float elapsedTime = 0f;
        Color color = levelFlashImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            levelFlashImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeFromWhite()
    {
        float elapsedTime = 0f;
        Color color = levelFlashImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            levelFlashImage.color = color;
            yield return null;
        }
    }
}
