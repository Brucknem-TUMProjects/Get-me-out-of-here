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
        int v = GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z];
        cost.text = v.ToString();
        cost.onValueChanged.AddListener(delegate { OnInputFieldChanged(); });
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickStartTime = Time.time;
        }
        if(Input.GetMouseButtonUp(0) && Time.time - clickStartTime < 0.25f)
        {
            LeftClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            EventSystem.current.SetSelectedGameObject(cost.gameObject, null);
        }
    }

    private void OnMouseDown()
    {
    }

    private void LeftClick()
    {
        GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z]);
        if (GameData3D.Instance.setGoals)
        {
            if (!GameData3D.Instance.goals.Contains(new Vector2(transform.position.x, transform.position.z)))
            {
                GameData3D.Instance.goals.Add(new Vector2(transform.position.x, transform.position.z));
                GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                GameData3D.Instance.goals.Remove(new Vector2(transform.position.x, transform.position.z));
            }
        }
        else
        {
            if (GameData3D.Instance.goals.Contains(new Vector2(transform.position.x, transform.position.z)))
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                if (!(GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z] == GameData3D.Instance.MaxCost))
                {
                    GetComponent<Renderer>().material.color = Color.black;
                    GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z] = GameData3D.Instance.MaxCost;
                    transform.GetChild(1).gameObject.SetActive(true);
                }
                else
                {
                    GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z] = 1;
                    GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z]);
                    transform.GetChild(1).gameObject.SetActive(false);
                }
            }
        }
    }

    void OnInputFieldChanged()
    {
        int value = int.Parse(cost.text);
        GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z] = value;
        GetComponent<Renderer>().material.color = GameData3D.Instance.CostToColor(value);
    }

    public void UpdateValue()
    {
        string value = (GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z]).ToString();
        cost.text = value;
    }
}
