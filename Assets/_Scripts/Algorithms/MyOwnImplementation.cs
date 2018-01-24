using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyOwnImplementation : Algorithm {

    private static int[,] value;
    private static char[,] policy;

    public static int[,] Value { get { return value; } }
    public static char[,] Policy { get { return policy; } }

    public static int currentGoal = 0;
    public static Vector2 currentOpen;
    public static Vector2 lastExpanded;
    public static List<Vector2> openList = new List<Vector2>();

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

        foreach (Vector2 goal in goals)
        {
            value[(int)goal.x, (int)goal.y] = 0;
            policy[(int)goal.x, (int)goal.y] = '*';
            //openList = new List<Vector2>() { goal };
            ValueFunction_Old(goal, new List<Vector2>() { goal });
        }

    }

    private static void ValueFunction_New(Vector2 pos)
    {
        Iterations++;
        openList.Remove(pos);

        for (int i = 0; i < deltas.Count; i++)
        {
            Vector2 prev = pos - deltas[i];
            if (prev.x >= 0 && prev.x < Width && prev.y >= 0 && prev.y < Height)
            {
                if (grid[(int)prev.x, (int)prev.y] == MaxCost)
                {
                    value[(int)prev.x, (int)prev.y] = MaxCost;
                }
                else if (goals.Contains(prev))
                {
                    value[(int)prev.x, (int)prev.y] = 0;
                }
                else if (value[(int)prev.x, (int)prev.y] > value[(int)pos.x, (int)pos.y] + grid[(int)pos.x, (int)pos.y])
                {
                    if (!openList.Contains(prev))
                        PriorityAdd(prev);

                    if (goals.Contains(prev))
                    {
                        value[(int)prev.x, (int)prev.y] = 0;
                        policy[(int)prev.x, (int)prev.y] = '*';
                    }
                    else
                    {
                        value[(int)prev.x, (int)prev.y] = value[(int)pos.x, (int)pos.y] + grid[(int)pos.x, (int)pos.y];
                        policy[(int)prev.x, (int)prev.y] = deltaNames[i];
                    }
                }
            }
        }

        //Sort list
        if (openList.Count > 0)
        {
            //Nimm den mit der niedrigsten value
            ValueFunction_New(openList[0]/*, closed*/);
        }
    }

    private static void ValueFunction_Old(Vector2 pos, List<Vector2> open/*, List<Vector2> closed*/)
    {
        Iterations++;
        open.Remove(pos);

        for (int i = 0; i < deltas.Count; i++)
        {
            Vector2 prev = pos - deltas[i];
            if (prev.x >= 0 && prev.x < Width && prev.y >= 0 && prev.y < Height)
            {
                if (grid[(int)prev.x, (int)prev.y] == MaxCost)
                {
                    value[(int)prev.x, (int)prev.y] = MaxCost;
                }
                else if (goals.Contains(prev))
                {
                    value[(int)prev.x, (int)prev.y] = 0;
                }
                else if (value[(int)prev.x, (int)prev.y] > value[(int)pos.x, (int)pos.y] + grid[(int)pos.x, (int)pos.y])
                {
                    if (!open.Contains(prev))
                        //PriorityAdd(prev, ref open);
                        open.Add(prev);

                    if (goals.Contains(prev))
                    {
                        value[(int)prev.x, (int)prev.y] = 0;
                        policy[(int)prev.x, (int)prev.y] = '*';
                    }
                    else
                    {
                        value[(int)prev.x, (int)prev.y] = value[(int)pos.x, (int)pos.y] + grid[(int)pos.x, (int)pos.y];
                        policy[(int)prev.x, (int)prev.y] = deltaNames[i];
                    }
                }
            }
        }

        //Sort list
        if (open.Count > 0)
        {
            int index = -1;
            int lowest = int.MaxValue;
            for (int i = 0; i < open.Count; i++)
            {
                if (value[(int)open[i].x, (int)open[i].y] + grid[(int)pos.x, (int)pos.y] < lowest)
                {
                    lowest = value[(int)open[i].x, (int)open[i].y];
                    index = i;
                }
            }
            //Nimm den mit der niedrigsten value
            ValueFunction_Old (open[index], open/*, closed*/);
        }
    }

    private static void PriorityAdd(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        int value = grid[x, y];
        for(int i = 0; i < openList.Count; i++)
        {
            x = (int)openList[i].x;
            y = (int)openList[i].y;

            if(value >= grid[x, y])
            {
                openList.Insert(i, pos);
                return;
            }
        }
    }

    public static void InitForSingleStep(int[,] grid, List<Vector2> goals)
    {
        Init(grid, goals);

        //Print2DArray(Algorithm.grid);

        value = new int[Width, Height];
        policy = new char[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                value[i, j] = MaxCost;
            }
        }

        if (goals.Count > 0)
        {
            currentOpen = goals[0];
            lastExpanded = currentOpen;
            openList = new List<Vector2>() { goals[0] };
            value[(int)goals[0].x, (int)goals[0].y] = 0;
            policy[(int)goals[0].x, (int)goals[0].y] = '*';
        }
    }

    public static bool SingleStep()
    {
        lastExpanded = currentOpen;
        openList.Remove(currentOpen);

        for (int i = 0; i < deltas.Count; i++)
        {
            Vector2 prev = currentOpen - deltas[i];
            //print(prev);
            if (prev.x >= 0 && prev.x < Width && prev.y >= 0 && prev.y < Height)
            {
                if (grid[(int)prev.x, (int)prev.y] == MaxCost)
                {
                    value[(int)prev.x, (int)prev.y] = MaxCost;
                }
                else if (value[(int)prev.x, (int)prev.y] > value[(int)currentOpen.x, (int)currentOpen.y] + grid[(int)currentOpen.x, (int)currentOpen.y])
                {
                    if (!openList.Contains(prev))
                        openList.Add(prev);

                    if (goals.Contains(prev))
                    {
                        value[(int)prev.x, (int)prev.y] = 0;
                        policy[(int)prev.x, (int)prev.y] = '*';
                        //openList.Remove(prev);
                    }
                    else
                    {
                        value[(int)prev.x, (int)prev.y] = value[(int)currentOpen.x, (int)currentOpen.y] + grid[(int)currentOpen.x, (int)currentOpen.y];
                        policy[(int)prev.x, (int)prev.y] = deltaNames[i];
                    }
                }
            }
        }

        if (openList.Count > 0)
        {
            int index = -1;
            int lowest = int.MaxValue;
            for (int i = 0; i < openList.Count; i++)
            {
                if (value[(int)openList[i].x, (int)openList[i].y] + grid[(int)currentOpen.x, (int)currentOpen.y] < lowest)
                {
                    lowest = value[(int)openList[i].x, (int)openList[i].y];
                    index = i;
                }
            }
            currentOpen = openList[index];
            return false;
        }
        else
        {
            currentGoal++;

            if (goals.Count > currentGoal)
            {
                openList = new List<Vector2>() { goals[currentGoal] };
                currentOpen = goals[currentGoal];
                value[(int)goals[currentGoal].x, (int)goals[currentGoal].y] = 0;
                policy[(int)goals[currentGoal].x, (int)goals[currentGoal].y] = '*';
                return false;
            }
            else
                return true;
        }

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
