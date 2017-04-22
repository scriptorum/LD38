using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Cameraman : MonoBehaviour 
{
	public GameObject stage;
	public float checkFrequency = 0;
	public float speed = 0;

	private float elapsed = 0;
	private Vector3 targetPosition;
	private float targetSize;
	private Camera camera;

	void Awake()
	{
		stage.ThrowIfNull();
		camera = gameObject.GetComponent<Camera>();

		targetPosition = transform.position;
		targetSize = camera.orthographicSize;
	}
	
	void Update () 
	{
		elapsed += Time.deltaTime;
		if(elapsed > checkFrequency)
		{
			Bounds bounds = new Bounds();
			foreach(Renderer r in stage.GetComponentsInChildren<Renderer>())
				bounds.Encapsulate(r.bounds);
			elapsed = 0;

			targetPosition = bounds.center;
			targetPosition.z = transform.position.z;
			targetSize  = bounds.size.y / 2;
		}

		float rate = Time.deltaTime * speed;
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, rate);
		camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, targetSize, rate);

	}
}
