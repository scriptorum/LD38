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
    public int idleMin;
    public int idleMax;
    public Sprite[] sprites;
    public TerrainBehavior[] behaviors;
}

[System.Serializable]
public struct TerrainBehavior
{
	public ParticleSystem fx; // If set, triggers this particle effect
    public string frames; // If set, triggers the sprite animation, pointing to frames in the sprites array (otherwise first sprite is used)
    public int fps; // If nonzero, overrides default fps when animating the sprite
    public int layer; // When animating the sprite, determines which layer to use (0-2)
    public string sound; // If set, plays a sound when triggered
    public TerrainBehaviorType type; // When is this behavior triggered
}

public enum TerrainBehaviorType
{
    OnStill,     // When the terrain is created, or after any other behavior ends
    OnIdle,         // When the terrain is idle, which is a random amount of time between 0 and idleMax
    OnMouseOver,    // When the mouse enters the terrain trigger
    OnBump,         // When the terrain receives a "bump" over the threshold
    OnDeath,         // When the terrain is removed (if no frames are provided, sprite is hidden)
    OnNull          // This is the null state before first going to still
}

public enum TerrainType
{
	Forest,
	Grass,
	Jungle,
	Desert,
	Tundra,
	Water, 
	Random
}
