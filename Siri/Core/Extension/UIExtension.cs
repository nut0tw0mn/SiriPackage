using System;
using UnityEngine;
using UnityEngine.UI;

namespace Siri
{
	public static class UIExtension
	{
		public static Vector2 SizeToParent(RawImage image, float padding = 0)
		{
			float w = 0, h = 0;

			var imageTransform = image.rectTransform;
			var parent = imageTransform.parent?.GetComponent<RectTransform>();

			if (image.texture != null)
			{
				if (!parent) { return imageTransform.sizeDelta; } 
				padding = 1 - padding;
				float ratio = image.texture.width / (float)image.texture.height;
				var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
				if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
				{
					bounds.size = new Vector2(bounds.height, bounds.width);
				}
				h = bounds.height * padding;
				w = h * ratio;
				if (w > bounds.width * padding)
				{ 
					w = bounds.width * padding;
					h = w / ratio;
				}
			}
			imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
			imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
			return imageTransform.sizeDelta;
		}

		public static float GetRatio(this Texture2D texture)
		{
			if(texture == null)
				throw new NullReferenceException();
			return  texture.width / (float)texture.height;
		}
		public static float GetRatio(this Texture texture)
		{
			if (texture == null)
				throw new NullReferenceException();
			return texture.width / (float)texture.height;
		}
		public static float GetRatio(this Sprite texture)
		{
			if (texture == null)
				throw new NullReferenceException();
			return texture.rect.width / (float)texture.rect.height;
		}

		public static bool LimitWidthText(Text textComponent,float limitW)
		{
			TextGenerationSettings settings = textComponent.GetGenerationSettings(textComponent.rectTransform.rect.size);
			float width = textComponent.cachedTextGenerator.GetPreferredWidth(textComponent.text, settings);

			if (width > limitW)
			{
				var le = textComponent.GetComponentExtensions<LayoutElement>();
				le.preferredWidth = limitW;
				return true;
			}

			return false;
		}

		public static bool LimitWidthText(Text textComponent, string text ,  float limitW)
		{
			TextGenerationSettings settings = textComponent.GetGenerationSettings(textComponent.rectTransform.rect.size);
			float width = textComponent.cachedTextGenerator.GetPreferredWidth(text, settings);

			var le = textComponent.GetComponentExtensions<LayoutElement>();

			if (width > limitW)
			{
				le.preferredWidth = limitW;
				return true;
			}

			le.preferredWidth = -1;
			return false;
		}

		public static void RefreshLayoutGroup(this RectTransform transform)
		{
			foreach (var layoutGroup in transform.GetComponentsInChildren<LayoutGroup>())
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
			}
		}
	}
}
