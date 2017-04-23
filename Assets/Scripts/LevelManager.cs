using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 
{
	private int level = 0;
	public Level[] levels;

	public void reset()
	{
		level = 0;
	}

	// Gets the current level
	public Level getCurrentLevel()
	{
		Level l = levels[level];
		l.lastLevel = level >= levels.Length - 1;
		return l;
	}

	public Level nextLevel()
	{
		if(++level >= levels.Length)
			throw new UnityException("Cannot advance to level " + level);
		return getCurrentLevel();
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
	
	[HideInInspector]
	public bool lastLevel;

	public List<TerrainType> getTerrainList()
	{
		List<TerrainType> list = new List<TerrainType>();
		foreach(char c in terrainList.ToCharArray())
			list.Add((TerrainType) (c - '0'));
 		return list;
	}
}