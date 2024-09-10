using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ToggleSprite : MonoBehaviour {
	[SerializeField]
	private bool isOn;

	public bool IsOn
	{
		get
		{
			return isOn;
		}

		set
		{
			isOn = value;
			if (isOn)
				img.sprite = sprOn;
			else
				img.sprite = SprOff;
		}
	}


	public Sprite sprOn;
	public Sprite SprOff;

	private Image img;
	
	void Start()
	{
		img = GetComponent<Image>();
		IsOn = isOn;
	}
}
