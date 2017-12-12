using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Algorithm : MonoBehaviour
{
    public static int[,] grid;
    //public static int[,] value;

    //List for holding goals, deltas...
    public static List<Vector2> goals = new List<Vector2>();
    public static List<Vector2> deltas = new List<Vector2> { new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1) };
    public static char[] deltaNames = new char[] { '>', 'v', '<', '^' };

    public static int Width { get { return grid.GetLength(0); } }
    public static int Height { get { return grid.GetLength(1); } }

    public static int MaxCost { get { return 999999999; } }

    public static int Iterations { get; set; }


    public static void Init(int[,] grid, List<Vector2> goals)
    {
        Algorithm.grid = grid;
        Algorithm.goals = goals;
        Iterations = 0;

    }

    public static void Print2DArray<T>(T[,] array)
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
        print(s);
    }

    public static void PrintList<T>(List<T> list)
    {
        string s = "";
        foreach(T t in list)
        {
            s += t + "\n";
        }
        print(s);
    }

    //public static void AddGoal(Vector2 pos)
    //{
    //    goals.Add(pos);
    //}

    //public static void AddWall(Vector2 pos)
    //{
    //    int x = (int)pos.x;
    //    int y = (int)pos.y;
    //    try
    //    {
    //        grid[x, y] = MaxCost;
    //        value[x, y] = MaxCost;
    //    }
    //    catch (Exception) { };
    //}
}
