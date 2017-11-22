using System;
using System.Collections;
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
    public bool[,] walls;
    public int[,] value;
    public char[,] policy;
    public int[,] aStar;
    public Vector2[,] predecessors;

    //List for holding goals, deltas...
    public List<Vector2> goals = new List<Vector2>();
    public List<Vector2> deltas = new List<Vector2> { new Vector2(1, 0), new Vector2 ( 0, -1 ),  new Vector2(- 1, 0 ), new Vector2(0, 1) };
    public char[] deltaNames = new char[] { '>', 'v', '<', '^' };

    public int MaxCost { get { return 100000000; } }
    public int MaxWidth { get { return grid.GetLength(0); } }
    public int MaxHeight { get { return grid.GetLength(1); } }

    public int currentWidth, currentHeight;
    //public bool calculateValues = false;
    //public bool setStart = false;

    public Vector2 start = new Vector2(-1, -1);

    //List<Vector2> closedList = new List<Vector2>();
    //List<Vector2> openList = new List<Vector2>();
    //private Vector2 shortestPathGoal;
    public List<Vector2> shortestPath = new List<Vector2>();

    public Color occupied = Color.black;
    public Color goal = Color.blue;
    public Color begin = Color.red;
    public Color onPath = Color.green;
    public Color unreachable = Color.grey;


    public void SetGridSize(int x, int y)
    {
        grid = new int[x, y];
        walls = new bool[x, y];
        value = new int[x, y];
        policy = new char[x, y];
        aStar = new int[x, y];
        predecessors = new Vector2[x, y];
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
        shortestPath = new List<Vector2>();
        goals = new List<Vector2>();
        start = new Vector2(-1, -1);
        for (int x = 0; x < currentWidth; x++)
        {
            for (int y = 0; y < currentHeight; y++)
            {
                int randomCost = UnityEngine.Random.Range(0, 255);
                if (randomCost <= 200 && UnityEngine.Random.Range(0, 100) < 3)
                    goals.Add(new Vector2( x, y ));
                if (randomCost > 200)
                {
                    grid[x, y] = MaxCost;
                    walls[x, y] = true;
                }
                else
                {
                    grid[x, y] = randomCost;
                    walls[x, y] = false;
                }
            }
        }
    }

    public void CalculatePolicy()
    {
        DynamicProgramming.CalculateValue(grid, ref value, ref policy, currentWidth, currentHeight, goals, deltas, deltaNames, MaxCost);
    }

    //public void CalculatePolicy()
    //{
    //    Debug.Log("Calculating optimal policy");
    //    value = new int[currentWidth, currentHeight];

    //    InitGrid<int>(ref value, MaxCost);

    //    bool change = true;

    //    while (change)
    //    {
    //        change = false;

    //        for (int x = 0; x < currentWidth; x++)
    //        {
    //            for (int y = 0; y < currentHeight; y++)
    //            {
    //                for (int i = 0; i < goals.Count; i++)
    //                {
    //                    if (goals[i][0] == x && goals[i][1] == y)
    //                    {
    //                        if (value[x, y] > 0)
    //                        {
    //                            value[x, y] = 0;
    //                            policy[x, y] = '*';
    //                            change = true;
    //                        }
    //                    }

    //                    else if (grid[x, y] < MaxCost)
    //                    {
    //                        for (int a = 0; a < deltas.Count; a++)
    //                        {
    //                            int x2 = x + (int)deltas[a][0];
    //                            int y2 = y + (int)deltas[a][1];

    //                            if (x2 >= 0 && x2 < currentWidth && y2 >= 0 && y2 < currentHeight)
    //                            {
    //                                int v2;
    //                                if (grid[x2, y2] == MaxCost)
    //                                    v2 = MaxCost;
    //                                else
    //                                    v2 = value[x2, y2] + grid[x2, y2];
    //                                if (v2 < value[x, y])
    //                                {
    //                                    change = true;
    //                                    value[x, y] = v2;
    //                                    policy[x, y] = deltaNames[a];
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public Color CostToColor(int cost)
    {
        return new Color(1, (255 - cost / 2) / 255.0f, (255 - cost) / 255.0f);
    }

    public void CalculateAStar()
    {
        if (start.x != -1)
            shortestPath = AStar.CalculateAStar(grid, currentWidth, currentHeight, goals, deltas, start, MaxCost);
        else
            shortestPath = new List<Vector2>();
    }

    //public string CalculateAStar()
    //{
    //    Debug.Log("Calculating A*");
    //    if (start.x != -1)
    //    {
    //        Debug.Log("start is set");
    //        closedList = new List<Vector2>();
    //        openList = new List<Vector2>();
    //        shortestPath = new List<Vector2>();
    //        InitGrid<int>(ref aStar, MaxCost);
    //        InitGrid<Vector2>(ref predecessors, Vector2.zero);

    //        openList.Add(start);
    //        int x = (int)start.x;
    //        int y = (int)start.y;
    //        aStar[x, y] = grid[x, y];
    //        predecessors[x, y] = start;

    //        string s = "Walls:\n";
    //        for (int i = 0; i < currentWidth; i++)
    //        {
    //            for (int j = 0; j < currentHeight; j++)
    //            {
    //                if (grid[i, j] == MaxCost)
    //                {
    //                    closedList.Add(new Vector2(i, j));
    //                    aStar[i, j] = MaxCost;
    //                }
    //            }
    //        }
    //        s += "---------------------------------\n\n";

    //        while (openList.Count != 0)
    //        {
    //            int lowest = 0;
    //            for (int i = 0; i < openList.Count; i++)
    //            {
    //                if (aStar[(int)openList[i].x, (int)openList[i].y] < aStar[(int)openList[lowest].x, (int)openList[lowest].y])
    //                    lowest = i;
    //            }
    //            Vector2 currentNode = openList[lowest];
    //            openList.RemoveAt(lowest);

    //            foreach (Vector2 goal in goals)
    //            {
    //                if (currentNode == goal)
    //                {
    //                    shortestPathGoal = currentNode;
    //                    MakeShortestPath();
    //                    //string s = "";
    //                    foreach (Vector2 v in shortestPath)
    //                    {
    //                        s += (v) + "\n";
    //                    }
    //                    return "Path found:\n" + s;
    //                }

    //                closedList.Add(currentNode);

    //                ExpandNode(currentNode);
    //            }
    //        }
    //        shortestPath.Add(start);
    //        return "No path";
    //    }
    //    return "No start set";
    //}

    //private void ExpandNode(Vector2 node)
    //{
    //    int x = (int)node.x;
    //    int y = (int)node.y;

    //    foreach (Vector2 delta in deltas)
    //    {
    //        Vector2 successor = node + delta;
    //        int dx = x + (int)delta.x;
    //        int dy = y + (int)delta.y;

    //        if (dx < 0 || dx >= currentWidth || dy < 0 || dy >= currentHeight)
    //            continue;

    //        if (closedList.Contains(successor))
    //            continue;

    //        //Maybe error
    //        int tentative_g = aStar[x, y] + grid[dx, dy];

    //        if (openList.Contains(successor) && tentative_g >= aStar[dx, dy])
    //            continue;

    //        predecessors[dx, dy] = node;
    //        aStar[dx, dy] = tentative_g;

    //        if (!openList.Contains(successor))
    //            openList.Add(successor);
    //    }
    //}

    //private void MakeShortestPath()
    //{
    //    int i = 0;

    //    shortestPath.Add(shortestPathGoal);
    //    while (!shortestPath.Contains(start) && i++ < 1000)
    //    {
    //        Vector2 t = predecessors[(int)shortestPath[shortestPath.Count-1].x, (int)shortestPath[shortestPath.Count - 1].y];
    //        shortestPath.Add(t);
    //    }
    //}

    public void RemoveShortestPath()
    {
        start = new Vector2(-1, -1);
        shortestPath = new List<Vector2>();
        //shortestPathGoal = new Vector2(-1, -1);
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
