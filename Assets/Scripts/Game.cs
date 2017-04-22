using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Game : MonoBehaviour 
{
	public GameObject[] terrains;
	private Transform stage;
	private int terrainId;

	void Awake()
	{
		stage = transform.Find("Stage");
		stage.ThrowIfNull();
	}

	void Start()
	{
		reset();
	}

	public void reset()
	{
		stage.DestroyChildren();

		float halfHeight = Camera.main.orthographicSize;
		float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
		terrainId = 0;

		// For concentric ring n > 0 (just 1 ball in 0), ball count is n*6 (1, 6, 12, 18...)
		// So for 4 rings (0-3), count=1+Sum(n=1->3)6n

		for(int i = 0; i < 200; i++)
		{
			GameObject prefab = terrains.Rnd();
			GameObject go = Instantiate(prefab);
			go.name = prefab.name + terrainId++;
			go.transform.parent = stage;
			go.transform.position = new Vector3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight));
		}

	}
}
