
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{
    public List<GameObject> lists;
    public bool isFinger = true;
    public RectTransform pointer;
    public Vector3 offetPointer;
    public void Hide()
    {
        foreach (var item in lists)
        {
            Canvas canvas = item.GetComponent<Canvas>();
            if (canvas)
                DestroyImmediate(canvas);
        }
        gameObject.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);

        
        foreach (var item in lists)
        {
            Canvas canvas = item.GetComponentExtensions<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 500;
            if (isFinger)
            {
                var finger = Resources.Load<GameObject>("Effect2D/FingerPoint");
                finger = Instantiate(finger, transform);
                finger.transform.position = item.transform.position;
                canvas = finger.GetComponentExtensions<Canvas>();
                canvas.overrideSorting = true;
                canvas.sortingOrder = 501;
            }

            if (pointer != null)
            {
                var _pointer = Instantiate(pointer, transform);
                _pointer.gameObject.SetActive(true);
                _pointer.position = item.transform.position;
                _pointer.localPosition += offetPointer;
                
            }
        }
        if(pointer)
            pointer?.gameObject?.SetActive(false);
    }

}

