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
        //RedrawMap();
        //map.ShowShortestPath();
    }

    public void CalculatePolicy()
    {
        if(inputs.showPolicy.isOn)
            GameData.Instance.CalculatePolicy();
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
        map.Redraw();
    }

    public void LeftClick(Vector2 position)
    {
        if (inputs.setStart.isOn && GameData.Instance.grid[(int)position.x, (int)position.y] != Algorithm.MaxCost)
        {
            if (GameData.Instance.start != position)
            {
                GameData.Instance.start = position;
                CalculateAStar();
            }
            else
            {
                RemoveAStar();
            }
            RedrawMap();
        }
    }

    public void RightClick(Vector2 position)
    {
        if (!GameData.Instance.goals.Contains(position) && GameData.Instance.start != position)
        {
            if (GameData.Instance.grid[(int)position.x, (int)position.y] == Algorithm.MaxCost)
            {
                GameData.Instance.grid[(int)position.x, (int)position.y] = 1;
                GameData.Instance.walls[(int)position.x, (int)position.y] = false;
            }
            else
            {
                GameData.Instance.grid[(int)position.x, (int)position.y] = Algorithm.MaxCost;
                GameData.Instance.walls[(int)position.x, (int)position.y] = true;
            }
            GameData.Instance.InitDynamicProgrammingSingleStep();
            CalculateAStar();
            CalculatePolicy();
            RedrawMap();
        }
    }

    public void MiddleClick(Vector2 position)
    {
        if (GameData.Instance.grid[(int)position.x, (int)position.y] != Algorithm.MaxCost)
        {
            if (!GameData.Instance.goals.Contains(position))
            {
                GameData.Instance.goals.Add(position);
            }
            else
            {
                GameData.Instance.goals.Remove(position);
            }
            GameData.Instance.InitDynamicProgrammingSingleStep();
            CalculateAStar();
            CalculatePolicy();
            RedrawMap();
        }
    }

    public void OnValueChanged(Vector2 position, string text)
    {
        int value = 0;
        bool success = int.TryParse(text, out value);

        if (!success)
            value = GameData.Instance.grid[(int)position.x, (int)position.y];

        //int value = int.Parse(GetComponent<InputField>().text);
        //int value = int.Parse(GetComponent<InputField>().text);
        GameData.Instance.grid[(int)position.x, (int)position.y] = value;
        CalculateAStar();
        CalculatePolicy();
        RedrawMap();
    }
}
