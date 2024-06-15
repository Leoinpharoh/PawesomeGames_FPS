using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //create the gameManager

    //Fields for menus
    [SerializeField] public GameObject menuActive;
    [SerializeField] public GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject sceneSelect;
    [SerializeField] public GameObject menuOptions;
    [SerializeField] GameObject optionsMainMenu;
    [SerializeField] TMP_Text ammoDisplayAmount;
    [SerializeField] TMP_Text clipDisplayAmount;
    [SerializeField] TMP_Text clipColtAmount;
    [SerializeField] TMP_Text clipShotgunAmount;
    [SerializeField] TMP_Text clipAssaultAmount;
    [SerializeField] TMP_Text clipRPGAmount;
    [SerializeField] TMP_Text timerText;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator UIAnimator;
    [SerializeField] public GameObject ToolTipsOn;
    [SerializeField] public GameObject ToolTipsOff;
    public GameObject playerOSToggle; //for unlocking the OS
    //public GameObject playerToolBeltToggle; //for unlocking the ToolBelt
    public GameObject playerPotionToggle; //for toolbelt image when swapping to potion
    public GameObject playerCureToggle; //for toolbelt image when swapping to cure
    //public GameObject playerOSPToggle; //for toolbelt image when swapping to OS Potion

    // Used for display of the players ammo for each gun
    [SerializeField]
    TMP_Text[] SlotRounds;

    [SerializeField] public List<string> objectives;
    [SerializeField] TMP_Text objective1Text;
    [SerializeField] TMP_Text objective2Text;
    [SerializeField] TMP_Text objective3Text;
    [SerializeField] public TMP_Text objectiveEnemiesToKill;
    public int objectiveEnemiesKilledCount;
    public int objectiveEnemiesToKillCount;

    public bool objective1Aquired;
    public bool objective2Aquired;
    public bool objective3Aquired;
    public bool needsObjective;


    //Tool Belt fields
    [SerializeField] Image[] itemSlotImages; // Array of Images to display the potion icons
    [SerializeField] TMP_Text AmountinBag; // Text to display the current potion count
    [SerializeField] Sprite[] potionSprites; // Array to hold the sprites for each potion type

    //status needs
    [SerializeField] TMP_Text currentEffectText;
    [SerializeField] public int poisonedTimer;
    [SerializeField] public int poisonedDamage;
    [SerializeField] public int burningTimer;
    [SerializeField] public int burningDamage;
    [SerializeField] public int freezingTimer;
    [SerializeField] public int freezingDamage;
    [SerializeField] public int slowedTimer;
    [SerializeField] public int confusedTimer;



    //non serialized
    //gameObject/screenflashes
    public Image playerHpBar;
    public Image playerOS;
    public PlayerManager playerScript;
    public GameObject playerFlashDamage;
    public GameObject poisonHitScreen;
    public GameObject burnHitScreen;
    public GameObject freezeHitScreen;
    public GameObject slowHitScreen;
    public GameObject confuseHitScreen;
    public GameObject player;
    public GameObject mainCamera;



    //basics
    public bool isPaused;
    //int enemyCount;
    string ammoCurrentType;
    int lightBullets;
    int MediumBullets;
    int HeavyBullets;
    public int confusedDamage = 0;
    public int slowedDamage = 0;
    float time = 0f;
    bool dead = false;

    public bool tutorialComplete = false;
    private CharacterController characterController;
    private WeaponSwap weaponSwap;
    private CameraController cameraController;


    //Tutorial Objects
    [SerializeField] GameObject Timer;
    [SerializeField] GameObject ToolBelt;
    [SerializeField] GameObject ObjectiveList;
    [SerializeField] GameObject Reticle;
    [SerializeField] GameObject Ammo;
    [SerializeField] GameObject HealthBar;
    [SerializeField] GameObject StatusBar;
    [SerializeField] GameObject OvershieldBar;
    [SerializeField] GameObject ToolTips;
    [SerializeField] GameObject Portals;
    [SerializeField] GameObject NewGameScreen;
    [SerializeField] GameObject LoadGameScreen;





    public static GameManager Instance;
    void Awake()
    {
        objective1Text.text = "";
        objective2Text.text = "";
        objective3Text.text = "";

        objectives = new List<string> { "", "", "" };
        Instance = this;
        //show player location
        player = GameObject.FindWithTag("Player");
        mainCamera = GameObject.FindWithTag("MainCamera");
        //define player script
        playerScript = player.GetComponent<PlayerManager>();
        //on awake needs objective
        needsObjective = true;
        Instance = this;

        playerPotionToggle.SetActive(true);
    }

    private void Start()
    {
        timerText.text = "0:00";

        LoadPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        // to pause
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePaused();

                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause || menuActive == menuOptions)
            {
                stateUnPaused();
            }
        }
        if (objective1Aquired && objective2Aquired && objective3Aquired)
        {
            needsObjective = false;
        }

        UpdateTimer();
    }



    public void statePaused()
    {
        //
        isPaused = !isPaused;
        //keep cursor in the window
        Cursor.lockState = CursorLockMode.Confined;
        //hide cursor
        Cursor.visible = true;
        //reset time passed to zero
        Time.timeScale = 0;
    }
    public void stateUnPaused()
    {
        isPaused = !isPaused;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //allow time to pass again
        Time.timeScale = 1;
        menuActive.SetActive(isPaused);
        menuOptions.SetActive(false);
        menuActive = null;
    }
    //public void updateGameGoal(int amount)
    //{
    //    enemyCount += amount;
    //    //send enemy count via as string
    //    enemyCountText.text = enemyCount.ToString("F0");
    //    //if (enemyCount <= 0 )
    //    //{
    //    //    statePaused();
    //    //    menuActive = menuWin;
    //    //    menuActive.SetActive(isPaused);
    //    //}
    //}


    public void LoadPlayer()
    {
        clipColtAmount.text = lightBullets.ToString();
        clipShotgunAmount.text = MediumBullets.ToString();
        clipAssaultAmount.text = HeavyBullets.ToString();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        PlayerPrefs.SetInt("TutorialComplete", 1);
        PlayerPrefs.Save();
        tutorialComplete = PlayerPrefs.GetInt("TutorialComplete") == 1;
        if (tutorialComplete == false && sceneName == "Player Hub")
        {
            TutorialTrigger();
        }
        if (tutorialComplete == true && sceneName == "Opening Scene")
        {
            OpeningScene();
        }

    }

    public void updateGameObjective()
    {
        if (objectives.Count <= 0)
        {

        }
        else if (objectives.Count >= 1)
        {
            objective1Text.text = objectives[0].ToString();
            objective1Aquired = true;
            if (objectives.Count >= 2)
            {
                objective2Text.text = objectives[1].ToString();
                objective2Aquired = true;
                if (objectives.Count >= 3)
                {
                    objective3Text.text = objectives[2].ToString();
                    objective3Aquired = true;
                }
                else
                {
                    objective3Text.text = "";
                }
            }
            else
            {
                objective2Text.text = "";
                objective3Text.text = "";
            }
        }
    }

    public void youLose()
    {
        if (dead == false)
        {
            dead = true;
            playerAnimator.enabled = true;
            playerAnimator.applyRootMotion = true;
            playerAnimator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
            playerAnimator.SetTrigger("Death");
            UIAnimator.SetTrigger("Death");
            Debug.Log("Dead");
            StartCoroutine(death());
        }

    }

    IEnumerator death()
    {
        yield return new WaitForSeconds(3);
        statePaused();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }

    public void sceneSelectMain()
    {
        statePaused();
        menuActive = sceneSelect;
        menuActive.SetActive(isPaused);
    }
    public void optionsMain()
    {
        statePaused();
        menuActive = optionsMainMenu;
        menuActive.SetActive(isPaused);
    }

    public void optionsMenu()
    {
        statePaused();
        menuActive = menuOptions;
        menuActive.SetActive(isPaused);
    }

    public void playerAmmo(string ammoType, int ammo)
    {
        ammoCurrentType = ammoType;
        lightBullets = ammo;
        MediumBullets = ammo;
        HeavyBullets = ammo;
        switch (ammoCurrentType)
        {
            case "Light":
                ammoDisplayAmount.text = lightBullets.ToString();
                clipColtAmount.text = lightBullets.ToString(); break;
            case "Medium":
                ammoDisplayAmount.text = MediumBullets.ToString();
                clipShotgunAmount.text = MediumBullets.ToString(); break;
            case "Heavy":
                ammoDisplayAmount.text = HeavyBullets.ToString();
                //clipRPGAmount.text = HeavyBullets.ToString();
                clipAssaultAmount.text = HeavyBullets.ToString(); break;
        }
    }

    public void playerClip(int clip) // Update the player's clip amount
    {
        clipDisplayAmount.text = clip.ToString(); // Update the clip display amount

        ShootingHandler[] shootingHandlers = player.GetComponents<ShootingHandler>();
        ShootingHandler[] disabledShootingHandlers = shootingHandlers.Where(handler => !handler.enabled).ToArray();
        for (int i = 0; i < disabledShootingHandlers.Length && i < SlotRounds.Length; i++)
        {
            int ammo = disabledShootingHandlers[i].Ammo;
            SlotRounds[i].text = ammo.ToString();
        }
    }

    public void playerEffect(string effect) //updates UI with current Status user is under
    {
        switch (effect)
        {
            case "Poisoned":
                //updates the UI here
                currentEffectText.text = "Poisoned";
                break;
            case "Burning":
                currentEffectText.text = "Burning";
                break;
            case "Freezing":
                currentEffectText.text = "Freezing";
                break;
            case "Slowed":
                currentEffectText.text = "Slowed";
                break;
            case "Confused":
                currentEffectText.text = "Confused";
                break;
            case "Normal":
                currentEffectText.text = "Normal";
                break;
        }
    }

    public void TutorialTrigger()
    {
        characterController = player.GetComponent<CharacterController>();
        weaponSwap = player.GetComponent<WeaponSwap>();
        cameraController = mainCamera.GetComponent<CameraController>();
        characterController.enabled = false;
        weaponSwap.enabled = false;
        cameraController.enabled = false;
        playerAnimator.applyRootMotion = false;
        playerAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        ToolBelt.SetActive(false);
        ObjectiveList.SetActive(false);
        Reticle.SetActive(false);
        Ammo.SetActive(false);
        HealthBar.SetActive(false);
        StatusBar.SetActive(false);
        OvershieldBar.SetActive(false);
        Portals.SetActive(false);
        Timer.SetActive(false);
        ToolTips.SetActive(false);
        playerAnimator.SetBool("Tutorial", true);
        PlayerPrefs.SetInt("TutorialComplete", tutorialComplete ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void TutorialComplete()
    {
        characterController = player.GetComponent<CharacterController>();
        weaponSwap = player.GetComponent<WeaponSwap>();
        cameraController = mainCamera.GetComponent<CameraController>();
        characterController.enabled = true;
        weaponSwap.enabled = true;
        cameraController.enabled = true;
        playerAnimator.applyRootMotion = true;
        playerAnimator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        ToolBelt.SetActive(true);
        ObjectiveList.SetActive(true);
        Reticle.SetActive(true);
        Ammo.SetActive(true);
        HealthBar.SetActive(true);
        StatusBar.SetActive(true);
        Timer.SetActive(true);
        ToolTips.SetActive(true);
        Portals.SetActive(true);
        tutorialComplete = true;
        PlayerPrefs.SetInt("TutorialComplete", tutorialComplete ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void CameraTrigger()
    {
        playerAnimator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
    }

    public void OpeningScene()
    {
        NewGameScreen.SetActive(false);
        LoadGameScreen.SetActive(true);

    }

    public void updateEnemiesToKill()
    {
        //if there are enemies to kill
        if (objectiveEnemiesToKillCount > 0)
        {
            objectiveEnemiesToKill.text = ("Enemies to kill: " + objectiveEnemiesKilledCount.ToString() + " / " + objectiveEnemiesToKillCount.ToString());
        }
        //else no ememies to kill
        else
        {
            objectiveEnemiesToKill.text = "";
        }


    }

    public void UpdateTimer()
    {
        // Increment the elapsed time
        time += Time.deltaTime;

        // Format the elapsed time into minutes and seconds
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        // Update the timer text
        if (timerText != null)
        {
            timerText.text = string.Format("{0}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }
    }
    // Method to update the potion slot UI
    public void UpdatePotionSlotUI(int potionIndex, int potionCount)
    {
        // Update all slot images
        if (itemSlotImages != null && potionIndex >= 0 && potionIndex < itemSlotImages.Length)
        {
            // Set the current potion image and clear the others
            for (int i = 0; i < itemSlotImages.Length; i++)
            {
                if (i == potionIndex && potionIndex < potionSprites.Length)
                {
                    itemSlotImages[i].sprite = potionSprites[potionIndex]; // Set the UI Image to the current potion sprite
                    if(itemSlotImages[i].sprite == potionSprites[0])
                    {
                        playerPotionToggle.SetActive(true);
                        playerCureToggle.SetActive(false);
                    //  playerOSPToggle.SetActive(false);
                    }
                    if (itemSlotImages[i].sprite == potionSprites[1])
                    {
                        playerCureToggle.SetActive(true);
                        playerPotionToggle.SetActive(false);
                    //  playerplayerOSPToggle.SetActive(false);
                    }
                    //if (itemSlotImages[i].sprite == potionSprites[2]) OSPotion
                    //{
                    //    playerOSPToggle.SetActive(true);
                    //    playerPotionToggle.SetActive(false);
                    //    playerCureToggle.SetActive(False);
                    //}
                    itemSlotImages[i].color = Color.white; // Ensure the image is visible
                }
                else
                {
                    itemSlotImages[i].sprite = null; // Clear other slots or set to a default/empty sprite
                    itemSlotImages[i].color = new Color(1, 1, 1, 0); // Make the image invisible
                }
            }
        }

        if (AmountinBag != null)
        {
            AmountinBag.text = potionCount.ToString(); // Set the UI Text to the current potion count
        }
    }
}
