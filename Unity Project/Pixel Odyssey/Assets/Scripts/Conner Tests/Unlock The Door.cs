using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class UnlockTheDoor : MonoBehaviour
{
    [SerializeField] SaveSystem saveSystem;
    void Start()
    {
        if (saveSystem.playerData.PotionbeltUnlocked == true) { Destroy(this.gameObject); }
    }
}
