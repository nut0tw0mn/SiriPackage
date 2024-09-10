using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class FollowRect : MonoBehaviour, ILayoutController
{
    public RectTransform target;
    private RectTransform m_rect;

    public void SetLayoutHorizontal()
    {
        UpdateSize();
    }

    public void SetLayoutVertical()
    {
        UpdateSize();
    }

    private void OnEnable()
    {
        UpdateSize();
    }

    private void UpdateSize()
    {
        if (target == null) return;
        //Debug.Log("UpdateSize");
        if (m_rect == null)
            m_rect = GetComponent<RectTransform>();
        m_rect.pivot = target.pivot;
        m_rect.sizeDelta = target.sizeDelta;
        m_rect.anchoredPosition = target.anchoredPosition;
        m_rect.anchorMax = target.anchorMax;
        m_rect.anchorMin = target.anchorMin;
    }
}