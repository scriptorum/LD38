using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Game : MonoBehaviour 
{
	public GameObject[] terrains;
	private Transform stage;

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

		for(int i = 0; i < (1 + 6 + 12 + 18); i++)
		{
			GameObject prefab = terrains.Rnd();
			GameObject go = Instantiate(prefab);
			go.transform.parent = stage;
			go.transform.position = new Vector3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight));
		}

	}
}
