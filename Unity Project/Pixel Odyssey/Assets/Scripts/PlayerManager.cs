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
    [HideInInspector]public float HPOrignal;
<<<<<<< Updated upstream
=======

    //status effect bools
    public bool Normal = true;
    public bool poisoned;
    public bool burning;
    public bool freezing;
    public bool slowed;
    public bool confused;
    public int damageOverTimeDivider;
>>>>>>> Stashed changes

    void Start()
    {

        HPOrignal = HP;
        updatePlayerUI();
    }
    void Update()
    {
        
        if(characterControl.isGrounded)
        {
            jumpCounter = 0;
            playerVelocity = Vector3.zero;
        }
        moveDirection = (Input.GetAxis("Horizontal") * transform.right) +
            (Input.GetAxis("Vertical") * transform.forward).normalized;
        characterControl.Move(moveDirection * moveSpeed * Time.deltaTime);

        Sprint();

        if(Input.GetButtonDown("Jump") && jumpCounter < maxJumps)
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
        if(Input.GetButtonDown("Sprint"))
        {
            moveSpeed *= dashMultiplier;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            moveSpeed /= dashMultiplier;
        }
    }


    public void takeDamage(int amount, Vector3 hitPosition)
    {
        HP -= amount;
        StartCoroutine(hitMe());
        updatePlayerUI();
        if (HP <= 0)
        {
            GameManager.Instance.youLose();
        }
    }
    public void poisonDamage(string effect)
    {
        int timerHpTick = GameManager.Instance.poisonedDamage / GameManager.Instance.poisonedTimer; //accounts for the hp down AND timer
        //turn on poisoned bool
        poisoned = true;

        for (int i = 0; i < timerHpTick; i++)
        {
            //lower health
            HP -= timerHpTick;
            //updates the health bar
            poisoned = true;
            updatePlayerUI();
            //starts the screen flash coroutine

            StartCoroutine(effectMe(effect)); //should account for the time in here
            if (HP <= 0)
            {
                GameManager.Instance.youLose();
            }
        }
        //turn off poisoned
        poisoned = false;

        GameManager.Instance.playerEffect("Regular");
    }
    public void burnDamage(string effect)
    {
        int timerHpTick = GameManager.Instance.burningDamage / GameManager.Instance.poisonedTimer;
        burning = true;
        for (int i = 0; i < timerHpTick; i++)
        {
            HP -= timerHpTick;
            updatePlayerUI();
            StartCoroutine(effectMe(effect)); 
            if (HP <= 0)
            {
                GameManager.Instance.youLose();
            }
        }
        burning = false;
        GameManager.Instance.playerEffect("Normal");
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
    }
    IEnumerator effectMe(string effect) //used to flash the screen based on what effect user has
    {
        switch(effect)
        {
            case "Poisoned":
                //updates status bar to poisoned
                GameManager.Instance.playerEffect(effect);
                //flash poisoned screen
                GameManager.Instance.poisonHitScreen.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                GameManager.Instance.poisonHitScreen.SetActive(false);
                //wait 1 second to return and start new loop so time between each damage is 1 second
                yield return new WaitForSeconds(.9f);
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