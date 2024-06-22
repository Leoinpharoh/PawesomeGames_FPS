//PuzzleTrigger

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    DoorSlide,
    PickupCount,
    KeyOpen,
    OpenMazeLid,
    OpenChest
}

public class PuzzleTrigger : MonoBehaviour, IInteractable
{
    public TriggerType triggerType;
    public PuzzleHandler puzzleHandler;

    public void Start()
    {
        if(puzzleHandler == null)
        {
            puzzleHandler = FindAnyObjectByType<PuzzleHandler>();   //finding puzzlehandler
        }
    }

    public void Interact(PlayerManager playerManager, DisplayInventory displayInventory, PickUpMessage pickUpMessage)
    {
        switch (triggerType)
        {
            case TriggerType.DoorSlide: puzzleHandler.OnSlideVineWall(); break;
            case TriggerType.OpenMazeLid: puzzleHandler.OnRotateMazeChest(); break;
            //case TriggerType.OpenChest: puzzleHandler.ChestTryOpen(); break;
        }
    }
}