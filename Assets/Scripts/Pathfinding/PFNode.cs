using System.Collections;
using UnityEngine;

public class PFNode : IHeapItem<PFNode>
{
    //Coordinate in grid array
    public int coordx;
    public int coordy;

    public bool isObstacle; //obstructed terrain
    public Vector3 position; //real world position

    //algorithm values
    public float gCost;
    public float hCost;

    public PFNode parent;
    private int heapIndex;

    public float fCost { get { return gCost + hCost; } }
    public int HeapIndex { get { return heapIndex; } set { heapIndex = value; } }

    public PFNode(int coordx, int coordy, bool isWall, Vector3 position)
    {
        this.coordx = coordx;
        this.coordy = coordy;
        this.isObstacle = isWall;
        this.position = position;
    }

    public int CompareTo(PFNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}