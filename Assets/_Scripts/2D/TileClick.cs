using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileClick : Click, IPointerClickHandler {

    private Vector2 position;

    // Use this for initialization
    void Start() {
        GetComponent<InputField>().onEndEdit.AddListener(delegate {
            OnValueChanged(position, GetComponent<InputField>().text);
        });
        if(inputs == null)
            inputs = transform.parent.parent.GetChild(1).GetComponent<Inputs>();
        map = transform.parent.gameObject.GetComponent<CreateMap>();
    }

  
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            RightClick(position);

        if (eventData.button == PointerEventData.InputButton.Middle)
            MiddleClick(position);

        if (eventData.button == PointerEventData.InputButton.Left && inputs.setStart)
            LeftClick(position);
    }

    public void SetPosition(Vector2 position)
    {
        this.position = position;
    }

    public Vector2 GetPosition()
    {
        return position;
    }
}
