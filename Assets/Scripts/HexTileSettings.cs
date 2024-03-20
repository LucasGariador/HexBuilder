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
        Forest
    }
    public GameObject LandTile;
    public GameObject WaterTile;
    public GameObject MountainsTile;
    public GameObject HillTile;
    public GameObject ForestTile;

    public GameObject GetTile(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Land:
                return LandTile;
            case TileType.Water:
                return WaterTile;
            case TileType.Mountains:
                return MountainsTile;
            case TileType.Hill:
                return HillTile;
            case TileType.Forest:
                return ForestTile;
        }
        return null;
    }
}
