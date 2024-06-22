using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] WeaponStats weaponStats;
    [SerializeField] TMP_Text displayAmmoHere;

    int ammo;

    private void Start()
    {
        
    }
    void Update()
    {
        if (weaponStats.Ammo + weaponStats.clip != ammo)
        {
            ammo = weaponStats.Ammo;
            displayAmmoHere.text = ammo.ToString();
        }
    }
}
