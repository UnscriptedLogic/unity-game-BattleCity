using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : Manager
{
    public Vector2 gridSize;
    public GameObject[] possibleSpawns;
    public Dictionary<Tuple<int, int>, Node> grid = new Dictionary<Tuple<int, int>, Node>();

    public int MaxSize { get { return (int)(gridSize.x * gridSize.y); } }

    [Header("Debug")]
    public bool showGrid;

    private void Awake()
    {
        InitializeGrid();
        InitializeSephamores();
    }

    private void InitializeGrid()
    {
        ForEveryNodePositionInGrid((coord, position) =>
        {
            grid.Add(coord, new Node(coord.Item1, coord.Item2, position));
        });
    }

    public void ForEveryNodeInGrid(Action<Tuple<int, int>> method)
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.y; z++)
            {
                method(new Tuple<int, int>(x, z));
            }
        }
    }

    public void ForEveryNodePositionInGrid(Action<Tuple<int, int>, Vector3> method)
    {
        ForEveryNodeInGrid((tuple) =>
        {
            Vector3 position = new Vector3(transform.position.x + tuple.Item1, transform.position.y, transform.position.z + tuple.Item2);
            position = new Vector3(position.x - (gridSize.x / 2), position.y, position.z - (gridSize.y / 2));
            method(tuple, position);
        });
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentageX = (worldPosition.x / gridSize.x) + 0.5f;
        float percentageY = (worldPosition.z / gridSize.y) + 0.5f;
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);

        int x = Mathf.FloorToInt(Mathf.Min(gridSize.x * percentageX, gridSize.x - 1));
        int y = Mathf.FloorToInt(Mathf.Min(gridSize.y * percentageY, gridSize.y - 1));
        return grid[new Tuple<int, int>(x,y)];
    }

    protected void OnDrawGizmos()
    {
        if (showGrid)
        {
            ForEveryNodePositionInGrid((coord, position) =>
            {
                Gizmos.DrawWireCube(position, Vector3.one);
            });
        }
    }
}
