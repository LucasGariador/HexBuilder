using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    public GameObject selector;
    public Dictionary<Vector3Int, HexTile> tiles;
    Transform[] childs;

    private void Awake()
    {
        instance = this;
        tiles = new Dictionary<Vector3Int, HexTile>();

        HexTile[] hexTiles = gameObject.GetComponentsInChildren<HexTile>();
        foreach (HexTile hextile in hexTiles)
        {
            RegisterTile(hextile);
        }

        foreach (HexTile hexTile in hexTiles)
        {
            List<HexTile> neighbours = GetNeighbours(hexTile);
            hexTile.neighbours = neighbours;
        }
    }

    private void Start()
    {
        childs = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
        {
            childs[i] = transform.GetChild(i);
        }
    }

    private List<HexTile> GetNeighbours(HexTile tile)
    {
        List<HexTile> neighbours = new();

        Vector3Int[] neighbourCoords = new Vector3Int[]
        {
            new Vector3Int(1, -1, 0),
            new Vector3Int(1, 0, -1),
            new Vector3Int(0, 1, -1),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(-1, 0, 1),
            new Vector3Int(0, -1, 1),
        };

        foreach(Vector3Int neighbourCoord in neighbourCoords)
        {
            Vector3Int tileCoord = tile.cubeCoordinate;

            if(tiles.TryGetValue(tileCoord + neighbourCoord, out HexTile neighbour))
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours;
    }

    private void RegisterTile(HexTile tile)
    {
        tiles.Add(tile.cubeCoordinate, tile);   
    }

    public void OnHighlitedTile(HexTile tile)
    {
        if (tile == null) return;

            Color color = new Color(1, 1, 1, .8f);
            tile.transform.GetComponentInChildren<MeshRenderer>().material.color = color;

    }

    public void OnSelectedTile(HexTile tile)
    {
        selector.transform.position = tile.transform.position;
    }

    public HexTile ReturnLastHexTile()
    {
        return childs[transform.childCount - 1].GetComponent<HexTile>();
    }
}
