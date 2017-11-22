using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap2D : CreateMap
{
    public override void AdjustMap()
    {
        float tileDim = Mathf.Min(1.0f * mapHeight / GameData.Instance.currentHeight, 1.0f * mapWidth / GameData.Instance.currentWidth, 30);

        //foreach (KeyValuePair<Vector2, GameObject> tile in tiles)
        //{
        //    tile.Value.SetActive(false);
        //    tile.Value.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
        //}

        for (int x = 0; x < GameData.Instance.MaxWidth; x++)
        {
            for (int y = 0; y < GameData.Instance.MaxHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);

                if (x < GameData.Instance.currentWidth && y < GameData.Instance.currentHeight)
                {
                    if (!tiles.ContainsKey(pos))
                    {
                        tiles.Add(pos, Instantiate(tile));
                        tiles[pos].transform.SetParent(transform);
                        tiles[pos].GetComponent<TileClick>().SetPosition(pos);
                    }
                    tiles[pos].SetActive(true);
                    tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;
                    tiles[pos].GetComponent<InputField>().interactable = true;

                    tiles[pos].GetComponent<RectTransform>().localPosition = new Vector3(
                        (x - GameData.Instance.currentWidth / 2.0f) * tileDim + 0.5f * tileDim,
                        (y - GameData.Instance.currentHeight / 2.0f) * tileDim + 0.5f * tileDim
                        );
                    tiles[pos].GetComponent<RectTransform>().localScale = new Vector3(
                        tileDim / 30, tileDim / 30
                        );

                    if (GameData.Instance.goals.Contains(pos))
                    {
                        int cost = GameData.Instance.grid[x, y];
                        tiles[pos].GetComponent<InputField>().text = cost.ToString();
                        tiles[pos].GetComponent<Image>().color = GameData.Instance.goal;
                    }
                    else if (GameData.Instance.grid[x, y] == GameData.Instance.MaxCost)
                    {
                        tiles[pos].GetComponent<Image>().color = GameData.Instance.occupied;
                        tiles[pos].GetComponent<InputField>().interactable = false;
                    }
                    else
                    {
                        int cost = GameData.Instance.grid[x, y];
                        tiles[pos].GetComponent<Image>().color = GameData.Instance.CostToColor(cost);
                        tiles[pos].GetComponent<InputField>().text = cost.ToString();
                    }
                }
                else
                {
                    if (tiles.ContainsKey(pos))
                    {
                        tiles[pos].gameObject.SetActive(false);
                    }
                }
            }
        }

        ShowShortestPath();
    }
    
    public override void ShowCostTable()
    {
        for (int x = 0; x < GameData.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                tiles[pos].SetActive(true);
                tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
                tiles[pos].GetComponent<InputField>().interactable = false;

                if (GameData.Instance.value[x, y] == GameData.Instance.MaxCost)
                {
                    tiles[pos].GetComponent<InputField>().text = "";

                    if (GameData.Instance.walls[x, y])
                        tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.occupied;
                    else
                        tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.unreachable;
                }                
                else
                {
                    if (GameData.Instance.goals.Contains(pos))
                    {
                        tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.goal;
                    }
                    else
                    {
                        tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.CostToColor(GameData.Instance.grid[x,y]);
                    }
                    tiles[pos].GetComponent<InputField>().text = GameData.Instance.policy[x, y].ToString();
                }
            }
        }
        ShowShortestPath();
    }

    public override void ShowShortestPath()
    {
        //print("Showing cost table");
        foreach(Vector2 v in GameData.Instance.shortestPath)
        {
            Color color;
            if (GameData.Instance.goals.Contains(v))
                color = GameData.Instance.goal;
            else if (GameData.Instance.start == v)
                color = GameData.Instance.begin;
            else
                color = GameData.Instance.onPath;
            tiles[v].GetComponent<InputField>().image.color = color;
        }
    }
}
