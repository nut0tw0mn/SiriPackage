using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace Siri.Core.Component
{
	/// <summary>
	/// Version 2021.9.21
	/// </summary>
	[ExecuteInEditMode]
	public class ScrollRectSnap : ScrollRectStepBehavior, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		public class m_Event : UnityEvent { }

		[Tooltip("How quickly the GUI snaps to each panel")]
		public float snapSpeed = 0.3f;
		public bool isDrag = true;

		//Data
		protected IEnumerator m_routine;

		//Delegate function
		public m_Event OnBeginDragEvent = new m_Event();
		public m_Event OnDraggingEvent = new m_Event();
		public m_Event OnEndDragEvent = new m_Event();
		public m_Event OnSnapping = new m_Event();
		public m_Event OnSnapped = new m_Event();

		public int Index => m_index;

		protected virtual void Awake()
		{
			// enable movement inertia
			scrollRect.inertia = true;
			Setup();
		}

		public void SnapNormalizedPosition(float normalTarget, float time, Action callback = null)
		{
			m_index = FindNearest(normalTarget, m_points);
			SnapIndex(m_index, time, callback);
		}

		///if(time -1  = snapSpeedDef)
		public void SnapIndex(int index, float time = -1f, Action callback = null)
		{
			m_index = Mathf.Clamp(index, 0, steps - 1);
			var positionTarget = m_points[m_index];
			//Stop routine
			if (m_routine != null && m_routine.MoveNext())
				StopCoroutine(m_routine);
			if (time < 0)
				time = snapSpeed;
			if (gameObject.activeInHierarchy)
			{
				m_routine = COTween(ScrollNormalizedPosition, positionTarget, time, callback);
				StartCoroutine(m_routine);
			}
			else
			{
				ScrollNormalizedPosition = positionTarget;
			}
		}

		#region Logic
		//Event
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (!isDrag) return;
			//-----------------
			if (m_routine != null && m_routine.MoveNext())
				StopCoroutine(m_routine);
			//CallEvent
			OnBeginDragEvent?.Invoke();
			//-----------------
		}
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!isDrag) return;
			//CallEvent
			OnDraggingEvent?.Invoke();
		}
		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (!isDrag) return;
			int index = FindNearest(ScrollNormalizedPosition, m_points);
			SnapIndex(index, snapSpeed);
			//Call Event
			OnEndDragEvent?.Invoke();
		}

		IEnumerator COTween(float a, float b, float t, Action callback)
		{
			float cnt = 0;
			while (cnt < 1)
			{
				cnt += Time.deltaTime / t;
				ScrollNormalizedPosition = Mathf.Lerp(a, b, Mathf.SmoothStep(0f, 1, cnt));
				OnSnapping?.Invoke();

				yield return null;
			}

			ScrollNormalizedPosition = b;
			//Call Event
			OnSnapped?.Invoke();
			callback?.Invoke();
		}

		#endregion
	}

}

