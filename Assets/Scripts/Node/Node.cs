using System.Collections;
using UnityEngine;

public class Node
{
    public int coordx;
    public int coordy;
    public Vector3 position;

    public Node(int coordx, int coordy, Vector3 position)
    {
        this.coordx = coordx;
        this.coordy = coordy;
        this.position = position;
    }
}