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
    //public List<Vector2> deltas = new List<Vector2> { new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1) };
    //public char[] deltaNames = new char[] { '>', 'v', '<', '^' };

    //public int MaxCost { get { return 100000000; } }
    public int MaxWidth;
    public int MaxHeight;

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
    

    public void Initialize(int x, int y)
    {
        InitGrid<int>(ref grid, 1, x, y);
        InitGrid<bool>(ref walls, false, x, y);
        InitGrid<int>(ref value, Algorithm.MaxCost, x, y);
        InitGrid<char>(ref policy, ' ', x, y);
        InitGrid<int>(ref aStar, Algorithm.MaxCost, x, y);
        InitGrid<Vector2>(ref predecessors, Vector2.zero, x, y);
        GameData.Instance.goals = new List<Vector2>();
        start = new Vector2(-1, -1);
        shortestPath = new List<Vector2>();
    }

    public void InitGrid<T>(ref T[,] grid, T value, int width, int height)
    {
        grid = new T[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = value;
            }
        }
    }

    public void RandomGrid()
    {
        shortestPath = new List<Vector2>();
        GameData.Instance.goals = new List<Vector2>();
        start = new Vector2(-1, -1);
        for (int x = 0; x < currentWidth; x++)
        {
            for (int y = 0; y < currentHeight; y++)
            {
                int randomCost = UnityEngine.Random.Range(0, 255);
                if (randomCost <= 200 && UnityEngine.Random.Range(0, 100) < 3)
                    GameData.Instance.goals.Add(new Vector2( x, y ));
                if (randomCost > 200)
                {
                    grid[x, y] = Algorithm.MaxCost;
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
        DynamicProgramming.CalculateValue(grid, goals);
        value = DynamicProgramming.Value;
        policy = DynamicProgramming.Policy;
    }

    public void InitAStarSingleStep()
    {
        AStar.InitForSingleStep(grid, goals, start);
    }

    public bool AStarSingleStep()
    {
        shortestPath = AStar.RunSingleStep();
        return shortestPath.Count != 0;
        //bool finished = AStar.SingleStep();
        //shortestPath = AStar.ShortestPath;
        //return finished;
    }

    public void ResetAStar()
    {
        start = new Vector2(-1, -1);
        AStar.Reset();
        shortestPath = AStar.ShortestPath;
    }

    public List<Vector2> OpenList { get { return AStar.OpenList; } }
    public List<Vector2> ClosedList { get { return AStar.ClosedList; } }
    public Vector2 LastExpanded { get { return AStar.LastExpanded; } }
    public int[,] AStarLengths { get { return AStar.Value; } }


    public void ResetDynamicProgramming()
    {
        DynamicProgramming.Reset();
    }

    public void InitDynamicProgrammingSingleStep()
    {
        DynamicProgramming.InitForSingleStep(grid, goals);
    }

    public bool DynamicProgrammingSingleStep()
    {
        bool finished = DynamicProgramming.RunSingleStep();
        value = DynamicProgramming.Value;
        policy = DynamicProgramming.Policy;
        return finished;
    }

    public Vector3 DynamicProgrammingHighlighted
    {
        get
        {
            return new Vector3(DynamicProgramming.CurrentPosition.x, DynamicProgramming.CurrentPosition.y, DynamicProgramming.CurrentDelta);
        }
    }

    public Color CostToColor(int cost)
    {
        return new Color(1, (255 - cost / 2) / 255.0f, (255 - cost) / 255.0f);
    }

    public void CalculateAStar()
    {
        if (start.x != -1)
            shortestPath = AStar.CalculateAStar(grid, GameData.Instance.goals, start);
        else
            shortestPath = new List<Vector2>();
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
