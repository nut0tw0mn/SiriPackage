using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Siri.Core.Component
{
	public class ScrollRectSwipe : ScrollRectSnap
	{
		public float percentThreshold = 0.2f;
		public RectTransform m_Template;

		//Data
		private float elementSize;

		private int axis => dimension == CapsuleDirection2D.Horizontal ? 0 : 1;

		protected override void Awake()
		{
			// enable movement inertia
			scrollRect.inertia = true;
		}

		void Start()
		{
			CalculateElementSize();
			Setup();
		}

		#region Logic

		//Event
		public override void OnEndDrag(PointerEventData eventData)
		{
			if (!isDrag) return;

			if (m_routine != null && m_routine.MoveNext())
				StopCoroutine(m_routine);

			////code for screen only
			//var screen = axis == 0 ? Screen.width : Screen.height;

			//percentage for elementSize
			float percentage = Mathf.Abs((eventData.pressPosition[axis] - eventData.position[axis]) / elementSize);
			if (percentage > 1)
			{
				SnapNormalizedPosition(ScrollNormalizedPosition, snapSpeed);
			}
			else if (percentage >= percentThreshold)
			{
				//var dir = _endPos[axis] - _startPos[axis];
				var dir = eventData.pressPosition[axis] - eventData.position[axis];

				if (dir > 0)
					m_index++;
				else if (dir < 0)
					m_index--;

				//Snap
				SnapIndex(m_index, snapSpeed);
			}
			else
			{
				//Snap
				SnapIndex(m_index, snapSpeed);
			}
			//Call Event
			OnEndDragEvent?.Invoke();

		}

		private void CalculateElementSize()
		{
			var layoutGroup = scrollRect.content.GetComponent<HorizontalOrVerticalLayoutGroup>();
			if (layoutGroup == null)
				throw new NullReferenceException(
					"Failed to get HorizontalOrVerticalLayoutGroup assigned to ScrollRect's content. ScrollRectSwipe won't work as expected.");
			if (m_Template == null)
				throw new NullReferenceException();

			var t = m_Template;
			var pt = m_Template.root.GetComponent<RectTransform>();
			var newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
				t.anchorMin.y + t.offsetMin.y / pt.rect.height);
			var newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
				t.anchorMax.y + t.offsetMax.y / pt.rect.height);

			var screen = axis == 0 ? Screen.width : Screen.height;
			var spacing = layoutGroup.spacing / pt.rect.width * screen;
			elementSize = (newAnchorsMax[axis] - newAnchorsMin[axis]) * screen;
			elementSize += spacing;
		}
		#endregion
	}
}
