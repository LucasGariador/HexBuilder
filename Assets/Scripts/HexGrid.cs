using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;
    public float radius = 1f;
    public bool isFlatTopped;


    public HexTileSettings settings;

    public void Clear()
    {
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            children.Add(child);
        }

        foreach (GameObject child in children)
        {
            DestroyImmediate(child, true);
        }
    }

    public void LayoutGrid()
    {
        Clear();
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                GameObject tile = new GameObject($"Hex C{x}, R{y}");

                HexTile hextile = tile.AddComponent<HexTile>();
                hextile.settings = settings;
                hextile.tileType = HexTileSettings.TileType.Free;
                hextile.AddTile();
                hextile.offsetCoordinate = new Vector2Int(x, y);
                tile.transform.position = GetPositionForHexFromCoordinate(x, y);
                hextile.cubeCoordinate = OffsetToCube(hextile.offsetCoordinate);
                tile.transform.SetParent(transform, true);
            }
        }
    }

    public static Vector3Int OffsetToCube(Vector2Int offset)
    {
        var q = offset.x - (offset.y + (offset.y % 2)) / 2;
        var r = offset.y;
        return new Vector3Int(q, r, -q - r);
    }

    public static Vector3 GetPositionForHexFromCoordinate(int column, int row, float radius = 1f, bool isFlatTopped = true)
    {
        float width, height, xPosition, yPosition, horizontalDistance, verticalDistance, offset;
        bool shouldOffset;
        float size = radius;

        if(isFlatTopped)
        {
            shouldOffset = (row % 2) == 0;
            width = Mathf.Sqrt(3) * size;
            height = 2f * size;

            horizontalDistance = width;
            verticalDistance = height * (3f / 4f);

            offset = (shouldOffset) ? width /2 : 0;

            xPosition = (column * (horizontalDistance)) + offset;
            yPosition = (row * verticalDistance);
        }
        else
        {
            shouldOffset = (column % 2) == 0;
            width = 2f * size;
            height = Mathf.Sqrt(3f) * size;

            horizontalDistance = width * (3f / 4f);
            verticalDistance = height;

            offset = (shouldOffset) ? height / 2 : 0;
            xPosition = (column * (horizontalDistance));
            yPosition = (row * verticalDistance) - offset;
        }

        return new(xPosition, 0, -yPosition);
    }
}