using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Click : MonoBehaviour {

    public float clickStartTime;
    public CreateMap map;
    public static Inputs inputs;

    public void CalculateAStar()
    {
        GameData.Instance.CalculateAStar();
        RedrawMap();
        //map.ShowShortestPath();
    }

    public void RemoveAStar()
    {
        GameData.Instance.start = new Vector2(-1, -1);
        GameData.Instance.shortestPath = new List<Vector2>();
        GameData.Instance.CalculateAStar();
        RedrawMap();
    }

    public void RedrawMap()
    {
        if (inputs.showPolicy.isOn)
        {
            GameData.Instance.CalculateValue();
            map.ShowCostTable();
        }
        else
        {
            map.AdjustMap();
        }
    }
}
