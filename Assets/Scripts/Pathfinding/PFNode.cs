using System.Collections;
using UnityEngine;

public class PFNode
{
    //Coordinate in grid array
    public int coordx;
    public int coordy;

    public bool isObstacle; //obstructed terrain
    public Vector3 position; //real world position

    //algorithm values
    public int gCost;
    public int hCost;

    public PFNode parent;

    public int fCost { get { return gCost + hCost; } }

    public PFNode(int coordx, int coordy, bool isWall, Vector3 position)
    {
        this.coordx = coordx;
        this.coordy = coordy;
        this.isObstacle = isWall;
        this.position = position;
    }
}