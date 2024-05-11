using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    [SerializeField] GameObject weaponOne;
    [SerializeField] GameObject weaponTwo;
    [SerializeField] GameObject weaponThree;

    GameObject currentWeapon;
    void Update()
    {
        SwapWeapon();
    }

    void SwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weaponOne != null)
        {
            if (currentWeapon != null) {currentWeapon.SetActive(false);}
            weaponOne.SetActive(true);
            currentWeapon = weaponOne;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && weaponTwo != null)
        {
            if (currentWeapon != null) { currentWeapon.SetActive(false); }
            weaponTwo.SetActive(true);
            currentWeapon = weaponOne;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponThree != null)
        {
            if (currentWeapon != null) { currentWeapon.SetActive(false); }
            weaponThree.SetActive(true);
            currentWeapon = weaponOne;
        }
    }
}
