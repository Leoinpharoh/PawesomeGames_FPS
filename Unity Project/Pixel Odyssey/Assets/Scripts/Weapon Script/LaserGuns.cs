using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserGuns : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject firePoint;

    [SerializeField] int shootDmg;
    [SerializeField] int shootDist;
    [SerializeField] float shootSpeed;
    [SerializeField] float LaserWaitTime;


    //Used so that ray wont hit player or things like glass
    [SerializeField] LayerMask shootableLayer;
    [SerializeField] AudioClip shootAudioSource;

    bool isShooting;
    [SerializeField] AudioSource Audio;

    //Play Weapon animation

    private void Update()
    {
        shoot();
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
        if (!isShooting)
        {
            isShooting = true;

            Audio.clip = shootAudioSource;
            Audio.Play();

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, shootableLayer))
            {
                IDamage dmg = hit.collider.GetComponent<IDamage>();
                if (dmg != null) { dmg.takeDamage(shootDmg, hit.point); }
                Debug.Log(hit.collider.gameObject);
            }
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, firePoint.transform.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        yield return new WaitForSeconds(LaserWaitTime);
        lineRenderer.enabled = false;

        yield return new WaitForSeconds(shootSpeed);
        isShooting = false;
    }
}
