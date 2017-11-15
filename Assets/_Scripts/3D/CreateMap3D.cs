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
    private bool setGoals = false;
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
        setGoalsButton.onClick.AddListener(delegate { setGoals = !setGoals; ButtonColorChange(setGoalsButton, setGoals); });
        costButton.onClick.AddListener(delegate { setCosts = !setCosts; ButtonColorChange(costButton, setCosts); AdjustMap(); });
        randomButton.onClick.AddListener(delegate { GameData3D.Instance.RandomGrid(); AdjustMap(); });
        createButton.onClick.AddListener(delegate { showValues = !showValues; if (showValues) GameData3D.Instance.CalculateValue(); else AdjustMap(); ButtonColorChange(createButton, showValues); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AdjustMap()
    {
        GameData3D.Instance.currentWidth = (int)widthSlider.value;
        GameData3D.Instance.currentHeight = (int)heightSlider.value;

        float tileDim = Mathf.Min(1.0f * mapHeight / GameData3D.Instance.currentHeight, 1.0f * mapWidth / GameData3D.Instance.currentWidth, 30);

        foreach (KeyValuePair<Vector2, GameObject> tile in tiles)
        {
            tile.Value.SetActive(false);
            tile.Value.GetComponent<Renderer>().material.color = CostToColor(GameData3D.Instance.grid[(int)tile.Key.x, (int)tile.Key.y]);
            //tile.Value.transform.GetChild(0).gameObject.SetActive(false);
            //tile.Value.GetComponent<Button>().enabled = true;
            //tile.Value.transform.GetChild(0).GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;
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
                    //tiles[pos].GetComponent<ButtonExtension>().SetPosition(x, y);
                    //tiles[pos].GetComponent<Button>().onClick.AddListener(delegate { OnClick(tiles[pos].GetComponent<ButtonExtension>().GetPosition()); });
                    //tiles[pos].transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(
                        //delegate
                        //{
                        //    OnCostChanged(tiles[pos].GetComponent<ButtonExtension>().GetPosition(),
                        //        tiles[pos].transform.GetChild(0).GetComponent<InputField>().text);
                        //});
                    tiles[pos].GetComponent<Renderer>().material.color = CostToColor(GameData3D.Instance.grid[x, y]);
                }
                tiles[pos].SetActive(true);
                if (setCosts)
                {
                    if (GameData3D.Instance.grid[x, y] == GameData3D.Instance.MaxCost)
                    {
                        tiles[pos].GetComponent<Renderer>().material.color = Color.black;
                    }
                    else
                    {
                        //tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
                        //tiles[pos].transform.GetChild(0).GetComponent<InputField>().text = grid[x, y].ToString();
                        //if (goals.Contains(pos))
                        //    tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = Color.blue;
                        //else
                        //    tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = CostToColor(grid[x, y]);
                    }
                }
                tiles[pos].transform.position = new Vector3(x, 0, y);

                if (GameData3D.Instance.goals.Contains(new int[] { (int)pos.x, (int)pos.y }))
                    tiles[pos].GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }

    //void OnClick(Vector2 pos)
    //{
    //    if (setGoals)
    //    {
    //        if (!goals.Contains(pos))
    //        {
    //            if (!(grid[(int)pos.x, (int)pos.y] == maxCost))
    //            {
    //                goals.Add(pos);
    //            }
    //        }
    //        else
    //            goals.Remove(pos);
    //    }
    //    else
    //    {
    //        if (grid[(int)pos.x, (int)pos.y] == maxCost)
    //            grid[(int)pos.x, (int)pos.y] = 0;
    //        else
    //            grid[(int)pos.x, (int)pos.y] = maxCost;
    //    }
    //    AdjustMap();
    //}

    //void OnCostChanged(Vector2 pos, string cost)
    //{
    //    if (!calculatingValues)
    //        grid[(int)pos.x, (int)pos.y] = int.Parse(cost);
    //}

    //void RandomGrid()
    //{
    //    goals = new List<Vector2>();
    //    for (int x = 0; x < widthSlider.value; x++)
    //    {
    //        for (int y = 0; y < heightSlider.value; y++)
    //        {
    //            int randomCost = UnityEngine.Random.Range(0, 255);
    //            if (randomCost <= 200 && UnityEngine.Random.Range(0, 100) < 3)
    //                goals.Add(new Vector2(x, y));
    //            grid[x, y] = randomCost > 200 ? maxCost : randomCost;
    //        }
    //    }
    //    AdjustMap();
    //}

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

    Color CostToColor(int cost)
    {
        int r, g;
        if (cost > 255 / 2)
        {
            r = 255;
            g = 255 - (cost - 255 / 2) * 2;
        }
        else
        {
            g = 255;
            r = cost * 2;
        }
        return new Color(r / 255.0f, g / 255.0f, 0);
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
        int width = (int)widthSlider.value;
        int height = (int)heightSlider.value;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
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
                    if (GameData3D.Instance.goals.Contains(new int[] { (int)pos.x, (int)pos.y }))
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

    void InitTable(ref int[,] table, int value)
    {
        for (int x = 0; x < table.GetLength(0); x++)
        {
            for (int y = 0; y < table.GetLength(1); y++)
            {
                table[x, y] = value;
            }
        }
    }
}
