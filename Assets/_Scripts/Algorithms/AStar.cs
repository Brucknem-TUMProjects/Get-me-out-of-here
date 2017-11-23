using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : Algorithm
{

    //private static int[,] grid;
    private static int[,] aStar;
    private static Vector2[,] predecessors;

    private static Vector2 start = new Vector2(-1, -1);

    static List<Vector2> closedList = new List<Vector2>();
    static List<Vector2> openList = new List<Vector2>();
    private static List<Vector2> shortestPath = new List<Vector2>();
    private static Vector2 shortestPathGoal;
    private static Vector2 lastExpanded;

    public static List<Vector2> ClosedList { get { return closedList; } }
    public static List<Vector2> OpenList { get { return openList; } }
    public static List<Vector2> ShortestPath { get { return shortestPath; } }
    public static Vector2 LastExpanded { get { return lastExpanded; } }
    public static int[,] value { get { return aStar; } }

    public static void Reset()
    {
        //Debug.Log("Calculating A*");
        closedList = new List<Vector2>();
        openList = new List<Vector2>();
        shortestPath = new List<Vector2>();
        lastExpanded = new Vector2(-1, -1);
    }

    public static void InitForSingleStep(int[,] grid, List<Vector2> goals, Vector2 start)
    {
        AStar.start = start;

        Reset();

        if (start.x == -1)
            return;
        Init(grid, goals);

        aStar = new int[Width, Height];
        Algorithm.grid = grid;
        predecessors = new Vector2[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
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

        print("OpenList");
        PrintList<Vector2>(OpenList);
        print("ClosedList");
        PrintList<Vector2>(ClosedList);
    }

    public static bool SingleStep()
    {
        if (start.x == -1)
            return false;
        print("Shortestpath length: " + shortestPath.Count);
        //print("OpenList: (" + OpenList.Count + ")");
        //PrintList<Vector2>(OpenList);
        //print("ClosedList: (" + closedList.Count + ")");
        //PrintList<Vector2>(ClosedList);
        if (openList.Count != 0 && shortestPath.Count == 0)
        {
            print("Innen");
            int lowest = 0;
            for (int i = 0; i < openList.Count; i++)
            {
                if (aStar[(int)openList[i].x, (int)openList[i].y] < aStar[(int)openList[lowest].x, (int)openList[lowest].y])
                    lowest = i;
            }
            Vector2 currentNode = openList[lowest];
            openList.RemoveAt(lowest);
            lastExpanded = currentNode;

            foreach (Vector2 goal in goals)
            {
                if (currentNode == goal)
                {
                    shortestPathGoal = currentNode;
                    MakeShortestPath();
                    return true;
                }
            }

                closedList.Add(currentNode);

                ExpandNode(currentNode);
            
        }
        return false;
    }


    public static List<Vector2> CalculateAStar(int[,] grid, List<Vector2> goals, Vector2 start)
    {

        Debug.Log("Calculating A*");
        Reset();
        Init(grid, goals);
        aStar = new int[Width, Height];
        Algorithm.grid = grid;
        predecessors = new Vector2[Width, Height];
        AStar.start = start;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
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
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
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
            }
                closedList.Add(currentNode);

                ExpandNode(currentNode);
        }
        shortestPath.Add(start);
        return shortestPath;
    }

    private static void ExpandNode(Vector2 node)
    {
        int x = (int)node.x;
        int y = (int)node.y;

        foreach (Vector2 delta in deltas)
        {
            Vector2 successor = node + delta;
            int dx = x + (int)delta.x;
            int dy = y + (int)delta.y;

            if (dx < 0 || dx >= Width || dy < 0 || dy >= Height)
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
        shortestPath = new List<Vector2>();
        shortestPath.Add(shortestPathGoal);
        while (!shortestPath.Contains(start) && i++ < 1000)
        {
            Vector2 t = predecessors[(int)shortestPath[shortestPath.Count - 1].x, (int)shortestPath[shortestPath.Count - 1].y];
            shortestPath.Add(t);
        }
    }
}