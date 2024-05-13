using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;

public class GameManager : MonoBehaviour
{
    //create the gameManager

    //Fields for menus
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text enemyCountText;

    //non serialized
    //image for hp
    public Image playerHpBar;
    public PlayerManager playerScript;

    [SerializeField] GameObject hitScreen;


    public GameObject player;

    public bool isPaused;

    int enemyCount;



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
        if(Input.GetButtonDown("Cancel"))
        {
            if(menuActive == null)
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
        enemyCountText.text = enemyCount.ToString("F0");
        if(enemyCount <= 0)
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
    public void onHit()
    {
        //turn hit screen on
        menuActive = hitScreen;
        //do i need to wait?
        //turn it off
        menuActive = null;
    }
}
