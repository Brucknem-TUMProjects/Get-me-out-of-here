using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeClick : MonoBehaviour {

    bool highlighted = false;
    float clickStartTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnMouseDown()
    {
        clickStartTime = Time.time;
    }

    private void OnMouseUp()
    {
        if (Time.time - clickStartTime < 0.25f)
        {
            highlighted = !highlighted;
            if (highlighted)
                GetComponent<Renderer>().material.color = Color.red;
            else
                GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
