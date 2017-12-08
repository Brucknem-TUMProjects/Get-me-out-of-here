using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicProgramming : Algorithm {

    private static int[,] value;
    private static char[,] policy;

    private static int iterations = 0;

    private static int currentPosition = -1;
    private static int currentDelta = 0;

    private static bool change = true;
    
    public static int[,] Value { get { return value; } }
    public static char[,] Policy { get { return policy; } }
    public static Vector2 CurrentPosition { get { return new Vector2(currentPosition % Width, Mathf.Floor(currentPosition / Width)); } }
    public static int CurrentDelta { get { return currentDelta; } }
    public static int Iterations { get { return iterations; } }


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
        iterations = 0;

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
        print(x + " - " + y + " - " + delta);
        bool isGoal = false;

        for (int i = 0; i < goals.Count; i++)
        {
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
        }

        if (grid[x, y] == MaxCost)
        {
            currentPosition++;
            currentDelta = -1;
            return;
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
            print(++iterations);
        }

            return !change && currentPosition == Width * Height - 1 && currentDelta == 3;
    }

    public static void CalculateValue(int[,] grid, List<Vector2> goals)
    {
        Debug.Log("Calculating optimal policy");
        Init(grid, goals);

        value = new int[Width, Height];
        policy = new char[Width, Height];

        iterations = 0;

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
            print(++iterations);
            change = false;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    bool isGoal = false;

                    for (int i = 0; i < goals.Count; i++)
                    {
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
                    }

                    if (grid[x, y] == MaxCost)
                    {
                        continue;
                    }

                    if (!isGoal && grid[x, y] < MaxCost)
                    {
                        for (int a = 0; a < deltas.Count; a++)
                        {
                            int x2 = x + (int)deltas[a][0];
                            int y2 = y + (int)deltas[a][1];

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

    private static int counter = 0;

    public static void Recursive(int[,] grid, List<Vector2> goals)
    {
        Init(grid, goals);

        value = new int[Width, Height];
        policy = new char[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                value[i, j] = MaxCost;
            }
        }

        foreach(Vector2 goal in goals)
        {
            counter = 0;
            value[(int)goal.x, (int)goal.y] = 0;
            policy[(int)goal.x, (int)goal.y] = '*';
            ValueFunction(goal, new List<Vector2>() { goal, }/*, new List<Vector2>()*/);
        }
    }

    private static void ValueFunction(Vector2 pos, List<Vector2> open/*, List<Vector2> closed*/)
    {
        if (counter++ > 1000)
            return;

        open.Remove(pos);
        //closed.Add(pos);

        for (int i = 0; i < deltas.Count; i++)
        {
            Vector2 prev = pos - deltas[i];
            //print("Current pos: " + pos + " - Maybe opening: " + prev);
            if (prev.x >= 0 && prev.x < Width && prev.y >= 0 && prev.y < Height)
            {
                //if (!closed.Contains(prev))
                //{
                    //print("Opening");
                    if(grid[(int)prev.x, (int)prev.y] == MaxCost)
                    {
                        value[(int)prev.x, (int)prev.y] = MaxCost;
                        //closed.Add(prev);
                        //open.Remove(prev);
                    }
                    else if (value[(int)prev.x, (int)prev.y] > value[(int)pos.x, (int)pos.y] + grid[(int)pos.x, (int)pos.y])
                    {
                        if (!open.Contains(prev))
                            open.Add(prev);

                        value[(int)prev.x, (int)prev.y] = value[(int)pos.x, (int)pos.y] + grid[(int)pos.x, (int)pos.y];
                        policy[(int)prev.x, (int)prev.y] = deltaNames[i];
                    }
                //}
            }
        }

        string s = "open: ";
        foreach (Vector2 v in open)
            s += v + ", ";
        //print(s);

        //Sort list
        if(open.Count > 0)
        {
            int index = -1;
            int lowest = int.MaxValue;
            for (int i = 0; i < open.Count; i++)
            {
                if(value[(int)open[i].x, (int)open[i].y] + grid[(int)pos.x, (int)pos.y] < lowest)
                {
                    lowest = value[(int)open[i].x, (int)open[i].y];
                    index = i;
                }
            }
            ValueFunction(open[index], open/*, closed*/);
        }

        //Nimm den mit der niedrigsten value
    }


    //public static void Recursive(int[,] grid, List<Vector2> goals)
    //{
    //    Init(grid, goals);

    //    value = new int[Width, Height];
    //    policy = new char[Width, Height];

    //    for (int i = 0; i < Width; i++)
    //    {
    //        for (int j = 0; j < Height; j++)
    //        {
    //            value[i, j] = MaxCost;
    //        }
    //    }

    //    Print2DArray(grid);

    //    for (int x = 0; x < Width; x++)
    //    {
    //        for (int y = 0; y < Height; y++)
    //        {
    //            counter = 0;
    //            //visited = new List<Vector2>();
    //            value[x, y] = ValueFunction(new Vector2(x, y), new List<Vector2>());
    //            Print2DArray(value);
    //        }
    //    }
    //}

    //private static int ValueFunction(Vector2 pos, List<Vector2> pre)
    //{
    //    //bool alreadyCalc = true;

    //    //for (int i = 0; i < 4; i++)
    //    //{
    //    //    Vector2 next = pos + deltas[i];
    //    //    if (!(next.x < 0 || next.x >= Width || next.y < 0 || next.y >= Height))
    //    //    {
    //    //        if (value[(int)next.x, (int)next.y] == MaxCost)
    //    //            alreadyCalc = false;
    //    //    }

    //    //}

    //    //if (alreadyCalc && value[(int)pos.x, (int)pos.y] != MaxCost)
    //    //    return value[(int)pos.x, (int)pos.y];

    //    if (counter++ > Width * Height)
    //    {
    //        print("MAAAAAAAAAAAAAAAAAAX COUNTER REACHED");
    //        return MaxCost;
    //    }

    //    print("Calculating for: " + pos);
    //    if (goals.Contains(pos))
    //    {
    //        value[(int)pos.x, (int)pos.y] = 0;
    //        string s = "Pos: " + pos + " - Goal found!" + " - Pre: ";
    //        foreach (Vector2 v in pre)
    //        {
    //            s += v + ", ";
    //        }
    //        print(s);
    //        return 0;
    //    }

    //    //if (pos.x < 0 || pos.x >= Width || pos.y < 0 || pos.y >= Height)
    //    //{
    //    //    //pre.Remove(pos);
    //    //    string s = "Pos: " + pos + " - Out of bounds!" + " - Pre: ";
    //    //    foreach (Vector2 v in pre)
    //    //    {
    //    //        s += v + ", ";
    //    //    }
    //    //    print(s);
    //    //    return MaxCost;
    //    //}

    //    int[] values = new int[4];

    //    for (int i = 0; i < 4; i++)
    //    {
    //        Vector2 next = pos + deltas[i];
    //        //print("Calculating for: " + pos + " - Next: " + next);

    //        if (next.x < 0 || next.x >= Width || next.y < 0 || next.y >= Height || pre.Contains(next))
    //        {
    //            //print("NOT CALCULATING");

    //            values[i] = MaxCost;
    //        }
    //        else
    //        {
    //            //print("Calculating");
    //            pre.Add(pos);
    //            values[i] = ValueFunction(next, pre) + grid[(int)next.x, (int)next.y];


    //            pre.Remove(pos);
    //            counter--;
    //        }

    //        string s = "Pos: " + pos + " - " + "values[" + i + "] = " + values[i].ToString() + " - Pre: ";
    //        foreach (Vector2 v in pre)
    //        {
    //            s += v + ", ";
    //        }
    //        //print(s);
    //    }

    //    value[(int)pos.x, (int)pos.y] = Mathf.Min(values);
    //    return Mathf.Min(values);
    //}
}
