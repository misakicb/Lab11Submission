using System.Collections.Generic;
using UnityEngine;

public class APathfinding : MonoBehaviour
{
    [HideInInspector] public Vector2Int start = new Vector2Int(0, 0);
    [HideInInspector] public Vector2Int goal = new Vector2Int(4, 4);
    [HideInInspector] public int[,] grid;
    [HideInInspector] public List<Vector2Int> path = new List<Vector2Int>();

    public int width = 5;
    public int height = 5;

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1,0), new Vector2Int(-1,0),
        new Vector2Int(0,1), new Vector2Int(0,-1)
    };

    public void GenerateRandomGrid(int w, int h, float obstacleProbability)
    {
        width = w;
        height = h;
        grid = new int[height, width];

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                grid[y, x] = Random.value < obstacleProbability ? 1 : 0;

        
        grid[start.y, start.x] = 0;
        grid[goal.y, goal.x] = 0;

        
        Debug.Log("Generated Grid:");
        for (int y = 0; y < height; y++)
        {
            string row = "";
            for (int x = 0; x < width; x++)
                row += grid[y, x] == 1 ? "X " : "O ";
            Debug.Log(row);
        }
    }

    private bool IsInBounds(Vector2Int p) => p.x >= 0 && p.x < width && p.y >= 0 && p.y < height;

    private int Heuristic(Vector2Int a, Vector2Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    public void FindPath(Vector2Int s, Vector2Int g)
    {
        path.Clear();
        Debug.Log("Finding path from " + s + " to " + g);

        
        List<(Vector2Int node, int priority)> frontier = new List<(Vector2Int, int)>();
        frontier.Add((s, 0));

        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var costSoFar = new Dictionary<Vector2Int, int>();
        cameFrom[s] = s;
        costSoFar[s] = 0;

        while (frontier.Count > 0)
        {
            
            frontier.Sort((a, b) => a.priority.CompareTo(b.priority));
            Vector2Int current = frontier[0].node;
            frontier.RemoveAt(0);

            if (current == g) break;

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;
                if (!IsInBounds(next) || grid[next.y, next.x] == 1) continue;

                int newCost = costSoFar[current] + 1;
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    int priority = newCost + Heuristic(next, g);
                    frontier.Add((next, priority));
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(g))
        {
            Debug.Log("Path not found!");
            return;
        }

        // Reconstruct path
        Vector2Int step = g;
        while (step != s)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        path.Add(s);
        path.Reverse();

        // Log path
        string pathStr = "Path: ";
        foreach (var p in path) pathStr += "(" + p.x + "," + p.y + ") -> ";
        Debug.Log(pathStr);
        Debug.Log("Path length: " + path.Count);
    }
}

