using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewTile/GenerationSettings")]
public class HexTileSettings : ScriptableObject
{

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

    public GameObject GetTile(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Land:
                return LandTile;
            case TileType.Water:
                return WaterTile;
            case TileType.Island:
                return IslandTile;
            case TileType.Mountains:
                return MountainsTile;
            case TileType.Hill:
                return HillTile;
            case TileType.Forest:
                return ForestTile;
            case TileType.HillForest:
                return HillForestTile;
            case TileType.Smelter:
                return SmelterTile;
            case TileType.Wall:
                return WallTile;
            case TileType.Village:
                return VillageTile;
            case TileType.WizardTower:
                return WizardTowerTile;
            case TileType.Archery:
                return ArcheryTile;
            case TileType.House:
                return HouseTile;
            case TileType.Castle:
                return CastleTile;
            case TileType.Sheep:
                return SheepTile;
            case TileType.Port:
                return PortTile;
            case TileType.Mine:
                return MineTile;
        }
        return null;
    }
}
