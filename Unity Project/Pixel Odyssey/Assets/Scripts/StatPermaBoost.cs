using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatPermaBoost : MonoBehaviour
{
    SaveSystem saveSystem;
    Scene currentScene;

    bool Scene1HPBoosterUnlocked = false;
    bool Scene2HPBoosterUnlocked = false;
    bool Scene3HPBoosterUnlocked = false;
    bool Scene4HPBoosterUnlocked = false;
    bool Scene5HPBoosterUnlocked = false;
    bool Scene6HPBoosterUnlocked = false;

    bool Scene1OSBoosterUnlocked = false;
    bool Scene2OSBoosterUnlocked = false;
    bool Scene3OSBoosterUnlocked = false;
    bool Scene4OSBoosterUnlocked = false;
    bool Scene5OSBoosterUnlocked = false;
    bool Scene6OSBoosterUnlocked = false;

    string Booster;

    // Start is called before the first frame update
    [SerializeField] enum PickUpType { HealthPlus, OverShieldPlus }
    [SerializeField] PickUpType type;
    int boostAmount = 10;

    private void Awake()
    {
        StatBoosterCheck();
    }
    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        //StatBoosterCheck();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GrabbedBooster();
            switch (type)
            {
                case PickUpType.HealthPlus:

                    PlayerPrefs.SetInt("HealthMax", other.gameObject.GetComponent<PlayerManager>().HPOrignal += boostAmount);
                    //other.gameObject.GetComponent<PlayerManager>().HP += boostAmount;
                    //other.gameObject.GetComponent<PlayerManager>().HPOrignal += boostAmount;
                    break;
                case PickUpType.OverShieldPlus:
                    PlayerPrefs.SetInt("OverShieldMax", other.gameObject.GetComponent<PlayerManager>().OSOrignal += boostAmount);
                    //other.gameObject.GetComponent<PlayerManager>().OS += boostAmount;
                    //other.gameObject.GetComponent<PlayerManager>().OSOrignal += boostAmount;
                    break;
            }
            StartCoroutine(DestroyAfterDelay(0.5f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void StatBoosterCheck()
    {
        string sceneName = currentScene.name;

        Scene1HPBoosterUnlocked = PlayerPrefs.GetInt("Scene1HPBoosterUnlocked") == 1;
        Scene2HPBoosterUnlocked = PlayerPrefs.GetInt("Scene2HPBoosterUnlocked") == 1;
        Scene3HPBoosterUnlocked = PlayerPrefs.GetInt("Scene3HPBoosterUnlocked") == 1;
        Scene4HPBoosterUnlocked = PlayerPrefs.GetInt("Scene4HPBoosterUnlocked") == 1;
        Scene5HPBoosterUnlocked = PlayerPrefs.GetInt("Scene5HPBoosterUnlocked") == 1;
        Scene6HPBoosterUnlocked = PlayerPrefs.GetInt("Scene6HPBoosterUnlocked") == 1;

        Scene1OSBoosterUnlocked = PlayerPrefs.GetInt("Scene1OSBoosterUnlocked") == 1;
        Scene2OSBoosterUnlocked = PlayerPrefs.GetInt("Scene2OSBoosterUnlocked") == 1;
        Scene3OSBoosterUnlocked = PlayerPrefs.GetInt("Scene3OSBoosterUnlocked") == 1;
        Scene4OSBoosterUnlocked = PlayerPrefs.GetInt("Scene4OSBoosterUnlocked") == 1;
        Scene5OSBoosterUnlocked = PlayerPrefs.GetInt("Scene5OSBoosterUnlocked") == 1;
        Scene6OSBoosterUnlocked = PlayerPrefs.GetInt("Scene6OSBoosterUnlocked") == 1;


        if (Scene1HPBoosterUnlocked == true && sceneName == "Scene1 - Dustin")
        {
            Destroy(gameObject);
        }
        if (Scene2HPBoosterUnlocked == true && sceneName == "Scene2 - Michael")
        {
            Destroy(gameObject);
        }
        if (Scene3HPBoosterUnlocked == true && sceneName == "Scene3 - Conner")
        {
            Destroy(gameObject);
        }
        if (Scene4HPBoosterUnlocked == true && sceneName == "Scene4 - Leo")
        {
            Destroy(gameObject);
        }
        if (Scene5HPBoosterUnlocked == true && sceneName == "Scene5 - Demetrice")
        {
            Destroy(gameObject);
        }
        if (Scene6HPBoosterUnlocked == true && sceneName == "Scene6 - Andrew")
        {
            Destroy(gameObject);
        }
        if (Scene1OSBoosterUnlocked == true && sceneName == "Scene1 - Dustin")
        {
            Destroy(gameObject);
        }
        if (Scene2OSBoosterUnlocked == true && sceneName == "Scene2 - Michael")
        {
            Destroy(gameObject);
        }
        if (Scene3OSBoosterUnlocked == true && sceneName == "Scene3 - Conner")
        {
            Destroy(gameObject);
        }
        if (Scene4OSBoosterUnlocked == true && sceneName == "Scene4 - Leo")
        {
            Destroy(gameObject);
        }
        if (Scene5OSBoosterUnlocked == true && sceneName == "Scene5 - Demetrice")
        {
            Destroy(gameObject);
        }
        if (Scene6OSBoosterUnlocked == true && sceneName == "Scene6 - Andrew")
        {
            Destroy(gameObject);
        }
    }

    public void GrabbedBooster()
    {
        string sceneName = currentScene.name;
        int HealthMax = PlayerPrefs.GetInt("HealthMax");
        int OvershieldMax = PlayerPrefs.GetInt("OvershieldMax");

        switch (type)
        {
            case PickUpType.HealthPlus:
                Debug.Log("Scene 6 HP Booster");
                HealthMax = HealthMax + boostAmount;
                Booster = "Health";
                break;
            case PickUpType.OverShieldPlus:
                OvershieldMax = OvershieldMax + boostAmount;
                Booster = "OverShield";
                break;
        }


        if (Scene1HPBoosterUnlocked == false && sceneName == "Scene1 - Dustin" && Booster == "Health")
        {
            PlayerPrefs.SetInt("Scene1HPBooster", 1);
            PlayerPrefs.SetInt("HealthMax", HealthMax);
            PlayerPrefs.Save();
        }
        if (Scene2HPBoosterUnlocked == false && sceneName == "Scene2 - Michael" && Booster == "Health")
        {
            PlayerPrefs.SetInt("Scene2HPBooster",1);
            PlayerPrefs.SetInt("HealthMax", HealthMax);
            PlayerPrefs.Save();
        }
        if (Scene3HPBoosterUnlocked == false && sceneName == "Scene3 - Conner" && Booster == "Health")
        {
            PlayerPrefs.SetInt("Scene3HPBooster", 1);
            PlayerPrefs.SetInt("HealthMax", HealthMax);
            PlayerPrefs.Save();
        }
        if (Scene4HPBoosterUnlocked == false && sceneName == "Scene4 - Leo" && Booster == "Health")
        {
            PlayerPrefs.SetInt("Scene4HPBooster", 1);
            PlayerPrefs.SetInt("HealthMax", HealthMax);
            PlayerPrefs.Save();
        }
        if (Scene5HPBoosterUnlocked == false && sceneName == "Scene5 - Demetrice" && Booster == "Health")
        {
            PlayerPrefs.SetInt("Scene5HPBooster", 1);
            PlayerPrefs.SetInt("HealthMax", HealthMax);
            PlayerPrefs.Save();
        }
        if (Scene6HPBoosterUnlocked == false && sceneName == "Scene6 - Andrew" && Booster == "Health")
        {
            Debug.Log("Scene 6 HP Booster Saving");
            PlayerPrefs.SetInt("Scene6HPBooster", 1);
            PlayerPrefs.SetInt("HealthMax", HealthMax);
            PlayerPrefs.Save();
        }
        if (Scene1OSBoosterUnlocked == false && sceneName == "Scene1 - Dustin" && Booster == "OverShield")
        {
            PlayerPrefs.SetInt("Scene1OSBooster", 1);
            PlayerPrefs.SetInt("OverShieldMax", OvershieldMax);
            PlayerPrefs.Save();
        }
        if (Scene2OSBoosterUnlocked == false && sceneName == "Scene2 - Michael" && Booster == "OverShield")
        {
            PlayerPrefs.SetInt("Scene2OSBooster", 1);
            PlayerPrefs.SetInt("OverShieldMax", OvershieldMax);
            PlayerPrefs.Save();
        }
        if (Scene3OSBoosterUnlocked == false && sceneName == "Scene3 - Conner" && Booster == "OverShield")
        {
            PlayerPrefs.SetInt("Scene3OSBooster", 1);
            PlayerPrefs.SetInt("OverShieldMax", OvershieldMax);
            PlayerPrefs.Save();
        }
        if (Scene4OSBoosterUnlocked == false && sceneName == "Scene4 - Leo" && Booster == "OverShield")
        {
            PlayerPrefs.SetInt("Scene4OSBooster", 1);
            PlayerPrefs.SetInt("OverShieldMax", OvershieldMax);
            PlayerPrefs.Save();
        }
        if (Scene5OSBoosterUnlocked == false && sceneName == "Scene5 - Demetrice" && Booster == "OverShield")
        {
            PlayerPrefs.SetInt("Scene5OSBooster", 1);
            PlayerPrefs.SetInt("OverShieldMax", OvershieldMax);
            PlayerPrefs.Save();
        }
        if (Scene6OSBoosterUnlocked == false && sceneName == "Scene6 - Andrew" && Booster == "OverShield")
        {
            PlayerPrefs.SetInt("Scene6OSBooster", 1);
            PlayerPrefs.SetInt("OverShieldMax", OvershieldMax);
            PlayerPrefs.Save();
        }
    }
}
