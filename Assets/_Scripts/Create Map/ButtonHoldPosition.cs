using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHoldPosition : MonoBehaviour {

    int x;
    int y;
	
    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(x, y);
    }
}
