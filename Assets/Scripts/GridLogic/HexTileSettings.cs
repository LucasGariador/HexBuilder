using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewTile/GenerationSettings")]
public class HexTileSettings : ScriptableObject
{
    public enum Biomes
    {
        Early,
        Mid,
        Late
    };
    public enum TileType
    {
        MainStation,
        Empty,
        EmptyEvent,
        Asteroids
    }

    public GameObject EmptyTile;
    public GameObject EmptyEventTile;
    public GameObject AsteroidsTile;
    public GameObject MainStation;



    [Range(0, 100)]
    public int EarlyRatio;
    [Range(0, 100)]
    public int MidLandRatio;
    [Range(0, 100)]
    public int LateRatio;

    public GameObject GetTile(TileType tileType)
    {
        return tileType switch
        {
            TileType.Empty => EmptyTile,
            TileType.EmptyEvent => EmptyEventTile,
            TileType.Asteroids => AsteroidsTile,
            TileType.MainStation => MainStation,
            _ => EmptyTile,
        };
    }

    public Dictionary<TileType, float> GetThreshold(Biomes biome)
    {
        return biome switch
        {
            Biomes.Early => thresholdsEarly,
            Biomes.Mid => thresholdsMid,
            Biomes.Late => thresholdsLate,
            _ => thresholdsEarly,
        };
    }

    public Biomes GetRandomBiome()
    {
        float totalPriority = EarlyRatio + MidLandRatio + LateRatio;

        float percentageBiome1 = (float)EarlyRatio / totalPriority;
        float percentageBiome2 = (float)MidLandRatio / totalPriority;
        float percentageBiome3 = (float)LateRatio / totalPriority;


        float randomValue = UnityEngine.Random.value;

        if (randomValue < percentageBiome1)
        {
            return Biomes.Early;
        }
        else if (randomValue < percentageBiome1 + percentageBiome2)
        {
            return Biomes.Mid;
        }
        else
        {
            return Biomes.Late;
        }
    }

    public Dictionary<TileType, float> thresholdsEarly = new Dictionary<TileType, float> { 
        { TileType.Asteroids, 0.2f },
        { TileType.Empty, 1f },
    };

    public Dictionary<TileType, float> thresholdsMid = new Dictionary<TileType, float> {
        { TileType.Asteroids, 0.2f },
        { TileType.Empty, 1f },

    };

    public Dictionary<TileType, float> thresholdsLate = new Dictionary<TileType, float> {
        { TileType.Asteroids, 0.2f },
        { TileType.Empty, 1f },
    };
}
