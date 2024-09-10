using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Converters 
{
	public static Vector3 ToVector3(this string input)
	{
		return StringToVector3(input);
	}

	public static string ToStringFormat(this Vector3 input)
	{
		return Vector3ToString(input);
	}
	public static string ToStringFormat(this Vector2 input)
	{
		return Vector3ToString(input);
	}

	public static Vector3 StringToVector3(string input)
	{
		if (input != null)
		{
			
			var vals = input.Replace("(","").Replace(")","")
				.Split(',').Select(s => s.Trim()).ToArray();
			if (vals.Length == 3)
			{
				Single v1, v2, v3;
				if (Single.TryParse(vals[0], out v1) &&
				    Single.TryParse(vals[1], out v2) &&
				    Single.TryParse(vals[2], out v3))
					return new Vector3(v1, v2, v3);
				else
					throw new ArgumentException();
			}
			else
				throw new ArgumentException();
		}
		else
			throw new ArgumentException();
	}

	public static string Vector3ToString(Vector3 input)
	{
		return $"{input.x},{input.y},{input.z}";
	}

	public static string EncodeBase64(this string text, Encoding encoding = null)
	{
		if (text == null) return null;

		encoding = encoding ?? Encoding.UTF8;
		var bytes = encoding.GetBytes(text);
		return Convert.ToBase64String(bytes);
	}

	public static string DecodeBase64(this string encodedText, Encoding encoding = null)
	{
		if (encodedText == null) return null;

		encoding = encoding ?? Encoding.UTF8;
		var bytes = Convert.FromBase64String(encodedText);
		return encoding.GetString(bytes);
	}
}
