using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileClick : Click, IPointerClickHandler {

    private Vector2 position;

    // Use this for initialization
    void Start() {
        GetComponent<InputField>().onEndEdit.AddListener(delegate { OnValueChanged(); });
        if(inputs == null)
            inputs = transform.parent.parent.GetChild(1).GetComponent<Inputs>();
        map = transform.parent.gameObject.GetComponent<CreateMap>();
    }

    void LeftClick()
    {
        if (inputs.setStart.isOn)
        {
            if (GameData.Instance.start != position && GameData.Instance.grid[(int)position.x, (int)position.y] != GameData.Instance.MaxCost)
            {
                GameData.Instance.start = position;
                CalculateAStar();
            }
            else
            {
                RemoveAStar();
            }
        }
    }

    void RightClick()
    {
        //print(!GameData.Instance.goals.Contains(position) && GameData.Instance.start != position);
        if (!GameData.Instance.goals.Contains(position) && GameData.Instance.start != position)
        {
            //print("innen tile");
            //Color color;
            if (GameData.Instance.grid[(int)position.x, (int)position.y] == GameData.Instance.MaxCost)
            {
                GameData.Instance.grid[(int)position.x, (int)position.y] = 1;
                GameData.Instance.walls[(int)position.x, (int)position.y] = false;
                //color = GameData.Instance.CostToColor(1);
                GetComponent<InputField>().interactable = true;
            }
            else
            {
                GameData.Instance.grid[(int)position.x, (int)position.y] = GameData.Instance.MaxCost;
                GameData.Instance.walls[(int)position.x, (int)position.y] = true;
                //color = GameData.Instance.occupied;
                GetComponent<InputField>().interactable = false;
            }
            //GetComponent<Image>().color = color;
            CalculateAStar();
            //RedrawMap();
        }
    }

    void OnValueChanged()
    {
        //if (!inputs.PreventInputChange)
        //{
            //print("Value changed");
            int value = 0;
            bool success = int.TryParse(GetComponent<InputField>().text, out value);

            if (!success)
                value = GameData.Instance.grid[(int)position.x, (int)position.y];

            //int value = int.Parse(GetComponent<InputField>().text);
            //int value = int.Parse(GetComponent<InputField>().text);
            GameData.Instance.grid[(int)position.x, (int)position.y] = value;
            GetComponent<Image>().color = GameData.Instance.CostToColor(value);
            CalculateAStar();
        //}
    }

    void MiddleClick()
    {
        if (GameData.Instance.grid[(int)position.x, (int)position.y] != GameData.Instance.MaxCost)
        {
            //Color color;
            if (!GameData.Instance.goals.Contains(position))
            {
                GameData.Instance.goals.Add(position);
                //color = GameData.Instance.goal;
            }
            else
            {
                GameData.Instance.goals.Remove(position);
                //color = GameData.Instance.CostToColor(GameData.Instance.grid[(int)position.x, (int)position.y]);
            }

            //GetComponent<Image>().color = color;
            CalculateAStar();
            //RedrawMap();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            RightClick();

        if (eventData.button == PointerEventData.InputButton.Middle)
            MiddleClick();

        if (eventData.button == PointerEventData.InputButton.Left && inputs.setStart)
            LeftClick();
    }

    public void SetPosition(Vector2 position)
    {
        this.position = position;
        GetComponent<Image>().color = GameData.Instance.CostToColor(GameData.Instance.grid[(int)position.x, (int)position.y]);
    }

    public Vector2 GetPosition()
    {
        return position;
    }
}
