using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyOwnImplementation : Algorithm {

    //Memoization table for value function
    private static int[,] value;
    //Table for storing best action
    private static char[,] policy;

    //Accessors
    public static int[,] Value { get { return value; } }
    public static char[,] Policy { get { return policy; } }

    //Needed for solving single step
    public static int currentGoal = 0;
    //Current examined tile
    public static Vector2 currentOpen;
    //Last ecamined tile
    public static Vector2 lastExpanded;
    //List of tiles that have to be visited
    public static List<Vector2> openList = new List<Vector2>();



    /// <summary>
    /// Implementation of the recursive backwards induction approach.
    /// </summary>
    /// <param name="grid">The gridmap containing the costs of the tiles.</param>
    /// <param name="goals">The set of goals.</param>
    public static void Recursive(int[,] grid, List<Vector2> goals)
    {
        //Initialization of the algorithm
        Init(grid, goals);

        //Initialization of the memoization table
        value = new int[Width, Height];
        //Init. of the action table
        policy = new char[Width, Height];

        //Presetting of the memoization table to a default max value 
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                value[i, j] = MaxCost;
            }
        }

        //Iteration over all goals
        //Needed because the algorithms spreads recursively from one goal into the maze.
        //If one goal wouldn't be reached by spreading, the goals wouldn't be processed.
        //--> Starting algorithm from every goal, but as seen later instant termination if it was already explored
        foreach (Vector2 goal in goals)
        {
            //Base case of the algorithm, goal value = 0
            value[(int)goal.x, (int)goal.y] = 0;
            policy[(int)goal.x, (int)goal.y] = '*';
            //Starting the recursion
            ValueFunction(goal, new List<Vector2>() { goal });
        }

    }

    /// <summary>
    /// Calculates the value of the given position.
    /// </summary>
    /// <param name="pos">The position.</param>
    /// <param name="open">The list of open positions.</param>
    private static void ValueFunction(Vector2 pos, List<Vector2> open/*, List<Vector2> closed*/)
    {
        //Just for counting
        Iterations++;

        //Remove current position from open list.
        open.Remove(pos);

        //Iteration over the adjacent tiles
        for (int i = 0; i < deltas.Count; i++)
        {
            //Calculating position of adjacent tile
            Vector2 prev = pos - deltas[i];

            //Rangecheck
            if (prev.x >= 0 && prev.x < Width && prev.y >= 0 && prev.y < Height)
            {
                //Trivial cases
                //v(i,j) = inf, if c(i,j) = inf
                if (grid[(int)prev.x, (int)prev.y] == MaxCost)
                {
                    value[(int)prev.x, (int)prev.y] = MaxCost;
                }

                //Check if memoized value bigger than newly calculated one
                else if (value[(int)prev.x, (int)prev.y] > value[(int)pos.x, (int)pos.y] + grid[(int)pos.x, (int)pos.y])
                {
                    //Add processing position to open list
                    if (!open.Contains(prev))
                        open.Add(prev);

                    //Trivial case
                    //v(i,j) = 0, if (i,j) in goals
                    if (goals.Contains(prev))
                    {
                        value[(int)prev.x, (int)prev.y] = 0;
                        policy[(int)prev.x, (int)prev.y] = '*';
                    }
                    else
                    {
                        //Non trivial case
                        //Update value and action table
                        value[(int)prev.x, (int)prev.y] = value[(int)pos.x, (int)pos.y] + grid[(int)pos.x, (int)pos.y];
                        policy[(int)prev.x, (int)prev.y] = deltaNames[i];
                    }
                }
            }
        }

        //Check if any tile left in open list
        if (open.Count > 0)
        {
            //Search for tile in open list with lowest sum of value and cost
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
            //Recursively process the next tile
            ValueFunction (open[index], open/*, closed*/);
        }
    }


    /// <summary>
    /// Initializes for single step.
    /// </summary>
    /// <param name="grid">The gridmap holding the costs.</param>
    /// <param name="goals">The set of goals.</param>
    public static void InitForSingleStep(int[,] grid, List<Vector2> goals)
    {
        //Initialization of the algorithm
        Init(grid, goals);

        //Initialization of the memoization table
        value = new int[Width, Height];
        //Init. of the action table
        policy = new char[Width, Height];

        //Presetting of the memoization table to a default max value 
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                value[i, j] = MaxCost;
            }
        }

        //Iteration over all goals
        //Needed because the algorithms spreads recursively from one goal into the maze.
        //If one goal wouldn't be reached by spreading, the goals wouldn't be processed.
        //--> Starting algorithm from every goal, but as seen later instant termination if it was already explored
        if (goals.Count > 0)
        {
            //Initialization for single step
            currentOpen = goals[0];
            lastExpanded = currentOpen;
            openList = new List<Vector2>() { goals[0] };
            //Base case of the algorithm, goal value = 0
            value[(int)goals[0].x, (int)goals[0].y] = 0;
            policy[(int)goals[0].x, (int)goals[0].y] = '*';
        }
    }

    /// <summary>
    /// Do a single step.
    /// </summary>
    /// <returns>If algorithm is finished.</returns>
    public static bool SingleStep()
    {
        //Stet last expanded to current node
        lastExpanded = currentOpen;
        //Remove current node from open list
        openList.Remove(currentOpen);

        //Iteration over the adjacent tiles
        for (int i = 0; i < deltas.Count; i++)
        {
            //Calculating position of adjacent tile
            Vector2 prev = currentOpen - deltas[i];
            //Rangecheck
            if (prev.x >= 0 && prev.x < Width && prev.y >= 0 && prev.y < Height)
            {
                //Trivial cases
                //v(i,j) = inf, if c(i,j) = inf
                if (grid[(int)prev.x, (int)prev.y] == MaxCost)
                {
                    value[(int)prev.x, (int)prev.y] = MaxCost;
                }
                //Check if memoized value bigger than newly calculated one
                else if (value[(int)prev.x, (int)prev.y] > value[(int)currentOpen.x, (int)currentOpen.y] + grid[(int)currentOpen.x, (int)currentOpen.y])
                {
                    //Add processing position to open list
                    if (!openList.Contains(prev))
                        openList.Add(prev);

                    //Trívial case
                    //v(i,j) = 0, if (i,j) in goals
                    if (goals.Contains(prev))
                    {
                        value[(int)prev.x, (int)prev.y] = 0;
                        policy[(int)prev.x, (int)prev.y] = '*';
                    }
                    else
                    {
                        //Non trivial case
                        //Update value and action table
                        value[(int)prev.x, (int)prev.y] = value[(int)currentOpen.x, (int)currentOpen.y] + grid[(int)currentOpen.x, (int)currentOpen.y];
                        policy[(int)prev.x, (int)prev.y] = deltaNames[i];
                    }
                }
            }
        }

        //Check if any tile left in open list
        if (openList.Count > 0)
        {
            //Search for tile in open list with lowest sum of value and cost
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
            //Set found tile as current open
            currentOpen = openList[index];
            return false;
        }
        //No tile left in open list
        else
        {
            //Jump to next goal
            currentGoal++;

            //If there is a goal left, restart algorithm with this goal as starting point
            if (goals.Count > currentGoal)
            {
                openList = new List<Vector2>() { goals[currentGoal] };
                currentOpen = goals[currentGoal];
                value[(int)goals[currentGoal].x, (int)goals[currentGoal].y] = 0;
                policy[(int)goals[currentGoal].x, (int)goals[currentGoal].y] = '*';
                return false;
            }
            //Algorithm is finished
            else
                return true;
        }
    }
}
