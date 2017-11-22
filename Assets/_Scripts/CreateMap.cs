using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreateMap : MonoBehaviour {

    public abstract void AdjustMap();
    public abstract void ShowCostTable();
    public abstract void ShowShortestPath();

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
	
	// Update is called once per frame
	void Update () {
		
	}

}
