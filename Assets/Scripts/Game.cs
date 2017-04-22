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

		for(int i = 0; i < (1 + 6 + 12 + 18); i++)
		{
			GameObject prefab = terrains.Rnd();
			GameObject go = Instantiate(prefab);
			go.name = prefab.name + terrainId++;
			go.transform.parent = stage;
			go.transform.position = new Vector3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight));
		}

	}
}
