using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour, IDamage, EDamage
{

    //Static Variables
    [SerializeField] AudioSource Audio;
    [SerializeField] Animator playerAnimator;
    [SerializeField] CharacterController characterControl;
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioClip jumpAudio;
    [SerializeField] GameObject flashlight;
    [SerializeField] AudioClip[] playerWalk;
    [Range(0, 1)][SerializeField] float playerWalkVolume;
    [SerializeField] AudioClip[] playerShot;
    [Range(0, 1)][SerializeField] float playerShotVolume;
    [SerializeField] AudioClip[] OSShot;
    [Range(0, 1)][SerializeField] float OSShotVolume;
    [SerializeField] AudioClip[] OSBroken;
    [Range(0, 1)][SerializeField] float OSBrokenVolume;
    [SerializeField] float walkAudioTimer;
    float walkAudioTimerOriginal;
    [SerializeField] AudioClip playerDeathAudio;
    [Range(0, 1)][SerializeField] float playerDeathVolume;
    private Subtitles subtitles;
    [SerializeField] GameObject subtitlesObject;


    private CharacterController CharCon;
    public Vector3 moveDirection;
    public Vector3 playerVelocity;
    float interpolationProgress = 1f;
    float targetHeight;
    float baseHeight;
    float crouchHeight;
    public int OSTimer = 0;
    public int moveSpeed = 4;
    public int dashMultiplier = 2;
    public int maxJumps = 1;
    public int jumpSpeed = 4;
    public int jumpCounter;
    public int gravity = 10;
    public int moveSpeedOriginal;


    //Coroutines
    public Coroutine poisonCoroutine;
    public Coroutine burnCoroutine;
    public Coroutine freezeCoroutine;
    public Coroutine slowCoroutine;
    public Coroutine confuseCoroutine;
    public Coroutine walkCoroutine;
    private Coroutine refillCoroutine;
    private Coroutine waitCoroutine;

    //Status Bools
    private bool flashlightToggle;
    private bool isWaitingToRefill = false;
    public bool Normal = true;
    public bool poisoned;
    public bool burning;
    public bool freezing;
    public bool slowed;
    public bool confused;
    public bool isMoving;
    public bool isSprinting;
    public bool OSRefilling;
    bool moveSpeedReduced;
    bool alive;
    bool playingWalkAudio;


    bool toolTipsOn;

    //Saved Variables
    public int HPOrignal;
    public int OSOrignal;
    public bool tutorialComplete;
    public bool shotgunUnlocked;
    public bool assaultRifleUnlocked;
    public bool RPGUnlocked;
    public bool meleeUnlocked;
    public bool overshieldUnlocked;
    public bool potionbeltUnlocked;
    public int healthPotions;
    public int overshieldPotions;
    public int currency;
    public int HP;
    public int OS;
    public int subtitleIndex = 0;

    //Inventory
    public InventoryObject inventory;   //inventory object that can be given by dragging inventory prefab onto
    public DisplayInventory inventoryDisplay;
    public InventoryManager inventoryManager;
    public Interact interactScript;
    public Dictionary<ItemObject, GroundItem> itemObjectToGroundItemMap = new Dictionary<ItemObject, GroundItem>();    //map for ground items in scene to itemObjects

    //Access Toolbelt
    ToolBelt toolBelt;
    public int currentPotionIndex = 0;
    void Awake()
    {

    }
    void Start()
    {
        LoadPlayer();

        StartUp();

        toolTipsOn = false;

        subtitlesObject = GameObject.Find("Subtitle1");

        toolBelt = GetComponent<ToolBelt>();
        UpdateCurrentPotionSlotUI();
    }
    void Update()
    {

        FlashLight();

        playerMoving();

        Movement();

        Crouch();

        AllowedToSprint();

        OverShieldSystems();

        Jump();

        PickUpThings();

        OpenInventory();

        Tootips();

        osCheck();

        ScrollPotions();
        //// Use potion when the player presses the "Q" key
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UsePotion();
        }
    }

    public void LoadPlayer()
    {

        currency = PlayerPrefs.GetInt("Currency");
        //PythonAmmo = PlayerPrefs.GetInt("PythonAmmo");
        //ShotgunAmmo = PlayerPrefs.GetInt("ShotgunAmmo");
        //AssaultRifleAmmo = PlayerPrefs.GetInt("AssaultRifleAmmo");
        //RPGAmmo = PlayerPrefs.GetInt("RPGAmmo");
        HPOrignal = PlayerPrefs.GetInt("HealthMax");
        OSOrignal = PlayerPrefs.GetInt("OvershieldMax");
        overshieldPotions = PlayerPrefs.GetInt("OvershieldPotions");
        healthPotions = PlayerPrefs.GetInt("HealthPotions");
        tutorialComplete = PlayerPrefs.GetInt("TutorialComplete") == 1;
        shotgunUnlocked = PlayerPrefs.GetInt("ShotgunUnlocked") == 1;
        assaultRifleUnlocked = PlayerPrefs.GetInt("AssaultRifleUnlocked") == 1;
        RPGUnlocked = PlayerPrefs.GetInt("RPGUnlocked") == 1;
        meleeUnlocked = PlayerPrefs.GetInt("MeleeUnlocked") == 1;
        overshieldUnlocked = PlayerPrefs.GetInt("OvershieldUnlocked") == 1;
        potionbeltUnlocked = PlayerPrefs.GetInt("PotionbeltUnlocked") == 1;
        if (HPOrignal == 0)
        {
            HPOrignal = 140;
        }
        if (OSOrignal == 0)
        {
            OSOrignal = 40;
        }
        HP = HPOrignal;
        OS = OSOrignal;
        osCheck();
        updatePlayerUI();
    }


    public void PauseAnimation()
    {
        playerAnimator.speed = 0;
    }

    public void subtitleTrigger()
    {
        subtitleIndex++;
        subtitlesObject = GameObject.Find("Subtitle" + subtitleIndex);
        subtitles = subtitlesObject.GetComponent<Subtitles>();
        subtitles.StartSubtitles();
    }

    public void TutorialComplete()
    {
        GameManager.Instance.TutorialComplete();
    }
    public void CameraTrigger()
    {
        GameManager.Instance.CameraTrigger();
    }

    #region Effects and Damage
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

        if (OS > 0 && overshieldUnlocked)
        {
            if (OS - amount < 0)
            {
                Audio.PlayOneShot(OSShot[Random.Range(0, OSShot.Length)], OSShotVolume);
                HP -= amount - OS;
                OS = 0;
                StartCoroutine(hitMe());
                updatePlayerUI();
            }
            else
            {
                Audio.PlayOneShot(OSBroken[Random.Range(0, OSBroken.Length)], OSBrokenVolume);
                OS -= amount;
                StartCoroutine(hitMe());
                updatePlayerUI();
            }
        }
        else
        {
            Audio.PlayOneShot(playerShot[Random.Range(0, playerShot.Length)], playerShotVolume);
            HP -= amount;
            StartCoroutine(hitMe());
            updatePlayerUI();
            if (HP <= 0 && Normal)
            {
                Audio.PlayOneShot(playerDeathAudio, playerDeathVolume);
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
        if (OS == 0 || !overshieldUnlocked)
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
        if (OS == 0 || !overshieldUnlocked)
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
        if (OS == 0 || !overshieldUnlocked)
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
        if (OS == 0 || !overshieldUnlocked)
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
        if (OS == 0 || !overshieldUnlocked)
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
    }

    IEnumerator hitMe()
    {
        //flash screen red
        GameManager.Instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.playerFlashDamage.SetActive(false);
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
    #endregion

    #region Movements and Interactions

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

    public Vector3 GetCurrentMoveDirection()
    {
        return moveDirection;
    }
    public void playerMoving()
    {
        //if player is moving play sound
        if (moveDirection == Vector3.zero)
        {
            isMoving = false;
        }
        else if (moveDirection != Vector3.zero)
        {
            isMoving = true;
            if (!playingWalkAudio)
            {
                StartCoroutine(walking());
            }
        }
    }
    IEnumerator walking()
    {
        if (!playingWalkAudio)
        {
            playingWalkAudio = true;
            Audio.PlayOneShot(playerWalk[Random.Range(0, playerWalk.Length)], playerWalkVolume);
            yield return new WaitForSeconds(walkAudioTimer);
            playingWalkAudio = false;
        }
    }
    void Sprint()
    {

        if ((Input.GetButtonDown("Sprint") && !isSprinting))
        {
            moveSpeed *= dashMultiplier;
            walkAudioTimer /= dashMultiplier;
            isSprinting = true;
        }
        else if ((Input.GetButtonUp("Sprint") && !freezing && !slowed && !confused && isSprinting))
        {
            moveSpeed = moveSpeedOriginal;
            walkAudioTimer = walkAudioTimerOriginal;
            isSprinting = false;
        }
    }

    void AllowedToSprint()
    {
        if (!freezing && !slowed && !confused)
        {
            Sprint();
        }
    }

    void Jump()
    {
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
    void Crouch()
    {
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
    }
    void PickUpThings()
    {
        if (Input.GetKeyDown(KeyCode.E))       //handles picking up items
        {
            interactScript.PickupItem();
        }
    }
    void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))  //handles toggling the invenotry on and off
        {
            if (!GameManager.Instance.menuActive)
            {
                inventoryDisplay.UpdateDisplay();
                inventoryManager.ToggleInventory();
            }
        }
    }

    #endregion

    #region Over Shield System

    IEnumerator WaitBeforeRefill()
    {
        yield return new WaitForSeconds(5);
        if (!OSRefilling && OS < OSOrignal)
        {
            OSRefilling = true;
            refillCoroutine = StartCoroutine(refillOS());
        }
        waitCoroutine = null; // Reset the waitCoroutine reference
    }

    IEnumerator refillOS()
    {
        for (int i = 0; i < 5; i++)
        {
            OSTimer++;
            yield return new WaitForSeconds(1);
        }
        refillCoroutine = null; // Reset the refillCoroutine reference
    }

    public void OverShieldSystems()
    {
        if (OS < OSOrignal && !OSRefilling && !isWaitingToRefill)
        {
            isWaitingToRefill = true;
            StartCoroutine(WaitBeforeRefill());
        }

        if (OSTimer >= 5)
        {
            OS = OSOrignal;
            updatePlayerUI();
            OSTimer = 0;
            OSRefilling = false;
            isWaitingToRefill = false;
        }
    }

    #endregion

    #region PlayerUI and StartUp and FlashLight

    public void updatePlayerUI()
    {
        float hpFillAmount = (float)HP / HPOrignal;
        float osFillAmount = (float)OS / OSOrignal;

        GameManager.Instance.playerHpBar.fillAmount = hpFillAmount;
        GameManager.Instance.playerOS.fillAmount = osFillAmount;

    }

    void StartUp()
    {
        walkAudioTimerOriginal = walkAudioTimer;
        playingWalkAudio = false;
        alive = true;
        moveSpeedOriginal = moveSpeed;
        CharCon = gameObject.GetComponent<CharacterController>();
        baseHeight = CharCon.height;
        crouchHeight = baseHeight / 2;
        updatePlayerUI();
    }
    void FlashLight()
    {
        if (Input.GetKeyDown(KeyCode.F)) { flashlightToggle = !flashlightToggle; flashlight.SetActive(flashlightToggle); } // input to toggle the flashlight on or off
    }
    void Tootips()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!toolTipsOn)
            {
                GameManager.Instance.ToolTipsOn.SetActive(true);
                GameManager.Instance.ToolTipsOff.SetActive(false);
                toolTipsOn = true;
            }
            else
            {
                GameManager.Instance.ToolTipsOn.SetActive(false);
                GameManager.Instance.ToolTipsOff.SetActive(true);
                toolTipsOn = false;
            }
        }
    }
    private void OnApplicationQuit()    //clears inventory once app is quit in editor
    {
        if (inventory != null)
        {
            inventory.Container.Items.Clear();
        }
    }

    public void osCheck()
    {
        if (overshieldUnlocked)
        {
            GameManager.Instance.playerOSToggle.SetActive(true);
        }
        else if (!overshieldUnlocked)
        {
            GameManager.Instance.playerOSToggle.SetActive(false);
        }
    }


    #endregion

    #region ToolBelt
    // Scroll through the potions in the ToolBelt
    private void ScrollPotions()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0)
            {
                // Scroll up
                currentPotionIndex++;
                if (currentPotionIndex >= toolBelt.potions.Length)
                {
                    currentPotionIndex = 0;
                }
            }
            else if (scroll < 0)
            {
                // Scroll down
                currentPotionIndex--;
                if (currentPotionIndex < 0)
                {
                    currentPotionIndex = toolBelt.potions.Length - 1;
                }
            }

            // Update the UI to show the current potion slot
            UpdateCurrentPotionSlotUI();
        }
    }

    // Use the currently selected potion
    public void UsePotion()
    {
        // Check if there are any potions available
        if (toolBelt.GetPotionCount(currentPotionIndex) > 0)
        {
            // Reduce the potion count
            toolBelt.AddPotion(currentPotionIndex, -1);

            // Heal the player
            HealPlayer(20);

            // Update the UI to reflect the new potion count
            UpdateCurrentPotionSlotUI();
            updatePlayerUI();
            Debug.Log("Used potion. Index: " + currentPotionIndex + ", Remaining Count: " + toolBelt.GetPotionCount(currentPotionIndex));
        }
        else
        {
            Debug.LogWarning("No potions left to use!");
        }
    }

    // Update the current potion slot UI
    private void UpdateCurrentPotionSlotUI()
    {
        int potionCount = toolBelt.GetPotionCount(currentPotionIndex);
        GameManager.Instance.UpdatePotionSlotUI(currentPotionIndex, potionCount);
    }

    // Heal the player by a certain amount
    private void HealPlayer(int healAmount)
    {
        HP = Mathf.Min(HP + healAmount, HPOrignal);
        Debug.Log("Player healed. Current HP: " + HP);
    }
}
    #endregion