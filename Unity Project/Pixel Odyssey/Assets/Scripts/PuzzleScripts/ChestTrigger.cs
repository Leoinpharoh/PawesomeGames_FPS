using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTrigger : MonoBehaviour, IInteractable
{
    public GameObject chestLid;
    public PuzzleHandler puzzleHandler;
    public bool isOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        if(puzzleHandler == null)
        {
            puzzleHandler = FindAnyObjectByType<PuzzleHandler>();
        }
    }
    public void Interact(PlayerManager playerManager, DisplayInventory displayInventory, PickUpMessage pickUpMessage)
    {
        if(!isOpen && chestLid != null)
        {
            isOpen = puzzleHandler.ChestTryOpen(chestLid);
            if(isOpen)
            {
                SphereCollider lidCollider = chestLid.GetComponent<SphereCollider>();
                lidCollider.enabled = false;
            }
        }
    }

}
