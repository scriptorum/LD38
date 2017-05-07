using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spewnity;

public class TerrainCatalog : MonoBehaviour 
{
    public TerrainData[] terrainData;

    private Dictionary<TerrainType, TerrainData> dict = new Dictionary<TerrainType, TerrainData>();

    void Awake()
    {
        // Cache terrain data by type
        for(int i = 0; i < terrainData.Length; i++)
            dict.Add(terrainData[i].type, terrainData[i]);
    }

    public TerrainData GetTerrainByType(TerrainType type)
    {
        if(type == TerrainType.Random)
            return terrainData.Rnd();
        return dict[type];
    }

}

[System.Serializable]
public struct TerrainData
{   
    public TerrainType type;
    public Sprite sprite;
}


public enum TerrainType
{
	Forest,
	Grass,
	Savannah,
	Desert,
	Tundra,
	Water, 
	Random
}
