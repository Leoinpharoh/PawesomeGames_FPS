using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;
using System.Linq;

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
    [SerializeField] TMP_Text timerText;
    [SerializeField] Animator playerAnimator;

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

    [SerializeField] TMP_Text amountinBagText;
    public int potionCount = 0;
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
    }

    private void Start()
    {
        timerText.text = "0:00";
        LoadPlayer();

        UpdateSelectedPotion();
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
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        tutorialComplete = PlayerPrefs.GetInt("TutorialComplete") == 1;
        if (tutorialComplete == false && sceneName == "Player Hub")
        {
            TutorialTrigger();
        }

        if(tutorialComplete == true && sceneName == "Opening Scene")
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
                ammoDisplayAmount.text = lightBullets.ToString(); break;
            case "Medium":
                ammoDisplayAmount.text = MediumBullets.ToString(); break;
            case "Heavy":
                ammoDisplayAmount.text = HeavyBullets.ToString(); break;
        }
    }

    public void playerClip(int clip) // Update the player's clip amount
    {
        clipDisplayAmount.text = clip.ToString(); // Update the clip display amount

        ShootingHandler[] shootingHandlers = player.GetComponents<ShootingHandler>();
        ShootingHandler[] disabledShootingHandlers = shootingHandlers.Where(handler => !handler.enabled).ToArray();
        for (int i = 0; i < disabledShootingHandlers.Length && i < SlotRounds.Length; i++)
        {
            Debug.Log("GotHere"); 
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
        Debug.Log("Opening Scene");
        NewGameScreen.SetActive(false);
        LoadGameScreen.SetActive(false);

    }

    public void updateEnemiesToKill()
    {
        //if there are enemies to kill
        if(objectiveEnemiesToKillCount > 0)
        {
            objectiveEnemiesToKill.text = ("Enemies to kill: " + objectiveEnemiesKilledCount.ToString() + " / " + objectiveEnemiesToKillCount.ToString());
        }
        //else no ememies to kill
        else
        {
            objectiveEnemiesToKill.text = ("");
        }


    }

    public void UpdateSelectedPotion()

    {
        string potionName = "None";
        int potionCount = 0;

        if (Instance.playerScript != null && Instance.playerScript.toolBelt != null)
        {
            var potion = Instance.playerScript.toolBelt.GetSelectedPotion();
            potionCount = Instance.playerScript.toolBelt.GetSelectedPotionCount();
            if (potion != null)
            {
                potionName = potion.potionName;
            }
        }

        amountinBagText.text = potionCount.ToString();
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
}
