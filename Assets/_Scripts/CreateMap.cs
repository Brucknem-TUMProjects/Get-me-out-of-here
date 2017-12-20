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

    public abstract void ShowCostTable_DynamicProgrammingHighlight();
    public abstract void ShowCostTable_MyOwnImplementationHighlight();
    public abstract void AStarHighlight();

    public abstract void ShortestPathColor(Vector2 pos, Color color);

    public Dictionary<Vector2, GameObject> tiles;

    [Header("Buttons and Sliders")]
    public Inputs inputs;

    [Header("GameObjects")]
    public GameObject tile;

    public readonly float mapWidth = 795;
    public readonly float mapHeight = 555;

    public static List<Vector2> LastHighlighted = new List<Vector2>();

    // Use this for initialization
    public void Init () {
        tiles = new Dictionary<Vector2, GameObject>();
        
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
                    else if (GameData.Instance.grid[x, y] == Algorithm.MaxCost)
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

        if (inputs.allOrStep.value == 0)
            ShowShortestPath();
        else if(inputs.mode.value == 1)
            AStarHighlight();
    }

    public void ShowCostTable()
    {
        LastHighlighted = new List<Vector2>();
        for (int x = 0; x < GameData.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                ProcessPosition(pos);

                //tiles[pos].SetActive(true);
                ////tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
                ////tiles[pos].GetComponent<InputField>().interactable = false;
                //ShowCostTable_Begin(pos);

                //if (GameData.Instance.value[x, y] == Algorithm.MaxCost)
                //{
                //    //tiles[pos].GetComponent<InputField>().text = "";

                //    if (GameData.Instance.walls[x, y])
                //        ShowCostTable_Occupied(pos);
                //    //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.occupied;
                //    else
                //        ShowCostTable_Unreachable(pos);
                //    //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.unreachable;
                //}
                //else
                //{
                //    if (GameData.Instance.goals.Contains(pos))
                //    {
                //        ShowCostTable_Goal(pos);
                //        //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.goal;
                //    }
                //    else
                //    {
                //        ShowCostTable_Reachable(pos);
                //        //tiles[pos].GetComponent<InputField>().image.color = GameData.Instance.CostToColor(GameData.Instance.grid[x,y]);
                //    }
                //    //tiles[pos].GetComponent<InputField>().text = GameData.Instance.policy[x, y].ToString();
                //}

                if (inputs.allOrStep.value == 1)
                {
                    if (inputs.mode.value == 0)
                        ShowCostTable_DynamicProgrammingHighlight();
                    else if (inputs.mode.value == 2)
                        ShowCostTable_MyOwnImplementationHighlight();
                }
            }
        }
        if (inputs.allOrStep.value == 0)
            ShowShortestPath();
        else if (inputs.mode.value == 1)
            AStarHighlight();
    }

    public void ProcessPosition(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        tiles[pos].SetActive(true);
        //tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
        //tiles[pos].GetComponent<InputField>().interactable = false;
        ShowCostTable_Begin(pos);

        if (GameData.Instance.value[x, y] == Algorithm.MaxCost)
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

    public void ResetForSingleSteps()
    {
        foreach(Vector2 v in tiles.Keys)
        {
            if (GameData.Instance.grid[(int)v.x, (int)v.y] == Algorithm.MaxCost)
                ShowCostTable_Occupied(v);
            else
                ShowCostTable_Unreachable(v);
        }
    }

    public Vector2 DynamicProgramming_HighlightedPosition
    {
        get
        {
            return GameData.Instance.DynamicProgrammingHighlighted;
        }
    }

    public Vector2 DynamicProgramming_HighlightedLookingAt
    {
        get
        {
            int delta = (int)GameData.Instance.DynamicProgrammingHighlighted.z;
            Vector2 hla = DynamicProgramming_HighlightedPosition + Algorithm.deltas[delta];
            if (hla.x < 0 || hla.y < 0 || hla.x >= inputs.Width || hla.y >= inputs.Height)
                return new Vector2(-1, -1);
            return hla;
        }
    }

    public Vector2 MyOwnImplementation_CurrentOpen { get { return GameData.Instance.MyOwnImplementationCurrentOpen; } }
    public List<Vector2> MyOwnImplementation_OpenList { get { return GameData.Instance.MyOwnImplementationOpenList; } }
    //public Vector2 MyOwnImplementation_ProcessingGoal { get { return GameData.Instance.MyOwnImplementationProcessingGoal; } }


    //public List<Vector2> HighlightedAll
    //{
    //    get
    //    {
    //        List<Vector2> l = GameData.Instance.HighlightedAll;

    //        for(int i = 0; i < l.Count; i++)
    //            if (l[i].x < 0 || l[i].y < 0 || l[i].x >= inputs.Width || l[i].y >= inputs.Height)
    //                l[i] = new Vector2(-1, -1);

    //        return l;
    //    }
    //}

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

    public void Redraw()
    {
        if (inputs.showPolicy.isOn)
        {
            ShowCostTable();
        }
        else
        {
            AdjustMap();
        }
    }

    public void SetInteractable(bool interactable)
    {
        foreach(GameObject o in tiles.Values)
        {
            o.GetComponent<Click>().SetInteractable(interactable);
        }
    }
}
