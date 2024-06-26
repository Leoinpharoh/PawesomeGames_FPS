using System;
using System.Xml.Serialization;

[Serializable]
public class PlayerData
{
    public bool ShotgunUnlocked;
    public bool AssaultRifleUnlocked;
    public bool RPGUnlocked;
    public bool OvershieldUnlocked;
    public bool PotionbeltUnlocked;
    public bool TutorialComplete;

    public bool Scene1HPBoosterUnlocked;
    public bool Scene2HPBoosterUnlocked;
    public bool Scene3HPBoosterUnlocked;
    public bool Scene4HPBoosterUnlocked;
    public bool Scene5HPBoosterUnlocked;
    public bool Scene6HPBoosterUnlocked;

    public bool Scene1OSBoosterUnlocked;
    public bool Scene2OSBoosterUnlocked;
    public bool Scene3OSBoosterUnlocked;
    public bool Scene4OSBoosterUnlocked;
    public bool Scene5OSBoosterUnlocked;
    public bool Scene6OSBoosterUnlocked;

    public int PythonAmmo;
    public int ShotgunAmmo;
    public int AssaultRifleAmmo;
    public int RPGAmmo;

    public int HealthMax;
    public int OvershieldMax;

    public int OvershieldPotions;
    public int HealthPotions;
    public int CurePotions;
}