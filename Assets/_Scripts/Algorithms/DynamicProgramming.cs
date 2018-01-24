using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicProgramming : Algorithm {

    private static int[,] value;
    private static char[,] policy;


    private static int currentPosition = -1;
    private static int currentDelta = 0;

    private static bool change = true;
    
    public static int[,] Value { get { return value; } }
    public static char[,] Policy { get { return policy; } }
    public static Vector2 CurrentPosition { get { return new Vector2(currentPosition % Width, Mathf.Floor(currentPosition / Width)); } }
    public static int CurrentDelta { get { return currentDelta; } }


    public static void Reset()
    {
        currentPosition = -1;
    }

    public static void InitForSingleStep(int[,] grid, List<Vector2> goals)
    {
        Init(grid, goals);
        //print(Width + " - " + Height);
        currentPosition = 0;
        currentDelta = 0;

        value = new int[Width, Height];
        policy = new char[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                value[i, j] = MaxCost;
                policy[i, j] = ' ';
            }
        }
    }

    private static void SingleStep(int x, int y, int delta)
    {
        //print(x + " - " + y + " - " + delta);
        bool isGoal = false;

        //for (int i = 0; i < goals.Count; i++)
        //{
            if (goals.Contains(new Vector2(x,y)))
            {
                if (value[x, y] > 0)
                {
                    value[x, y] = 0;
                    policy[x, y] = '*';
                    change = true;
                    //print("UPPER");
                }
                isGoal = true;
            }
        //}

        if (grid[x, y] == MaxCost)
        {
            if (!(x == Width - 1 && y == Height - 1))
            {
                currentPosition++;
                currentDelta = -1;
                return;
            }
        }
        
        if (!isGoal && grid[x, y] < MaxCost)
        {
            int x2 = x + (int)deltas[delta][0];
            int y2 = y + (int)deltas[delta][1];

            if (x2 >= 0 && x2 < Width && y2 >= 0 && y2 < Height)
            {
                int v2;
                if (grid[x2, y2] == MaxCost)
                    v2 = MaxCost;
                else
                    v2 = value[x2, y2] + grid[x2, y2];
                if (v2 < value[x, y])
                {
                    change = true;
                    //print("LOWER");
                    value[x, y] = v2;
                    policy[x, y] = deltaNames[delta];
                }
            }
        }
        //Print2DArray<int>(value);
    }

    public static bool RunSingleStep()
    {
        Iterations++;
        if (currentPosition == 0)
            change = false;
        //for (int i = 0; i < 4; i++)
        //{
            SingleStep(currentPosition % Width, Mathf.FloorToInt(currentPosition / Width), currentDelta);
            currentDelta++;
        //}
        if (currentDelta == 4)
        {
            currentPosition++;
            currentDelta = 0;
        }

        if (currentPosition == Width * Height)
        {
            currentPosition = 0;
            //print(++Iterations);
        }

            return !change && currentPosition >= Width * Height - 1 && currentDelta == 3;
    }

    public static void CalculateValue(int[,] grid, List<Vector2> goals)
    {
        //Debug.Log("Calculating optimal policy");
        Init(grid, goals);

        value = new int[Width, Height];
        policy = new char[Width, Height];

        Iterations = 0;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                value[i, j] = MaxCost;
            }
        }

        //GameData.Instance.InitGrid<int>(ref value, GameData.Instance.MaxCost);

        bool change = true;

        while (change)
        {
            change = false;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    bool isGoal = false;
                    Iterations++;
                    //for (int i = 0; i < goals.Count; i++)
                    //{
                        if (goals.Contains(new Vector2(x, y)))
                        {
                            if (value[x, y] > 0)
                            {
                                value[x, y] = 0;
                                policy[x, y] = '*';
                                change = true;
                            }
                            isGoal = true;
                        }
                    //}
                    
                    if (!isGoal && grid[x, y] < MaxCost)
                    {
                        for (int i = deltas.Count - 1; i >= 0; i--)
                        {
                            Vector2 next = new Vector2(x, y) + deltas[i];
                            if (next.x >= 0 && next.x < Width && next.y >= 0 && next.y < Height)
                            {
                                int v2;
                                if (grid[(int)next.x, (int)next.y] == MaxCost)
                                    v2 = MaxCost;
                                else
                                    v2 = value[(int)next.x, (int)next.y] + grid[(int)next.x, (int)next.y];
                                if (v2 < value[x, y])
                                {
                                    change = true;
                                    value[x, y] = v2;
                                    policy[x, y] = deltaNames[i];
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
