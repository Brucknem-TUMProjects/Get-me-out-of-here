using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap2D : CreateMap
{
    [Header("GameObjects")]
    public GameObject tile;


    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void AdjustMap()
    {
        float tileDim = Mathf.Min(1.0f * mapHeight / GameData3D.Instance.currentHeight, 1.0f * mapWidth / GameData3D.Instance.currentWidth, 30);

        foreach (KeyValuePair<Vector2, GameObject> tile in tiles)
        {
            tile.Value.SetActive(false);
            tile.Value.GetComponent<Image>().color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)tile.Key.x, (int)tile.Key.y]);
            tile.Value.transform.GetChild(0).gameObject.SetActive(false);
            tile.Value.GetComponent<Button>().enabled = true;
            tile.Value.transform.GetChild(0).GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;
        }

        for (int x = 0; x < GameData3D.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData3D.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (!tiles.ContainsKey(pos))
                {
                    tiles.Add(pos, Instantiate(tile));
                    tiles[pos].transform.SetParent(transform);
                    tiles[pos].GetComponent<ButtonExtension>().SetPosition(x, y);
                    tiles[pos].GetComponent<Button>().onClick.AddListener(delegate { OnClick(tiles[pos].GetComponent<ButtonExtension>().GetPosition()); });
                    tiles[pos].transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(
                        delegate
                        {
                            OnCostChanged(
                     tiles[pos].GetComponent<ButtonExtension>().GetPosition(),
                     tiles[pos].transform.GetChild(0).GetComponent<InputField>().text);
                        });
                    tiles[pos].GetComponent<Image>().color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[x, y]);
                }
                tiles[pos].SetActive(true);
                if (inputs.SetCosts)
                {
                    if (GameData3D.Instance.grid[x, y] == GameData3D.Instance.MaxCost)
                    {
                        tiles[pos].GetComponent<Button>().enabled = false;
                    }
                    else
                    {
                        tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
                        tiles[pos].transform.GetChild(0).GetComponent<InputField>().text = GameData3D.Instance.grid[x, y].ToString();
                        if (GameData3D.Instance.goals.Contains(pos))
                            tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = Color.blue;
                        else
                            tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[x, y]);
                    }
                }
                tiles[pos].GetComponent<RectTransform>().localPosition = new Vector3(
                    (x - GameData3D.Instance.currentWidth / 2.0f) * tileDim + 0.5f * tileDim,
                    (y - GameData3D.Instance.currentHeight / 2.0f) * tileDim + 0.5f * tileDim
                    );
                tiles[pos].GetComponent<RectTransform>().localScale = new Vector3(
                    tileDim / 30, tileDim / 30
                    );

                if (GameData3D.Instance.goals.Contains(pos))
                    tiles[pos].GetComponent<Image>().color = Color.blue;
                else if (GameData3D.Instance.grid[x, y] == GameData3D.Instance.MaxCost)
                    tiles[pos].GetComponent<Image>().color = Color.black;
            }
        }
    }

    void OnClick(Vector2 pos)
    {
        if (GameData3D.Instance.setGoals)
        {
            if (!GameData3D.Instance.goals.Contains(pos))
            {
                if (!(GameData3D.Instance.grid[(int)pos.x, (int)pos.y] == GameData3D.Instance.MaxCost))
                {
                    GameData3D.Instance.goals.Add(pos);
                }
            }
            else
                GameData3D.Instance.goals.Remove(pos);
        }
        else
        {
            if (GameData3D.Instance.grid[(int)pos.x, (int)pos.y] == GameData3D.Instance.MaxCost)
                GameData3D.Instance.grid[(int)pos.x, (int)pos.y] = 0;
            else
                GameData3D.Instance.grid[(int)pos.x, (int)pos.y] = GameData3D.Instance.MaxCost;
        }
        AdjustMap();
    }

    void OnCostChanged(Vector2 pos, string cost)
    {
        if(!inputs.CalculatingValues)
            GameData3D.Instance.grid[(int)pos.x, (int)pos.y] = int.Parse(cost);
    }
    
    public override void ShowCostTable()
    {
        for (int x = 0; x < GameData3D.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData3D.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                tiles[pos].SetActive(true);
                if (GameData3D.Instance.value[x, y] == GameData3D.Instance.MaxCost)
                {
                    tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
                    tiles[pos].GetComponent<Button>().image.color = Color.black;
                }
                else
                {
                    tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
                    if (GameData3D.Instance.goals.Contains(pos))
                    {
                        tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = Color.blue;
                    }
                    else
                    {
                        tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = Color.white;
                    }
                    tiles[pos].transform.GetChild(0).GetComponent<InputField>().contentType = InputField.ContentType.Standard;
                    tiles[pos].transform.GetChild(0).GetComponent<InputField>().text = GameData3D.Instance.policy[x, y].ToString();
                }
            }
        }
    }
}
