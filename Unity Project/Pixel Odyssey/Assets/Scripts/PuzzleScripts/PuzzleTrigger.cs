//PuzzleTrigger

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    Switch,
    Pickup
}

public class PuzzleTrigger : MonoBehaviour, IInteractable
{
    public TriggerType triggerType;
    public string eventName;
    public PuzzleHandler puzzleHandler;

    public void Interact(PlayerManager playerManager, DisplayInventory displayInventory, PickUpMessage pickUpMessage)
    {
        switch (triggerType)
        {
            case TriggerType.Switch:
                HandleSwitch();
                break;
            case TriggerType.Pickup:
                HandlePickup();
                break;
        }
    }

    private void HandlePickup()
    {
        // Logic to handle item pickup
        // Assuming we need to increment some counter or similar
        int collectedItems = 1; // Replace with the actual logic to count pickups
        if (collectedItems >= 1) // Placeholder condition, update with actual requirement
        {
            PuzzleEventManager.TriggerEvent(eventName);
        }
    }

    private void HandleSwitch()
    {
        PuzzleEventManager.TriggerEvent(eventName);
    }
    //TODO: may need to add levers, buttons.... etc
}