using UnityEngine;

public static class RectTransformExtensions
{
	public static void SetAndStretchToParentSize(this RectTransform _mRect, RectTransform _parent)
    {
        _mRect.anchoredPosition = _parent.position;
        _mRect.anchorMin = new Vector2(0, 0);
        _mRect.anchorMax = new Vector2(1, 1);

        _mRect.pivot = new Vector2(0.5f, 0.5f);
        _mRect.sizeDelta = _parent.rect.size;
        _mRect.transform.SetParent(_parent);
        _mRect.All(_mRect, 0, 0, 0, 0);
    }

    public static RectTransform All(this RectTransform rt, RectTransform objRec, float left, float right, float top,
        float down)
    {
        rt.offsetMin = new Vector2(objRec.GetWidth() * left, objRec.offsetMin.y);
        //Debug.Log("left"+rt.offsetMin);
        rt.offsetMax = new Vector2(objRec.GetWidth() * -right, objRec.offsetMax.y);
        //Debug.Log("right" + rt.offsetMax);
        rt.offsetMax = new Vector2(objRec.offsetMax.x, -top * objRec.GetHeight());
        //Debug.Log("top" + rt.offsetMax);
        rt.offsetMin = new Vector2(objRec.offsetMin.x, down * objRec.GetHeight());
        //Debug.Log("down" + rt.offsetMin);

        return rt;
    }

    public static RectTransform AllRect(this RectTransform rt, float left, float right, float bot, float top)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        rt.offsetMin = new Vector2(-right, rt.offsetMin.y);
        rt.offsetMin = new Vector2(rt.offsetMin.x, bot);
        rt.offsetMax = new Vector2(rt.offsetMax.x, top);
        return rt;
    }

    public static RectTransform Left(this RectTransform rt, float x)
    {
        rt.offsetMin = new Vector2(x, rt.offsetMin.y);
        return rt;
    }

    public static RectTransform Right(this RectTransform rt, float x)
    {
        rt.offsetMax = new Vector2(-x, rt.offsetMax.y);
        return rt;
    }

    public static RectTransform Bottom(this RectTransform rt, float y)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, y);
        return rt;
    }

    public static RectTransform Top(this RectTransform rt, float y)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -y);
        return rt;
    }
}