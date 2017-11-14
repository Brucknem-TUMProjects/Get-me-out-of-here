using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour {

    [Header("Sliders")]
    public Slider width, height;
    [Header("Buttons")]
    public Button createButton, setGoalsButton, costButton, debugButton;
    public RectTransform map;
    [Header("GameObjects")]
    public GameObject tile;
    private int[,] grid;
    private Dictionary<Vector2, GameObject> tiles;
    bool setGoals = false;
    bool setCosts = false;
    private List<Vector2> goals;

    private readonly float mapWidth = 800;
    private readonly float mapHeight = 555;


	// Use this for initialization
	void Start () {
        tiles = new Dictionary<Vector2, GameObject>();
        grid = new int[(int)width.maxValue, (int)height.maxValue];
        //RandomGrid();
        goals = new List<Vector2>();
        width.onValueChanged.AddListener(delegate { AdjustMap(); });
        height.onValueChanged.AddListener(delegate { AdjustMap(); });
        setGoalsButton.onClick.AddListener(delegate { setGoals = !setGoals; });
        costButton.onClick.AddListener(delegate { setCosts = !setCosts; AdjustMap(); });
        debugButton.onClick.AddListener(delegate { Print2DArray<int>(grid); });
    }
	
	// Update is called once per frame
	void Update () {

	}

    void AdjustMap()
    {
        int w = (int)width.value;
        int h = (int)height.value;

        float tileDim = Mathf.Min(1.0f * mapHeight / h , 1.0f * mapWidth / w, 30);

        foreach(KeyValuePair<Vector2, GameObject> tile in tiles)
        {
            tile.Value.SetActive(false);
            tile.Value.GetComponent<Image>().color = CostToColor(grid[(int)tile.Key.x, (int)tile.Key.y]);
            tile.Value.transform.GetChild(0).gameObject.SetActive(false);
            tile.Value.GetComponent<Button>().enabled = true;
        }

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (!tiles.ContainsKey(pos))
                {
                    tiles.Add(pos, Instantiate(tile));
                    tiles[pos].transform.SetParent(map.transform);
                    tiles[pos].GetComponent<ButtonExtension>().SetPosition(x, y);
                    tiles[pos].GetComponent<Button>().onClick.AddListener(delegate { OnClick(tiles[pos].GetComponent<ButtonExtension>().GetPosition()); });
                    tiles[pos].transform.GetChild(0).GetComponent<InputField>().onValueChanged.AddListener(
                        delegate { OnCostChanged(
                            tiles[pos].GetComponent<ButtonExtension>().GetPosition(),
                            tiles[pos].transform.GetChild(0).GetComponent<InputField>().text); });
                    tiles[pos].GetComponent<Image>().color = CostToColor(grid[x,y]);
                }
                tiles[pos].SetActive(true);
                if (setCosts)
                {
                    if (grid[x, y] == int.MaxValue)
                    {
                        tiles[pos].GetComponent<Button>().enabled = false;
                    }
                    else
                    {
                        tiles[pos].transform.GetChild(0).gameObject.SetActive(true);
                        tiles[pos].transform.GetChild(0).GetComponent<InputField>().text = grid[x, y].ToString();
                    }
                }
                tiles[pos].GetComponent<RectTransform>().localPosition = new Vector3(
                    (x - w / 2.0f) * tileDim + 0.5f * tileDim,
                    (y - h / 2.0f) * tileDim + 0.5f * tileDim
                    );
                tiles[pos].GetComponent<RectTransform>().localScale = new Vector3(
                    tileDim / 30, tileDim / 30
                    );

                if(goals.Contains(pos))
                    tiles[pos].GetComponent<Image>().color = Color.blue;
                else if (grid[x, y] == int.MaxValue)
                    tiles[pos].GetComponent<Image>().color = Color.black;
            }
        }
    }

    void OnClick(Vector2 pos)
    {
        if (setGoals)
            goals.Add(pos);
        else
        {
            if (grid[(int)pos.x, (int)pos.y] == int.MaxValue)
                grid[(int)pos.x, (int)pos.y] = 0;
            else
                grid[(int)pos.x, (int)pos.y] = int.MaxValue;
        }
        AdjustMap();
    }

    void OnCostChanged(Vector2 pos, string cost)
    {
        grid[(int)pos.x, (int)pos.y] = int.Parse(cost);
    }

    void RandomGrid()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                int randomCost = Random.Range(0, 255);
                grid[x, y] = randomCost > 200 ? int.MaxValue : randomCost;
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
}
