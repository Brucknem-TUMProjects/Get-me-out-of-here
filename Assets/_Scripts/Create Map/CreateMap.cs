using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour
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

    private int maxCost = 100000000;

    private int[,] grid;
    private int[,] value;
    private char[,] policy;
    private Dictionary<Vector2, GameObject> tiles;
    private bool setGoals = false;
    private bool setCosts = false;
    private bool showValues = false;
    private bool calculatingValues = false;
    private List<Vector2> goals;

    private readonly float mapWidth = 800;
    private readonly float mapHeight = 555;

    private Vector2[] delta;
    private char[] deltaNames;

    // Use this for initialization
    void Start()
    {
        tiles = new Dictionary<Vector2, GameObject>();
        grid = new int[(int)widthSlider.maxValue, (int)heightSlider.maxValue];
        policy = new char[(int)widthSlider.maxValue, (int)heightSlider.maxValue];
        InitTable(ref grid, 1);
        goals = new List<Vector2>();
        widthSlider.onValueChanged.AddListener(delegate { AdjustMap(); });
        heightSlider.onValueChanged.AddListener(delegate { AdjustMap(); });
        setGoalsButton.onClick.AddListener(delegate { setGoals = !setGoals; ButtonColorChange(setGoalsButton, setGoals); });
        costButton.onClick.AddListener(delegate { setCosts = !setCosts; ButtonColorChange(costButton, setCosts); AdjustMap(); });
        randomButton.onClick.AddListener(delegate { RandomGrid(); });
        createButton.onClick.AddListener(delegate { showValues = !showValues; if (showValues) CalculateValue();else AdjustMap(); ButtonColorChange(createButton, showValues); });
        delta = new Vector2[]
        {
            new Vector2( 0, 1 ),
            new Vector2( 0,-1 ),
            new Vector2( 1, 0 ),
            new Vector2(-1, 0 )
        };
        deltaNames = new char[] { '^', 'v', '>', '<' };
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AdjustMap()
    {
        int width = (int)widthSlider.value;
        int height = (int)heightSlider.value;

        float tileDim = Mathf.Min(1.0f * mapHeight / height, 1.0f * mapWidth / width, 30);

        foreach (KeyValuePair<Vector2, GameObject> tile in tiles)
        {
            tile.Value.SetActive(false);
            tile.Value.GetComponent<Image>().color = CostToColor(grid[(int)tile.Key.x, (int)tile.Key.y]);
            tile.Value.transform.GetChild(0).gameObject.SetActive(false);
            tile.Value.GetComponent<Button>().enabled = true;
            tile.Value.transform.GetChild(0).GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (!tiles.ContainsKey(pos))
                {
                    tiles.Add(pos, Instantiate(tile));
                    tiles[pos].transform.SetParent(map.transform);
                    tiles[pos].GetComponent<ButtonExtension>().SetPosition(x, y);
                    tiles[pos].GetComponent<Button>().onClick.AddListener(delegate { OnClick(tiles[pos].GetComponent<ButtonExtension>().GetPosition()); });
                    tiles[pos].transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(
                        delegate
                        {
                            OnCostChanged(
                     tiles[pos].GetComponent<ButtonExtension>().GetPosition(),
                     tiles[pos].transform.GetChild(0).GetComponent<InputField>().text);
                        });
                    tiles[pos].GetComponent<Image>().color = CostToColor(grid[x, y]);
                }
                tiles[pos].SetActive(true);
                if (setCosts)
                {
                    if (grid[x, y] == maxCost)
                    {
                        tiles[pos].GetComponent<Button>().enabled = false;
                    }
                    else
                    {
                        tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
                        tiles[pos].transform.GetChild(0).GetComponent<InputField>().text = grid[x, y].ToString();
                        if (goals.Contains(pos))
                            tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = Color.blue;
                        else
                            tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = CostToColor(grid[x, y]);
                    }
                }
                tiles[pos].GetComponent<RectTransform>().localPosition = new Vector3(
                    (x - width / 2.0f) * tileDim + 0.5f * tileDim,
                    (y - height / 2.0f) * tileDim + 0.5f * tileDim
                    );
                tiles[pos].GetComponent<RectTransform>().localScale = new Vector3(
                    tileDim / 30, tileDim / 30
                    );

                if (goals.Contains(pos))
                    tiles[pos].GetComponent<Image>().color = Color.blue;
                else if (grid[x, y] == maxCost)
                    tiles[pos].GetComponent<Image>().color = Color.black;
            }
        }
    }

    void OnClick(Vector2 pos)
    {
        if (setGoals)
        {
            if (!goals.Contains(pos))
            {
                if (!(grid[(int)pos.x, (int)pos.y] == maxCost))
                {
                    goals.Add(pos);
                }
            }
            else
                goals.Remove(pos);
        }
        else
        {
            if (grid[(int)pos.x, (int)pos.y] == maxCost)
                grid[(int)pos.x, (int)pos.y] = 0;
            else
                grid[(int)pos.x, (int)pos.y] = maxCost;
        }
        AdjustMap();
    }

    void OnCostChanged(Vector2 pos, string cost)
    {
        if(!calculatingValues)
            grid[(int)pos.x, (int)pos.y] = int.Parse(cost);
    }

    void RandomGrid()
    {
        goals = new List<Vector2>();
        for (int x = 0; x < widthSlider.value; x++)
        {
            for (int y = 0; y < heightSlider.value; y++)
            {
                int randomCost = UnityEngine.Random.Range(0, 255);
                if (randomCost <= 200 && UnityEngine.Random.Range(0, 100) < 10)
                    goals.Add(new Vector2(x, y));
                grid[x, y] = randomCost > 200 ? maxCost : randomCost;
            }
        }
        AdjustMap();
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

    void CalculateValue()
    {
        calculatingValues = true;
        int width = (int)widthSlider.value;
        int height = (int)heightSlider.value;
        value = new int[width, height];

        InitTable(ref value, maxCost);

        bool change = true;

        while (change)
        {
            change = false;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int i = 0; i < goals.Count; i++)
                    {
                        if (goals[i].x == x && goals[i].y == y)
                        {
                            if (value[x, y] > 0)
                            {
                                value[x, y] = 0;
                                policy[x, y] = '*';
                                change = true;
                            }
                        }

                        else if (grid[x, y] < maxCost)
                        {
                            for (int a = 0; a < delta.GetLength(0); a++)
                            {
                                int x2 = x + (int)delta[a].x;
                                int y2 = y + (int)delta[a].y;

                                if (x2 >= 0 && x2 < width && y2 >= 0 && y2 < height)
                                {
                                    int v2;
                                    if (grid[x2, y2] == maxCost)
                                        v2 = maxCost;
                                    else
                                        v2 = value[x2, y2] + grid[x2, y2];
                                    if (v2 < value[x, y])
                                    {
                                        change = true;
                                        value[x, y] = v2;
                                        policy[x, y] = deltaNames[a];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        ShowCostTable();
        calculatingValues = false;
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
                if (value[x, y] == maxCost)
                {
                    tiles[pos].transform.GetChild(0).gameObject.SetActive(false);
                    tiles[pos].GetComponent<Button>().image.color = Color.black;
                }
                else
                {
                    tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
                    if (goals.Contains(pos))
                    {
                        tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = Color.blue;
                    }
                    else
                    {
                        tiles[pos].transform.GetChild(0).GetComponent<InputField>().image.color = Color.white;
                    }
                    tiles[pos].transform.GetChild(0).GetComponent<InputField>().contentType = InputField.ContentType.Standard;
                    tiles[pos].transform.GetChild(0).GetComponent<InputField>().text = policy[x, y].ToString();
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
