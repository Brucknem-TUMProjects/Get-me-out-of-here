using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData3D {

    private static GameData3D instance;

    private GameData3D()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public static GameData3D Instance
    {
        get
        {
            if (instance == null)
                instance = new GameData3D();
            return instance;
        }
    }

    //Tables for holding the map
    public int[,] grid;
    public int[,] value;
    public char[,] policy;

    //List for holding goals, deltas...
    public List<Vector2> goals = new List<Vector2>();
    public List<Vector2> delta = new List<Vector2> { new Vector2(1, 0), new Vector2 ( 0, -1 ),  new Vector2(- 1, 0 ), new Vector2(0, 1) };
    public char[] deltaNames = new char[] { '>', 'v', '<', '^' };

    public int MaxCost { get { return 100000000; } }

    public int currentWidth, currentHeight;
    public bool setGoals = false;

    public void SetGridSize(int x, int y)
    {
        grid = new int[x, y];
        value = new int[x, y];
        policy = new char[x, y];
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
        goals = new List<Vector2>();
        for (int x = 0; x < currentWidth; x++)
        {
            for (int y = 0; y < currentHeight; y++)
            {
                int randomCost = UnityEngine.Random.Range(0, 255);
                if (randomCost <= 200 && UnityEngine.Random.Range(0, 100) < 3)
                    goals.Add(new Vector2( x, y ));
                grid[x, y] = randomCost > 200 ? MaxCost : randomCost;
            }
        }
    }

    public void CalculateValue()
    {
        value = new int[currentWidth, currentHeight];

        InitGrid<int>(ref value, MaxCost);

        bool change = true;

        while (change)
        {
            change = false;

            for (int x = 0; x < currentWidth; x++)
            {
                for (int y = 0; y < currentHeight; y++)
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
                            for (int a = 0; a < delta.Count; a++)
                            {
                                int x2 = x + (int)delta[a][0];
                                int y2 = y + (int)delta[a][1];

                                if (x2 >= 0 && x2 < currentWidth && y2 >= 0 && y2 < currentHeight)
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

    public Color CostToColor(int cost)
    {
        return new Color(1, (255 - cost / 2) / 255.0f, (255 - cost) / 255.0f);
    }
}
