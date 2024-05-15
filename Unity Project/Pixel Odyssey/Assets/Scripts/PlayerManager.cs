using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamage
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

    //UI Components
    //Flashred when hit
    bool isHit;

    public float HP;

    int jumpCounter;
    Vector3 moveDirection;
    Vector3 playerVelocity;
    [HideInInspector]public float HPOrignal;

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