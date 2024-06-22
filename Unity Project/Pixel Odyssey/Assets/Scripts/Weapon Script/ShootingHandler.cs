using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingHandler : MonoBehaviour
{
    // Public variables
    public WeaponStats weaponStats;
    public LineRenderer lineRenderer;
    [Header("Outside References")]
    [SerializeField] GameObject firePoint;
    [SerializeField] AudioSource fireAudioSource;
    [SerializeField] AudioSource reloadAudioSource;
    [SerializeField] LayerMask shootableLayer;
    [SerializeField] GameObject projectileBullet;
    [SerializeField] Animator Anim;
    [SerializeField] GameObject hitParticleEffect;

    // Private variables
    private Vector3 dir;
    private int i = 0;
    [HideInInspector] public bool isShooting;
    [HideInInspector]public int Ammo;

    private void Start()
    {
        weaponStats.clip = weaponStats.TilReload;
        Ammo = weaponStats.Ammo;
    }

    private void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            HandleShooting();
            UpdateAmmoDisplay();

            if (Input.GetKeyDown(KeyCode.R) && !isShooting)
            {
                StartCoroutine(Reload());
            }
        }
    }

    private void HandleShooting()
    {
        if (weaponStats.weaponType == WeaponStats.WeaponType.Automatic)
        {
            if (Input.GetButton("Fire1") && !isShooting)
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && !isShooting)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private void UpdateAmmoDisplay()
    {
        if (i == 0)
        {
            GameManager.Instance.playerAmmo(weaponStats.ammoType.ToString(), weaponStats.Ammo);
            i = 1;
        }

        GameManager.Instance.playerClip(weaponStats.clip);
    }

    private IEnumerator Shoot()
    {
        if (!isShooting && !GameManager.Instance.isPaused && weaponStats.clip != 0)
        {
            isShooting = true;
            fireAudioSource.PlayOneShot(weaponStats.audioSFXShoot);
            Anim.SetBool("isShooting", true);
            weaponStats.clip--;
            GameManager.Instance.playerClip(weaponStats.clip);

            if (weaponStats.weaponType == WeaponStats.WeaponType.Projectile)
            {
                FireProjectile();
            }
            else
            {
                FireRays();
            }

            yield return new WaitForSeconds(weaponStats.shootSpeed);
            isShooting = false;
            Anim.SetBool("isShooting", false);
        }
    }

    private void FireProjectile()
    {
        var bullet = Instantiate(projectileBullet, firePoint.transform.position, transform.rotation);
        var bulletComponent = bullet.GetComponent<playerBullet>();
        bulletComponent.damage = weaponStats.shootDamage;
        bulletComponent.speed = weaponStats.projectileSpeed;
    }

    private void FireRays()
    {
        for (int i = 0; i < weaponStats.numberOfRays; i++)
        {
            DrawRay();
        }

        if (weaponStats.weaponType == WeaponStats.WeaponType.Laser)
        {
            StartCoroutine(DisableLaserAfterDelay());
        }
    }

    private IEnumerator DisableLaserAfterDelay()
    {
        yield return new WaitForSeconds(weaponStats.LaserWaitTime);
        lineRenderer.enabled = false;
    }

    private IEnumerator Reload()
    {
        reloadAudioSource.PlayOneShot(weaponStats.audioSFXReload);
        Anim.SetBool("isReloading", true);
        isShooting = true;

        yield return new WaitForSeconds(weaponStats.reloadTime);

        UpdateAmmoAfterReload();
        reloadAudioSource.Stop();
        Anim.SetBool("isReloading", false);
        isShooting = false;

        GameManager.Instance.playerAmmo(weaponStats.ammoType.ToString(), weaponStats.Ammo);
    }

    private void UpdateAmmoAfterReload()
    {
        if (weaponStats.Ammo == 0) return;

        if (weaponStats.Ammo < weaponStats.TilReload)
        {
            weaponStats.clip += weaponStats.Ammo;
            weaponStats.Ammo = 0;
        }
        else
        {
            weaponStats.Ammo -= (weaponStats.TilReload - weaponStats.clip);
            weaponStats.clip = weaponStats.TilReload;
        }
    }

    private void DrawRay()
    {
        if (weaponStats.weaponType == WeaponStats.WeaponType.SpreadRay)
        {
            dir = GetSpreadDirection();
        }
        else
        {
            dir = Camera.main.transform.forward;
        }

        if (Physics.Raycast(Camera.main.transform.position, dir, out RaycastHit hit, Mathf.Infinity, shootableLayer))
        {
            DisplayRay(hit);

            var damageable = hit.collider.GetComponent<IDamage>();
            var mDamageable = hit.collider.GetComponent<MDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(weaponStats.shootDamage, hit.point);
            }
            else if (mDamageable != null && weaponStats.weaponType == WeaponStats.WeaponType.Automatic)
            {

                mDamageable.ObjectDamage(weaponStats.shootDamage);
            }
            else
            {
                Instantiate(hitParticleEffect, hit.point, Quaternion.identity);
            }
        }
    }

    private Vector3 GetSpreadDirection()
    {
        var direction = Camera.main.transform.forward;
        direction += Camera.main.transform.up * Random.Range(-0.1f, 0.1f);
        direction += Camera.main.transform.right * Random.Range(-0.1f, 0.1f);
        return direction.normalized;
    }

    private void DisplayRay(RaycastHit hit)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.transform.position);
        lineRenderer.SetPosition(1, hit.point);
    }
}
