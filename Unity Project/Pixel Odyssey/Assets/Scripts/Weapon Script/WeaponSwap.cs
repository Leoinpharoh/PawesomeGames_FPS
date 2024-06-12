using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    [SerializeField] GameObject weaponOne;
    [SerializeField] GameObject weaponTwo;
    [SerializeField] GameObject weaponThree;
    [SerializeField] GameObject weaponFour;

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
            redundancy();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && weaponTwo != null)
        {
            if (currentWeapon != null) { currentWeapon.SetActive(false); }
            weaponTwo.SetActive(true);
            currentWeapon = weaponTwo;
            redundancy();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponThree != null)
        {
            if (currentWeapon != null) { currentWeapon.SetActive(false); }
            weaponThree.SetActive(true);
            currentWeapon = weaponThree;
            redundancy();
        }if (Input.GetKeyDown(KeyCode.Alpha4) && weaponFour != null)
        {
            if (currentWeapon != null) { currentWeapon.SetActive(false); }
            weaponFour.SetActive(true);
            currentWeapon = weaponFour;
            redundancy();
        }
    }

    void redundancy()
    {
        ShootingHandler gunScript = currentWeapon.GetComponent<ShootingHandler>();
        gunScript.i = 0;
        gunScript.isShooting = false;
        gunScript.lineRenderer.enabled = false;
        if (gunScript.lineRenderer != null) { gunScript.lineRenderer.enabled = false; }
    }
}
