using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
//lol
    public int[,] grid;
    public Vector2 goal;
    public int[,] delta;
    public int[,] value;
    
    public int width, height;
    public int costStep = 1;
    public GameObject cube;
    public Dictionary<Vector2, GameObject> board = new Dictionary<Vector2, GameObject>();
    //public GameObject camera;

    // Use this for initialization
    void Start()
    {
        grid = new int[,]{
            {0,0,1,0,0,0 },
            {0,0,1,0,0,0 },
            {0,0,1,0,0,0 },
            {0,0,0,0,1,0 },
            {0,0,1,1,1,0 },
            {0,0,0,0,1,0 },
        };

        DrawBoard<int>(grid);

        Print2DArray<int>(grid);

        width = grid.GetLength(1);
        height = grid.GetLength(0);

        print(width + " - " + height);

        goal = new Vector2(width - 1, height - 1);
        delta = new int[,]
        {
            {0,1 },
            {0,-1 },
            {1,0 },
            {-1,0 }
        };

        CalculateValue();
    }

    void CalculateValue()
    {
        int[,] value = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                value[i, j] = 99;
            }
        }

        Print2DArray<int>(value);

        bool change = true;

        while (change)
        {
            change = false;

            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    if(goal.x == x && goal.y == y)
                    {
                        if (value[x, y] > 0)
                        {
                            value[x, y] = 0;
                            change = true;
                        }
                    }else if(grid[x,y] == 0)
                    {
                        for(int a = 0; a < delta.GetLength(0); a++)
                        {
                            int x2 = x + delta[a, 0];
                            int y2 = y + delta[a, 1];

                            if (x2 >= 0 && x2 < width && y2 >= 0 && y2 < height)
                            {
                                int v2 = value[x2, y2] + costStep;
                                if(v2 < value[x, y])
                                {
                                    change = true;
                                    value[x, y] = v2;
                                }
                            }
                        }
                    }
                }
            }
        }
        Print2DArray<int>(value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitCheckerBoard(int width, int height)
    {
        grid = new int[width, height];


    }

    void Print2DArray<T>(T[,] array)
    {
        string s = "";
        for(int i = 0; i < array.GetLength(1); i++)
        {
            for(int j = 0; j < array.GetLength(0); j++)
            {
                s += (array[i, j] + " ");
            }
            s += ("\n");
        }
        print(s);
    }

    void DrawBoard<T>(T[,] toDraw)
    {
        //camera.transform.position = new Vector3(toDraw.GetLength(1) / 2, 10, -toDraw.GetLength(0) / 2);
        for (int x = 0; x < toDraw.GetLength(1); x++)
        {
            for (int y = 0; y < toDraw.GetLength(0); y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (!board.ContainsKey(pos))
                    board.Add(pos, Instantiate(cube, new Vector3(y, 0, -x), Quaternion.identity));

                board[pos].GetComponentInChildren<TextMesh>().text = toDraw[x, y].ToString();
                board[pos].transform.parent = this.transform;
            }
        }
    }
}