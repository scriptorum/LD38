using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class BombManager : MonoBehaviour 
{
	public GameObject bombPrefab;
	public List<GameObject> bombs;
	private Vector2 guide;
	private float spacing = 10f;

	public void Awake()
	{
		guide = transform.Find("BombGuide").GetComponent<RectTransform>().anchoredPosition;
	}

	public void reset()
	{
		foreach(GameObject bomb in bombs)
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
				GameObject go = Instantiate(bombPrefab, bombPrefab.transform.position, bombPrefab.transform.rotation);				
				go.transform.SetParent(transform, false);
				go.name = "Bomb" + bombs.Count;

				Vector2 pos = guide;
				pos.x += spacing * bombs.Count;
				go.GetComponent<RectTransform>().anchoredPosition = pos;

				bombs.Add(go);
			}
		}

		// Remove bombs
		else
		{
			while(bombs.Count > count)
			{
				GameObject lastBomb = bombs[bombs.Count - 1];
				bombs.Remove(lastBomb);
				Destroy(lastBomb);
			}
		}
	}
}
