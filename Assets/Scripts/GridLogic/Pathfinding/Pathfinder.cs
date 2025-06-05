using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    public static List<HexTile> FindPath(HexTile start, HexTile goal, int maxMovementCost)
    {
        List<Node> openSet = new List<Node>();
        HashSet<HexTile> closedSet = new HashSet<HexTile>();
        Dictionary<HexTile, int> costSoFar = new Dictionary<HexTile, int>();

        Node startNode = new Node(start, null, 0, CubeDistance(start.cubeCoordinate, goal.cubeCoordinate));
        openSet.Add(startNode);
        costSoFar[start] = 0;

        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) =>
             a.fCost != b.fCost ? a.fCost.CompareTo(b.fCost) : a.hCost.CompareTo(b.hCost));

            Node current = openSet[0];
            openSet.RemoveAt(0);
            closedSet.Add(current.tile);

            if (current.tile == goal)
                return ReconstructPath(current);


            foreach (HexTile neighbor in current.tile.neighbours)
            {


                if (closedSet.Contains(neighbor)) continue;

                int tileCost = neighbor.movementCost;
                if (tileCost >= 9999) continue; // Intransitable

                int newCost = current.gCost + tileCost;

                if (newCost > maxMovementCost) continue;

                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;

                    int hCost = CubeDistance(neighbor.cubeCoordinate, goal.cubeCoordinate);

                    // Penalización por alejarse de la línea directa
                    int deviation = DistanceFromLine(start.cubeCoordinate, goal.cubeCoordinate, neighbor.cubeCoordinate);
                    hCost += deviation * 2; // 2 es un peso que podés ajustar


                    // Penaliza cambio de dirección
                    if (current.parent != null)
                    {
                        var dirFromParent = GetDirection(current.parent.tile.cubeCoordinate, current.tile.cubeCoordinate);
                        var dirToNeighbor = GetDirection(current.tile.cubeCoordinate, neighbor.cubeCoordinate);

                        if (dirFromParent != dirToNeighbor)
                        {
                            hCost += 1;
                        }
                    }


                    Node newNode = new Node(neighbor, current, newCost, hCost);
                    openSet.Add(newNode);
                }
            }
        }

        return null; // No path found
    }

    private static List<HexTile> ReconstructPath(Node endNode)
    {
        List<HexTile> path = new List<HexTile>();
        Node current = endNode;

        while (current != null)
        {
            path.Insert(0, current.tile);
            current = current.parent;
        }

        // ✅ Remove first tile if it's the current tile
        if (path.Count > 0 && path[0] == TileManager.instance.currentTile)
        {
            path.RemoveAt(0);
        }

        return path;
    }


    public static int CubeDistance(Vector3Int a, Vector3Int b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z)) / 2;
    }

    public static Vector3Int GetDirection(Vector3Int from, Vector3Int to)
    {
        return new Vector3Int(
            to.x - from.x,
            to.y - from.y,
            to.z - from.z
        );
    }

    public static int DistanceFromLine(Vector3Int start, Vector3Int end, Vector3Int point)
    {
        // Proyección del punto sobre la línea
        Vector3 ap = point - start;
        Vector3 ab = end - start;

        float t = Vector3.Dot(ap, ab) / Vector3.Dot(ab, ab);
        t = Mathf.Clamp01(t);
        Vector3 closest = start + t * ab;

        Vector3Int closestRounded = CubeRound(closest);
        return CubeDistance(point, closestRounded);
    }

    // Redondear coordenadas cúbicas al hexágono más cercano
    public static Vector3Int CubeRound(Vector3 cube)
    {
        float rx = Mathf.Round(cube.x);
        float ry = Mathf.Round(cube.y);
        float rz = Mathf.Round(cube.z);

        float x_diff = Mathf.Abs(rx - cube.x);
        float y_diff = Mathf.Abs(ry - cube.y);
        float z_diff = Mathf.Abs(rz - cube.z);

        if (x_diff > y_diff && x_diff > z_diff)
            rx = -ry - rz;
        else if (y_diff > z_diff)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return new Vector3Int((int)rx, (int)ry, (int)rz);
    }
}

