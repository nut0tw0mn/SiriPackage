using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class LogKeeper
{
	private static List<string> list = null;
	protected static void Initial()
	{
		if (list == null)
		{
			list = Siri.Services.ReadJson<List<string>>("TextureError") ?? new List<string>();
		}
	}

	public static void Log(string obj, string error)
	{
		Initial(); 

		if (list.Contains(obj))
			return;
		list.Add(obj);
		Siri.Services.WriteJson(JsonUtility.ToJson(obj), "TextureError");
	}
}
