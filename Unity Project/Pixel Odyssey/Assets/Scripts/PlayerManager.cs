using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamage, EDamage
{
    public InventoryObject inventory;   //inventory object that can be given by dragging inventory prefab onto

    [SerializeField] AudioSource Audio;
    [SerializeField] CharacterController characterControl;

    //move
    [SerializeField] int moveSpeed;
    [SerializeField] int dashMultiplier;
    [SerializeField] Rigidbody rb;

    //jumps
    [SerializeField] int maxJumps;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] AudioClip jumpAudio;

    public float HP;

    int jumpCounter;
    Vector3 moveDirection;
    Vector3 playerVelocity;
    [HideInInspector] public float HPOrignal;

    //overshield
    public float OS;
    [HideInInspector] public float OSOrignal;
    public int OSTimer = 0;
    public bool OSRefilling;
    private bool isWaitingToRefill = false;
    private Coroutine refillCoroutine;
    private Coroutine waitCoroutine;

    [SerializeField] AudioClip[] playerWalk;
    [Range(0, 1)][SerializeField] float playerWalkVolume;
    [SerializeField] AudioClip[] playerShot;
    [Range(0, 1)][SerializeField] float playerShotVolume;
    [SerializeField] AudioClip[] OSShot;
    [Range(0, 1)][SerializeField] float OSShotVolume;
    [SerializeField] AudioClip[] OSBroken;
    [Range(0, 1)][SerializeField] float OSBrokenVolume;

    //status effect bools
    public bool Normal = true;
    public bool poisoned;
    public bool burning;
    public bool freezing;
    public bool slowed;
    public bool confused;
    public Coroutine poisonCoroutine;
    public Coroutine burnCoroutine;
    public Coroutine freezeCoroutine;
    public Coroutine slowCoroutine;
    public Coroutine confuseCoroutine;
    bool moveSpeedReduced;
    bool alive;
    public bool isSprinting;
    int moveSpeedOriginal;

    [SerializeField] GameObject flashlight;
    private bool flashlightToggle;

    private CharacterController CharCon;
    float interpolationProgress = 1f;
    float targetHeight;
    float baseHeight;
    float crouchHeight;


    void Start()
    {
        alive = true;
        moveSpeedOriginal = moveSpeed;
        HPOrignal = HP;
        OSOrignal = OS;
        updatePlayerUI();
        CharCon = gameObject.GetComponent<CharacterController>();
        baseHeight = CharCon.height;
        crouchHeight = baseHeight / 2;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) { flashlightToggle = !flashlightToggle; flashlight.SetActive(!flashlight); } // input to toggle the flashlight on or off
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            moveSpeed = moveSpeed / 2;
            interpolationProgress = 0;
            targetHeight = crouchHeight;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            moveSpeed = moveSpeedOriginal;
            interpolationProgress = 0;
            targetHeight = baseHeight;
        }

        if (interpolationProgress < 1f)
        {
            interpolationProgress = Mathf.Clamp01(interpolationProgress + Time.deltaTime * 0.3f);
            CharCon.height = Mathf.Lerp(CharCon.height, targetHeight, interpolationProgress);
        }


        Movement();

        if (!freezing && !slowed && !confused)
        {
            Sprint();
        }
            

        if (OS < OSOrignal && !OSRefilling && !isWaitingToRefill)
        {
            isWaitingToRefill = true;
            StartCoroutine(WaitBeforeRefill());
            Debug.Log("Starting Wait Before Refill");
        }

        if (OSTimer >= 5)
        {
            OS = OSOrignal;
            updatePlayerUI();
            OSTimer = 0;
            OSRefilling = false;
            isWaitingToRefill = false;
            Debug.Log("Refilling Complete");
        }

        if (Input.GetButtonDown("Jump") && jumpCounter < maxJumps)
        {
            jumpCounter++;
            // handles the audio for jumping. 
            Audio.clip = jumpAudio;
            Audio.Play();
            playerVelocity.y = jumpSpeed;

        }
        playerVelocity.y -= gravity * Time.deltaTime;
        characterControl.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint()
    {
        
        if ((Input.GetButtonDown("Sprint") && !isSprinting))
        {
            moveSpeed *= dashMultiplier;
            isSprinting = true;
        }
        else if ((Input.GetButtonUp("Sprint") && !freezing && !slowed && !confused && isSprinting))
        {
            moveSpeed = moveSpeedOriginal;
            isSprinting = false;
        }
    }
    IEnumerator WaitBeforeRefill()
    {
        yield return new WaitForSeconds(5);
        if (!OSRefilling && OS < OSOrignal)
        {
            OSRefilling = true;
            refillCoroutine = StartCoroutine(refillOS());
            Debug.Log("Refilling");
        }
        waitCoroutine = null; // Reset the waitCoroutine reference
    }

    IEnumerator refillOS()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Incrementing Timer");
            OSTimer++;
            Debug.Log("Waiting 1 Second");
            yield return new WaitForSeconds(1);
        }
        refillCoroutine = null; // Reset the refillCoroutine reference
    }

    public void takeDamage(int amount, Vector3 hitPosition)
    {
        OSTimer = 0;
        OSRefilling = false;
        isWaitingToRefill = false;
        // Stop the refill coroutine if it is running
        if (refillCoroutine != null)
        {
            StopCoroutine(refillCoroutine);
            refillCoroutine = null;
        }

        // Stop the wait coroutine if it is running
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }

        if (OS > 0)
        {
            if (OS - amount < 0)
            {
                Audio.PlayOneShot(OSShot[Random.Range(0, OSShot.Length)], OSShotVolume);
                Debug.Log("True 1");
                HP -= amount - OS;
                OS = 0;
                StartCoroutine(hitMe());
                updatePlayerUI();
            }
            else
            {
                Audio.PlayOneShot(OSBroken[Random.Range(0, OSBroken.Length)], OSBrokenVolume);
                Debug.Log("True 2");
                OS -= amount;
                StartCoroutine(hitMe());
                updatePlayerUI();
            }
        }
        else if (OS <= 0)
        {
            Audio.PlayOneShot(playerShot[Random.Range(0, playerShot.Length)], playerShotVolume);
            HP -= amount;
            StartCoroutine(hitMe());
            updatePlayerUI();
            if (HP <= 0 && Normal)
            {
                playerDeath();
            }

        }
    }
    public void poisonDamage(int damage, float duration)
    {
        if (poisoned)
        {
            StopCoroutine(poisonCoroutine);
        }

        poisonCoroutine = StartCoroutine(poisonMe(damage, duration));
    }

    private IEnumerator poisonMe(int damage, float duration)
    {
        poisoned = true;
        Normal = false;
        int ticks = Mathf.FloorToInt(duration);

        for (int i = 0; i < ticks; i++)
        {
            HP -= damage;
            updatePlayerUI();
            StartCoroutine(effectMe("Poisoned"));
            playerDeath();
            yield return new WaitForSeconds(1);
        }
        poisoned = false;
        Normal = true;
        StartCoroutine(effectMe("Normal"));
    }
    public void burnDamage(int damage, float duration)
    {
        if (burning)
        {
            StopCoroutine(burnCoroutine);
        }

        burnCoroutine = StartCoroutine(burnMe(damage, duration));
    }

    private IEnumerator burnMe(int damage, float duration)
    {
        burning = true;
        Normal = false;
        int ticks = Mathf.FloorToInt(duration);

        for (int i = 0; i < ticks; i++)
        {
            HP -= damage;
            updatePlayerUI();
            StartCoroutine(effectMe("Burning"));
            playerDeath();
            yield return new WaitForSeconds(1);
        }
        burning = false;
        Normal = true;
        StartCoroutine(effectMe("Normal"));
    }

    public void freezeDamage(int damage, float duration)
    {

        if (freezing)
        {
            StopCoroutine(freezeCoroutine);
        }

        freezeCoroutine = StartCoroutine(freezeMe(damage, duration));
    }

    private IEnumerator freezeMe(int damage, float duration)
    {
        if (!moveSpeedReduced)
        {
            moveSpeed /= 2;
            moveSpeedReduced = true;
        }
        freezing = true;
        isSprinting = false;
        Normal = false;
        int ticks = Mathf.FloorToInt(duration);
        for (int i = 0; i < ticks; i++)
        {
            HP -= damage;
            updatePlayerUI();
            StartCoroutine(effectMe("Freezing"));
            playerDeath();
            yield return new WaitForSeconds(1);
        }
        freezing = false;
        Normal = true;
        if (moveSpeedReduced)
        {
            moveSpeed = moveSpeedOriginal;
            moveSpeedReduced = false;
        }
        StartCoroutine(effectMe("Normal"));
    }

    public void slowDamage(int damage, float duration)
    {
        if (slowed)
        {
            StopCoroutine(slowCoroutine);

        }
        slowCoroutine = StartCoroutine(slowMe(damage, duration));

    }

    private IEnumerator slowMe(int damage, float duration)
    {
        if (!moveSpeedReduced)
        {
            moveSpeed /= 2;
            moveSpeedReduced = true;
        }
        slowed = true;
        isSprinting = false;
        Normal = false;
        int ticks = Mathf.FloorToInt(duration);

        for (int i = 0; i < ticks; i++)
        {
            HP -= damage;
            updatePlayerUI();
            StartCoroutine(effectMe("Slowed"));
            playerDeath();
            yield return new WaitForSeconds(1);
        }
        slowed = false;
        Normal = true;
        if (moveSpeedReduced)
        {
            moveSpeed = moveSpeedOriginal;
            moveSpeedReduced = false;
        }
        StartCoroutine(effectMe("Normal"));
    }
    public void confuseDamage(int damage, float duration)
    {
        if (confused)
        {
            StopCoroutine(confuseCoroutine);
        }
        confuseCoroutine = StartCoroutine(confuseMe(damage, duration));
    }

    private IEnumerator confuseMe(int damage, float duration)
    {
        Normal = false;
        confused = true;
        isSprinting = false;
        int ticks = Mathf.FloorToInt(duration);

        for (int i = 0; i < ticks; i++)
        {
            updatePlayerUI();
            StartCoroutine(effectMe("Confused"));
            playerDeath();
            yield return new WaitForSeconds(1);
        }
        confused = false;
        Normal = true;
        moveSpeed = moveSpeedOriginal;
        StartCoroutine(effectMe("Normal"));
    }

    IEnumerator hitMe()
    {
        //flash screen red
        GameManager.Instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.playerFlashDamage.SetActive(false);
    }

    public void updatePlayerUI()
    {
        GameManager.Instance.playerHpBar.fillAmount = HP / HPOrignal;
        GameManager.Instance.playerOS.fillAmount = OS / OSOrignal;
    }
    public IEnumerator effectMe(string effect) //used to flash the screen based on what effect user has
    {
        switch (effect)
        {
            case "Poisoned":
                //updates status bar to poisoned
                GameManager.Instance.playerEffect(effect);
                //flash poisoned screen
                GameManager.Instance.poisonHitScreen.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                GameManager.Instance.poisonHitScreen.SetActive(false);
                yield return new WaitForSeconds(.9f);
                //wait 1 second to return and start new loop so time between each damage is 1 second
                break;
            case "Burning":
                GameManager.Instance.playerEffect(effect);
                GameManager.Instance.burnHitScreen.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                GameManager.Instance.burnHitScreen.SetActive(false);
                yield return new WaitForSeconds(.9f);
                break;
            case "Freezing":
                GameManager.Instance.playerEffect(effect);
                GameManager.Instance.freezeHitScreen.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                GameManager.Instance.freezeHitScreen.SetActive(false);
                yield return new WaitForSeconds(.9f);
                break;
            case "Slowed":
                GameManager.Instance.playerEffect(effect);
                GameManager.Instance.slowHitScreen.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                GameManager.Instance.slowHitScreen.SetActive(false);
                yield return new WaitForSeconds(.9f);
                break;
            case "Confused":
                GameManager.Instance.playerEffect(effect);
                GameManager.Instance.confuseHitScreen.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                GameManager.Instance.confuseHitScreen.SetActive(false);
                yield return new WaitForSeconds(.9f);
                break;
            case "Normal":
                GameManager.Instance.playerEffect(effect);
                moveSpeed = moveSpeedOriginal;
                break;
        }
    }
    public void playerDeath()
    {
        if (alive)
        {
            if (HP <= 0 && Normal || HP <= 0 && !Normal)
            {
                GameManager.Instance.youLose();
                alive = false;
            }
        }
    }
    public void Movement()
    {
        if (characterControl.isGrounded)
        {
            jumpCounter = 0;
            playerVelocity = Vector3.zero;
        }
        if (!confused)
        {
            moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
                (Input.GetAxis("Vertical") * transform.forward).normalized;
            characterControl.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            moveDirection = (Input.GetAxis("Vertical") * transform.right) +
                (Input.GetAxis("Horizontal") * transform.forward);
            characterControl.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }



    /*public void OnTriggerEnter(Collider other)  //when player collides with an item that can be picked up DM
    {
        var groundItem = other.GetComponent<GroundItem> ();

        if (groundItem) //if it is a ground item
        {
            Item _item = new Item(groundItem.itemObject);
            inventory.AddItem(_item, 1);    //adds one item if it found one
            Destroy(other.gameObject);      //destroys the item that it picked up
        }
    }

    private void OnApplicationQuit()    //clears inventory once app is quit in editor
    {
        if (inventory != null)
        {
            inventory.Container.Items.Clear();
        }
    }*/
}