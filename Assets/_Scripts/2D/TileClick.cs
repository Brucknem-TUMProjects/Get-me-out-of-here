using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileClick : MonoBehaviour, IPointerClickHandler {

    private Vector2 position;
    public Input inputs;

    // Use this for initialization
    void Start() {
        GetComponent<InputField>().onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    void LeftClick()
    {
        GameData.Instance.start = position;
        GetComponent<Image>().color = Color.red;
        GameData.Instance.CalculateAStern();
    }

    void RightClick()
    {
        print("RightCLick");
        Color color;
        if (GameData.Instance.grid[(int)position.x, (int)position.y] == GameData.Instance.MaxCost)
        {
            GameData.Instance.grid[(int)position.x, (int)position.y] = 1;
            color = GameData.Instance.CostToColor(1);
            GetComponent<InputField>().interactable = true;
        }
        else
        {
            GameData.Instance.grid[(int)position.x, (int)position.y] = GameData.Instance.MaxCost;
            color = Color.black;
            GetComponent<InputField>().interactable = false;
        }
        GetComponent<Image>().color = color;
    }

    void OnValueChanged()
    {
        if (!GameData.Instance.calculateValues)
        {
            int value = int.Parse(GetComponent<InputField>().text);
            GameData.Instance.grid[(int)position.x, (int)position.y] = value;
            GetComponent<Image>().color = GameData.Instance.CostToColor(value);
        }
    }

    void MiddleClick()
    {
        Color color;
        if (!GameData.Instance.goals.Contains(position))
        {
            GameData.Instance.goals.Add(position);
            color = Color.blue;
        }
        else
        {
            GameData.Instance.goals.Remove(position);
            color = GameData.Instance.CostToColor(GameData.Instance.grid[(int)position.x, (int)position.y]);
        }

        GetComponent<Image>().color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            RightClick();

        if (eventData.button == PointerEventData.InputButton.Middle)
            MiddleClick();

        if (eventData.button == PointerEventData.InputButton.Left && GameData.Instance.setStart)
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
