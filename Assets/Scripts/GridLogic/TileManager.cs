using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public static TileManager instance; //Singleton

    public GameObject selector;
    public Dictionary<Vector3Int, HexTile> tiles;
    private Transform[] childs;
    private HexTile selectedTile;
    private HexTile mainTilePosition = null;

    public HexTile currentTile;

    [SerializeField]
    private GameObject player;

    private PlayerOnWorldMap playerOnWorldMap;


    private List<HexTile> path = new List<HexTile>(); //pathfinding

    [SerializeField]
    private Button travelButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerOnWorldMap = player.GetComponent<PlayerOnWorldMap>();
        travelButton.onClick.AddListener(MoveTo);
        InitializeGrid();
    }

    public HexGrid InitializeGrid()
    {
        Debug.Log("InitializeGrid");
        tiles = new Dictionary<Vector3Int, HexTile>();

        HexTile[] hexTiles = gameObject.GetComponentsInChildren<HexTile>();
        foreach (HexTile hextile in hexTiles)
        {
            if(hextile.tileType == HexTileSettings.TileType.MainStation)
                mainTilePosition = hextile;
            RegisterTile(hextile);
        }

        foreach (HexTile hexTile in hexTiles)
        {
            List<HexTile> neighbours = GetNeighbours(hexTile);
            hexTile.neighbours = neighbours;
        }

        
        player.transform.position = mainTilePosition.transform.position + new Vector3(0, 0.5f , 0);
        playerOnWorldMap.currentTile = mainTilePosition;
        currentTile = playerOnWorldMap.currentTile;

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
        Debug.Log("Finished starting manager");
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
        path.Clear();
        currentTile = playerOnWorldMap.currentTile;

        if (currentTile == null || tile == null) { return; }
        path = Pathfinder.FindPath(currentTile ,tile, playerOnWorldMap.GetCurrentFuel());

        if (path == null)
        {
            Debug.LogWarning("No se pudo encontrar un camino desde " + currentTile.name + " hasta " + tile.name);
            return;
        }

        if (tile.tileType == HexTileSettings.TileType.Empty || tile.tileType == HexTileSettings.TileType.EmptyEvent)
        {

            if (selectedTile != null)
                selectedTile.IsSelected(false);
            
            selectedTile = tile;
            selectedTile.IsSelected(true);
            travelButton.gameObject.SetActive(true);
            selector.SetActive(true);
            selector.transform.position = new Vector3(tile.transform.position.x, selector.transform.position.y, tile.transform.position.z);
        }
        else
        {
            selector.SetActive(false);
        }
    }

    public void DeactivateUi()
    {
        selector.SetActive(false);
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

    public void MoveTo()
    {
        selector.SetActive(false);

        travelButton.gameObject.SetActive(false);

        if (path != null && path.Count > 0)
        {
            currentTile = path.Last();
            playerOnWorldMap.StartPathMovement(path);
        }
    }



    public HexTile ReturnLastHexTile()
    {
        return childs[transform.childCount - 1].GetComponent<HexTile>();
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            foreach(HexTile tile in path)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(tile.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(0.5f, 0.5f, 0.5f));
            }
        }
    }

}
