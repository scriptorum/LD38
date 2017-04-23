using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
	private int levelNum = 0;
	public Level[] levels;
	public Level level;

	public void Awake()
	{
		level = levels[0];
	}

	public void reset()
	{
		levelNum = 0;
	}

	// Gets the current level
	public Level getCurrentLevel()
	{
		return level;
	}

	public Level nextLevel()
	{
		if(++levelNum >= levels.Length)
		{
			level = new Level();
			int numParts = 5 * levelNum;
			level.bombs = numParts / 4;
			level.terrainList = new string('6', numParts);
			level.startMessage = "World " + levelNum + ". "  + level.bombs +  " bombs allotted.";
			level.winMessage = "Success. Plotting another course...";
		}
		else level = levels[levelNum];

		return level;			
	}
}

[System.Serializable]
public struct Level
{
	public string terrainList;
	public string startMessage;
	public string winMessage;
	public int bombs;
	public bool holdStartMesage;
	
	public List<TerrainType> getTerrainList()
	{
		List<TerrainType> list = new List<TerrainType>();
		foreach(char c in terrainList.ToCharArray())
			list.Add((TerrainType) (c - '0'));
 		return list;
	}
}