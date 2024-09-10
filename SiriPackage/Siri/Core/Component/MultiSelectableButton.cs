using UnityEngine;
using UnityEngine.UI;

namespace Siri {
    public class MultiSelectableButton : Button {
        [SerializeField] private Selectable[] selectables;

       protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
        }

        //public Button.ButtonClickedEvent onClick { get { Init(); return m_btn.onClick; } set { Init(); m_btn.onClick = value; } }

    } }
