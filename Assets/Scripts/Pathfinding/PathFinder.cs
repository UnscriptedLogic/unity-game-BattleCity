using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : Semaphore
{
    public PathFinderGrid pfGrid;
    public List<PFNode> path = new List<PFNode>();
    public bool drawGizmos;

    public Transform start;
    public Transform end;
    public float delay = 0.5f;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        FindPath(start.position, end.position);
    }

    public void FindPath(Vector3 startPos, Vector3 endPos)
    {
        PFNode startnode = pfGrid.GetPFNodeFromWorldPoint(startPos);
        PFNode endNode = pfGrid.GetPFNodeFromWorldPoint(endPos);

        List<PFNode> openSet = new List<PFNode>();
        HashSet<PFNode> closedSet = new HashSet<PFNode>();

        openSet.Add(startnode);
        while (openSet.Count > 0)
        {
            PFNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost <= currentNode.fCost)
                {
                    if (openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == endNode)
            {
                Debug.Log("Path found!");

                RetracePath(startnode, endNode);
                return;
            }

            foreach (PFNode neighbour in pfGrid.GetNeighbours(currentNode))
            {
                if (neighbour.isObstacle || closedSet.Contains(neighbour))
                {
                    continue;
                }

                float moveCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
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
        PFNode _node = endNode;
        while (_node != startNode)
        {
            path.Add(_node);
            _node = _node.parent;
        }
        path.Reverse();
    }

    private float GetDistance(PFNode nodeA, PFNode nodeB)
    {
        int distX = (int)MathF.Abs(nodeA.coordx - nodeB.coordx);
        int distY = (int)MathF.Abs(nodeA.coordy - nodeB.coordy);

        if (distX > distY)
            return (14 * distY) + 10 * (distX - distY);

        return (14 * distX) + 10 * (distY - distX);
        //return Vector3.Distance(nodeB.position, nodeA.position);
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (pfGrid.GetPFNodeFromWorldPoint(start.position) != null && pfGrid.GetPFNodeFromWorldPoint(end.position) != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(pfGrid.GetPFNodeFromWorldPoint(start.position).position, Vector3.one);

                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(pfGrid.GetPFNodeFromWorldPoint(end.position).position, Vector3.one);
            }

            if (path == null) { return; }
            if (path.Count > 0)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(path[i].position, Vector3.one);
                }
            }
        }
    }
}