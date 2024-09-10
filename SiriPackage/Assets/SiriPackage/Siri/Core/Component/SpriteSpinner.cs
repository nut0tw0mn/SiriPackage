using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SpriteSpinner : MonoBehaviour
{
#pragma warning disable IDE0044 // Add readonly modifier
    [SerializeField] int m_index = 0;
    [SerializeField] Image m_target = null;
    [SerializeField] Button plusBtn = null;
    [SerializeField] Button minusBtn = null;
#pragma warning restore IDE0044 // Add readonly modifier
    [SerializeField]
    private List<Sprite> list = new List<Sprite>();

    [System.Serializable]
    public class SpinnerEvent : UnityEvent<int> { }
    public SpinnerEvent onValueChanged = new SpinnerEvent();

    public int Value
    {
        get { return m_index; }
        set
        {
            this.m_index = value;
            OnValidate();
        }
    }

    // Use this for initialization
    private void Start()
    {
        if (plusBtn)
            plusBtn.onClick.AddListener(() =>
            {
                Value++;
            });
        if (minusBtn)
            minusBtn.onClick.AddListener(() =>
            {
                Value--;
            });
        OnValidate();
    }

    private void OnValidate()
    {
        if (list == null || list.Count == 0)
            return;

        if (m_index < 0)
            m_index = 0;

        plusBtn.interactable = m_index < list.Count - 1;
        minusBtn.interactable = m_index > 0;
        m_target.sprite = list[m_index];
        //Call Event
        onValueChanged.Invoke(Value);
    }
}