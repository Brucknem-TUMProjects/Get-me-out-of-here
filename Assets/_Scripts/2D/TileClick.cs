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

    void RightClick()
    {
        print("RightCLick");
        Color color;
        if (GameData3D.Instance.grid[(int)position.x, (int)position.y] == GameData3D.Instance.MaxCost)
        {
            GameData3D.Instance.grid[(int)position.x, (int)position.y] = 1;
            color = GameData3D.Instance.CostToColor(1);
            GetComponent<InputField>().interactable = true;
        }
        else
        {
            GameData3D.Instance.grid[(int)position.x, (int)position.y] = GameData3D.Instance.MaxCost;
            color = Color.black;
            GetComponent<InputField>().interactable = false;
        }
        GetComponent<Image>().color = color;
    }

    void OnValueChanged()
    {
        if (!GameData3D.Instance.calculateValues)
        {
            int value = int.Parse(GetComponent<InputField>().text);
            GameData3D.Instance.grid[(int)position.x, (int)position.y] = value;
            GetComponent<Image>().color = GameData3D.Instance.CostToColor(value);
        }
    }

    void MiddleClick()
    {
        Color color;
        if (!GameData3D.Instance.goals.Contains(position))
        {
            GameData3D.Instance.goals.Add(position);
            color = Color.blue;
        }
        else
        {
            GameData3D.Instance.goals.Remove(position);
            color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)position.x, (int)position.y]);
        }

        GetComponent<Image>().color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            RightClick();

        if (eventData.button == PointerEventData.InputButton.Middle)
            MiddleClick();
    }

    public void SetPosition(Vector2 position)
    {
        this.position = position;
        GetComponent<Image>().color = GameData3D.Instance.CostToColor(GameData3D.Instance.grid[(int)position.x, (int)position.y]);
    }

    public Vector2 GetPosition()
    {
        return position;
    }
}
