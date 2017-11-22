using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CubeClick : Click {

    private InputField cost;

	// Use this for initialization
	void Start () {
        cost = transform.GetChild(2).transform.GetChild(0).GetComponent<InputField>();
        int v = GameData.Instance.grid[(int)transform.position.x, (int)transform.position.z];
        cost.text = v.ToString();
        cost.onEndEdit.AddListener(delegate { OnValueChanged(new Vector2(transform.position.x, transform.position.z), cost.text); });
        if (inputs == null)
            inputs = GameObject.Find("Inputs").GetComponent<Inputs>();
        map = transform.parent.GetComponent<CreateMap>();
    }
	
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0)) clickStartTime = Time.time;
        if (Input.GetMouseButtonUp(0) && Time.time - clickStartTime < 0.25f) LeftClick(new Vector2(transform.position.x, transform.position.z));
        if (Input.GetMouseButtonUp(1) && Time.time - clickStartTime < 0.25f) RightClick(new Vector2(transform.position.x, transform.position.z));
        if (Input.GetMouseButtonDown(2)) MiddleClick(new Vector2(transform.position.x, transform.position.z));
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
}
