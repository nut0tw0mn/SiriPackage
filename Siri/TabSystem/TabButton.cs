using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Siri.TabSystem
{
	//[RequireComponent(typeof(Image))]
	public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
	{
		public TabGroup tabGroup;
		//public Image Background {
		//	get
		//	{
		//		if(m_Background == null)
		//			m_Background = GetComponent<Image>();
		//		return m_Background;
		//	}
		//}

		//private Image m_Background;

		public UnityEvent onTabSelected;
		public UnityEvent onTabDeselected;
		protected virtual void Start()
		{
			tabGroup.Subscribe(this);
		}

		public virtual void OnPointerClick(PointerEventData eventData) { tabGroup?.OnTabSelected(this); }

		public virtual void OnPointerEnter(PointerEventData eventData) { tabGroup?.OnTabEnter(this); }

		public virtual void OnPointerExit(PointerEventData eventData) { tabGroup?.OnTabExit(this); }

		public virtual void Select() { onTabSelected?.Invoke(); }

		public virtual void Deselect() { onTabDeselected?.Invoke(); }
	}
}
