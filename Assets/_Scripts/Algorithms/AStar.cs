﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour {

    private static int[,] grid;
    private static int[,] aStar;
    private static Vector2[,] predecessors;

    private static Vector2 start = new Vector2(-1, -1);

    static List<Vector2> closedList = new List<Vector2>();
    static List<Vector2> openList = new List<Vector2>();
    private static List<Vector2> shortestPath = new List<Vector2>();
    private static Vector2 shortestPathGoal;

    private static List<Vector2> moves;
    private static int currentWidth;
    private static int currentHeight;

    public static List<Vector2> CalculateAStar(int[,] table, int width, int height, List<Vector2> goals, List<Vector2> deltas, Vector2 start, int MaxCost)
    {
        Debug.Log("Calculating A*");
        closedList = new List<Vector2>();
        openList = new List<Vector2>();
        shortestPath = new List<Vector2>();

        aStar = new int[width, height];
        grid = table;
        predecessors = new Vector2[width, height];

        moves = deltas;
        currentWidth = width;
        currentHeight = height;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                aStar[i, j] = MaxCost;
                predecessors[i, j] = Vector2.zero;
            }
        }

        openList.Add(start);
        int x = (int)start.x;
        int y = (int)start.y;
        int l = grid[x, y];
        aStar[x, y] = l;
        predecessors[x, y] = start;

        string s = "Walls:\n";
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[i, j] == MaxCost)
                {
                    closedList.Add(new Vector2(i, j));
                    aStar[i, j] = MaxCost;
                }
            }
        }
        s += "---------------------------------\n\n";

        while (openList.Count != 0)
        {
            int lowest = 0;
            for (int i = 0; i < openList.Count; i++)
            {
                if (aStar[(int)openList[i].x, (int)openList[i].y] < aStar[(int)openList[lowest].x, (int)openList[lowest].y])
                    lowest = i;
            }
            Vector2 currentNode = openList[lowest];
            openList.RemoveAt(lowest);

            foreach (Vector2 goal in goals)
            {
                if (currentNode == goal)
                {
                    shortestPathGoal = currentNode;
                    MakeShortestPath();
                    //string s = "";
                    //foreach (Vector2 v in shortestPath)
                    //{
                    //    s += (v) + "\n";
                    //}
                    //return "Path found:\n" + s;
                    return shortestPath;
                }

                closedList.Add(currentNode);

                ExpandNode(currentNode);
            }
        }
        shortestPath.Add(start);
        return shortestPath;
    }

    private static void ExpandNode(Vector2 node)
    {
        int x = (int)node.x;
        int y = (int)node.y;

        foreach (Vector2 delta in moves)
        {
            Vector2 successor = node + delta;
            int dx = x + (int)delta.x;
            int dy = y + (int)delta.y;

            if (dx < 0 || dx >= currentWidth || dy < 0 || dy >= currentHeight)
                continue;

            if (closedList.Contains(successor))
                continue;

            //Maybe error
            int tentative_g = aStar[x, y] + grid[dx, dy];

            if (openList.Contains(successor) && tentative_g >= aStar[dx, dy])
                continue;

            predecessors[dx, dy] = node;
            aStar[dx, dy] = tentative_g;

            if (!openList.Contains(successor))
                openList.Add(successor);
        }
    }

    private static void MakeShortestPath()
    {
        int i = 0;

        shortestPath.Add(shortestPathGoal);
        while (!shortestPath.Contains(start) && i++ < 1000)
        {
            Vector2 t = predecessors[(int)shortestPath[shortestPath.Count - 1].x, (int)shortestPath[shortestPath.Count - 1].y];
            shortestPath.Add(t);
        }
    }
}
