using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    [SerializeField] GameObject weaponOne;
    [SerializeField] GameObject weaponTwo;
    [SerializeField] GameObject weaponThree;
    [SerializeField] GameObject weaponFour;
    PlayerManager playerManager;


    GameObject currentWeapon;
    void Update()
    {
        SwapWeapon();
    }

    void SwapWeapon()
    {
        //shotgunUnlocked = playerManager.shotgunUnlocked;
        //assaultRifleUnlocked = playerManager.assaultRifleUnlocked;
        //rpgUnlocked = playerManager.RPGUnlocked;


        if (Input.GetKeyDown(KeyCode.Alpha1) && weaponOne != null)
        {
            if (currentWeapon != null) {currentWeapon.SetActive(false);}
            weaponOne.SetActive(true);
            currentWeapon = weaponOne;
            redundancy();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && weaponTwo != null && GameManager.Instance.playerManager.shotgunUnlocked)
        {
            if (currentWeapon != null) { currentWeapon.SetActive(false); }
            weaponTwo.SetActive(true);
            currentWeapon = weaponTwo;
            redundancy();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponThree != null && GameManager.Instance.playerManager.assaultRifleUnlocked)
        {
            if (currentWeapon != null) { currentWeapon.SetActive(false); }
            weaponThree.SetActive(true);
            currentWeapon = weaponThree;
            redundancy();
        }if (Input.GetKeyDown(KeyCode.Alpha4) && weaponFour != null && GameManager.Instance.playerManager.RPGUnlocked)
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
        gunScript.isShooting = false;
        gunScript.lineRenderer.enabled = false;
        if (gunScript.lineRenderer != null) { gunScript.lineRenderer.enabled = false; }
    }
}
