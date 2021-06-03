using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for the algorithms.
/// Mostly for holding consistent grid value
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public abstract class Algorithm : MonoBehaviour
{
    //The gridmap
    public static int[,] grid;

    //Lists for holding goals, deltas...
    public static List<Vector2> goals = new List<Vector2>();
    //Possible moves in the grid (right, down, left, up)
    public static List<Vector2> deltas = new List<Vector2> { new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1) };
    //Action names
    public static char[] deltaNames = new char[] { '>', 'v', '<', '^' };

    //Width and hight properties of grid
    public static int Width { get { return grid.GetLength(0); } }
    public static int Height { get { return grid.GetLength(1); } }

    //Arbitrary max cost representing infinite cost
    public static int MaxCost { get { return 999999999; } }

    //For later showing in simulation
    public static int Iterations { get; set; }


    /// <summary>
    /// Initializes the specified grid.
    /// </summary>
    /// <param name="grid">The gridmap.</param>
    /// <param name="goals">The set of goals.</param>
    public static void Init(int[,] grid, List<Vector2> goals)
    {
        Algorithm.grid = grid;
        Algorithm.goals = goals;
        Iterations = 0;
    }


    /// <summary>
    /// Debug print a 2D array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">The array.</param>
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

    /// <summary>
    /// Prints the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list">The list.</param>
    public static void PrintList<T>(List<T> list)
    {
        string s = "";
        foreach (T t in list)
        {
            s += t + "\n";
        }
        print(s);
    }
}
