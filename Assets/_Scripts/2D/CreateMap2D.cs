using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap2D : CreateMap
{
    [Header("GameObjects")]
    public GameObject tile;

    public override void AdjustMap()
    {
        float tileDim = Mathf.Min(1.0f * mapHeight / GameData.Instance.currentHeight, 1.0f * mapWidth / GameData.Instance.currentWidth, 30);

        foreach (KeyValuePair<Vector2, GameObject> tile in tiles)
        {
            tile.Value.SetActive(false);
            tile.Value.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
        }

        for (int x = 0; x < GameData.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (!tiles.ContainsKey(pos))
                {
                    tiles.Add(pos, Instantiate(tile));
                    tiles[pos].transform.SetParent(transform);
                    tiles[pos].GetComponent<TileClick>().SetPosition(pos);
                }
                tiles[pos].SetActive(true);
               
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
                    tiles[pos].GetComponent<Image>().color = Color.blue;
                }
                else if (GameData.Instance.grid[x, y] == GameData.Instance.MaxCost)
                    tiles[pos].GetComponent<Image>().color = Color.black;
                else
                {
                    int cost = GameData.Instance.grid[x, y];
                    tiles[pos].GetComponent<Image>().color = GameData.Instance.CostToColor(cost);
                    tiles[pos].GetComponent<InputField>().text = cost.ToString();
                }
            }
        }
    }

    //void OnClick(Vector2 pos)
    //{
    //    if (inputs.setGoals.isOn)
    //    {
    //        if (!GameData3D.Instance.goals.Contains(pos))
    //        {
    //            if (!(GameData3D.Instance.grid[(int)pos.x, (int)pos.y] == GameData3D.Instance.MaxCost))
    //            {
    //                GameData3D.Instance.goals.Add(pos);
    //            }
    //        }
    //        else
    //            GameData3D.Instance.goals.Remove(pos);
    //    }
    //    else
    //    {
    //        if (GameData3D.Instance.grid[(int)pos.x, (int)pos.y] == GameData3D.Instance.MaxCost)
    //            GameData3D.Instance.grid[(int)pos.x, (int)pos.y] = 0;
    //        else
    //            GameData3D.Instance.grid[(int)pos.x, (int)pos.y] = GameData3D.Instance.MaxCost;
    //    }
    //    AdjustMap();
    //}

    //void OnCostChanged(Vector2 pos, string cost)
    //{
    //    if (!inputs.ShowValues)
    //        GameData3D.Instance.grid[(int)pos.x, (int)pos.y] = int.Parse(cost);
    //}
    
    public override void ShowCostTable()
    {
        for (int x = 0; x < GameData.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                tiles[pos].SetActive(true);
                if (GameData.Instance.value[x, y] == GameData.Instance.MaxCost)
                {
                    tiles[pos].GetComponent<InputField>().image.color = Color.black;
                    tiles[pos].GetComponent<InputField>().interactable = false;
                }
                else
                {
                    tiles[pos].GetComponent<InputField>().interactable = true;
                    if (GameData.Instance.goals.Contains(pos))
                    {
                        tiles[pos].GetComponent<InputField>().image.color = Color.blue;
                    }
                    else
                    {
                        tiles[pos].GetComponent<InputField>().image.color = Color.white;
                    }
                    tiles[pos].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
                    tiles[pos].GetComponent<InputField>().text = GameData.Instance.policy[x, y].ToString();
                }
            }
        }
    }
}
