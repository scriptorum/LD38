using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class Game : MonoBehaviour 
{
	public GameObject[] terrains;
	public Tracker tracker;
	public MessageBar messageBar;
	public BombManager bombManager;
	public bool running = false;
	public int bombs = 0;

	private Transform stage;
	private int terrainId;
	private int terrainAdded = 0;
	private int terrainRemoved = 0;
	private int minimumTerrain = 4;
	private LevelManager levelManager;
	private float jostleAmount  = 0.1f;

	void Awake()
	{
		stage = transform.Find("Stage");
		stage.ThrowIfNull();
		levelManager = GetComponent<LevelManager>();
		levelManager.ThrowIfNull();
	}

	void Start()
	{
		levelManager.reset();
		reset();
	}

	public void onTerrainRemoved(int count)
	{
		terrainRemoved += count;
		int removeThreshold = terrainAdded - minimumTerrain;
		float pos = (float) terrainRemoved/ (float) removeThreshold;
		tracker.setPosition(Mathf.Max(pos, 0.0f));

		bombs--;
		bombManager.setBombs(bombs);
		if(terrainRemoved >= removeThreshold)
		{
			running = false;
			messageBar.showMessage(levelManager.getCurrentLevel().winMessage, true);
		}
		else if(bombs <= 0)
		{
			messageBar.showMessage("No good! You no bombs! We sad.")
				.queueMessage("We go to SPACE to get more bombs. Press SPACE.", true);
		}
		else messageBar.showMessage("Kill"); // Todo come up with random message set
	}

	public void onSpace()
	{
		// Not running? Level is over. Advance to next level.
		if(!running)
			levelManager.nextLevel(); 
		reset();
	}

	// Super secret cheat
	public void onSkip()
	{
		levelManager.nextLevel();
		reset();
	}

	// Reset the current level
	public void reset()
	{
		messageBar.reset();
		tracker.reset();
		stage.DestroyChildren();

		terrainId = 0;
		terrainRemoved = terrainAdded = 0;		

		Level level = levelManager.getCurrentLevel();
		List<TerrainType> list = level.getTerrainList();
		bombs = level.bombs;		
		bombManager.setBombs(bombs);

		int ring = 0;
		int count = 0;
		int max = 1;
		float ringSize = 0.666f;

		foreach(TerrainType type in list)
		{
			GameObject prefab = (type == TerrainType.Random ? 
				terrains.Rnd() : terrains[(int) type]);
			
			GameObject go = Instantiate(prefab);
			go.name = prefab.name + terrainId++;
			go.transform.parent = stage;

			float magnitude = ringSize * ring;
			float radians = ((float) count / (float) max) * Mathf.PI * 2.0f;
			float zed = (int) type * 0.01f;
			Vector3 pos = new Vector3(Mathf.Cos(radians) * magnitude, Mathf.Sin(radians) * magnitude, zed);
			pos.x += Random.Range(-jostleAmount, jostleAmount);
			pos.y += Random.Range(-jostleAmount, jostleAmount);
			go.transform.position = pos;
			terrainAdded++;

			if(++count >= max)
			{
				count = 0;
				max = ++ring * 6;
			}
		}

		running = true;
		messageBar.showMessage(level.startMessage, level.holdStartMesage);
	}
}
