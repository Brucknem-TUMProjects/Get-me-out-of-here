using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeClick : MonoBehaviour {

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
            GetComponent<Renderer>().material.color = Color.green;
            if (GameData3D.Instance.setGoals)
            {
                if (!GameData3D.Instance.goals.Contains( new Vector2(transform.position.x, transform.position.z) ))
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
                    }
                    else
                    {
                        GameData3D.Instance.grid[(int)transform.position.x, (int)transform.position.z] = 1;
                    }
                }
            }
        }
    }
}
