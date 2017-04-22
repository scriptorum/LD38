using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Terrain : MonoBehaviour 
{
	private Rigidbody2D rb;

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		rb.ThrowIfNull();
	}

	void Update () 
	{
		Vector3 force = Vector3.zero - transform.position;
		rb.AddForce(force);
	}
}
