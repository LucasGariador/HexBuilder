using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "NewTile/GenerationSettings")]
public class HexTileSettings : ScriptableObject
{
    public enum Biomes
    {
        GreenLand,
        Desert,
        Stone
    };
    public enum TileType
    {
        Land,
        Water,
        Mountains,
        Hill,
        Forest,
        HillForest,
        Island,
        Archery,
        Castle,
        House,
        Mine,
        Port,
        Sheep,
        Smelter,
        Village,
        Wall,
        WizardTower,
        Sand,
        SandDesert,
        SandRocks,
        StoneRocks, 
        Stone
    }

    public GameObject LandTile;
    public GameObject WaterTile;
    public GameObject MountainsTile;
    public GameObject HillTile;
    public GameObject ForestTile;
    public GameObject IslandTile;
    public GameObject HillForestTile;
    public GameObject SmelterTile;
    public GameObject VillageTile;
    public GameObject WallTile;
    public GameObject WizardTowerTile;
    public GameObject ArcheryTile;
    public GameObject HouseTile;
    public GameObject CastleTile;
    public GameObject SheepTile;
    public GameObject PortTile;
    public GameObject MineTile;
    public GameObject SandTile;
    public GameObject SandDesertTile;
    public GameObject SandRockseTile;
    public GameObject StoneRocksTile;
    public GameObject StoneTile;

    [Range(0, 98)]
    public float DesertRatio;
    [Range(0, 98)]
    public float GreenLandRatio;
    [Range(0, 98)]
    public float StoneRatio;

    public GameObject GetTile(TileType tileType)
    {
        return tileType switch
        {
            TileType.Land => LandTile,
            TileType.Water => WaterTile,
            TileType.Island => IslandTile,
            TileType.Mountains => MountainsTile,
            TileType.Hill => HillTile,
            TileType.Forest => ForestTile,
            TileType.HillForest => HillForestTile,
            TileType.Smelter => SmelterTile,
            TileType.Wall => WallTile,
            TileType.Village => VillageTile,
            TileType.WizardTower => WizardTowerTile,
            TileType.Archery => ArcheryTile,
            TileType.House => HouseTile,
            TileType.Castle => CastleTile,
            TileType.Sheep => SheepTile,
            TileType.Port => PortTile,
            TileType.Mine => MineTile,
            TileType.Sand => SandTile,
            TileType.SandDesert => SandDesertTile,
            TileType.SandRocks => SandRockseTile,
            TileType.Stone => StoneTile,
            TileType.StoneRocks => StoneRocksTile,
            _ => null,
        };
    }

    public Dictionary<TileType, float> GetThreshold(Biomes biome)
    {
        return biome switch
        {
            Biomes.GreenLand => thresholdsGreenLand,
            Biomes.Desert => thresholdsDesert,
            Biomes.Stone => thresholdsRock,
            _ => thresholdsGreenLand,
        };
    }

    public Biomes GetRandomBiome(int v)
    {
        float totalPriority = DesertRatio + StoneRatio + GreenLandRatio;

        // Calcula los porcentajes en función de las prioridades
        float percentageBiome1 = (float)DesertRatio / totalPriority;
        float percentageBiome2 = (float)StoneRatio / totalPriority;
        float percentageBiome3 = (float)GreenLandRatio / totalPriority;

        // Genera un número aleatorio entre 0 y 1
        float randomValue = UnityEngine.Random.value;

        // Asigna biomas en función de los porcentajes calculados
        if (randomValue < percentageBiome1)
        {
            return Biomes.Desert;
        }
        else if (randomValue < percentageBiome1 + percentageBiome2)
        {
            return Biomes.Stone;
        }
        else
        {
            return Biomes.GreenLand;
        }
        return null;
    }

    public Dictionary<TileType, float> thresholdsGreenLand = new Dictionary<TileType, float> { 
        { TileType.Island, 0.04f },
        { TileType.Water, 0.3f },
        { TileType.Forest, 0.36f },
        { TileType.Land, 0.75f },
        { TileType.HillForest,0.77f },
        { TileType.Hill, 0.85f },
        { TileType.Mountains, 1.0f }

    };

    public Dictionary<TileType, float> thresholdsDesert = new Dictionary<TileType, float> {
        {TileType.Sand, 0.5f},
        { TileType.SandDesert, 0.7f},
        { TileType.SandRocks, 1f},

    };

    public Dictionary<TileType, float> thresholdsRock = new Dictionary<TileType, float> {
        {TileType.Stone, 0.6f},
        {TileType.StoneRocks, 0.85f},
        {TileType.Hill, 1f },
    };
}
