using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap2D : CreateMap
{
    //public override void AdjustMap()
    //{
    //    //float tileDim = Mathf.Min(1.0f * mapHeight / GameData.Instance.currentHeight, 1.0f * mapWidth / GameData.Instance.currentWidth, 30);
        
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
    //                    tiles[pos].transform.SetParent(transform);
    //                    //tiles[pos].GetComponent<TileClick>().SetPosition(pos);
    //                }
    //                tiles[pos].SetActive(true);

    //                //tiles[pos].GetComponent<TileClick>().SetPosition(pos);
    //                //tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;
    //                //tiles[pos].GetComponent<InputField>().interactable = true;

    //                //tiles[pos].GetComponent<RectTransform>().localPosition = new Vector3(
    //                //    (x - GameData.Instance.currentWidth / 2.0f) * tileDim + 0.5f * tileDim,
    //                //    (y - GameData.Instance.currentHeight / 2.0f) * tileDim + 0.5f * tileDim
    //                //    );
    //                //tiles[pos].GetComponent<RectTransform>().localScale = new Vector3(
    //                //    tileDim / 30, tileDim / 30
    //                //    );
    //                AdjustMap_Activate(pos);

    //                if (GameData.Instance.goals.Contains(pos))
    //                {
    //                    AdjustMap_Goal(pos);
    //                    //int cost = GameData.Instance.grid[x, y];
    //                    //tiles[pos].GetComponent<InputField>().text = cost.ToString();
    //                    //tiles[pos].GetComponent<Image>().color = GameData.Instance.goal;
    //                }
    //                else if (GameData.Instance.grid[x, y] == GameData.Instance.MaxCost)
    //                {
    //                    AdjustMap_Occupied(pos);
    //                    //tiles[pos].GetComponent<Image>().color = GameData.Instance.occupied;
    //                    //tiles[pos].GetComponent<InputField>().interactable = false;
    //                }
    //                else
    //                {
    //                    AdjustMap_Reachable(pos);
    //                    //int cost = GameData.Instance.grid[x, y];
    //                    //tiles[pos].GetComponent<Image>().color = GameData.Instance.CostToColor(cost);
    //                    //tiles[pos].GetComponent<InputField>().text = cost.ToString();
    //                }
    //            }
    //            else
    //            {
    //                if (tiles.ContainsKey(pos))
    //                {
    //                    tiles[pos].gameObject.SetActive(false);
    //                }
    //            }
    //        }
    //    }

    //    ShowShortestPath();
    //}

    public override void AdjustMap_Activate(Vector2 pos)
    {
        float tileDim = Mathf.Min(1.0f * mapHeight / GameData.Instance.currentHeight, 1.0f * mapWidth / GameData.Instance.currentWidth, 30);
        tiles[pos].GetComponent<TileClick>().SetPosition(pos);
        tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.Custom;

        tiles[pos].GetComponent<InputField>().interactable = true;

        tiles[pos].GetComponent<RectTransform>().localPosition = new Vector3(
            ((int)pos.x - GameData.Instance.currentWidth / 2.0f) * tileDim + 0.5f * tileDim,
            ((int)pos.y - GameData.Instance.currentHeight / 2.0f) * tileDim + 0.5f * tileDim
            );
        tiles[pos].GetComponent<RectTransform>().localScale = new Vector3(
            tileDim / 30, tileDim / 30
            );
    }

    public override void AdjustMap_Goal(Vector2 pos)
    {
        int cost = GameData.Instance.grid[(int)pos.x, (int)pos.y];
        tiles[pos].GetComponent<InputField>().text = cost.ToString();
        tiles[pos].GetComponent<Image>().color = GameData.Instance.goal;
    }

    public override void AdjustMap_Occupied(Vector2 pos)
    {
        tiles[pos].GetComponent<Image>().color = GameData.Instance.occupied;
        tiles[pos].GetComponent<InputField>().text = "";
        tiles[pos].GetComponent<InputField>().interactable = false;
    }

    public override void AdjustMap_Normal(Vector2 pos)
    {
        int cost = GameData.Instance.grid[(int)pos.x, (int)pos.y];
        tiles[pos].GetComponent<Image>().color = GameData.Instance.CostToColor(cost);
        tiles[pos].GetComponent<InputField>().text = cost.ToString();
    }

    //public override void ShowCostTable()
    //{
    //    for (int x = 0; x < GameData.Instance.currentWidth; x++)
    //    {
    //        for (int y = 0; y < GameData.Instance.currentHeight; y++)
    //        {
    //            Vector2 pos = new Vector2(x, y);
    //            tiles[pos].SetActive(true);
    //            //tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
    //            //tiles[pos].GetComponent<InputField>().interactable = false;
    //            ShowCostTable_Begin(pos);

    //            if (GameData.Instance.value[x, y] == GameData.Instance.MaxCost)
    //            {
    //                //tiles[pos].GetComponent<InputField>().text = "";

    //                if (GameData.Instance.walls[x, y])
    //                    ShowCostTable_Occupied(pos);
    //                //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.occupied;
    //                else
    //                    ShowCostTable_Unreachable(pos);
    //                    //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.unreachable;
    //            }                
    //            else
    //            {
    //                if (GameData.Instance.goals.Contains(pos))
    //                {
    //                    ShowCostTable_Goal(pos);
    //                    //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.goal;
    //                }
    //                else
    //                {
    //                    ShowCostTable_Reachable(pos);
    //                    //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.CostToColor(GameData.Instance.grid[x,y]);
    //                }
    //                //tiles[pos].GetComponent<InputField>().text = GameData.Instance.policy[x, y].ToString();
    //            }
    //        }
    //    }
    //    ShowShortestPath();
    //}

    //public override void ShowShortestPath()
    //{
    //    //print("Showing cost table");
    //    foreach(Vector2 v in GameData.Instance.shortestPath)
    //    {
    //        Color color;
    //        if (GameData.Instance.goals.Contains(v))
    //            color = GameData.Instance.goal;
    //        else if (GameData.Instance.start == v)
    //            color = GameData.Instance.begin;
    //        else
    //            color = GameData.Instance.onPath;
    //        tiles[v].GetComponent<InputField>().image.color = color;
    //    }
    //}

    public override void ShowCostTable_Begin(Vector2 pos)
    {
        tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
        tiles[pos].GetComponent<InputField>().interactable = false;
        //print("Interactable False");
    }

    public override void ShowCostTable_Occupied(Vector2 pos)
    {
        tiles[pos].GetComponent<InputField>().text = "";
        tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.occupied;
    }

    public override void ShowCostTable_Unreachable(Vector2 pos)
    {
        tiles[pos].GetComponent<InputField>().text = "";
        tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.unreachable;
    }

    public override void ShowCostTable_Goal(Vector2 pos)
    {
        tiles[pos].GetComponent<InputField>().text = GameData.Instance.policy[(int)pos.x, (int)pos.y].ToString();
        tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.goal;
    }

    public override void ShowCostTable_Reachable(Vector2 pos)
    {
        if (inputs.allOrStep.value == 0)
            tiles[pos].GetComponent<InputField>().text = GameData.Instance.policy[(int)pos.x, (int)pos.y].ToString();
        //tiles[pos].GetComponent<InputField>().text = GameData.Instance.value[(int)pos.x, (int)pos.y].ToString();
        else
        {
            if (inputs.mode.value == 1)
            {
                tiles[pos].GetComponent<InputField>().text = GameData.Instance.grid[(int)pos.x, (int)pos.y].ToString();
            }
            else
            {
                tiles[pos].GetComponent<InputField>().text = GameData.Instance.value[(int)pos.x, (int)pos.y].ToString();
                //tiles[pos].GetComponent<InputField>().text = GameData.Instance.policy[(int)pos.x, (int)pos.y].ToString();
            }
        }
        tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.CostToColor(GameData.Instance.grid[(int)pos.x, (int)pos.y]);
    }

    public override void ShortestPathColor(Vector2 pos, Color color)
    {
        tiles[pos].GetComponent<InputField>().image.color = color;
    }


    public override void ShowCostTable_DynamicProgrammingHighlight()
    {
        int x = (int)DynamicProgramming_HighlightedPosition.x;
        int y = (int)DynamicProgramming_HighlightedPosition.y;

        if (x == -1)
            return;

        foreach(Vector2 last in LastHighlighted)
        {
            ProcessPosition(last);
        }

        LastHighlighted = new List<Vector2>();

        tiles[DynamicProgramming_HighlightedPosition].GetComponent<InputField>().image.color = Color.yellow;
        tiles[DynamicProgramming_HighlightedPosition].GetComponent<InputField>().text = GameData.Instance.value[x, y].ToString();
        LastHighlighted.Add(DynamicProgramming_HighlightedPosition);

        if (DynamicProgramming_HighlightedLookingAt != new Vector2(-1, -1))
        {
            x = (int)DynamicProgramming_HighlightedLookingAt.x;
            y = (int)DynamicProgramming_HighlightedLookingAt.y;
            tiles[DynamicProgramming_HighlightedLookingAt].GetComponent<InputField>().image.color = Color.cyan;
            tiles[DynamicProgramming_HighlightedLookingAt].GetComponent<InputField>().text = GameData.Instance.value[x, y].ToString();
            LastHighlighted.Add(DynamicProgramming_HighlightedLookingAt);

        }

    }

    public override void ShowCostTable_MyOwnImplementationHighlight()
    {
        print("Highlight");

        foreach (Vector2 last in LastHighlighted)
        {
            ProcessPosition(last);
        }

        LastHighlighted = new List<Vector2>();

        tiles[MyOwnImplementation_CurrentOpen].GetComponent<InputField>().text = (GameData.Instance.value[(int)MyOwnImplementation_CurrentOpen.x, (int)MyOwnImplementation_CurrentOpen.y]).ToString();

        LastHighlighted.Add(MyOwnImplementation_CurrentOpen);

        foreach (Vector2 v in MyOwnImplementation_OpenList)
        {
            tiles[v].GetComponent<InputField>().image.color = Color.cyan;
           // ProcessPosition(v);
            tiles[v].GetComponent<InputField>().text = (GameData.Instance.value[(int)v.x, (int)v.y]).ToString();
            LastHighlighted.Add(v);
        }

        tiles[MyOwnImplementation_CurrentOpen].GetComponent<InputField>().image.color = Color.magenta;
        foreach (Vector2 delta in Algorithm.deltas)
        {
            Vector2 v = MyOwnImplementation_CurrentOpen + delta;

            if (v.x >= 0 && v.x < GameData.Instance.currentWidth && v.y >= 0 && v.y < GameData.Instance.currentHeight && GameData.Instance.grid[(int)v.x, (int)v.y] != Algorithm.MaxCost)
            {
                tiles[v].GetComponent<InputField>().image.color = Color.yellow;
                LastHighlighted.Add(v);
            }
        }
    }

    public override void AStarHighlight()
    {
        int x, y;
        //foreach(Vector2 pos in GameData.Instance.ClosedList)
        //{
        //    x = (int)pos.x;
        //    y = (int)pos.y;
        //    if (GameData.Instance.grid[x, y] == Algorithm.MaxCost)
        //        tiles[pos].GetComponent<InputField>().image.color = Color.black;
        //    else
        //        tiles[pos].GetComponent<InputField>().image.color = Color.yellow;
        //    tiles[pos].GetComponent<InputField>().text = GameData.Instance.AStarLengths[x, y].ToString();
        //}

        //foreach(Vector2 pos in GameData.Instance.OpenList)
        //{
        //    x = (int)pos.x;
        //    y = (int)pos.y;
        //    if(GameData.Instance.grid[x,y] == Algorithm.MaxCost)
        //        tiles[pos].GetComponent<InputField>().image.color = Color.black;
        //    else
        //        tiles[pos].GetComponent<InputField>().image.color = Color.cyan;
        //    tiles[pos].GetComponent<InputField>().text = GameData.Instance.AStarLengths[x, y].ToString();
        //}

        x = (int)GameData.Instance.LastExpanded.x;
        if (x != -1)
        {
            y = (int)GameData.Instance.LastExpanded.y;
            tiles[GameData.Instance.LastExpanded].GetComponent<InputField>().image.color = Color.magenta;
            tiles[GameData.Instance.LastExpanded].GetComponent<InputField>().text = GameData.Instance.AStarLengths[x, y].ToString();

            foreach (Vector2 delta in Algorithm.deltas)
            {
                Vector2 v = GameData.Instance.LastExpanded + delta;
                if (v.x >= 0 && v.x < GameData.Instance.currentWidth && v.y >= 0 && v.y < GameData.Instance.currentHeight && GameData.Instance.grid[(int)v.x, (int)v.y] != Algorithm.MaxCost)
                {
                    ProcessPosition(v);
                    if (GameData.Instance.OpenList.Contains(v))
                    {
                        x = (int)v.x;
                        y = (int)v.y;
                        if (GameData.Instance.grid[x, y] == Algorithm.MaxCost)
                            tiles[v].GetComponent<InputField>().image.color = Color.black;
                        else
                            tiles[v].GetComponent<InputField>().image.color = Color.cyan;
                        tiles[v].GetComponent<InputField>().text = GameData.Instance.AStarLengths[x, y].ToString();
                    }else if (GameData.Instance.ClosedList.Contains(v))
                    {
                        x = (int)v.x;
                        y = (int)v.y;
                        if (GameData.Instance.grid[x, y] == Algorithm.MaxCost)
                            tiles[v].GetComponent<InputField>().image.color = Color.black;
                        else
                            tiles[v].GetComponent<InputField>().image.color = Color.yellow;
                        tiles[v].GetComponent<InputField>().text = GameData.Instance.AStarLengths[x, y].ToString();
                    }
                }
            }
        }

        ShowShortestPath();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}