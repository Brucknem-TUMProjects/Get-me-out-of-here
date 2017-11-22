using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CreateMap : MonoBehaviour {

    //public abstract void AdjustMap();
    //public abstract void ShowCostTable();
    //public abstract void ShowShortestPath();

    public abstract void AdjustMap_Goal(Vector2 pos);
    public abstract void AdjustMap_Occupied(Vector2 pos);
    public abstract void AdjustMap_Normal(Vector2 pos);
    public abstract void AdjustMap_Activate(Vector2 pos);

    public abstract void ShowCostTable_Begin(Vector2 pos);
    public abstract void ShowCostTable_Occupied(Vector2 pos);
    public abstract void ShowCostTable_Unreachable(Vector2 pos);
    public abstract void ShowCostTable_Goal(Vector2 pos);
    public abstract void ShowCostTable_Reachable(Vector2 pos);

    public abstract void ShortestPathColor(Vector2 pos, Color color);

    public Dictionary<Vector2, GameObject> tiles;

    [Header("Buttons and Sliders")]
    public Inputs inputs;

    [Header("GameObjects")]
    public GameObject tile;

    public readonly float mapWidth = 800;
    public readonly float mapHeight = 555;


    // Use this for initialization
    public void Init () {
        tiles = new Dictionary<Vector2, GameObject>();
        GameData.Instance.SetGridSize(inputs.MaxWidth, inputs.MaxHeight);
        GameData.Instance.InitGrid<int>(ref GameData.Instance.grid, 1);
        inputs.widthSlider.value = GameData.Instance.currentWidth = 6;
        inputs.heightSlider.value = GameData.Instance.currentHeight = 6;
        AdjustMap();
    }

    public void AdjustMap()
    {
        //float tileDim = Mathf.Min(1.0f * mapHeight / GameData.Instance.currentHeight, 1.0f * mapWidth / GameData.Instance.currentWidth, 30);

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
                        //tiles[pos].GetComponent<TileClick>().SetPosition(pos);
                    }
                    tiles[pos].SetActive(true);

                    //tiles[pos].GetComponent<TileClick>().SetPosition(pos);
                    //tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;
                    //tiles[pos].GetComponent<InputField>().interactable = true;

                    //tiles[pos].GetComponent<RectTransform>().localPosition = new Vector3(
                    //    (x - GameData.Instance.currentWidth / 2.0f) * tileDim + 0.5f * tileDim,
                    //    (y - GameData.Instance.currentHeight / 2.0f) * tileDim + 0.5f * tileDim
                    //    );
                    //tiles[pos].GetComponent<RectTransform>().localScale = new Vector3(
                    //    tileDim / 30, tileDim / 30
                    //    );
                    AdjustMap_Activate(pos);

                    if (GameData.Instance.goals.Contains(pos))
                    {
                        AdjustMap_Goal(pos);
                        //int cost = GameData.Instance.grid[x, y];
                        //tiles[pos].GetComponent<InputField>().text = cost.ToString();
                        //tiles[pos].GetComponent<Image>().color = GameData.Instance.goal;
                    }
                    else if (GameData.Instance.grid[x, y] == GameData.Instance.MaxCost)
                    {
                        AdjustMap_Occupied(pos);
                        //tiles[pos].GetComponent<Image>().color = GameData.Instance.occupied;
                        //tiles[pos].GetComponent<InputField>().interactable = false;
                    }
                    else
                    {
                        AdjustMap_Normal(pos);
                        //int cost = GameData.Instance.grid[x, y];
                        //tiles[pos].GetComponent<Image>().color = GameData.Instance.CostToColor(cost);
                        //tiles[pos].GetComponent<InputField>().text = cost.ToString();
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

    public void ShowCostTable()
    {
        for (int x = 0; x < GameData.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                tiles[pos].SetActive(true);
                //tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
                //tiles[pos].GetComponent<InputField>().interactable = false;
                ShowCostTable_Begin(pos);

                if (GameData.Instance.value[x, y] == GameData.Instance.MaxCost)
                {
                    //tiles[pos].GetComponent<InputField>().text = "";

                    if (GameData.Instance.walls[x, y])
                        ShowCostTable_Occupied(pos);
                    //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.occupied;
                    else
                        ShowCostTable_Unreachable(pos);
                    //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.unreachable;
                }
                else
                {
                    if (GameData.Instance.goals.Contains(pos))
                    {
                        ShowCostTable_Goal(pos);
                        //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.goal;
                    }
                    else
                    {
                        ShowCostTable_Reachable(pos);
                        //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.CostToColor(GameData.Instance.grid[x,y]);
                    }
                    //tiles[pos].GetComponent<InputField>().text = GameData.Instance.policy[x, y].ToString();
                }
            }
        }
        ShowShortestPath();
    }

    public void ShowShortestPath()
    {
        //print("Showing cost table");
        foreach (Vector2 v in GameData.Instance.shortestPath)
        {
            Color color;
            if (GameData.Instance.goals.Contains(v))
                color = GameData.Instance.goal;
            else if (GameData.Instance.start == v)
                color = GameData.Instance.begin;
            else
                color = GameData.Instance.onPath;

            ShortestPathColor(v, color);
        }
    }
}
