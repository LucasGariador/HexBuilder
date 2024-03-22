using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Animations;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    public AnimatorController _controller;
    public AnimationClip _clip;
    public GameObject buildButton;

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

        buildButton.GetComponent<Button>().onClick.AddListener(StartSettlement);
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

            selectedTile = tile;

            int water = 0, rock = 0, forest = 0;

            water = GetClosestDistance(tile, HexTileSettings.TileType.Water);
            rock = Math.Min(GetClosestDistance(tile, HexTileSettings.TileType.Mountains), GetClosestDistance(tile, HexTileSettings.TileType.Hill));
            forest = GetClosestDistance(tile, HexTileSettings.TileType.Forest);

            stats.GetComponentInChildren<TMP_Text>().text = $"Distance to water source:<color=#FF0000> {water} </color>\nDistance to Minerals: {rock}\nDistance to wood source {forest}";
            buildButton.SetActive(true);
            //Button to build
        }
        else
        {
            if(stats.activeSelf == true && buildButton.activeSelf == true)
            {
                stats.SetActive(false);
                buildButton.SetActive(false);
            }
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

                // Verificar si el hexágono actual es un tile de agua
                if (IsWaterTile(currentHex, type))
                {
                    return distance;
                }

                // Agregar hexágonos vecinos no visitados a la cola
                foreach (HexTile neighborHexTile in currentHex.neighbours)
                {
                    if (!visited.Contains(neighborHexTile))
                    {
                        queue.Enqueue(neighborHexTile);
                        visited.Add(neighborHexTile);
                    }
                }
            }
            distance++; // Incrementar la distancia después de cada nivel de búsqueda
        }

        return -1; // Si no se encuentra ningún tile de agua
    }

    // Verificar si un hexágono es un tile de agua
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
        settlementIsUp = true;
        selectedTile.NewType = HexTileSettings.TileType.Castle;
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
