using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public PathFinderGrid pfGrid;
    public List<PFNode> path = new List<PFNode>();

    private void FindPath(Vector3 startPos, Vector3 endPos)
    {
        PFNode startnode = pfGrid.GetPFNodeFromWorldPoint(startPos);
        PFNode endNode = pfGrid.GetPFNodeFromWorldPoint(endPos);

        List<PFNode> openSet = new List<PFNode>();
        HashSet<PFNode> closedSet = new HashSet<PFNode>();
        openSet.Add(startnode);

        while (openSet.Count > 0)
        {
            PFNode currentNode = openSet[0];
            for (int i = i; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startnode, endNode);
                break;
            }

            foreach (PFNode neighbour in pfGrid.GetNeighbours(currentNode))
            {
                if (neighbour.isObstacle && closedSet.Contains(neighbour))
                {
                    continue;
                }

                int moveCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (moveCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = moveCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    private void RetracePath(PFNode startNode, PFNode endNode)
    {
        List<PFNode> path = new List<PFNode>();
        PFNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.parent);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        this.path = path;
    }

    private int GetDistance(PFNode nodeA, PFNode nodeB)
    {
        int distX = (int)MathF.Abs(nodeA.coordx - nodeB.coordx);
        int distY = (int)MathF.Abs(nodeA.coordy - nodeB.coordy);

        if (distX > distY)
            return (14 * distY) + 10 * (distX - distY);

        return (14 * distX) + 10 * (distY - distX);
    }
}