using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectMultiTouch : ScrollRect {
    [SerializeField]
    public int amount = 2;

    public bool IsCondition {
        get {
#if UNITY_EDITOR
            return true;
#else
            return Input.touchCount == amount;
#endif
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (IsCondition)
        {
            base.OnBeginDrag(eventData);
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (IsCondition)
            base.OnDrag(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (IsCondition)
            base.OnEndDrag(eventData);
    }
}
