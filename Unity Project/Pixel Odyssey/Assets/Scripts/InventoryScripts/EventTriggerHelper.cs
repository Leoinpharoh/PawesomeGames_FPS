//EventTriggerHelper

using UnityEngine;
using UnityEngine.EventSystems;
/*
/// <summary>
/// Helper class to add the EventTrigger Listeners
/// </summary>
public static class EventTriggerHelper
{
    public static void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> callback)
    {
        if (trigger == null)
            return;
        //TODO: need to fix the eventTriggerHelper
        EventTrigger entry = new EventTrigger();
        entry.callback.AddListener((data) => { callback.Invoke(data); });
        trigger.triggers.Add(entry);
    }
}*/