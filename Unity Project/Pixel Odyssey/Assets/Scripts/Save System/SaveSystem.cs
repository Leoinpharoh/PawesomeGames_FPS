using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public PlayerData playerData = new PlayerData();

    private string GetFilePath()
    {
        // Ensure the directory exists
        string folderPath = Path.Combine(Application.dataPath, "Scripts/Save System");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return Path.Combine(folderPath, "PlayerSave.xml");
    }

    public void SavePlayer()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
        string filePath = GetFilePath();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, playerData);
        }
    }

    public void LoadPlayer()
    {
        string path = GetFilePath();
        if (File.Exists(path))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                playerData = serializer.Deserialize(stream) as PlayerData;
            }
        }
        else
        {
            ResetPlayer();
            SavePlayer();
        }
    }

    public void ResetPlayer()
    {
        playerData = new PlayerData
        {
            PythonAmmo = 30,
            ShotgunAmmo = 0,
            AssaultRifleAmmo = 0,
            RPGAmmo = 0,
            HealthMax = 140,
            OvershieldMax = 10,
            OvershieldPotions = 0,
            HealthPotions = 0,
            TutorialComplete = false,
            ShotgunUnlocked = false,
            AssaultRifleUnlocked = false,
            RPGUnlocked = false,
            MeleeUnlocked = false,
            OvershieldUnlocked = false,
            PotionbeltUnlocked = false,
            Scene1HPBoosterUnlocked = false,
            Scene2HPBoosterUnlocked = false,
            Scene3HPBoosterUnlocked = false,
            Scene4HPBoosterUnlocked = false,
            Scene5HPBoosterUnlocked = false,
            Scene6HPBoosterUnlocked = false,
            Scene1OSBoosterUnlocked = false,
            Scene2OSBoosterUnlocked = false,
            Scene3OSBoosterUnlocked = false,
            Scene4OSBoosterUnlocked = false,
            Scene5OSBoosterUnlocked = false,
            Scene6OSBoosterUnlocked = false
        };
    }

    public void UnlockShotgun()
    {
        playerData.ShotgunUnlocked = true;
        SavePlayer();
    }

    public void UnlockAssaultRifle()
    {
        playerData.AssaultRifleUnlocked = true;
        SavePlayer();
    }

    public void UnlockRPG()
    {
        playerData.RPGUnlocked = true;
        SavePlayer();
    }

    public void UnlockMelee()
    {
        playerData.MeleeUnlocked = true;
        SavePlayer();
    }

    public void UnlockOvershield()
    {
        playerData.OvershieldUnlocked = true;
        SavePlayer();
    }

    public void UnlockPotionbelt()
    {
        playerData.PotionbeltUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene1HPBooster()
    {
        playerData.Scene1HPBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene2HPBooster()
    {
        playerData.Scene2HPBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene3HPBooster()
    {
        playerData.Scene3HPBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene4HPBooster()
    {
        playerData.Scene4HPBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene5HPBooster()
    {
        playerData.Scene5HPBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene6HPBooster()
    {
        playerData.Scene6HPBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene1OSBooster()
    {
        playerData.Scene1OSBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene2OSBooster()
    {
        playerData.Scene2OSBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene3OSBooster()
    {
        playerData.Scene3OSBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene4OSBooster()
    {
        playerData.Scene4OSBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene5OSBooster()
    {
        playerData.Scene5OSBoosterUnlocked = true;
        SavePlayer();
    }

    public void UnlockScene6OSBooster()
    {
        playerData.Scene6OSBoosterUnlocked = true;
        SavePlayer();
    }
}