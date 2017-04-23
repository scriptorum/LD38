using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Cameraman : MonoBehaviour 
{
	public GameObject stage;
	public float checkFrequency = 0;
	public float speed = 0;
	public float zoomBorder = 0.2f;

	private float elapsed = 0;
	private Vector3 targetPosition;
	private float targetSize;
	private Camera cam;
	private float defaultOrthographicSize;

	void Awake()
	{
		stage.ThrowIfNull();
		cam = gameObject.GetComponent<Camera>();
		defaultOrthographicSize = cam.orthographicSize;
		targetPosition = transform.position;
		targetSize = cam.orthographicSize;
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

			// Known issue: assumes shape not (much) wider than tall
			targetSize  = (bounds.size.y  + zoomBorder) / 2f; 

			if(targetSize <= 0.01f)
			{
				targetPosition = Vector3.zero;
				targetPosition.z = transform.position.z;
				targetSize = defaultOrthographicSize;
			}
			else
			{
				targetPosition = bounds.center;
				targetPosition.z = transform.position.z;
			}
		}

		float rate = Time.deltaTime * speed;
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, rate);
		cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetSize, rate);

	}
}
