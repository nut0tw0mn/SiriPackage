using System.Collections;
using UnityEngine;

namespace Siri.Core.Component
{
	public class Rotate : MonoBehaviour
	{
		public enum Axis
		{
			X,Y,Z
		}
		public bool clockwise;
		public float speed = 1;
		public Axis axis = Axis.Z;

		Vector3 angle;

		void Start()
		{
			angle = transform.eulerAngles;
		}

		void Update()
		{
			var value = Time.deltaTime * 100 * speed;
			if (clockwise)
				value *= -1;
			angle[(int)axis] += value;
			transform.eulerAngles = angle;
		}

	}

}