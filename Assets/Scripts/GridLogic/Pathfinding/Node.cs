public class Node
{
    public Node parent;
    public HexTile tile;

    public int gCost; // Cost from start to this node
    public int hCost; // Heuristic to destination
    public int fCost => gCost + hCost;

    public Node(HexTile tile, Node parent, int gCost, int hCost)
    {
        this.tile = tile;
        this.parent = parent;
        this.gCost = gCost;
        this.hCost = hCost;
    }
}
