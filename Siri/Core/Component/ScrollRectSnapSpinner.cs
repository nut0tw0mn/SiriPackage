using Siri.Core.Component;
using UnityEngine;

[RequireComponent(typeof(ScrollRectSnap))]
[ExecuteInEditMode]
[AddComponentMenu("UI/ScrollRectSnapSpinner")]
public class ScrollRectSnapSpinner : MonoBehaviour
{
    public bool isEventSubscription;

    private ScrollRectSnap scrollRectSnap;
    protected void Start()
    {
        scrollRectSnap = GetComponent<ScrollRectSnap>();
        var input = gameObject.GetComponentExtensions<InputSpinner>();

        if (isEventSubscription)
        {
            input.onNext.AddListener(Next);
            input.onPrevious.AddListener(Previous);
        }
    }


    public void Next()
    {
        var index = scrollRectSnap.Index;
        index++;
        scrollRectSnap.SnapIndex(index);
    }

    public void Previous()
    {
        var index = scrollRectSnap.Index;
        index--;
        scrollRectSnap.SnapIndex(index);
    }
}
