using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Terrain : MonoBehaviour 
{
	public TerrainType type;
	private Rigidbody2D rb;
	private List<Terrain> contacts = new List<Terrain>();

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		rb.ThrowIfNull();

		if(!gameObject.name.StartsWith("Terrain" + type.ToString()))
			throw new UnityException(gameObject.name + " is of unexpected type " + type.ToString());
	}

	void Update () 
	{
		Vector3 force = Vector3.zero - transform.position;
		rb.AddForce(force);
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		// Debug.Log(gameObject.name + " ENTER");
		contacts.Add(c.transform.parent.GetComponent<Terrain>());
	}

	void OnTriggerExit2D(Collider2D c)
	{
		// Debug.Log(gameObject.name + " EXIT");
		contacts.Remove(c.transform.parent.GetComponent<Terrain>());
	}

	void OnMouseDown()
	{
		// Find all contiguously adjacent matching contacts 
		List<Terrain> matches = new List<Terrain>();
		matches.Add(this);
		getMatches(matches);

		// If cheat key, just report matches
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			debugTerrainList(matches);
			return;
		}

		// Remove matching terrain
		foreach(Terrain t in matches)
			GameObject.Destroy(t.gameObject);
	}

	// Recursively return contacts that match this terrain
	// Prevent endless loop by ensuring match wasn't already processed
	public void getMatches(List<Terrain> matches)
	{
		foreach(Terrain t in contacts)
			if(t.type == type && !matches.Contains(t)) 
			{
				matches.Add(t);
				t.getMatches(matches);
			}
	}

	private void debugTerrainList(List<Terrain> list)
	{
		string[] str = new string[list.Count];

		int i = 0;
		foreach(Terrain t in list)
			str[i++] = t.gameObject.name;

		Debug.Log(gameObject.name + " -> " + list.Count +  " Contact" + (list.Count == 1 ? "" : "s") + " -> " + string.Join(",", str));
	}
}

public enum TerrainType
{
	Desert,
	Forest,
	Grass,
	Savannah,
	Tundra,
	Water
}
