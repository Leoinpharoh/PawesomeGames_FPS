using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] WeaponStats weaponStats;
    [SerializeField] TMP_Text displayAmmoHere;

    int ammo;

    void Update()
    {
        if (weaponStats.Ammo + weaponStats.clip != ammo)
        {
            ammo = weaponStats.Ammo + weaponStats.clip;
            displayAmmoHere.text = ammo.ToString();
        }
    }
}
