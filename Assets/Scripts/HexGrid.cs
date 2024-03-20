using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HexTileSettings;

public class HexGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;
    public float radius = 1f;

    public HexTileSettings settings;

    private Dictionary<TileType, float> thresholds = new Dictionary<TileType, float> {
        { TileType.Water, 0.3f },
        { TileType.Forest, 0.4f },
        { TileType.Land, 0.75f },
        { TileType.Hill, 0.85f },
        { TileType.Mountains, 1.0f }

        // Agrega más umbrales y tipos de tile según sea necesario
    };

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

        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                // Utilizar el valor de offset junto con las coordenadas de la cuadrícula para generar el ruido Perlin
                float perlinValue = Mathf.PerlinNoise((x + xOffset) * 0.1f, (y + yOffset) * 0.1f);
                Debug.Log(perlinValue);
                TileType tileType = GetTileType(perlinValue);

                GameObject tile = new GameObject($"Hex C{x}, R{y}");
                HexTile hextile = tile.AddComponent<HexTile>();
                hextile.settings = settings;
                hextile.tileType = tileType;
                hextile.AddTile();
                hextile.offsetCoordinate = new Vector2Int(x, y);
                tile.transform.position = GetPositionForHexFromCoordinate(x, y);
                hextile.cubeCoordinate = OffsetToCube(hextile.offsetCoordinate);
                tile.transform.SetParent(transform, true);
            }
        }
    }

    private TileType GetTileType(float perlinValue)
    {
        // Busca el tipo de tile correspondiente al valor de ruido Perlin en los umbrales definidos
        foreach (var threshold in thresholds)
        {
            if (perlinValue < threshold.Value)
            {
                return threshold.Key;
            }
        }
        return TileType.Land; // Por defecto, devuelve Land si no se encuentra un tipo correspondiente
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