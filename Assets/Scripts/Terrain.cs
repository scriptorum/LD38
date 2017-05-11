using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Terrain : MonoBehaviour 
{
	public ParticleSystem fx;
	private TerrainData data;
	private Rigidbody2D rb;
	private List<Terrain> contacts = new List<Terrain>();
	private static float SFX_IMPACT_MIN = 0.5f;
	private Game game;
	private Anim lowAnim;
	private Anim highAnim;
	private float nextIdle = 9999f;

	void Awake()
	{
		enabled = false;
	}

	void Start()
	{
		if(lowAnim != null)
			lowAnim.Freeze("idle-low");
		if(highAnim != null)
			highAnim.Freeze("idle-high");		
	}

	public void init(TerrainData data)
	{
		this.data = data;

		SpriteRenderer lowSR = gameObject.GetChild("Lower").GetComponent<SpriteRenderer>();
		if(data.lowSprites.Length == 1)
			lowSR.sprite = data.lowSprites[0];
		else if(data.lowSprites.Length > 0)
		{
			lowAnim = lowSR.gameObject.GetComponent<Anim>();
			lowAnim.enabled = true;
			lowAnim.sequences.Add(data.lowIdle);
			lowAnim.sequences.Add(data.lowBump);
			lowAnim.frames.AddRange(data.lowSprites);
			lowAnim.UpdateCache();
		}
		else lowSR.enabled = false;

		SpriteRenderer highSR = gameObject.GetChild("Upper").GetComponent<SpriteRenderer>();
		if(data.highSprites.Length == 1)
			highSR.sprite = data.highSprites[0];
		else if(data.highSprites.Length > 0)
		{
			highAnim = highSR.gameObject.GetComponent<Anim>();
			highAnim.enabled = true;
			highAnim.sequences.Add(data.highIdle);
			highAnim.sequences.Add(data.highBump);
			highAnim.frames.AddRange(data.highSprites);
			highAnim.UpdateCache();
			highAnim.onLoop.AddListener(pauseAnimation);
		}
		else highSR.enabled = false;

		rb = gameObject.GetComponent<Rigidbody2D>();
		rb.ThrowIfNull();

		game = GameObject.Find("/Game").GetComponent<Game>();
		game.ThrowIfNull();

		enabled = true;
		setNextIdle();
	}

	void Update()
	{
		nextIdle -= Time.deltaTime;
		if(nextIdle <= 0f)
		{
			if(highAnim != null && highAnim.paused)
				highAnim.Play("idle-high");
			setNextIdle();			
		}
	}

	private void setNextIdle()
	{
		nextIdle = Random.Range(0f, 5.0f);
	}

	public void pauseAnimation(Anim anim)
	{
		anim.Pause();
	}	

	void FixedUpdate () 
	{
		Vector3 force = Vector3.zero - transform.position;
		rb.AddForce(force);
	}

	void OnCollisionEnter2D(Collision2D c)
	{
		if(c.relativeVelocity.magnitude > SFX_IMPACT_MIN)
		{
			SoundManager.instance.Play(data.type.ToString());
			if(highAnim != null)  highAnim.Play("bump-high");
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		Debug.Log(gameObject.name + " contacts " + c.transform.parent.name);
		contacts.Add(c.transform.parent.GetComponent<Terrain>());
	}

	void OnTriggerExit2D(Collider2D c)
	{
		Debug.Log(gameObject.name + " loses contact with " + c.transform.parent.name);
		contacts.Remove(c.transform.parent.GetComponent<Terrain>());
	}

	public void OnMouseDown()
	{
		if(!game.running || game.bombs <= 0)
			return;

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

		SoundManager.instance.Play("clink");

		// Remove matching terrain
		foreach(Terrain t in matches)
			t.explode();

		// No point caching this
		game.onTerrainRemoved(matches.Count);
	}

	public void explode()
	{
		 Instantiate(fx, transform.position, Quaternion.identity);
		GameObject.Destroy(gameObject);
	}

	// Recursively return contacts that match this terrain
	// Prevent endless loop by ensuring match wasn't already processed
	public void getMatches(List<Terrain> matches)
	{
		foreach(Terrain t in contacts)
			if(t.data.type == data.type && !matches.Contains(t)) 
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
			str[i++] = t.gameObject.name + " (" + t.data.type + ")";

		Debug.Log(gameObject.name + "(" + data.type + ")" + " -> " + list.Count +  " Contact" + (list.Count == 1 ? "" : "s") + " -> " + string.Join(",", str));
	}
}
