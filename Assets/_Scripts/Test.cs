using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    public InputField left, right;

    private void Start()
    {
        left.onValueChanged.AddListener(delegate { right.text = left.text; });
    }
    
}
