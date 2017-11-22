using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicProgramming : MonoBehaviour {

    public static void CalculateValue(int[,] grid, ref int[,] value, ref char[,] policy, int width, int height, List<Vector2> goals, List<Vector2> deltas, char[] deltaNames, int MaxCost)
    {
        Debug.Log("Calculating optimal policy");
        value = new int[width, height];
        policy = new char[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                value[i, j] = MaxCost;
            }
        }

        GameData.Instance.InitGrid<int>(ref value, GameData.Instance.MaxCost);

        bool change = true;

        while (change)
        {
            change = false;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
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

                                if (x2 >= 0 && x2 < width && y2 >= 0 && y2 < height)
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
}
