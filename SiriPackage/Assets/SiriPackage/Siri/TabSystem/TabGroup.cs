using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Siri.TabSystem
{
	public class TabGroup : MonoBehaviour
	{
		public List<TabButton> tabButtons;
		public List<GameObject> objectsToSwap;
		public bool autoSelect = false;

		[Serializable]
		public class TabEvent : UnityEvent<int> { }
		public TabEvent onSelectedIndex = new TabEvent();

		private TabButton selectedTab;
		public int TabIndex
		{
			get
			{
				if (selectedTab == null || tabButtons == null || !tabButtons.Contains(selectedTab))
					return -1;
				return tabButtons.IndexOf(selectedTab);
			}
		}

		void Start()
		{
			if (autoSelect && selectedTab == null && tabButtons.Count > 0)
			{
				OnTabSelected(tabButtons[0]);
			}
		}

		public virtual void SetSelectIndex(int i)
		{
			if (i >= tabButtons.Count)
				throw new IndexOutOfRangeException();
			//Shift index
			for (var index = i; index < tabButtons.Count; index++)
			{
				var tab = tabButtons[index];
				if (tab.isActiveAndEnabled)
				{
					break;
				}
				i++;
			}

			if (i >= tabButtons.Count)
				i = 0;
			OnTabSelected(tabButtons[i]);
		}
		public virtual void Subscribe(TabButton button)
		{
			if (tabButtons == null)
			{
				tabButtons = new List<TabButton>();
			}
			if(!tabButtons.Contains(button))
				tabButtons.Add(button);
		}

		public virtual void OnTabEnter(TabButton button)
		{
			ResetTab();
			if (selectedTab == null || button != selectedTab)
			{
				//Enter
			}
		}

		public virtual void OnTabExit(TabButton button)
		{
			ResetTab();
		}

		public virtual void OnTabSelected(TabButton button)
		{
			if(button == selectedTab)
				return;
			selectedTab?.Deselect();
			selectedTab = button;
			selectedTab?.Select();

			ResetTab();

			//swap object
			for (var i = 0; i < objectsToSwap.Count; i++)
			{
				objectsToSwap[i].SetActive(i == TabIndex);
			}
			onSelectedIndex?.Invoke(TabIndex);
		}

		public void ResetTab()
		{
			//Reset
		}
	}
}
