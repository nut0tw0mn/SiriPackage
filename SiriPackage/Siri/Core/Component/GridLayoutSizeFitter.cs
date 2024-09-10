using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Grid Layout Size Fitter", 201)]
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class GridLayoutSizeFitter : GridLayoutGroup
{
    private UISizeFitter m_layoutSizeFitter;

    protected override void OnEnable()
    {
        if (m_layoutSizeFitter == null)
        {
            m_layoutSizeFitter = gameObject.GetComponent<UISizeFitter>();
            if (m_layoutSizeFitter == null)
                m_layoutSizeFitter = gameObject.AddComponent<UISizeFitter>();
        }
    }
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Yield());
    }
    //Fixed LoadScene not resize
    private IEnumerator Yield()
    {
        yield return null;
        for (int i = 0; i < 30; i++)
        {
            SetLayoutHorizontal();
            SetLayoutVertical();
            yield return new WaitForEndOfFrame();
        }
       
    }

    public override void SetLayoutHorizontal()
    {
        HandleSelfFittingAlongAxis(0);
        base.SetLayoutHorizontal();
    }
    public override void SetLayoutVertical()
    {
        HandleSelfFittingAlongAxis(1);
        base.SetLayoutVertical();
    }

    private void HandleSelfFittingAlongAxis(int axis)
    {
        
        UISizeFitter.FitMode fitting = (axis == 0 ? m_layoutSizeFitter.m_HorizontalFit : m_layoutSizeFitter.m_VerticalFit);
        if (fitting == UISizeFitter.FitMode.Unconstrained)
            return;

        // Set anchor max to same as anchor min along axis
        //Vector2 anchorMax = rectTransform.anchorMax;
        //anchorMax[axis] = rectTransform.anchorMin[axis];
        //rectTransform.anchorMax = anchorMax;

        // Set size to min size
        Vector2 sizeDelta = rectTransform.sizeDelta;
        if (fitting == UISizeFitter.FitMode.MinSize)
            sizeDelta[axis] = LayoutUtility.GetMinSize(rectTransform,axis);// new Vector2(minWidth, minHeight);
        else
            sizeDelta[axis] = LayoutUtility.GetPreferredSize(rectTransform,axis); //new Vector2(preferredWidth, preferredHeight);
        rectTransform.sizeDelta = sizeDelta;
    }
}
