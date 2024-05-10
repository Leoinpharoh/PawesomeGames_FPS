using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootingHandeler : MonoBehaviour
{
    [SerializeField] int shootDmg;
    [SerializeField] int shootDist;
    [SerializeField] float shootSpeed;

    //Used so that ray wont hit player or things like glass
    [SerializeField] LayerMask shootableLayer;
    [SerializeField] AudioClip shootAudioSource;

    bool isShooting;
    [SerializeField] AudioSource Audio;

    //Play Weapon animation
    //Play weapon fire sound

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
        if (Input.GetButtonUp("Fire1"))
        {
            isShooting = false;
        }
    }

    IEnumerator shooting()
    {
        isShooting = true;
        Audio.clip = shootAudioSource;
        Audio.Play();

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, shootableLayer) ) 
        { 
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if( dmg != null ) { dmg.takeDamage(shootDmg); }
            Debug.Log(hit.collider.gameObject);
        }
        

        yield return new WaitForSeconds(shootSpeed);
        isShooting = false;
    }
}
