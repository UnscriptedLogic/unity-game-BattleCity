using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinderGrid : Semaphore
{
    public NodeManager nodeManager;
    public LayerMask obstacleLayer;
    public Dictionary<Tuple<int, int>, PFNode> pathfindingGrid = new Dictionary<Tuple<int, int>, PFNode>();

    [Header("Debug")]
    public bool drawGizmos;

    protected override void SephamoreStart(Manager manager)
    {
        CreateGrid();

        base.SephamoreStart(manager);
    }

    private void CreateGrid()
    {
        for (int i = 0; i < nodeManager.grid.Count; i++)
        {
            Tuple<int, int> coord = nodeManager.grid.ElementAt(i).Key;
            Node value = nodeManager.grid.ElementAt(i).Value;
            bool isObstructed = Physics.CheckSphere(value.position, 0.45f, obstacleLayer);

            pathfindingGrid.Add(coord, new PFNode(coord.Item1, coord.Item2, isObstructed, value.position));
        }
    }

    private void ForEvery(Action<PFNode> method)
    {
        for (int i = 0; i < pathfindingGrid.Count; i++)
        {
            method(pathfindingGrid.ElementAt(i).Value);
        }
    }

    public PFNode GetPFNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentageX = (worldPosition.x / nodeManager.gridSize.x) + 0.5f;
        float percentageY = (worldPosition.z / nodeManager.gridSize.y) + 0.5f;
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);

        int x = Mathf.FloorToInt(Mathf.Min(nodeManager.gridSize.x * percentageX, nodeManager.gridSize.x - 1));
        int y = Mathf.FloorToInt(Mathf.Min(nodeManager.gridSize.y * percentageY, nodeManager.gridSize.y - 1));
        return pathfindingGrid[new Tuple<int, int>(x, y)];
    }

    public List<PFNode> GetNeighbours(PFNode node)
    {
        List<PFNode> neighbours = new List<PFNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y))
                {
                    continue;
                }

                //if (x == 0 && y == 0)
                //    continue;

                int checkX = node.coordx + x;
                int checkY = node.coordy + y;

                if (checkX >= 0 && checkX < nodeManager.gridSize.x && checkY >= 0 && checkY < nodeManager.gridSize.y)
                {
                    neighbours.Add(pathfindingGrid[new Tuple<int, int>(checkX, checkY)]);
                }
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos && pathfindingGrid != null)
        {
            ForEvery(node =>
            {
                if (node.isObstacle)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(node.position, Vector3.one);
                }
            });
        }
    }
}