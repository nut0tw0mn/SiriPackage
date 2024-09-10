using System;
using UnityEngine;

public class CZParallax : MonoBehaviour
{
	public static float GlobalSpeed = 1;
	/// the relative speed of the object
	public float ParallaxSpeed = 0;

	private GameObject _clone;
	private Vector3 _movement;
	private Vector3 _initialPosition;
	private float _width;

	/// <summary>
	/// On start, we store various variables and clone the object
	/// </summary>
	void Start()
	{
		_clone = Instantiate(gameObject);
		CZParallax parallaxComponent = _clone.GetComponent<CZParallax>();
		Destroy(parallaxComponent);

		_width = GetBounds().size.x;
		_initialPosition = transform.position;

		_clone.transform.position = new Vector3(transform.position.x + _width, transform.position.y,
			transform.position.z);
		_clone.transform.SetParent(gameObject.transform);
	}

	private Bounds GetBounds()
	{
		if (GetComponent<Collider>() != null) //3d
		{
			return GetComponent<Collider>().bounds;
		}
		if (GetComponent<BoxCollider2D>() != null) //2d
		{
			return GetComponent<BoxCollider2D>().bounds;
		}

		throw new Exception("The PoolableObject " + gameObject.name +
											" is set as having Collider based bounds but no Collider component can be found.");
	}

	/// <summary>
	/// On Update, we move the object and its clone
	/// </summary>
	void Update()
	{
		// we determine the movement the object and its clone need to apply, based on their speed and the Global Speed
		_movement = Vector3.left * (ParallaxSpeed / 10) * Time.deltaTime * GlobalSpeed;

		// we move both objects
		transform.Translate(_movement);

		// if the object has reached its left limit, we teleport both objects to the right
		if (transform.position.x + _width < _initialPosition.x)
		{
			transform.Translate(Vector3.right * _width);
		}
	}
}