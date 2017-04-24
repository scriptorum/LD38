using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMonitor : MonoBehaviour 
{
	private ParticleSystem ps;

	void Awake()
	{
		ps = gameObject.GetComponent<ParticleSystem>();		
	}

	void Update()
	{
		if(!ps.IsAlive())
			Destroy(gameObject);
	}
}
