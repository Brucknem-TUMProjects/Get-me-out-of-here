using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CubeClick : MonoBehaviour {

    private float clickStartTime;
    private InputField cost;
    public static Inputs inputs;

	// Use this for initialization
	void Start () {
        cost = transform.GetChild(2).transform.GetChild(0).GetComponent<InputField>();
        int v = GameData.Instance.grid[(int)transform.position.x, (int)transform.position.z];
        cost.text = v.ToString();
        cost.onValueChanged.AddListener(delegate { OnInputFieldChanged(); });
        if (inputs == null)
            inputs = GameObject.Find("Inputs").GetComponent<Inputs>();// transform.parent.parent.GetChild(3).GetChild(1).GetComponent<Inputs>();
    }
	
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) clickStartTime = Time.time;
        if (Input.GetMouseButtonUp(0) && Time.time - clickStartTime < 0.25f) LeftClick();
        if (Input.GetMouseButtonUp(1) && Time.time - clickStartTime < 0.25f) RightClick();
        if (Input.GetMouseButtonDown(2)) MiddleClick();
    }

    private void LeftClick()
    {
        if (inputs.setStart.isOn)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.z);
            if (GameData.Instance.start != position && GameData.Instance.grid[(int)position.x, (int)position.y] != GameData.Instance.MaxCost)
            {
                GameData.Instance.start = position;
                CalculateAStar(true);
            }
            else
            {
                CalculateAStar(false);
            }
        }
    }

    private void RightClick()
    {
        //print("Right Click");
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;
        Vector2 position = new Vector2(x, y);
        print(!GameData.Instance.goals.Contains(position) && GameData.Instance.start != position);
        if (!GameData.Instance.goals.Contains(position) && GameData.Instance.start != position)
        {
            //print("innen cube");

            Color color;

            //print(x + " - " + y);
            //print(GameData.Instance.grid[x, y]);
            if (GameData.Instance.grid[x, y] == GameData.Instance.MaxCost)
            {
                GameData.Instance.grid[x, y] = 1;
                GameData.Instance.walls[x, y] = false;
                color = GameData.Instance.CostToColor(1);
                SetChildrenOff();
            }
            else
            {
                GameData.Instance.grid[x, y] = GameData.Instance.MaxCost;
                GameData.Instance.walls[x, y] = true;
                color = GameData.Instance.occupied;
                ShowBush();
            }

            GetComponent<Renderer>().material.color = color;
            //GameData.Instance.Print2DArray<int>(GameData.Instance.grid);
            CalculateAStar(true);
        }
    }

    public void ShowBush()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void ShowArrow()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetChildrenOff()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    private void MiddleClick()
    {
        Color color;
        Vector2 position = new Vector2(transform.position.x, transform.position.z);
        if (GameData.Instance.grid[(int)position.x, (int)position.y] != GameData.Instance.MaxCost)
        {
            if (!GameData.Instance.goals.Contains(position))
            {
                GameData.Instance.goals.Add(position);
                color = GameData.Instance.goal;
            }
            else
            {
                GameData.Instance.goals.Remove(position);
                color = GameData.Instance.CostToColor(GameData.Instance.grid[(int)position.x, (int)position.y]);
            }

            GetComponent<Renderer>().material.color = color;
            CalculateAStar(true);
        }
    }

    void OnInputFieldChanged()
    {
        if (!inputs.PreventInputChange)
        {
            int value = int.Parse(cost.text);
            GameData.Instance.grid[(int)transform.position.x, (int)transform.position.z] = value;
            GetComponent<Renderer>().material.color = GameData.Instance.CostToColor(value);
            CalculateAStar(true);
        }
    }

    public void UpdateValue()
    {
        string value = (GameData.Instance.grid[(int)transform.position.x, (int)transform.position.z]).ToString();
        cost.text = value;
    }

    private void CalculateAStar(bool preventRemove)
    {
        if (!preventRemove)
        {
            GameData.Instance.start = new Vector2(-1, -1);
            GameData.Instance.shortestPath = new List<Vector2>();
        }
        GameData.Instance.CalculateAStar();
        //RedrawMap(preventRemove);
        inputs.showPolicy.isOn = !inputs.showPolicy.isOn;
        inputs.showPolicy.isOn = !inputs.showPolicy.isOn;
    }
}
