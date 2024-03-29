using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TileManager : MonoBehaviour
{
    public static TileManager instance; //Singleton
    public GameObject selector;
    public GameObject stats;
    public Dictionary<Vector3Int, HexTile> tiles;
    private Transform[] childs;
    private HexTile selectedTile;

    [HideInInspector]
    public bool settlementIsUp = false;

    public RuntimeAnimatorController _controller;
    public AnimationClip _clip;
    public GameObject buildButton;
    private void Awake()
    {
        instance = this;
    }
    public HexGrid InitializeGrid()
    {
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
        Invoke(nameof(StartManager), 0.5f);

        return GetComponent<HexGrid>();
    }

    private void StartManager()
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
        if (tile.tileType == HexTileSettings.TileType.Land)
        {
            stats.SetActive(false);
            stats.SetActive(true);

            if (selectedTile != null)
                selectedTile.IsSelected(false);
            
            selectedTile = tile;
            selectedTile.IsSelected(true);
            selector.SetActive(true);
            selector.transform.position = new Vector3(tile.transform.position.x, selector.transform.position.y, tile.transform.position.z);
            int water = GetClosestDistance(tile, HexTileSettings.TileType.Water);
            int rock = Math.Min(GetClosestDistance(tile, HexTileSettings.TileType.StoneRocks), GetClosestDistance(tile, HexTileSettings.TileType.Hill));
            int forest = GetClosestDistance(tile, HexTileSettings.TileType.Forest);

            stats.GetComponentInChildren<TMP_Text>().text = $"Resources\nWater:{GetColoredTextByDistance(water)}\nMinerals: {GetColoredTextByDistance(rock)}\nWood{GetColoredTextByDistance(forest)}";
            buildButton.SetActive(true);
        }
        else
        {
            if(stats.activeSelf == true && buildButton.activeSelf == true)
            {
                stats.SetActive(false);
                buildButton.SetActive(false);
                selector.SetActive(false);
            }
        }
    }

    public void DeactivateUi()
    {
        stats.SetActive(false);
        buildButton.SetActive(false);
        selector.SetActive(false);
    }

    private string GetColoredTextByDistance(int d)
    {
        if (d == 51)
        {
            return "Not available";
        }

        if (d<= 4)
        {
            return "<color=#00ff00>" + d.ToString() + "</color>(very close)";
        }else if(d<= 8)
        {
            return "<color=#ffff66>" + d.ToString() + "</color>(far)";
        }
        else
        {
            return "<color=#ff0000>" + d.ToString() + "</color>(too far)";
        }


    }

    public int GetClosestDistance(HexTile startHex, HexTileSettings.TileType type)
    {
        Queue<HexTile> queue = new Queue<HexTile>();
        HashSet<HexTile> visited = new HashSet<HexTile>();

        queue.Enqueue(startHex);
        visited.Add(startHex);

        int distance = 0;

        while (queue.Count > 0)
        {
            int queueSize = queue.Count;
            for (int i = 0; i < queueSize; i++)
            {
                HexTile currentHex = queue.Dequeue();

                if (IsWaterTile(currentHex, type))
                {
                    return distance;
                }

                foreach (HexTile neighborHexTile in currentHex.neighbours)
                {
                    if (!visited.Contains(neighborHexTile))
                    {
                        queue.Enqueue(neighborHexTile);
                        visited.Add(neighborHexTile);
                    }
                }
            }
            distance++; // increase at each level of search
        }

        return 51; // if not found
    }

    //call to check te type of the tile
    private bool IsWaterTile(HexTile hex, HexTileSettings.TileType type)
    {
        if (hex != null && hex.tileType == type)
        {
            return true;
        }
        return false;
    }

    public void StartSettlement()
    {
        selector.SetActive(false);
        settlementIsUp = true;
        selectedTile.NewType = HexTileSettings.TileType.Village;
        _controller.animationClips[0].events[0].functionName = nameof(HexTile.ChangeTileOnAnimation);
        Animator anim = selectedTile.gameObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = _controller;

        stats.SetActive(false);
        buildButton.SetActive(false);
    }


    public HexTile ReturnLastHexTile()
    {
        return childs[transform.childCount - 1].GetComponent<HexTile>();
    }
}
