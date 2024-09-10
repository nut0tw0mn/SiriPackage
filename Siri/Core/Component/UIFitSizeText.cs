using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Siri
{
	[RequireComponent(typeof(Text))]
	[RequireComponent(typeof(LayoutElement))]
	public class UIFitSizeText : MonoBehaviour
	{
		Text myText;
		LayoutElement layoutElm;
		bool isInit;
		float x = 0;
		float y = 0;

		void Init()
		{
			if (isInit) return;
			isInit = true;
			myText = GetComponent<Text>();
			layoutElm = GetComponent<LayoutElement>();
		}

		public void CalculateSize()
		{
			Init();
			x = layoutElm.minWidth;
			y = layoutElm.minHeight;
			if (x < myText.preferredWidth)
				x = myText.preferredWidth;
			if (y < myText.preferredHeight)
				y = myText.preferredHeight;
			if (x > layoutElm.preferredWidth && layoutElm.preferredWidth > -1)
				x = layoutElm.preferredWidth;
			if (y > layoutElm.preferredHeight && layoutElm.preferredHeight > -1)
				y = layoutElm.preferredHeight;

			myText.rectTransform.sizeDelta = new Vector2(x,y);
		}
	}
}
