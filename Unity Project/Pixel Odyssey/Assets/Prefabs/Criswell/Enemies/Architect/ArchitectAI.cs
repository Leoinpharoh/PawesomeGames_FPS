using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArchitectAI : MonoBehaviour //IDamage
{
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] CapsuleCollider body;
    [SerializeField] SphereCollider leftHand;
    [SerializeField] SphereCollider rightHand;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSource; // Reference to the AudioSource component



    public bool phase1 = true;
    public bool phase2 = false;
    public bool phase3 = false;
    public bool playerOSUp = false;
    public bool leftSwing = false;
    public bool rightSwing = false;
    public bool slam = false;
    public bool breathAttack = false;
    public bool poisonAttack = false;
    public bool dazed = false;
    public bool roar = false;
    public bool isDead = false;
    public int HP = 5000; // Enemy Health
    public int maxHP = 5000; // Enemy Health
    public int dazedTimer;


    Vector3 playerDir; // Vector3 to store the direction to the player
    Vector3 startingPos; // Vector3 to store the starting position of the enemy
    float angleToPlayer; // Float to store the angle to the player
    private Transform playerTransform; // Reference to the player's transform
    PlayerManager playerManager; // Reference to the PlayerManager script
    SaveSystem saveSystem; // Reference to the SaveSystem script
    PlayerData playerData;


    private void Awake()
    {
        saveSystem.LoadPlayer();
        playerData = saveSystem.playerData;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Startup();
    }

    // Update is called once per frame
    void Update()
    {
        //AICheck();
        OScheck();
    }


    void OScheck()
    {
        if (saveSystem.playerData.OvershieldUnlocked == true)
        {
            if (playerManager.OS > 0)
            {
                playerOSUp = true;
            }
            else
            {
                playerOSUp = false;
            }

        }
        else
        {
            playerOSUp = false;
        }
    }


    IEnumerator PoisonBreath()
    {
        anim.SetBool("poisonBreath", true);
        poisonAttack = true;
        breathAttack = true;
        yield return new WaitForSeconds(10);
        anim.SetBool("poisonBreath", false);
        poisonAttack = false;
        breathAttack = false;
    }

    IEnumerator IceBreath()
    {
        anim.SetBool("iceBreath", true);
        breathAttack = true;
        yield return new WaitForSeconds(10);
        anim.SetBool("iceBreath", false);
        breathAttack = false;
    }

    IEnumerator FlameBreath()
    {
        anim.SetBool("flameBreath", true);
        breathAttack = true;
        yield return new WaitForSeconds(10);
        anim.SetBool("flameBreath", false);
        breathAttack = false;
    }


    IEnumerator Slam()
    {
        anim.SetBool("slam", true);
        slam = true;
        yield return new WaitForSeconds(10);
        anim.SetBool("slam", false);
        slam = false;
    }

    IEnumerator LeftSwing()
    {
        anim.SetBool("leftSwing", true);
        leftSwing = true;
        yield return new WaitForSeconds(10);
        anim.SetBool("leftSwing", false);
        leftSwing = false;
    }

    IEnumerator RightSwing()
    {
        anim.SetBool("rightSwing", true);
        rightSwing = true;
        yield return new WaitForSeconds(10);
        anim.SetBool("rightSwing", false);
        rightSwing = false;
    }

    IEnumerator Roar()
    {
        anim.SetBool("roar", true);
        roar = true;
        yield return new WaitForSeconds(10);
        anim.SetBool("roar", false);
        roar = false;
    }

    IEnumerator Dazed()
    {
        anim.SetBool("dazed", true);
        dazed = true;
        yield return new WaitForSeconds(dazedTimer);
        anim.SetBool("dazed", false);
        dazed = false;
    }


    void AICheck()
    {
        if(HP <= 0 && phase1)
        {
            HP = 8000;
            phase1 = false;
            phase2 = true;
        }

        if (HP <= 0 && phase2)
        {
            HP = 10000;
            phase2 = false;
            phase3 = true;
        }

        if(HP <= 0 && phase3)
        {
            isDead = true;
        }
    }

}