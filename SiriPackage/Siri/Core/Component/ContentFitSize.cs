using UnityEngine;
using UnityEngine.UI;
namespace Siri
{
	public class ContentFitSize : MonoBehaviour
	{
		public enum GetPadding
		{
			Inspecter,
			LayoutGroup
		}
		public RectTransform content;
		public RectOffset padding;
		public GetPadding getPadding;
		public ContentSizeFitter.FitMode horizontalFit;
		public ContentSizeFitter.FitMode verticalFit;

		private RectTransform my;
		private bool isInit;

		private void Init()
		{
			if (isInit) return;
			isInit = true;
			my = GetComponent<RectTransform>();
		}

		// Use this for initialization
		void Start()
		{
			Init();
			if (getPadding == GetPadding.LayoutGroup)
				padding = (gameObject).GetComponent<LayoutGroup>().padding; 
		}

		// Update is called once per frame
		private void FixedUpdate()
		{
			if (content != null && content.gameObject.activeSelf)
			{
				if (content.sizeDelta != my.sizeDelta)
				{
					if (horizontalFit == ContentSizeFitter.FitMode.MinSize ||
						horizontalFit == ContentSizeFitter.FitMode.PreferredSize)
					{
						my.sizeDelta = new Vector2(content.sizeDelta.x + padding.left + padding.right,my.sizeDelta.y);
					}
					if (verticalFit == ContentSizeFitter.FitMode.MinSize ||
						verticalFit == ContentSizeFitter.FitMode.PreferredSize)
					{
						my.sizeDelta = new Vector2(my.sizeDelta.x,content.sizeDelta.y + padding.top + padding.bottom);
					}
				}
			}
		}

		public void SetContent(RectTransform rectContent)
		{
			Init();
			content = rectContent;
		}
	}
}
