using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap3D : MonoBehaviour
{

    [Header("Sliders")]
    public Slider widthSlider;
    public Slider heightSlider;
    [Header("Buttons")]
    public Button createButton;
    public Button setGoalsButton;
    public Button costButton;
    public Button randomButton;
    public RectTransform map;
    [Header("GameObjects")]
    public GameObject tile;


    private Dictionary<Vector2, GameObject> tiles;
    private bool setCosts = false;
    private bool showValues = false;
    private bool calculatingValues = false;

    private readonly float mapWidth = 800;
    private readonly float mapHeight = 555;

    // Use this for initialization
    void Start()
    {
        tiles = new Dictionary<Vector2, GameObject>();
        GameData3D.Instance.SetGridSize((int)widthSlider.maxValue, (int)heightSlider.maxValue);
        GameData3D.Instance.InitGrid<int>(ref GameData3D.Instance.grid, 1);

        widthSlider.onValueChanged.AddListener(delegate { AdjustMap(); });
        heightSlider.onValueChanged.AddListener(delegate { AdjustMap(); });
        setGoalsButton.onClick.AddListener(delegate { GameData3D.Instance.setGoals = !GameData3D.Instance.setGoals; ButtonColorChange(setGoalsButton, GameData3D.Instance.setGoals); });
        costButton.onClick.AddListener(delegate { setCosts = !setCosts; ButtonColorChange(costButton, setCosts); AdjustMap(); });
        randomButton.onClick.AddListener(delegate { GameData3D.Instance.RandomGrid(); Print2DArray<int>(GameData3D.Instance.grid); AdjustMap(); });
        createButton.onClick.AddListener(delegate { showValues = !showValues; if (showValues) { GameData3D.Instance.CalculateValue(); ShowCostTable(); } else AdjustMap(); ButtonColorChange(createButton, showValues); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AdjustMap()
    {
        GameData3D.Instance.currentWidth = (int)widthSlider.value;
        GameData3D.Instance.currentHeight = (int)heightSlider.value;

        float tileDim = Mathf.Min(1.0f * mapHeight / GameData3D.Instance.currentHeight, 1.0f * mapWidth / GameData3D.Instance.currentWidth, 30);

        foreach (KeyValuePair<Vector2, GameObject> tile in tiles)
        {
            tile.Value.SetActive(false);
            tile.Value.GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)tile.Key.x, (int)tile.Key.y]);
            tile.Value.transform.GetChild(0).gameObject.SetActive(false);
            tile.Value.transform.GetChild(1).gameObject.SetActive(false);
        }
        
        for (int x = 0; x < GameData3D.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData3D.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (!tiles.ContainsKey(pos))
                {
                    tiles.Add(pos, Instantiate(tile));
                    tiles[pos].transform.SetParent(map.transform);
                    tiles[pos].GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[x, y]);
                }
                tiles[pos].SetActive(true);
                tiles[pos].transform.position = new Vector3(x, 0, y);

                if (GameData3D.Instance.goals.Contains(pos))
                {
                    tiles[pos].GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (GameData3D.Instance.grid[x, y] == GameData3D.Instance.MaxCost)
                {
                    tiles[pos].GetComponent<Renderer>().material.color = Color.black;
                    tiles[pos].transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }

    void Print2DArray<T>(T[,] array)
    {
        string s = "";
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                s += (array[i, j] + " ");
            }
            s += ("\n");
        }
        print(s);
    }
    
    void ButtonColorChange(Button b, bool var)
    {
        if (var)
            b.GetComponent<Image>().color = Color.cyan;
        else
            b.GetComponent<Image>().color = Color.white;
    }

    void ShowCostTable()
    {
        for (int x = 0; x < GameData3D.Instance.currentWidth; x++)
        {
            for (int y = 0; y < GameData3D.Instance.currentHeight; y++)
            {
                Vector2 pos = new Vector2(x, y);
                tiles[pos].SetActive(true);
                if (GameData3D.Instance.value[x, y] == GameData3D.Instance.MaxCost)
                {
                    tiles[pos].GetComponent<Renderer>().material.color = Color.black;
                    tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
                    tiles[pos].transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
                    if (GameData3D.Instance.policy[x, y] == '*')
                    {
                        tiles[pos].GetComponent<Renderer>().material.color = Color.blue;
                        tiles[pos].transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, -90);
                        tiles[pos].transform.GetChild(0).transform.localScale = new Vector3(2, .4f, .4f);
                        tiles[pos].transform.GetChild(0).transform.localPosition = new Vector3(0, 2.5f, 0);
                    }
                    else
                    {
                        tiles[pos].GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[x,y]);
                        int rotation = Array.IndexOf(GameData3D.Instance.deltaNames, GameData3D.Instance.policy[x, y]);
                        tiles[pos].transform.GetChild(0).transform.rotation = Quaternion.Euler(-90, 90 * rotation, 0);
                        tiles[pos].transform.GetChild(0).transform.localScale = new Vector3(.4f, .4f, 1);
                        tiles[pos].transform.GetChild(0).transform.localPosition = new Vector3(0, 1, 0);
                    }
                }
            }
        }
    }
}
