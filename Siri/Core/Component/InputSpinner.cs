using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InputSpinner : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] InputField m_input;
    [SerializeField] Button plusBtn = null;
    [SerializeField] Button minusBtn = null;
    [SerializeField] Vector2 MinMax = new Vector2(0, 1);

    [System.Serializable]
    public class SpinnerEvent : UnityEvent<int> { }
    public SpinnerEvent onValueChanged = new SpinnerEvent();

    [System.Serializable]
    public class ButtonEvent : UnityEvent { }

    public ButtonEvent onNext = new ButtonEvent();
    public ButtonEvent onPrevious = new ButtonEvent();


    public int Value
    {
        get => value;
        set
        {
            this.value = value;
            OnValidate();
            CallEvent();
        }
    }

    public void SetMinMax(Vector2 minmax)
    {
        MinMax = minmax;
        OnValidate();
    }
    // Use this for initialization
    private void Awake()
    {
        plusBtn?.onClick.AddListener(() =>
        {
            value++;
            OnValidate();
            CallEvent();
            onNext?.Invoke();
        });
        minusBtn?.onClick.AddListener(() =>
        {
            value--;
            OnValidate();
            CallEvent();
            onPrevious?.Invoke();
        });

        m_input?.onEndEdit.AddListener(word =>
        {
            value = int.Parse(word);
            if (value < MinMax[0])
                value = (int)MinMax[0];
            else if (value > MinMax[1])
                value = (int)MinMax[1];
            OnValidate();
            CallEvent();
        });


        OnValidate();
    }

    private void CallEvent()
    {
        onValueChanged.Invoke(Value);
    }

    private void OnValidate()
    {
        if (MinMax[0] > MinMax[1])
            MinMax[1] = MinMax[0];

        if (value < MinMax[0])
            value = (int)MinMax[0];

        if (plusBtn)
            plusBtn.interactable = value < MinMax[1];
        if (minusBtn)
            minusBtn.interactable = value > MinMax[0];

        if (m_input)
            m_input.text = value.ToString();

    }
}