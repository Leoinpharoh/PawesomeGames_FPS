using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArchitectAI : MonoBehaviour, IDamage
{
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] CapsuleCollider body;
    [SerializeField] CapsuleCollider leftHand;
    [SerializeField] CapsuleCollider rightHand;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource audioSource; // Reference to the AudioSource component
    [SerializeField] GameObject bloodSplash;
    [SerializeField] GameObject Phase1Spawner;
    [SerializeField] GameObject Phase2Spawner;
    [SerializeField] GameObject Phase3Spawner;
    [SerializeField] GameObject PoisonArea;
    [SerializeField] GameObject FlameArea;
    [SerializeField] GameObject IceArea;
    [SerializeField] AudioClip[] leftAttackSound;
    [SerializeField] AudioClip[] rightAttackSound;
    [SerializeField] AudioClip[] dazeSound;
    [SerializeField] AudioClip[] deathSound;
    [SerializeField] AudioClip[] damagedSound;
    [SerializeField] AudioClip[] idleSound;
    [SerializeField] AudioClip[] poisonSound;
    [SerializeField] AudioClip[] iceSound;
    [SerializeField] AudioClip[] fireSound;
    [SerializeField] AudioClip[] slamSound;
    [SerializeField] AudioClip[] roarSound;




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
    public float HP = 2000; // Enemy Health
    public float HPOriginal = 2000;
    public int dazedTimer;
    private float breathAttackCooldown = 30f;
    private float nextBreathAttackTime = 0f;


    Vector3 playerDir; // Vector3 to store the direction to the player
    Vector3 startingPos; // Vector3 to store the starting position of the enemy
    float angleToPlayer; // Float to store the angle to the player
    public Transform playerTransform; // Reference to the player's transform
    public PlayerManager playerManager; // Reference to the PlayerManager script
    public SaveSystem saveSystem; // Reference to the SaveSystem script
    PlayerData playerData;
    

    private void Awake()
    {
        saveSystem.LoadPlayer();
        playerData = saveSystem.playerData;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttackSequence());
        Phase1Spawner.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        OScheck();
        if (!isDead && !breathAttack && !slam && !leftSwing && !rightSwing && !dazed)
        {
            FacePlayer();
        }

    }

    void FacePlayer()
    {

        if (playerTransform != null)
        {
            Vector3 direction = (playerTransform.position - headPos.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 300f);
        }
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
        PlayPosionBreath();
        PoisonArea.SetActive(true);
        yield return new WaitForSeconds(4f);
        anim.SetBool("poisonBreath", false);
        poisonAttack = false;
        breathAttack = false;
        yield return new WaitForSeconds(3f);
        PoisonArea.SetActive(false);
    }

    IEnumerator IceBreath()
    {
        anim.SetBool("iceBreath", true);
        breathAttack = true;
        PlayIceBreath();
        yield return new WaitForSeconds(5f);
        IceArea.SetActive(true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("iceBreath", false);
        breathAttack = false;
        yield return new WaitForSeconds(12f);
        IceArea.SetActive(false);
    }

    IEnumerator FlameBreath()
    {
        anim.SetBool("flameBreath", true);
        breathAttack = true;
        PlayFireBreath();
        yield return new WaitForSeconds(5f);
        FlameArea.SetActive(true);
        yield return new WaitForSeconds(2f);
        anim.SetBool("flameBreath", false);
        breathAttack = false;
        yield return new WaitForSeconds(8f);
        FlameArea.SetActive(false);
    }


    IEnumerator Slam()
    {
        anim.SetBool("slam", true);
        slam = true;
        yield return new WaitForSeconds(2f);
        PlaySlamSound();
        yield return new WaitForSeconds(1.5f);
        anim.SetBool("slam", false);
        slam = false;
    }

    IEnumerator LeftSwing()
    {
        anim.SetBool("leftHandAttack", true);
        leftSwing = true;
        PlayLeftAttackSound();
        yield return new WaitForSeconds(1.5f);
        PlayLeftAttackSound();
        anim.SetBool("leftHandAttack", false);
        leftSwing = false;
    }

    IEnumerator RightSwing()
    {
        anim.SetBool("rightHandAttack", true);
        rightSwing = true;
        PlayRightAttackSound();
        yield return new WaitForSeconds(1.5f);
        PlayRightAttackSound();
        anim.SetBool("rightHandAttack", false);
        rightSwing = false;
    }

    IEnumerator Roar()
    {
        anim.SetBool("Roar", true);
        roar = true;
        PlayRoarSound();
        StartCoroutine(Edamage(GameManager.Instance.player));
        yield return new WaitForSeconds(4f);
        PlayRoarSound();
        anim.SetBool("Roar", false);
        roar = false;
    }

    IEnumerator Dazed()
    {
        anim.SetBool("Dazed", true);
        dazed = true;
        PlayDazedSound();
        yield return new WaitForSeconds(dazedTimer);
        PlayDazedSound();
        anim.SetBool("Dazed", false);
        dazed = false;
        StartCoroutine(Roar());
    }


    public void takeDamage(int amount, Vector3 hitPosition) // Method to take damage
    {
        if (!dazed)
        {
            HP -= amount; // Subtract the amount from the enemy's health
            StartCoroutine(Damage(hitPosition)); // Start the flash coroutine
            playerManager.updatePlayerUI(); // Call the updatePlayerUI function from the playerManager script
            if (HP <= 0 && phase1)
            {
                HP = 3000;
                HPOriginal = 3000;
                phase1 = false;
                phase2 = true;
                dazedTimer = 10;
                Phase1Spawner.SetActive(false);
                Phase2Spawner.SetActive(true);
                StartCoroutine(Dazed());
            }
            if (HP <= 0 && phase2)
            {
                HP = 4000;
                HPOriginal = 4000;
                phase2 = false;
                phase3 = true;
                dazedTimer = 10;
                Phase2Spawner.SetActive(false);
                Phase3Spawner.SetActive(true);
                StartCoroutine(Dazed());
            }
            if (HP <= 0 && phase3) // Check if the enemy's health is less than or equal to 0
            {
                Phase3Spawner.SetActive(false);
                anim.SetTrigger("isDead"); // Set the trigger for the death animation
                isDead = true; // Set isDead to true
                StartCoroutine(Death()); // Start the death coroutine
            }
        }   
    }

    IEnumerator Death() // Coroutine to handle the enemy's death
    {
        agent.isStopped = true; // Stop the agent from moving
        agent.speed = 0;
        agent.angularSpeed = 0;
        PlayDeathSound();
        yield return new WaitForSeconds(2.5f); // Wait for the destroyTime from the EnemyParams scriptable object
        Destroy(gameObject); // Destroy the enemy
        Vector3 dropPosition = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
    }

    IEnumerator Damage(Vector3 hitPosition)
    {
        GameObject bloodEffect = Instantiate(bloodSplash, hitPosition, Quaternion.identity); // Instantiate at the hit position
        bloodEffect.transform.SetParent(transform); // Optionally set the parent to the enemy's transform
        PlayDamageSound();
        yield return new WaitForSeconds(1); // Wait for 1 second (adjust based on your effect's needs)
        Destroy(bloodEffect); // Optionally destroy the effect after it finishes playing
    }

    IEnumerator Edamage(GameObject player)
    {
        EDamage damageable = player.GetComponent<EDamage>();

        if (damageable != null)
        {
            damageable.slowDamage(0,8);
        }
        yield return new WaitForSeconds(8);

    }



    private void PlayRightAttackSound()
    {
        if (audioSource != null && rightAttackSound.Length > 0)
        {
            AudioClip clip = rightAttackSound[Random.Range(0, rightAttackSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
    private void PlayLeftAttackSound()
    {
        if (audioSource != null && leftAttackSound.Length > 0)
        {
            AudioClip clip = leftAttackSound[Random.Range(0, leftAttackSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayDazedSound()
    {
        if (audioSource != null && dazeSound.Length > 0)
        {
            AudioClip clip = dazeSound[Random.Range(0, dazeSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
    private void PlaySlamSound()
    {
        if (audioSource != null && slamSound.Length > 0)
        {
            AudioClip clip = slamSound[Random.Range(0, slamSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayDeathSound()
    {
        if (audioSource != null && deathSound.Length > 0)
        {
            AudioClip clip = deathSound[Random.Range(0, deathSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
    private void PlayPosionBreath()
    {
        if (audioSource != null && poisonSound.Length > 0)
        {
            AudioClip clip = poisonSound[Random.Range(0, poisonSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
    private void PlayFireBreath()
    {
        if (audioSource != null && fireSound.Length > 0)
        {
            AudioClip clip = fireSound[Random.Range(0, fireSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
    private void PlayIceBreath()
    {
        if (audioSource != null && iceSound.Length > 0)
        {
            AudioClip clip = iceSound[Random.Range(0, iceSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
    private void PlayRoarSound()
    {
        if (audioSource != null && roarSound.Length > 0)
        {
            AudioClip clip = roarSound[Random.Range(0, roarSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayDamageSound()
    {
        if (audioSource != null && damagedSound.Length > 0)
        {
            AudioClip clip = damagedSound[Random.Range(0, damagedSound.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayIdleSound()
    {
        if (audioSource != null && idleSound.Length > 0)
        {
            AudioClip clip = idleSound[Random.Range(0, idleSound.Length)];
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    IEnumerator AttackSequence()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(5f);

            if (!isDead && !dazed)
            {
                if (phase1 && !roar)
                {
                    ChooseAttack();
                }
                else if (phase2 && !roar)
                {
                    ChooseAttack();
                }
                else if (phase3 && !roar)
                {
                    ChooseAttack();
                }
            }
        }
    }

    void ChooseAttack()
    {
        if (Time.time >= nextBreathAttackTime)
        {
            if (!playerOSUp)
            {
                StartCoroutine(PoisonBreath());
                nextBreathAttackTime = Time.time + breathAttackCooldown;
            }
            else
            {
                int randomAttack = Random.Range(0, 3);
                if (randomAttack == 0)
                {
                    StartCoroutine(IceBreath());
                }
                else if (randomAttack == 1)
                {
                    StartCoroutine(FlameBreath());
                }
                else
                {
                    StartCoroutine(Slam());
                }
                nextBreathAttackTime = Time.time + breathAttackCooldown;
            }
        }
        else
        {
            int randomAttack2 = Random.Range(0, 3);
            if (randomAttack2 == 0)
            {
                StartCoroutine(LeftSwing());
            }
            else if (randomAttack2 == 1)
            {
                StartCoroutine(RightSwing());
            }
            else
            {
                StartCoroutine(Slam());
            }
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            StartCoroutine(Slap());
        }  
    }

    IEnumerator Slap()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerManager playerHealth = GameManager.Instance.player.GetComponent<PlayerManager>();
        if (playerHealth != null) // Check if the playerHealth component is found on the player
        {
            playerHealth.takeDamage(20, GameManager.Instance.player.transform.position);
        }
    }

}