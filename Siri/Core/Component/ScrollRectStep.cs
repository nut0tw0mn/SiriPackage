using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace Siri.Core.Component
{
	/// <summary>
	/// Version 2023.7.04
	/// </summary>
	public class ScrollRectStep : ScrollRectStepBehavior
	{
		public class m_Event : UnityEvent { }
		public m_Event OnNextStep = new m_Event();
		public int Index => m_index;
		public int lastIndex;

		protected virtual void Awake()
		{
			Setup();
			scrollRect.onValueChanged.AddListener(OnValueChanged);
		}

		protected override void Setup()
		{
			base.Setup();
			lastIndex = Index;
		}

		private void OnValueChanged(Vector2 nor)
		{
			m_index = FindNearest(ScrollNormalizedPosition, m_points);
			if (lastIndex != m_index)
			{
				lastIndex = m_index;
				OnNextStep.Invoke();
			}
		}


	}

}

