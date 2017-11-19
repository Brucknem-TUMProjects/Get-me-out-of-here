using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CubeClick : MonoBehaviour {

    float clickStartTime;
    InputField cost;

	// Use this for initialization
	void Start () {
        cost = transform.GetChild(2).transform.GetChild(0).GetComponent<InputField>();
        int v = GameData.Instance.grid[(int)transform.position.x, (int)transform.position.z];
        cost.text = v.ToString();
        cost.onValueChanged.AddListener(delegate { OnInputFieldChanged(); });
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(1)) clickStartTime = Time.time;
        if (Input.GetMouseButtonUp(1) && Time.time - clickStartTime < 0.25f) RightClick();
        if (Input.GetMouseButtonDown(2)) MiddleClick();
    }

    private void RightClick()
    {
        //Vector2 position = new Vector2(transform.position.x, transform.position.z);
        int x = (int)transform.position.x;
        int y = (int)transform.position.z;
        //if (!GameData3D.Instance.goals.Contains(position))
        {
            Color color;
        
            if (GameData.Instance.grid[x, y] == GameData.Instance.MaxCost)
            {
                GameData.Instance.grid[x, y] = 1;
                color = GameData.Instance.CostToColor(1);
                SetChildrenOff();
            }
            else
            {
                GameData.Instance.grid[x, y] = GameData.Instance.MaxCost;
                color = Color.black;
                ShowBush();
            }

            GetComponent<Renderer>().material.color = color;
            GameData.Instance.Print2DArray<int>(GameData.Instance.grid);

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
        if (!GameData.Instance.goals.Contains(position))
        {
            GameData.Instance.goals.Add(position);
            color = Color.blue;
        }
        else
        {
            GameData.Instance.goals.Remove(position);
            color = Color.white;
        }

        GetComponent<Renderer>().material.color = color;
        foreach (Vector2 v in GameData.Instance.goals)
            print(v);
    }

    //private void LeftClick()
    //{
    //    Color color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z]);

    //    //GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z]);
    //    if (GameData3D.Instance.setGoals)
    //    {
    //        if (!GameData3D.Instance.goals.Contains(new Vector2(transform.position.x, transform.position.z)))
    //        {
    //            GameData3D.Instance.goals.Add(new Vector2(transform.position.x, transform.position.z));
    //            //GetComponent<Renderer>().material.color = Color.blue;
    //            color = Color.blue;
    //        }
    //        else
    //        {
    //            GameData3D.Instance.goals.Remove(new Vector2(transform.position.x, transform.position.z));
    //        }
    //    }
    //    else
    //    {
    //        if (GameData3D.Instance.goals.Contains(new Vector2(transform.position.x, transform.position.z)))
    //        {
    //            //GetComponent<Renderer>().material.color = Color.blue;
    //            color = Color.blue;
    //        }
    //        else
    //        {
    //            if (!(GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z] == GameData3D.Instance.MaxCost))
    //            {
    //                //GetComponent<Renderer>().material.color = Color.black;
    //                color = Color.black;
    //                GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z] = GameData3D.Instance.MaxCost;
    //                transform.GetChild(1).gameObject.SetActive(true);
    //            }
    //            else
    //            {
    //                GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z] = 1;
    //                //GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z]);
    //                color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z]);
    //                transform.GetChild(1).gameObject.SetActive(false);
    //            }
    //        }
    //    }
    //    GetComponent<Renderer>().material.color = color;
    //}

    void OnInputFieldChanged()
    {
        //if (!GameData3D.Instance.calculateValues)
        //{
            int value = int.Parse(cost.text);
            GameData.Instance.grid[(int)transform.position.x, (int)transform.position.z] = value;
            GetComponent<Renderer>().material.color = GameData.Instance.CostToColor(value);
        //}
    }

    public void UpdateValue()
    {
        string value = (GameData.Instance.grid[(int)transform.position.x, (int)transform.position.z]).ToString();
        cost.text = value;
    }
}
