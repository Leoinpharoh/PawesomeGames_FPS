using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayAmmo : MonoBehaviour
{
    [SerializeField] WeaponStats weaponStats;
    [SerializeField] TMP_Text displayHere;
    private int Ammo;
    void Update()
    {
        AmmoAmountHandler();
    }

    public void AmmoAmountHandler()
    {
        if ((weaponStats.Ammo + weaponStats.clip) != Ammo)
        Ammo = weaponStats.Ammo + weaponStats.clip;
        displayHere.text = Ammo.ToString();
    }
}
