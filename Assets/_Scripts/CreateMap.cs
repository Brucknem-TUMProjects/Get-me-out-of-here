using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CreateMap : MonoBehaviour {

    public abstract void AdjustMap();
    public abstract void ShowCostTable();

    public Dictionary<Vector2, GameObject> tiles;

    [Header("Buttons and Sliders")]
    public Inputs inputs;

    public readonly float mapWidth = 800;
    public readonly float mapHeight = 555;

    // Use this for initialization
    public void Init () {
        tiles = new Dictionary<Vector2, GameObject>();
        GameData3D.Instance.SetGridSize(inputs.MaxWidth, inputs.MaxHeight);
        GameData3D.Instance.InitGrid<int>(ref GameData3D.Instance.grid, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
