using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Animations;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance; //Singleton
    public GameObject selector;
    public GameObject stats;
    public Dictionary<Vector3Int, HexTile> tiles;
    private Transform[] childs;
    private HexTile SelectedTile;

    public AnimatorController _controller;
    public AnimationClip _clip;

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

    public void OnSelectedTile(HexTile tile)
    {
        if(tile.tileType == HexTileSettings.TileType.Land)
        {
            stats.SetActive(false);
            stats.SetActive(true);
            tile.NewType = HexTileSettings.TileType.Castle;
            _controller.animationClips[0].events[0].functionName = nameof(HexTile.ChangeTileOnAnimation);
            Animator anim = tile.gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = _controller;
            // set stats info
            //Button to build
        }
        else
        {
            if(stats.activeSelf == true)
            {
                stats.SetActive(false);
            }
        }
    }


    public HexTile ReturnLastHexTile()
    {
        return childs[transform.childCount - 1].GetComponent<HexTile>();
    }
}
