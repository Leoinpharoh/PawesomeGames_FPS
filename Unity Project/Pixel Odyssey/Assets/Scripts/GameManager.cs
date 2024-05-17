using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    //create the gameManager

    //Fields for menus
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject sceneSelect;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text ammoDisplayAmount;
    [SerializeField] TMP_Text clipDisplayAmount;

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
    public PlayerManager playerScript;
    public GameObject playerFlashDamage;
    public GameObject poisonHitScreen;
    public GameObject burnHitScreen;
    public GameObject freezeHitScreen;
    public GameObject slowHitScreen;
    public GameObject confuseHitScreen;
    public GameObject player;

    //basics
    public bool isPaused;
    int enemyCount;
    string ammoCurrentType;
    int lightBullets;
    int MediumBullets;
    int HeavyBullets;
    public int confusedDamage = 0;
    public int slowedDamage = 0;



    public static GameManager Instance;
    void Awake()
    {
        Instance = this;
        //show player location
        player = GameObject.FindWithTag("Player");
        //define player script
        playerScript = player.GetComponent<PlayerManager>();
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
            else if (menuActive == menuPause)
            {
                stateUnPaused();
            }
        }
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
        menuActive = null;
    }
    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        //send enemy count via as string
        enemyCountText.text = enemyCount.ToString("F0");
        if (enemyCount <= 0)
        {
            statePaused();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
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
}
