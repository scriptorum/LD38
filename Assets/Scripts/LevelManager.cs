﻿using System.Collections;
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
			level.bombs = (int) (levelNum * 1.25f);
			int numParts = (int) (4f * level.bombs);

			// Early missions are hard, here's an extra bomb
			if(levelNum <= 14)
				level.bombs++;

			level.terrainList = "";
			for(int i = 0; i < numParts; i++)
				level.terrainList += Random.Range(0, 6).ToString();
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