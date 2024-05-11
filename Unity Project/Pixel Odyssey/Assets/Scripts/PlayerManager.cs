using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] int moveSpeed;
    [SerializeField] int dashMultiplier;
    [SerializeField] CharacterController characterControl;
    [SerializeField] int maxJumps;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [SerializeField] float HP;

    int jumpCounter;
    Vector3 moveDirection;
    Vector3 playerVelocity;
    float HPOrignal;
 
    void Start()
    {
        HPOrignal = HP;
    }

    // Update is called once per frame
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
    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        if (HP <= 0)
        {
            GameManager.Instance.youLose();
        }
    }
    void updatePlayerUI()
    {
        GameManager.Instance.playerHpBar.fillAmount = HP / HPOrignal;
    }
}
