using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatPermaBoost : MonoBehaviour
{
    public SaveSystem saveSystem;
    public PlayerManager playerManager;
    public PlayerData playerData;
    Scene currentScene;

    bool Scene1HPBoosterUnlocked;
    bool Scene2HPBoosterUnlocked;
    bool Scene3HPBoosterUnlocked;
    bool Scene4HPBoosterUnlocked;
    bool Scene5HPBoosterUnlocked;
    bool Scene6HPBoosterUnlocked;

    bool Scene1OSBoosterUnlocked;
    bool Scene2OSBoosterUnlocked;
    bool Scene3OSBoosterUnlocked;
    bool Scene4OSBoosterUnlocked;
    bool Scene5OSBoosterUnlocked;
    bool Scene6OSBoosterUnlocked;

    string Booster;

    // Start is called before the first frame update
    [SerializeField] enum PickUpType { HealthPlus, OverShieldPlus }
    [SerializeField] PickUpType type;
    int boostHPAmount = 10;
    int boostOSAmount = 5;

    private void Awake()
    {
        saveSystem.LoadPlayer();
        playerData = saveSystem.playerData;
    }
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        StatBoosterCheck();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GrabbedBooster();
            switch (type)
            {

                case PickUpType.HealthPlus:
                    playerManager = other.gameObject.GetComponent<PlayerManager>();
                    playerManager.HPOrignal += boostHPAmount;
                    break;
                case PickUpType.OverShieldPlus:
                    playerManager = other.gameObject.GetComponent<PlayerManager>();
                    playerManager.OSOrignal += boostOSAmount;
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

        Scene1HPBoosterUnlocked = saveSystem.playerData.Scene1HPBoosterUnlocked;
        Scene2HPBoosterUnlocked = saveSystem.playerData.Scene2HPBoosterUnlocked;
        Scene3HPBoosterUnlocked = saveSystem.playerData.Scene3HPBoosterUnlocked;
        Scene4HPBoosterUnlocked = saveSystem.playerData.Scene4HPBoosterUnlocked;
        Scene5HPBoosterUnlocked = saveSystem.playerData.Scene5HPBoosterUnlocked;
        Scene6HPBoosterUnlocked = saveSystem.playerData.Scene6HPBoosterUnlocked;
        Scene1OSBoosterUnlocked = saveSystem.playerData.Scene1OSBoosterUnlocked;
        Scene2OSBoosterUnlocked = saveSystem.playerData.Scene2OSBoosterUnlocked;
        Scene3OSBoosterUnlocked = saveSystem.playerData.Scene3OSBoosterUnlocked;
        Scene4OSBoosterUnlocked = saveSystem.playerData.Scene4OSBoosterUnlocked;
        Scene5OSBoosterUnlocked = saveSystem.playerData.Scene5OSBoosterUnlocked;
        Scene6OSBoosterUnlocked = saveSystem.playerData.Scene6OSBoosterUnlocked;


        if (Scene1HPBoosterUnlocked == true && sceneName == "Scene1 - Dustin" && type == PickUpType.HealthPlus)
        {
            Destroy(gameObject);
        }
        if (Scene2HPBoosterUnlocked == true && sceneName == "Scene2 - Michael" && type == PickUpType.HealthPlus)
        {
            Destroy(gameObject);
        }
        if (Scene3HPBoosterUnlocked == true && sceneName == "Scene3 - Conner" && type == PickUpType.HealthPlus)
        {
            Destroy(gameObject);
        }
        if (Scene4HPBoosterUnlocked == true && sceneName == "Scene4 - Leo" && type == PickUpType.HealthPlus)
        {
            Destroy(gameObject);
        }
        if (Scene5HPBoosterUnlocked == true && sceneName == "Scene5 - Demetrice" && type == PickUpType.HealthPlus)
        {
            Destroy(gameObject);
        }
        if (Scene6HPBoosterUnlocked == true && sceneName == "Scene6 - Andrew" && type == PickUpType.HealthPlus)
        {
            Destroy(gameObject);
        }
        if (Scene1OSBoosterUnlocked == true && sceneName == "Scene1 - Dustin" && type == PickUpType.OverShieldPlus)
        {
            Destroy(gameObject);
        }
        if (Scene2OSBoosterUnlocked == true && sceneName == "Scene2 - Michael" && type == PickUpType.OverShieldPlus)
        {
            Destroy(gameObject);
        }
        if (Scene3OSBoosterUnlocked == true && sceneName == "Scene3 - Conner" && type == PickUpType.OverShieldPlus)
        {
            Destroy(gameObject);
        }
        if (Scene4OSBoosterUnlocked == true && sceneName == "Scene4 - Leo" && type == PickUpType.OverShieldPlus)
        {
            Destroy(gameObject);
        }
        if (Scene5OSBoosterUnlocked == true && sceneName == "Scene5 - Demetrice" && type == PickUpType.OverShieldPlus)
        {
            Destroy(gameObject);
        }
        if (Scene6OSBoosterUnlocked == true && sceneName == "Scene6 - Andrew" && type == PickUpType.OverShieldPlus)
        {
            Destroy(gameObject);
        }
    }

    public void GrabbedBooster()
    {
        string sceneName = currentScene.name;
        int HealthMax = saveSystem.playerData.HealthMax;
        int OvershieldMax = saveSystem.playerData.OvershieldMax;

        switch (type)
        {
            case PickUpType.HealthPlus:
                HealthMax = HealthMax + boostHPAmount;
                Booster = "Health";
                break;
            case PickUpType.OverShieldPlus:
                OvershieldMax = OvershieldMax + boostOSAmount;
                Booster = "OverShield";
                break;
        }


        if (Scene1HPBoosterUnlocked == false && sceneName == "Scene1 - Dustin" && Booster == "Health")
        {
            saveSystem.playerData.Scene1HPBoosterUnlocked = true;
            saveSystem.playerData.HealthMax = HealthMax;
            saveSystem.SavePlayer();
        }
        if (Scene2HPBoosterUnlocked == false && sceneName == "Scene2 - Michael" && Booster == "Health")
        {
            saveSystem.playerData.Scene2HPBoosterUnlocked = true;
            saveSystem.playerData.HealthMax = HealthMax;
            saveSystem.SavePlayer();
        }
        if (Scene3HPBoosterUnlocked == false && sceneName == "Scene3 - Conner" && Booster == "Health")
        {
            saveSystem.playerData.Scene3HPBoosterUnlocked = true;
            saveSystem.playerData.HealthMax = HealthMax;
            saveSystem.SavePlayer();
        }
        if (Scene4HPBoosterUnlocked == false && sceneName == "Scene4 - Leo" && Booster == "Health")
        {
            saveSystem.playerData.Scene4HPBoosterUnlocked = true;
            saveSystem.playerData.HealthMax = HealthMax;
            saveSystem.SavePlayer();
        }
        if (Scene5HPBoosterUnlocked == false && sceneName == "Scene5 - Demetrice" && Booster == "Health")
        {
            saveSystem.playerData.Scene5HPBoosterUnlocked = true;
            saveSystem.playerData.HealthMax = HealthMax;
            saveSystem.SavePlayer();
        }
        if (Scene6HPBoosterUnlocked == false && sceneName == "Scene6 - Andrew" && Booster == "Health")
        {
            saveSystem.playerData.Scene6HPBoosterUnlocked = true;
            saveSystem.playerData.HealthMax = HealthMax;
            saveSystem.SavePlayer();
        }
        if (Scene1OSBoosterUnlocked == false && sceneName == "Scene1 - Dustin" && Booster == "OverShield")
        {
            saveSystem.playerData.Scene1OSBoosterUnlocked = true;
            saveSystem.playerData.OvershieldMax = OvershieldMax;
            saveSystem.SavePlayer();
        }
        if (Scene2OSBoosterUnlocked == false && sceneName == "Scene2 - Michael" && Booster == "OverShield")
        {
            saveSystem.playerData.Scene2OSBoosterUnlocked = true;
            saveSystem.playerData.OvershieldMax = OvershieldMax;
            saveSystem.SavePlayer();
        }
        if (Scene3OSBoosterUnlocked == false && sceneName == "Scene3 - Conner" && Booster == "OverShield")
        {
            saveSystem.playerData.Scene3OSBoosterUnlocked = true;
            saveSystem.playerData.OvershieldMax = OvershieldMax;
            saveSystem.SavePlayer();
        }
        if (Scene4OSBoosterUnlocked == false && sceneName == "Scene4 - Leo" && Booster == "OverShield")
        {
            saveSystem.playerData.Scene4OSBoosterUnlocked = true;
            saveSystem.playerData.OvershieldMax = OvershieldMax;
            saveSystem.SavePlayer();
        }
        if (Scene5OSBoosterUnlocked == false && sceneName == "Scene5 - Demetrice" && Booster == "OverShield")
        {
            saveSystem.playerData.Scene5OSBoosterUnlocked = true;
            saveSystem.playerData.OvershieldMax = OvershieldMax;
            saveSystem.SavePlayer();
        }
        if (Scene6OSBoosterUnlocked == false && sceneName == "Scene6 - Andrew" && Booster == "OverShield")
        {
            saveSystem.playerData.Scene6OSBoosterUnlocked = true;
            saveSystem.playerData.OvershieldMax = OvershieldMax;
            saveSystem.SavePlayer();
        }
        playerManager.updatePlayerUI();
    }
}
