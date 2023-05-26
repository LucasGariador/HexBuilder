using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexTile : MonoBehaviour
{
    public HexTileSettings settings;

    public HexTileSettings.TileType tileType;

    public bool isEmpty = true; 

    public GameObject tile;

    private bool isDirty = false;

    public Vector2Int offsetCoordinate;
    public Vector3Int cubeCoordinate;
    public MeshFilter meshFilter;

    public List<HexTile> neighbours;

    private void OnValidate()
    {
        if (tile == null) {  return; }

        isDirty = true;
    }

    private void Update()
    {
        if(isDirty)
        {
            if(Application.isPlaying)
            {
                GameObject.Destroy(tile);
            }
            else
            {
                GameObject.DestroyImmediate(tile);
            }

            AddTile();
            isDirty = false;
        }
    }


    public void RollTileType()
    {
        tileType = (HexTileSettings.TileType)(Random.Range(0, 3));
    }

    public void AddTile()
    {
        tile = GameObject.Instantiate(settings.GetTile(tileType), transform.position, Quaternion.identity, transform);
    }
}
