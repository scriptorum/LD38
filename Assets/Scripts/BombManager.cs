using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class BombManager : MonoBehaviour 
{
	public GameObject prefab;
	public List<Transform> bombs;
	private Vector3 guidePosition;
	private Vector3 guideScale;
	private float spacing = 10f;

	public void Awake()
	{
		Transform guide = transform.Find("BombGuide");
		guide.ThrowIfNull();
		guidePosition = guide.localPosition;
		guideScale = guide.localScale;
	}

	public void reset()
	{
		foreach(Transform bomb in bombs)
			Destroy(bomb.gameObject);
		bombs.Clear();
	}

	public void setBombs(int count)
	{		
		Debug.Assert(count >= 0);

		if(count == 0)
			reset();
			
		// Add bombs
		else if (count > bombs.Count)
		{
			while(bombs.Count < count)
			{
				GameObject go = Instantiate(prefab);
				go.name = "Bomb" + bombs.Count;
				go.transform.SetParent(transform);
				Vector3 pos = guidePosition;
				pos.x += spacing * bombs.Count;
				go.transform.localPosition = pos; 
				go.transform.localScale = guideScale;
				bombs.Add(go.transform);
			}
		}

		// Remove bombs
		else
		{
			while(bombs.Count > count)
			{
				Transform lastBomb = bombs[bombs.Count - 1];
				bombs.Remove(lastBomb);
				Destroy(lastBomb.gameObject);
			}
		}
	}
}
