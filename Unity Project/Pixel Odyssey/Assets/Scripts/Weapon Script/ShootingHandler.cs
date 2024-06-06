using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingHandler : MonoBehaviour
{
    public WeaponStats weaponStats;

    [Header("Outside References")]
    public LineRenderer lineRenderer;
    [SerializeField] GameObject firePoint;
    [SerializeField] AudioSource fireAudioSource;
    [SerializeField] AudioSource reloadAudioSource;
    [SerializeField] LayerMask shootableLayer;
    [SerializeField] GameObject projectileBullet;
    [SerializeField] Animator Anim;
    Vector3 dir;

    [HideInInspector] public int i = 0;
    [HideInInspector] public bool isShooting;
    [HideInInspector] public int Ammo;

    private void Start()
    {
        weaponStats.clip = weaponStats.TilReload;
        weaponStats.Ammo = weaponStats.Ammo - weaponStats.TilReload;
    }

    private void Update()
    {
        // If game is unpaused, execute the code below
        if (!GameManager.Instance.isPaused)
        {
            shoot();

            // This is used to ensure that the correct ammo count is displayed.
            if (i == 0)
            {
                GameManager.Instance.playerAmmo(weaponStats.ammoType.ToString(), weaponStats.Ammo);
                i = 1;
            }

            GameManager.Instance.playerClip(weaponStats.clip);

            // Handles the input of the player reloading the gun.
            if (Input.GetKeyDown(KeyCode.R) && !isShooting) { StartCoroutine(reloading()); }
        }
    }

    void shoot()
    {
        // Handles the input of the player to fire the gun.
        if (Input.GetButtonDown("Fire1") && !isShooting) { StartCoroutine(shooting()); }
    }

    IEnumerator shooting()
    {
        if (!isShooting && GameManager.Instance.isPaused == false && weaponStats.clip != 0)
        {
            // Play audio and mark that the player is shooting.
            isShooting = true;
            fireAudioSource.clip = weaponStats.audioSFXShoot;
            fireAudioSource.PlayOneShot(weaponStats.audioSFXShoot);
            Anim.SetBool("isShooting", true);

            // Apply ammo changes
            GameManager.Instance.playerAmmo(weaponStats.ammoType.ToString(), weaponStats.Ammo);
            weaponStats.clip--;
            GameManager.Instance.playerClip(weaponStats.clip);

            if (weaponStats.weaponType == WeaponStats.WeaponType.Laser || weaponStats.weaponType == WeaponStats.WeaponType.RayCast || weaponStats.weaponType == WeaponStats.WeaponType.SpreadRay)
            {
                for (int i = 0; i < weaponStats.numberOfRays; i++) { rayDraw(); } // Fire a bullet in a straight line.
            }
            else if (weaponStats.weaponType == WeaponStats.WeaponType.Projectile)
            {
                projectileBullet.GetComponent<playerBullet>().damage = weaponStats.shootDamage;
                projectileBullet.GetComponent<playerBullet>().speed = weaponStats.projectileSpeed;
                Instantiate(projectileBullet, firePoint.transform.position, transform.rotation);
            }

            // Turn off the laser.
            if (weaponStats.weaponType.ToString() == "Laser")
            {
                yield return new WaitForSeconds(weaponStats.LaserWaitTime);
                lineRenderer.enabled = false;
            }
        }

        // Toggle the player as no longer shooting.
        yield return new WaitForSeconds(weaponStats.shootSpeed);
        isShooting = false;
        Anim.SetBool("isShooting", false);
    }

    // Handles reloading the gun when the player runs out of bullets.
    IEnumerator reloading()
    {
        reloadAudioSource.clip = weaponStats.audioSFXReload;
        reloadAudioSource.Play();
        Anim.SetBool("isReloading", true);

        isShooting = true;
        yield return new WaitForSeconds(weaponStats.reloadTime);
        reloadAudioSource.Stop();

        if (weaponStats.Ammo == 0)
        {
            // No action needed if Ammo is already zero
        }
        else if (weaponStats.Ammo < weaponStats.TilReload) // If the player doesn't have enough ammo to fill the clip
        {
            weaponStats.clip = weaponStats.Ammo + weaponStats.clip; // Fill the clip with the remaining ammo
            weaponStats.Ammo = 0; // Set the ammo to zero
        }
        else
        {
            weaponStats.Ammo = weaponStats.Ammo - (weaponStats.TilReload - weaponStats.clip);
            weaponStats.clip = weaponStats.TilReload;
        }

        Anim.SetBool("isReloading", false);
        isShooting = false;
        GameManager.Instance.playerAmmo(weaponStats.ammoType.ToString(), weaponStats.Ammo);
    }

    private void rayDraw()
    {
        if (weaponStats.weaponType == WeaponStats.WeaponType.SpreadRay) // Fire bullets in a randomized spread.
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
            if (dmg != null) { dmg.takeDamage(weaponStats.shootDamage, hit.point); }
        }
    }
}
