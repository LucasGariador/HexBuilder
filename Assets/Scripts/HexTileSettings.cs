using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewTile/GenerationSettings")]
public class HexTileSettings : ScriptableObject
{

    public enum TileType
    {
        Free,
        Garbage,
        Asteroid
    }
    public GameObject FreeTile;
    public GameObject Garbage;
    public GameObject Asteroid;

    public GameObject GetTile(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.Free:
                return FreeTile;
            case TileType.Garbage:
                return Garbage;
            case TileType.Asteroid:
                return Asteroid;
        }
        return null;
    }
}
