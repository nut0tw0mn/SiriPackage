using UnityEngine;
namespace Siri
{
	public class UIRotate : MonoBehaviour
	{
		public bool clockwise;
		public float speed = 1;

		private RectTransform rectTrf;

		void Start()
		{
			rectTrf = GetComponent<RectTransform>();
		}
		// Update is called once per frame
		void FixedUpdate()
		{
			if (clockwise)
				rectTrf.Rotate(0,0,180 * Time.deltaTime * speed);
			else
				rectTrf.Rotate(0,0,-180 * Time.deltaTime * speed);

		}
	}
}
