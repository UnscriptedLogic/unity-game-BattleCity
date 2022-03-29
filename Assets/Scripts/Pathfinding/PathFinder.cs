using System;
using System.Linq;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : Semaphore
{
    public PathFinderGrid pfGrid;
    public bool drawGizmos;

    private PathRequestManager requestManager;

    protected override void SephamoreStart(Manager manager)
    {
        base.SephamoreStart(manager);
        requestManager = PathRequestManager.instance;
    }

    public IEnumerator FindPath(Vector3 startPos, Vector3 endPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathValid = false;

        PFNode startnode = pfGrid.GetPFNodeFromWorldPoint(startPos);
        PFNode endNode = pfGrid.GetPFNodeFromWorldPoint(endPos);

        if (!startnode.isObstacle && !endNode.isObstacle)
        {
            Heap<PFNode> openSet = new Heap<PFNode>(pfGrid.nodeManager.MaxSize);
            HashSet<PFNode> closedSet = new HashSet<PFNode>();

            openSet.Add(startnode);
            while (openSet.Count > 0)
            {
                PFNode currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == endNode)
                {
                    sw.Stop();
                    pathValid = true;
                    //UnityEngine.Debug.Log("Path found: " + sw.ElapsedMilliseconds + "ms");

                    break;
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
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        yield return null;
        if (pathValid)
        {
            waypoints = RetracePath(startnode, endNode);
        }
        requestManager.FinishProcessingPath(waypoints, pathValid);
    }

    internal void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
    {
        StartCoroutine(FindPath(pathStart, pathEnd));
    }

    private Vector3[] RetracePath(PFNode startNode, PFNode endNode)
    {
        List<PFNode> path = new List<PFNode>();

        PFNode _node = endNode;
        while (_node != startNode)
        {
            path.Add(_node);
            _node = _node.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    private Vector3[] SimplifyPath(List<PFNode> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        //Vector2 directionOld = Vector2.zero;

        //for (int i = 1; i < path.Count; i++)
        //{
        //    Vector2 directionNew = new Vector2(path[i-1].coordx - path[i].coordx, path[i - 1].coordy - path[i].coordy);
        //    if (directionNew != directionOld)
        //    {
        //        waypoints.Add(path[i].position);
        //    }
        //    directionOld = directionNew;
        //}

        for (int i = 0; i < path.Count; i++)
        {
            waypoints.Add(path[i].position);
        }
        return waypoints.ToArray();
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
}