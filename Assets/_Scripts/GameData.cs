﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData {

    private static GameData instance;

    private GameData()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public static GameData Instance
    {
        get
        {
            if (instance == null)
                instance = new GameData();
            return instance;
        }
    }

    //Tables for holding the map
    public int[,] grid;
    public int[,] value;
    public char[,] policy;
    public int[,] astern;
    public Vector2[,] predecessors;

    //List for holding goals, deltas...
    public List<Vector2> goals = new List<Vector2>();
    public List<Vector2> deltas = new List<Vector2> { new Vector2(1, 0), new Vector2 ( 0, -1 ),  new Vector2(- 1, 0 ), new Vector2(0, 1) };
    public char[] deltaNames = new char[] { '>', 'v', '<', '^' };

    public int MaxCost { get { return 100000000; } }

    public int currentWidth, currentHeight;
    public bool calculateValues = false;
    public bool setStart = false;

    public Vector2 start;

    List<Vector2> closedList = new List<Vector2>();
    List<Vector2> openList = new List<Vector2>();

    public void SetGridSize(int x, int y)
    {
        grid = new int[x, y];
        value = new int[x, y];
        policy = new char[x, y];
    }

    public void InitGrid<T>(ref T[,] grid, T value)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = value;
            }
        }
    }

    public void RandomGrid()
    {
        goals = new List<Vector2>();
        for (int x = 0; x < currentWidth; x++)
        {
            for (int y = 0; y < currentHeight; y++)
            {
                int randomCost = UnityEngine.Random.Range(0, 255);
                if (randomCost <= 200 && UnityEngine.Random.Range(0, 100) < 3)
                    goals.Add(new Vector2( x, y ));
                grid[x, y] = randomCost > 200 ? MaxCost : randomCost;
            }
        }
    }

    public void CalculateValue()
    {
        value = new int[currentWidth, currentHeight];

        InitGrid<int>(ref value, MaxCost);

        bool change = true;

        while (change)
        {
            change = false;

            for (int x = 0; x < currentWidth; x++)
            {
                for (int y = 0; y < currentHeight; y++)
                {
                    for (int i = 0; i < goals.Count; i++)
                    {
                        if (goals[i][0] == x && goals[i][1] == y)
                        {
                            if (value[x, y] > 0)
                            {
                                value[x, y] = 0;
                                policy[x, y] = '*';
                                change = true;
                            }
                        }

                        else if (grid[x, y] < MaxCost)
                        {
                            for (int a = 0; a < deltas.Count; a++)
                            {
                                int x2 = x + (int)deltas[a][0];
                                int y2 = y + (int)deltas[a][1];

                                if (x2 >= 0 && x2 < currentWidth && y2 >= 0 && y2 < currentHeight)
                                {
                                    int v2;
                                    if (grid[x2, y2] == MaxCost)
                                        v2 = MaxCost;
                                    else
                                        v2 = value[x2, y2] + grid[x2, y2];
                                    if (v2 < value[x, y])
                                    {
                                        change = true;
                                        value[x, y] = v2;
                                        policy[x, y] = deltaNames[a];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public Color CostToColor(int cost)
    {
        return new Color(1, (255 - cost / 2) / 255.0f, (255 - cost) / 255.0f);
    }

    public bool CalculateAStern()
    {
        InitGrid<int>(ref astern, MaxCost);
        InitGrid<Vector2>(ref predecessors, Vector2.zero);

        openList.Add(start);
        int x = (int)start.x;
        int y = (int)start.y;
        astern[x, y] = 0;
        predecessors[x, y] = start;

        while (openList.Count != 0)
        {
            Vector2 currentNode = openList[0];
            openList.RemoveAt(0);

            foreach (Vector2 goal in GameData.Instance.goals)
            {
                if (currentNode == goal)
                    return true;

                closedList.Add(currentNode);

                ExpandNode(currentNode);
            }
        }
        return false;
    }

    private void ExpandNode(Vector2 node)
    {
        int x = (int)node.x;
        int y = (int)node.y;

        foreach (Vector2 delta in deltas)
        {
            Vector2 successor = node + delta;
            int dx = x + (int)delta.x;
            int dy = y + (int)delta.y;

            if (closedList.Contains(successor))
                continue;

            int tentative_g = astern[x, y] + grid[dx, dy];

            if (openList.Contains(successor) && tentative_g >= grid[dx, dy])
                continue;

            predecessors[dx, dy] = node;
            astern[dx, dy] = tentative_g;

            if (!openList.Contains(successor))
                openList.Add(successor);
        }
    }


    public string Print2DArray<T>(T[,] array)
    {
        string s = "";
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                s += (array[i, j] + " ");
            }
            s += ("\n");
        }
        return (s);
    }
}
