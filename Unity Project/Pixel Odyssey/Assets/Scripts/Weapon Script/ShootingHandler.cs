using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingHandler : MonoBehaviour
{
    public LineRenderer lineRenderer;
    [SerializeField] GameObject firePoint;

    [SerializeField] int shootDmg;
    [SerializeField] int shootDist;
    [SerializeField] float shootSpeed;
    [SerializeField] float LaserWaitTime;

    [SerializeField] int Ammo;

    //Used so that ray wont hit player or things like glass
    [SerializeField] LayerMask shootableLayer;
    [SerializeField] AudioClip audioSFXShoot;

    [HideInInspector]public int stopPlease = 0;

    [HideInInspector] public bool isShooting;
    [SerializeField] AudioSource audioSource;

    enum AmmoType {Light, Medium, Heavy }
    [SerializeField] AmmoType ammoType;

    enum WeaponType { RayCast, Laser }
    [SerializeField] WeaponType weaponType;


    //Play Weapon animation

    private void Start()
    {
        
    }

    private void Update()
    {
        shoot();
#if UNITY_EDITOR
        if (stopPlease == 0)
        {
            GameManager.Instance.playerAmmo(ammoType.ToString(), Ammo);
            stopPlease = 1;
        }
#endif
    }

    void shoot() 
    { 
        if (Input.GetButtonDown("Fire1") && !isShooting)
        {
            StartCoroutine(shooting());
        }
    }

    IEnumerator shooting()
    {
        if (!isShooting && Ammo != 0 && GameManager.Instance.isPaused == false)
        {


            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, shootableLayer))
            {
                if (weaponType.ToString() == "Laser" && hit.collider != null)
                {
                    lineRenderer.enabled = true;
                    lineRenderer.SetPosition(0, firePoint.transform.position);
                    lineRenderer.SetPosition(1, hit.point);
                    IDamage dmg = hit.collider.GetComponent<IDamage>();
                    if (dmg != null) { dmg.takeDamage(shootDmg, hit.point); }

                    isShooting = true;
                    audioSource.clip = audioSFXShoot;
                    audioSource.Play();

                    Ammo -= 1;
                    GameManager.Instance.playerAmmo(ammoType.ToString(), Ammo);
                }
                else
                {
                    isShooting = true;
                    audioSource.clip = audioSFXShoot;
                    audioSource.Play();

                    IDamage dmg = hit.collider.GetComponent<IDamage>();
                    if (dmg != null) { dmg.takeDamage(shootDmg, hit.point); }

                    Ammo -= 1;
                    GameManager.Instance.playerAmmo(ammoType.ToString(), Ammo);
                }
            }
            if(weaponType.ToString() == "Laser")
            {

                yield return new WaitForSeconds(LaserWaitTime);
                lineRenderer.enabled = false;
            }
        }
        yield return new WaitForSeconds(shootSpeed);
        isShooting = false;
    }


}
