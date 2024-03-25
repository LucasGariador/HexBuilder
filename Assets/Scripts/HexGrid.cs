using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static HexTileSettings;

public class HexGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize;
    public float radius = 1f;

    public HexTileSettings settings;

    public Dictionary<Vector2Int, Biomes> biomeType = new Dictionary<Vector2Int, Biomes>();

    public void Clear()
    {
        List<GameObject> children = new List<GameObject>();
        biomeType.Clear();

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

        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));
        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));
        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));
        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));
        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));
        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));
        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));
        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));
        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));
        biomeType.Add(new(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y)), settings.GetRandomBiome(Random.Range(0, 101)));


        foreach (KeyValuePair<Vector2Int, Biomes> items in biomeType)
        {
            print("You have " + items.Value + " " + items.Key);

        }

        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Biomes biome = GetClosestControl(new Vector2(x,y));

                // Offset value is used to get diferents positions on the noise
                float perlinValue = Mathf.PerlinNoise((x + xOffset) * 0.1f, (y + yOffset) * 0.1f);
                TileType tileType = GetTileType(perlinValue, biome);

                GameObject tile = new($"Hex C{x}, R{y}");
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

    private Biomes GetClosestControl(Vector2 vector2)
    {
        Biomes biome = Biomes.GreenLand;
        float distance = 100f;
        for (int index = 0; index < biomeType.Count; index++)
        {
            var item = biomeType.ElementAt(index);
            float newDistance = Vector2.Distance(item.Key, vector2);
            if (newDistance<=distance)
            {
                distance = newDistance;
                biome = item.Value;
            }
        }
        return biome;
    }

        private TileType GetTileType(float perlinValue, Biomes biome)
        {
        // Returns the corresponding tiletype, acording to his threshold value an biome type
        foreach (var threshold in settings.GetThreshold(biome))
        {
            if (perlinValue < threshold.Value)
            {
                return threshold.Key;
            }
        }
        return TileType.Land; // Returns Land by DEFAULT
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