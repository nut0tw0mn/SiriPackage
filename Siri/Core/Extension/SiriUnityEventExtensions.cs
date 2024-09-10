using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public static class SiriUnityEventExtensions
{
    public static void EventTrigger(this UIBehaviour ui, EventTriggerType eventType, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = ui.GetComponentInParent<EventTrigger>();
        if (trigger == null)
            trigger = ui.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
    public static void EventTrigger(this UnityEngine.UI.Selectable selectable, EventTriggerType eventType, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = selectable.GetComponentInParent<EventTrigger>();
        if (trigger == null)
            trigger = selectable.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((eventData) =>
        {
            if (selectable.interactable)
            {
                action(eventData);
            }
        });
        trigger.triggers.Add(entry);
    }

    public static void onClick_SetListener(this Button btn, UnityAction new_event)
    {
        Button.ButtonClickedEvent events = new Button.ButtonClickedEvent();
        events.AddListener(new_event);
        btn.onClick = events;
    }

    public static T GetComponentNextLayer<T>(this PointerEventData eventData, List<GameObject> ignores = null)
    {
        List<RaycastResult> ray = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, ray);

        //ray.RemoveAt(0);
        foreach (RaycastResult item in ray)
        {
            if (item.gameObject == eventData.selectedObject)
                continue;
            if (ignores != null)
                if (ignores.Contains(item.gameObject))
                    continue;
            T handler = item.gameObject.GetComponent<T>();
            if (handler != null)
            {
                return handler;
            }
        }
        return default(T);
    }
}