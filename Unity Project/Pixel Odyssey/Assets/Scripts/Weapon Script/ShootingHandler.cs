using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingHandler : MonoBehaviour
{
    [Header("Outside Referances")]  // Outside references.
    public LineRenderer lineRenderer;
    [SerializeField] GameObject firePoint;
    [SerializeField] AudioSource audioSource;
    [SerializeField] LayerMask shootableLayer; 
    [SerializeField] GameObject projectileBullet;

    [Header("Gun Stats")]   // The stats of the gun.
    [SerializeField] int shootDmg;
    [SerializeField] int shootDist;
    [SerializeField] float shootSpeed;
    enum WeaponType { RayCast, Laser, Projectile }
    [SerializeField] WeaponType weaponType;

    [Header("Ammo Stats")] // Anything having to do with the weapons ammo.
    public int Ammo;
    public int clip;
    [SerializeField] int TilReload;
    [SerializeField] float reloadTime;
    [SerializeField] float LaserWaitTime;
    [HideInInspector] public enum AmmoType { Light, Medium, Heavy }
    public AmmoType ammoType;

    [Header("Audio Files")] // Anything to do with Audio Files except for the AudioSource, that's in "Outside Referances"
    [SerializeField] AudioClip audioSFXShoot;
    [SerializeField] AudioClip audioSFXReload;

    
    [HideInInspector]public int i = 0;
    [HideInInspector] public bool isShooting;

    private void Start()
    {
        clip = TilReload;
        Ammo = Ammo - TilReload;
    }
    private void Update()
    {
        //if game is unpaused all the code below
        if (!GameManager.Instance.isPaused)
        {
            shoot();

            // This is used to ensure that the correct ammo count is displayed.
            if (i == 0) { GameManager.Instance.playerAmmo(ammoType.ToString(), (Ammo)); i = 1; }

            GameManager.Instance.playerClip(clip);

            // Handles the Input of the player reloading the gun.
            if (Input.GetKeyDown(KeyCode.R) && !isShooting) { StartCoroutine(reloading()); }
        }
    }
    void shoot() 
    { 
        // Handles the Input of the player to fire the gun.
        if (Input.GetButtonDown("Fire1") && !isShooting) { StartCoroutine(shooting());}
    }



    IEnumerator shooting()
    {
        if (!isShooting && GameManager.Instance.isPaused == false && clip != 0)
        {

            // Play audio and mark that the player is shooting.
            isShooting = true;
            audioSource.clip = audioSFXShoot;
            audioSource.Play();

            // Apply ammo changes
            GameManager.Instance.playerAmmo(ammoType.ToString(), Ammo);
            clip--;
            GameManager.Instance.playerClip(clip);

            if (weaponType == WeaponType.Laser || weaponType == WeaponType.RayCast)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, shootableLayer))
                {
                    // Shows the laser that the player has fired.
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, firePoint.transform.position);
                    lineRenderer.SetPosition(1, hit.point);

                    // Handles the damage that the player deals to the enemy.
                    IDamage dmg = hit.collider.GetComponent<IDamage>();
                    Debug.Log(hit.transform.name);
                    if (dmg != null) { dmg.takeDamage(shootDmg, hit.point); }
                }
            }else if (weaponType == WeaponType.Projectile)
            {
                Instantiate(projectileBullet, firePoint.transform.position, transform.rotation);
            }
          // Turn off the laser.
            if(weaponType.ToString() == "Laser") { yield return new WaitForSeconds(LaserWaitTime); lineRenderer.enabled = false; }
        }
        // Toggle the player as no longer shooting.
        yield return new WaitForSeconds(shootSpeed);
        isShooting = false;
    }

    // Handles reloading the gun when the player runs out of bullets.
    IEnumerator reloading()
    {
        audioSource.clip = audioSFXReload;
        audioSource.Play();

        isShooting = true;
        yield return new WaitForSeconds(reloadTime);
        audioSource.Stop();
        if (Ammo < TilReload) // If the player doesn't have the ammo to fill the clip
        {
            clip = Ammo; // Fill the clip with the remaining ammo
            Ammo = 0; // Set the ammo to 0
        }
        else
        {
            Ammo = Ammo - (TilReload - clip);
            clip = TilReload;     
        }
        isShooting = false;
        GameManager.Instance.playerAmmo(ammoType.ToString(), Ammo);
    }


}
