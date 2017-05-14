using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

// TODO Instead of having three fixed layers and a TerrainBehavior.layer property, I should instantiate my layers, and assume
// each behavior gets its own new layer with its own Anim and SpriteRenderer (assuming frames are supplied)
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
	private SpriteRenderer[] srLayers;
	private Anim[] animLayers;

	void Awake()
	{
		enabled = false;

		GameObject go0 = gameObject.GetChild("Layer0");
		GameObject go1 = gameObject.GetChild("Layer1");
		GameObject go2 = gameObject.GetChild("Layer2");

		srLayers = new SpriteRenderer[] { go0.GetComponent<SpriteRenderer>(), 
			go1.GetComponent<SpriteRenderer>(), go2.GetComponent<SpriteRenderer>() };				

		animLayers = new Anim[] { go0.GetComponent<Anim>(), 
			go1.GetComponent<Anim>(), go2.GetComponent<Anim>() };		

		rb = gameObject.GetComponent<Rigidbody2D>();
		rb.ThrowIfNull();

		game = GameObject.Find("/Game").GetComponent<Game>();
		game.ThrowIfNull();	
	}

	// Should be theoretically called before Start()!
	public void init(TerrainData initData)
	{
		this.data = initData;

		// Provide idle max default
		if(data.idleMax <= 0)
			data.idleMax = 5;

		if(data.sprites.Length == 0)
			throw new UnityException("No sprites defined for " + name + " (" + data.type + ")");
		srLayers[0].sprite = data.sprites[0]; // use first sprite as default value for first layer's sprite

		foreach(TerrainBehavior tb in data.behaviors)
		{
			Anim anim = animLayers[tb.layer];			
			SpriteRenderer sr = srLayers[tb.layer];
			sr.sprite = null;

			if(tb.frames != "")
			{
				sr.enabled = true;
				anim.enabled = true;
			}

			// if(tb.type == TerrainBehaviorType.OnIdle)
			animLayers[tb.layer].onLoop.AddListener(onAnimLoop);

			if(tb.frames != "")
				defineSequence(animLayers[tb.layer], tb.type.ToString().ToLower(), tb.frames, tb.fps);
		}

		for(int i = 0; i < 3; i++)
		{
			animLayers[i].frames = new List<Sprite>(data.sprites);
			animLayers[i].UpdateCache(); // Process defined anim sequences
			srLayers[i].sprite = data.sprites[0]; // Not sure this makes the most sense			
		}

		enabled = true;
		setNextIdle();
	}

	private void defineSequence(Anim anim, string name, string frames, int fps)
	{
		if(frames == null || frames == "")
			frames = "0";

		if(fps <= 0)
			fps = 30;

		anim.Add(name, frames, fps);
	}

	void Start()
	{
		Debug.Assert(enabled);
		processBehavior(TerrainBehaviorType.OnStill);
	}

	// This should not be called until init completes.
	void Update()
	{			
		Debug.Assert(enabled);
		nextIdle -= Time.deltaTime;
		if(nextIdle <= 0f)
		{
			processBehavior(TerrainBehaviorType.OnIdle);
			setNextIdle();			
		}
	}

	private void processBehavior(TerrainBehavior tb)
	{
		if(tb.fx != null)
			Instantiate(tb.fx, transform.position, Quaternion.identity);

		if(tb.sound != null && tb.sound != "")
			SoundManager.instance.Play(tb.sound);

		string seq = tb.type.ToString().ToLower();
		Anim anim = animLayers[tb.layer];
		if(anim.IsCached(seq))
		{
			if(seq == TerrainBehaviorType.OnStill.ToString().ToLower() && anim.sequenceName == seq)
				return;
			anim.Play(seq);
		}
	}

	public void processBehavior(TerrainBehaviorType type)
	{
		foreach(TerrainBehavior tb in data.behaviors)
			if(tb.type == type)
				processBehavior(tb);
	}
	
	private void setNextIdle()
	{
		Debug.Assert(data.idleMax > 0);
		nextIdle = Random.Range(data.idleMin, data.idleMax);
	}

	public void onAnimLoop(Anim anim)
	{
		if(anim.sequenceName == TerrainBehaviorType.OnStill.ToString().ToLower())
			anim.Pause();

		else processBehavior(TerrainBehaviorType.OnStill);

		setNextIdle();
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
			string soundName = data.type.ToString();
			SoundManager.instance.Play(soundName); // Play  bump sound for each terrains 
			processBehavior(TerrainBehaviorType.OnBump);
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		if(!game.running || game.bombs <= 0)
			return;

		contacts.Add(c.transform.parent.GetComponent<Terrain>());
	}

	void OnTriggerExit2D(Collider2D c)
	{
		if(!game.running || game.bombs <= 0)
			return;

		contacts.Remove(c.transform.parent.GetComponent<Terrain>());
	}

	public void OnMouseEnter()
	{
		processBehavior(TerrainBehaviorType.OnMouseOver);
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
			t.die();

		// No point caching this
		game.onTerrainRemoved(matches.Count);
	}

	public void die()
	{		
		processBehavior(TerrainBehaviorType.OnDeath);
		Instantiate(fx, transform.position, Quaternion.identity); // basic death fx for all terrains
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
