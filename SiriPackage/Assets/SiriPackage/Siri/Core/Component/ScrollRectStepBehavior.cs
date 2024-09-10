using UnityEngine;
using UnityEngine.UI;

namespace Siri.Core.Component
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(ScrollRect))]
	public abstract class ScrollRectStepBehavior: MonoBehaviour
	{
		[SerializeField][Tooltip("how many steps are there within the content")]
		private int m_Steps = 1;
		[SerializeField][Tooltip("Dimension2D")]
		protected CapsuleDirection2D dimension;

		protected float[] m_points;
		protected int m_index;
		private ScrollRect m_ScrollRect;

		public int steps => m_Steps;
		public ScrollRect scrollRect
		{
			get
			{
				if (!m_ScrollRect) 
					m_ScrollRect = GetComponent<ScrollRect>();
				return m_ScrollRect;
			}
		}
		public float ScrollNormalizedPosition
		{
			get => (int)dimension == 1
				? scrollRect.horizontalNormalizedPosition
				: scrollRect.verticalNormalizedPosition;
			protected set {
				if ((int)dimension == 1)
				{
					scrollRect.horizontalNormalizedPosition = value;
				}
				else
				{
					scrollRect.verticalNormalizedPosition = value;
				}
			}
		}

		public void SetStep(int amount)
		{
			m_Steps = amount;
			Setup();
		}

		protected virtual void Setup()
		{
			if (steps > 0)
			{
				m_points = new float[steps];
				float stepSize = 1 / (float)(steps - 1);
				for (int i = 0; i < steps; i++)
				{
					m_points[i] = i * stepSize;
				}
			}
			else
			{
				m_points[0] = 0;
			}

			m_index = FindNearest(ScrollNormalizedPosition, m_points);
		}

		protected int FindNearest(float f, float[] array)
		{
			float distance = Mathf.Infinity;
			int output = 0;
			for (int i = 0; i < array.Length; i++)
			{
				//BetterApproximate
				if (Mathf.Abs(array[i] - f) < distance)
				{
					distance = Mathf.Abs(array[i] - f);
					output = i;
				}
			}

			return output;
		}
	}
}
