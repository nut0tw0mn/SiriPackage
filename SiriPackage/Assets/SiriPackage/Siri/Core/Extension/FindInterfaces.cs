using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FindInterfaces
{
	public static List<T> Find<T>(bool includeInactive = false)
	{
		List<T> interfaces = new List<T>();
		var rootGameObjects = GameObject.FindObjectsOfType<MonoBehaviour>();

		foreach (var obj in rootGameObjects)
		{
			T[] childInterfaces = obj.GetComponentsInChildren<T>(includeInactive);
			interfaces.AddRange(childInterfaces);
		}

		return interfaces;
	}
}
