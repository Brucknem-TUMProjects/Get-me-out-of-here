using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap3D : CreateMap
{
    //public override void AdjustMap()
    //{
    //    //float tileDim = Mathf.Min(1.0f * mapHeight / GameData3D.Instance.currentHeight, 1.0f * mapWidth / GameData3D.Instance.currentWidth, 30);

    //    //foreach (KeyValuePair<Vector2, GameObject> tile in tiles)
    //    //{
    //    //    tile.Value.SetActive(false);

    //    //    tile.Value.transform.GetChild(0).gameObject.SetActive(false);
    //    //    tile.Value.transform.GetChild(1).gameObject.SetActive(false);
    //    //    tile.Value.transform.GetChild(2).gameObject.SetActive(true);
    //    //    tile.Value.transform.GetChild(3).gameObject.SetActive(false);
    //    //}

    //    for (int x = 0; x < GameData.Instance.MaxWidth; x++)
    //    {
    //        for (int y = 0; y < GameData.Instance.MaxHeight; y++)
    //        {
    //            Vector2 pos = new Vector2(x, y);

    //            if (x < GameData.Instance.currentWidth && y < GameData.Instance.currentHeight)
    //            {
    //                if (!tiles.ContainsKey(pos))
    //                {
    //                    tiles.Add(pos, Instantiate(tile));
    //                    tiles[pos].transform.SetParent(/*map.*/transform);
    //                    //tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.CostToColor(GameData.Instance.grid[x, y]);
    //                    //tiles[pos].transform.position = new Vector3(x, 0, y);
    //                }

    //                tiles[pos].SetActive(true);

    //                tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
    //                tiles[pos].transform.GetChild(1).gameObject.SetActive(false);
    //                tiles[pos].transform.GetChild(2).gameObject.SetActive(true);
    //                tiles[pos].transform.GetChild(3).gameObject.SetActive(false);
    //                tiles[pos].transform.position = new Vector3(x, 0, y);

    //                if (GameData.Instance.goals.Contains(pos))
    //                {
    //                    tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.goal;
    //                }
    //                else if (GameData.Instance.grid[x, y] == GameData.Instance.MaxCost)
    //                {
    //                    tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.occupied;
    //                    tiles[pos].transform.GetChild(1).gameObject.SetActive(true);
    //                }
    //                else
    //                {
    //                    tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.CostToColor(GameData.Instance.grid[(int)pos.x, (int)pos.y]);
    //                    tiles[pos].transform.GetChild(2).transform.GetChild(0).GetComponent<InputField>().text = GameData.Instance.grid[(int)pos.x, (int)pos.y].ToString();
    //                }
    //            }
    //            else
    //            {
    //                if (tiles.ContainsKey(pos))
    //                {
    //                    tiles[pos].SetActive(false);
    //                }
    //            }
    //        }
    //    }
    //    ShowShortestPath();
    //}

    public override void AdjustMap_Activate(Vector2 pos)
    {
        tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
        tiles[pos].transform.GetChild(1).gameObject.SetActive(false);
        tiles[pos].transform.GetChild(2).gameObject.SetActive(true);
        tiles[pos].transform.GetChild(3).gameObject.SetActive(false);
        tiles[pos].transform.position = new Vector3((int)pos.x, 0, (int)pos.y);
    }

    public override void AdjustMap_Goal(Vector2 pos)
    {
        tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.goal;
    }

    public override void AdjustMap_Occupied(Vector2 pos)
    {
        tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.occupied;
        tiles[pos].transform.GetChild(1).gameObject.SetActive(true);
    }

    public override void AdjustMap_Normal(Vector2 pos)
    {
        tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.CostToColor(GameData.Instance.grid[(int)pos.x, (int)pos.y]);
        tiles[pos].transform.GetChild(2).transform.GetChild(0).GetComponent<InputField>().text = GameData.Instance.grid[(int)pos.x, (int)pos.y].ToString();
    }

    //public override void ShowCostTable()
    //{
    //    for (int x = 0; x < GameData.Instance.currentWidth; x++)
    //    {
    //        for (int y = 0; y < GameData.Instance.currentHeight; y++)
    //        {
    //            Vector2 pos = new Vector2(x, y);
    //            tiles[pos].SetActive(true);

    //            //tiles[pos].transform.GetChild(2).gameObject.SetActive(false);

    //            ShowCostTable_Begin(pos);

    //            if (GameData.Instance.value[x, y] == GameData.Instance.MaxCost)
    //            {
    //                if (GameData.Instance.walls[x, y])
    //                {
    //                    //tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.occupied;
    //                    //tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
    //                    //tiles[pos].transform.GetChild(1).gameObject.SetActive(true);
    //                    //tiles[pos].transform.GetChild(3).gameObject.SetActive(false);

    //                    ShowCostTable_Occupied(pos);
    //                }
    //                else
    //                {
    //                    //tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.unreachable;
    //                    //tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
    //                    //tiles[pos].transform.GetChild(1).gameObject.SetActive(false);
    //                    //tiles[pos].transform.GetChild(3).gameObject.SetActive(true);

    //                    ShowCostTable_Unreachable(pos);
    //                }
    //            }
    //            else
    //            {
    //                //tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
    //                //tiles[pos].transform.GetChild(1).gameObject.SetActive(false);
    //                //tiles[pos].transform.GetChild(3).gameObject.SetActive(false);
    //                if (GameData.Instance.policy[x, y] == '*')
    //                {
    //                    //tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.goal;
    //                    //tiles[pos].transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, -90);
    //                    //tiles[pos].transform.GetChild(0).transform.localScale = new Vector3(2, .4f, .4f);
    //                    //tiles[pos].transform.GetChild(0).transform.localPosition = new Vector3(0, 2.5f, 0);
    //                    ShowCostTable_Goal(pos);
    //                }
    //                else
    //                {
    //                    //tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.CostToColor(GameData.Instance.grid[x, y]);
    //                    //int rotation = Array.IndexOf(GameData.Instance.deltaNames, GameData.Instance.policy[x, y]);
    //                    //tiles[pos].transform.GetChild(0).transform.rotation = Quaternion.Euler(-90, 90 * rotation, 0);
    //                    //tiles[pos].transform.GetChild(0).transform.localScale = new Vector3(.4f, .4f, 1);
    //                    //tiles[pos].transform.GetChild(0).transform.localPosition = new Vector3(0, 1, 0);
    //                    ShowCostTable_Reachable(pos);
    //                }
    //            }
    //        }
    //    }
    //    ShowShortestPath();
    //}

    //public override void ShowShortestPath()
    //{
    //    //print("Showing cost table");
    //    foreach (Vector2 v in GameData.Instance.shortestPath)
    //    {
    //        Color color;
    //        if (GameData.Instance.goals.Contains(v))
    //            color = GameData.Instance.goal;
    //        else if (GameData.Instance.start == v)
    //            color = GameData.Instance.begin;
    //        else
    //            color = GameData.Instance.onPath;
    //        tiles[v].GetComponent<Renderer>().material.color = color;
    //    }
    //}

    public override void ShowCostTable_Begin(Vector2 pos)
    {
        tiles[pos].transform.GetChild(2).gameObject.SetActive(false);
    }

    public override void ShowCostTable_Occupied(Vector2 pos)
    {
        tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.occupied;
        tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
        tiles[pos].transform.GetChild(1).gameObject.SetActive(true);
        tiles[pos].transform.GetChild(3).gameObject.SetActive(false);
    }

    public override void ShowCostTable_Unreachable(Vector2 pos)
    {
        tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.unreachable;
        tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
        tiles[pos].transform.GetChild(1).gameObject.SetActive(false);
        tiles[pos].transform.GetChild(3).gameObject.SetActive(true);
    }

    public override void ShowCostTable_Goal(Vector2 pos)
    {
        tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
        tiles[pos].transform.GetChild(1).gameObject.SetActive(false);
        tiles[pos].transform.GetChild(3).gameObject.SetActive(false);

        tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.goal;
        tiles[pos].transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, -90);
        tiles[pos].transform.GetChild(0).transform.localScale = new Vector3(2, .4f, .4f);
        tiles[pos].transform.GetChild(0).transform.localPosition = new Vector3(0, 2.5f, 0);
    }

    public override void ShowCostTable_Reachable(Vector2 pos)
    {
        tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
        tiles[pos].transform.GetChild(1).gameObject.SetActive(false);
        tiles[pos].transform.GetChild(3).gameObject.SetActive(false);

        tiles[pos].GetComponent<Renderer>().material.color = GameData.Instance.CostToColor(GameData.Instance.grid[(int)pos.x, (int)pos.y]);
        int rotation = Array.IndexOf(GameData.Instance.deltaNames, GameData.Instance.policy[(int)pos.x, (int)pos.y]);
        tiles[pos].transform.GetChild(0).transform.rotation = Quaternion.Euler(-90, 90 * rotation, 0);
        tiles[pos].transform.GetChild(0).transform.localScale = new Vector3(.4f, .4f, 1);
        tiles[pos].transform.GetChild(0).transform.localPosition = new Vector3(0, 1, 0);
    }

    public override void ShortestPathColor(Vector2 pos, Color color)
    {
        tiles[pos].GetComponent<Renderer>().material.color = color;
    }
}
