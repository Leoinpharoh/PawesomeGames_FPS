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

    public int PythonAmmo = 0;
    public int ShotgunAmmo = 0;
    public int AssaultRifleAmmo = 0;
    public int RPGAmmo = 0;

    public int HealthMax = 100;
    public int OvershieldMax = 0;

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

    }
}
