using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class FollowText : UIBehaviour,ILayoutController
{
	public RectOffset offset;
    public Vector2 min;
    public RectTransform target;
	private RectTransform myRect;


#if UNITY_EDITOR
	private void Update()
	{
		if (Application.isEditor)
		{
			RectFollowSize();
		}
	}
#endif
    public void SetLayoutHorizontal()
    {
        RectFollowSize();
    }

    public void SetLayoutVertical()
    {
        RectFollowSize();
    }

    protected override void OnRectTransformDimensionsChange()
	{
		RectFollowSize();
	}

	void RectFollowSize()
	{
		if (target == null)
		{
			return;
		}
		if (myRect == null)
		{
			myRect = GetComponent<RectTransform>();
		}
		//Fix Pivot and Anchors;
		myRect.SetPivotAndAnchors(new Vector2(0,1));
        myRect.anchoredPosition = new Vector2(offset.left, -offset.top);
        Vector2 size = new Vector2(myRect.GetSize().x + offset.horizontal, myRect.GetSize().y + offset.vertical);
        if (min != Vector2.zero)
        {
            for (int i = 0; i < 2; i++)
            {
                size[i] = Mathf.Max(min[i], size[i]);
            }
        }
        target.SetSize(size);

	}
}
