using System.Collections.Generic;
using UnityEngine;
using static HexTileSettings;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class HexTile : MonoBehaviour
{
    public HexTileSettings settings;
    public TileType tileType;
    public TileType NewType = TileType.Empty;

    public EventSO eventSO = null;

    public bool isEmpty = true;
    public Vector2Int offsetCoordinate;
    public Vector3Int cubeCoordinate;

    [HideInInspector]
    public List<HexTile> neighbours = new List<HexTile>();

    [SerializeField]
    private GameObject tile;

    public int movementCost => tileType switch
    {
        TileType.Empty => 1,
        TileType.EmptyEvent => 1,
        TileType.Asteroids => 9999,
        TileType.MainStation => 1,
        _ => 1
    };

    private void OnValidate()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this == null) return;
            RefreshTile();
        };
#endif
    }

    public void ChangeTileOnAnimation()
    {
        tileType = NewType;
        RefreshTile();
    }

    public void RefreshTile()
    {
        if (tile != null)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                Destroy(tile);
            else
                DestroyImmediate(tile);
#else
            Destroy(tile);
#endif
        }

        if (settings != null)
        {
            GameObject prefab = settings.GetTile(tileType);
            if (prefab != null)
            {
                tile = Instantiate(prefab, transform.position, transform.rotation, transform);
                if (!tile.GetComponent<HexTileCollider>())
                    tile.AddComponent<HexTileCollider>();
            }
            else
            {
                Debug.LogWarning($"No prefab assigned for tile type {tileType}.");
            }
        }
    }

    public void IsSelected(bool selected)
    {
        var renderer = GetComponentInChildren<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = selected ? Color.red : Color.white;
        }
    }
}

