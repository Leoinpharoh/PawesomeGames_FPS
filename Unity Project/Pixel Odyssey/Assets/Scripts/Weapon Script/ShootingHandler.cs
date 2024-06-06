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
    [SerializeField] AudioSource fireAudioSource;
    [SerializeField] AudioSource reloadAudioSource;
    [SerializeField] LayerMask shootableLayer; 
    [SerializeField] GameObject projectileBullet;
    [SerializeField] Animator Anim;
    Vector3 dir;

    [Header("Gun Stats")]   // The stats of the gun.
    [SerializeField] int shootDmg;
    [SerializeField] int shootDist;
    [SerializeField] float shootSpeed;
    [SerializeField] int projectileSpeed;
    [SerializeField] int numberOfRays; // This can be higher so that the weapon can be a burst gun.
    enum WeaponType { RayCast, Laser, Projectile, SpreadRay }
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
            fireAudioSource.clip = audioSFXShoot;
            fireAudioSource.PlayOneShot(audioSFXShoot);
            Anim.SetBool("isShooting", true);

            // Apply ammo changes
            GameManager.Instance.playerAmmo(ammoType.ToString(), Ammo);
            clip--;
            GameManager.Instance.playerClip(clip);

            if (weaponType == WeaponType.Laser || weaponType == WeaponType.RayCast || weaponType == WeaponType.SpreadRay)
            {
                for (int i = 0; i < numberOfRays; i++) { rayDraw(); }   // Fire a bullet in a straight line.          
            }else if (weaponType == WeaponType.Projectile)
            {
                projectileBullet.GetComponent<playerBullet>().damage = shootDmg;
                projectileBullet.GetComponent<playerBullet>().speed = projectileSpeed;
                Instantiate(projectileBullet, firePoint.transform.position, transform.rotation);
            }
          // Turn off the laser.
            if(weaponType.ToString() == "Laser") { yield return new WaitForSeconds(LaserWaitTime); lineRenderer.enabled = false; }
        }
        // Toggle the player as no longer shooting.
        yield return new WaitForSeconds(shootSpeed);
        isShooting = false;
        Anim.SetBool("isShooting", false);
    }

    // Handles reloading the gun when the player runs out of bullets.
    IEnumerator reloading()
    {
        reloadAudioSource.clip = audioSFXReload;
        reloadAudioSource.Play();
        Anim.SetBool("isReloading", true);

        isShooting = true;
        yield return new WaitForSeconds(reloadTime);
        reloadAudioSource.Stop();
        if(Ammo == 0)
        {
           
        }
        else if (Ammo < TilReload) // If the player doesn't have the ammo to fill the clip
        {
            clip = Ammo + clip; // Fill the clip with the remaining ammo
            Ammo = 0; // Set the ammo to 0
        }
        else
        {
            Ammo = Ammo - (TilReload - clip);
            clip = TilReload;     
        }
        Anim.SetBool("isReloading", false);
        isShooting = false;
        GameManager.Instance.playerAmmo(ammoType.ToString(), Ammo);
    }

    private void rayDraw()
    {
        if (weaponType == WeaponType.SpreadRay) // Fire bullets in a randomized spread.
        {
            dir = Camera.main.transform.forward;
            Vector3 spread = Vector3.zero;
            spread += Camera.main.transform.up * Random.Range(-1f, 1f);
            spread += Camera.main.transform.right * Random.Range(-1f, 1f);
            dir += spread.normalized * Random.Range(0f, 0.2f);
        }
        else { dir = Camera.main.transform.forward; }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, dir, out hit, Mathf.Infinity, shootableLayer))
        {
            // Shows the laser that the player has fired.
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, firePoint.transform.position);
            lineRenderer.SetPosition(1, hit.point);

            // Handles the damage that the player deals to the enemy.
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null) { dmg.takeDamage(shootDmg, hit.point); }
        }
    }
}
