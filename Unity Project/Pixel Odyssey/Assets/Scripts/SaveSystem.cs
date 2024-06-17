using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public bool ShotgunUnlocked = false;
    public bool AssaultRifleUnlocked = false;
    public bool RPGUnlocked = false;
    public bool MeleeUnlocked = false;
    public bool OvershieldUnlocked = false;
    public bool PotionbeltUnlocked = false;
    public bool TutorialComplete = false;

    public bool Scene1HPBoosterUnlocked = false;
    public bool Scene2HPBoosterUnlocked = false;
    public bool Scene3HPBoosterUnlocked = false;
    public bool Scene4HPBoosterUnlocked = false;
    public bool Scene5HPBoosterUnlocked = false;
    public bool Scene6HPBoosterUnlocked = false;

    public bool Scene1OSBoosterUnlocked = false;
    public bool Scene2OSBoosterUnlocked = false;
    public bool Scene3OSBoosterUnlocked = false;
    public bool Scene4OSBoosterUnlocked = false;
    public bool Scene5OSBoosterUnlocked = false;
    public bool Scene6OSBoosterUnlocked = false;

    public int PythonAmmo = 0;
    public int ShotgunAmmo = 0;
    public int AssaultRifleAmmo = 0;
    public int RPGAmmo = 0;

    public int HealthMax = 140;
    public int OvershieldMax = 40;

    public int OvershieldPotions = 0;
    public int HealthPotions = 0;

    public int Currency = 0;

    public void SavePlayer()
    {
        PlayerPrefs.SetInt("Currency", Currency);
        PlayerPrefs.SetInt("PythonAmmo", PythonAmmo);
        PlayerPrefs.SetInt("ShotgunAmmo", ShotgunAmmo);
        PlayerPrefs.SetInt("AssaultRifleAmmo", AssaultRifleAmmo);
        PlayerPrefs.SetInt("RPGAmmo", RPGAmmo);
        PlayerPrefs.SetInt("HealthMax", HealthMax);
        PlayerPrefs.SetInt("OvershieldMax", OvershieldMax);
        PlayerPrefs.SetInt("OvershieldPotions", OvershieldPotions);
        PlayerPrefs.SetInt("HealthPotions", HealthPotions);
        PlayerPrefs.SetInt("TutorialComplete", TutorialComplete ? 1 : 0);
        PlayerPrefs.SetInt("ShotgunUnlocked", ShotgunUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("AssaultRifleUnlocked", AssaultRifleUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("RPGUnlocked", RPGUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("MeleeUnlocked", MeleeUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("OvershieldUnlocked", OvershieldUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("PotionbeltUnlocked", PotionbeltUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene1HPBooster", Scene1HPBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene2HPBooster", Scene2HPBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene3HPBooster", Scene3HPBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene4HPBooster", Scene4HPBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene5HPBooster", Scene5HPBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene6HPBooster", Scene6HPBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene1OSBooster", Scene1OSBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene2OSBooster", Scene2OSBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene3OSBooster", Scene3OSBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene4OSBooster", Scene4OSBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene5OSBooster", Scene5OSBoosterUnlocked ? 1 : 0);
        PlayerPrefs.SetInt("Scene6OSBooster", Scene6OSBoosterUnlocked ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadPlayer()
    {

        Currency = PlayerPrefs.GetInt("Currency");
        PythonAmmo = PlayerPrefs.GetInt("PythonAmmo");
        ShotgunAmmo = PlayerPrefs.GetInt("ShotgunAmmo");
        AssaultRifleAmmo = PlayerPrefs.GetInt("AssaultRifleAmmo");
        RPGAmmo = PlayerPrefs.GetInt("RPGAmmo");
        HealthMax = PlayerPrefs.GetInt("HealthMax");
        OvershieldMax = PlayerPrefs.GetInt("OvershieldMax");
        OvershieldPotions = PlayerPrefs.GetInt("OvershieldPotions");
        HealthPotions = PlayerPrefs.GetInt("HealthPotions");
        TutorialComplete = PlayerPrefs.GetInt("TutorialComplete") == 1;
        ShotgunUnlocked = PlayerPrefs.GetInt("ShotgunUnlocked") == 1;
        AssaultRifleUnlocked = PlayerPrefs.GetInt("AssaultRifleUnlocked") == 1;
        RPGUnlocked = PlayerPrefs.GetInt("RPGUnlocked") == 1;
        MeleeUnlocked = PlayerPrefs.GetInt("MeleeUnlocked") == 1;
        OvershieldUnlocked = PlayerPrefs.GetInt("OvershieldUnlocked") == 1;
        PotionbeltUnlocked = PlayerPrefs.GetInt("PotionbeltUnlocked") == 1;
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

    }

    public void ResetPlayer()
    {
        Currency = 0;
        PythonAmmo = 50;
        ShotgunAmmo = 0;
        AssaultRifleAmmo = 0;
        RPGAmmo = 0;
        HealthMax = 100;
        OvershieldMax = 0;
        OvershieldPotions = 0;
        HealthPotions = 0;
        TutorialComplete = false;
        ShotgunUnlocked = false;
        AssaultRifleUnlocked = false;
        RPGUnlocked = false;
        MeleeUnlocked = false;
        OvershieldUnlocked = false;
        PotionbeltUnlocked = false;
    }
}
