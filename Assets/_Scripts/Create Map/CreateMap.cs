using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour {

    public Slider width, height;
    public Button create;
    public RectTransform map;
    public GameObject tile;
    private bool[,] grid;
    private Dictionary<Vector2, GameObject> tiles;

    private readonly float mapWidth = 800;
    private readonly float mapHeight = 555;


	// Use this for initialization
	void Start () {
        tiles = new Dictionary<Vector2, GameObject>();
        grid = new bool[50, 30];
        width.onValueChanged.AddListener(delegate { AdjustMap(); });
        height.onValueChanged.AddListener(delegate { AdjustMap(); });
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
            tile.Value.GetComponent<Image>().color = Color.green;
        }

        for(int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector2 pos = new Vector2(x, y);
                if (!tiles.ContainsKey(pos))
                {
                    tiles.Add(pos, Instantiate(tile));
                    tiles[pos].transform.parent = map.transform;
                    tiles[pos].GetComponent<ButtonHoldPosition>().SetPosition(x, y);
                    tiles[pos].GetComponent<Button>().onClick.AddListener(delegate { OnClick(tiles[pos].GetComponent<ButtonHoldPosition>().GetPosition()); });
                    tiles[pos].GetComponent<Image>().color = Color.green;
                }
                tiles[pos].SetActive(true);
                tiles[pos].GetComponent<RectTransform>().localPosition = new Vector3(
                    (x - w / 2.0f) * tileDim + 0.5f * tileDim,
                    (y - h / 2.0f) * tileDim + 0.5f * tileDim
                    );
                tiles[pos].GetComponent<RectTransform>().localScale = new Vector3(
                    tileDim / 30, tileDim / 30
                    );

                if (grid[x, y])
                    tiles[pos].GetComponent<Image>().color = Color.red;
            }
        }
    }

    void OnClick(Vector2 pos)
    {
        grid[(int)pos.x, (int)pos.y] = true;
        AdjustMap();
    }
}
