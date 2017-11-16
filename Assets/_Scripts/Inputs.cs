using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputs : MonoBehaviour {

    [Header("Sliders")]
    public Slider widthSlider;
    public Slider heightSlider;
    [Header("Buttons")]
    public Button createButton;
    public Button setGoalsButton;
    public Button costButton;
    public Button randomButton;

    private bool threeDim = true;

    private bool setCosts = false;
    private bool showValues = false;
    private bool calculatingValues = false;

    public CreateMap map;

    // Use this for initialization
    void Start () {
        widthSlider.onValueChanged.AddListener(delegate {
            SetMapDimensions();
            map.AdjustMap(); });
        heightSlider.onValueChanged.AddListener(delegate {
            SetMapDimensions();
            map.AdjustMap(); });
        setGoalsButton.onClick.AddListener(delegate { GameData3D.Instance.setGoals = !GameData3D.Instance.setGoals; ButtonColorChange(setGoalsButton, GameData3D.Instance.setGoals); });
        costButton.onClick.AddListener(delegate { setCosts = !setCosts; ButtonColorChange(costButton, setCosts); map.AdjustMap(); });
        randomButton.onClick.AddListener(delegate { GameData3D.Instance.RandomGrid(); map.AdjustMap(); });
        createButton.onClick.AddListener(delegate { showValues = !showValues; if (showValues) {
                calculatingValues = true;
                GameData3D.Instance.CalculateValue();
                map.ShowCostTable();
                calculatingValues = false;
            } else
                map.AdjustMap(); ButtonColorChange(createButton, showValues); });
    }

    void ButtonColorChange(Button b, bool var)
    {
        if (var)
            b.GetComponent<Image>().color = Color.cyan;
        else
            b.GetComponent<Image>().color = Color.white;
    }

    public int MaxWidth
    {
        get
        {
            return (int)widthSlider.maxValue;
        }
    }

    public int MaxHeight
    {
        get
        {
            return (int)heightSlider.maxValue;
        }
    }

    public int CurrentWidth
    {
        get
        {
            return (int)widthSlider.value;
        }
    }

    public int CurrentHeight
    {
        get
        {
            return (int)heightSlider.value;
        }
    }

    public bool SetCosts
    {
        get
        {
            return setCosts;
        }
    }

    public bool ShowValues
    {
        get
        {
            return showValues;
        }
    }

    public bool CalculatingValues
    {
        get
        {
            return calculatingValues;
        }
    }
    
    public void SetMapDimensions()
    {
        GameData3D.Instance.currentWidth = CurrentWidth;
        GameData3D.Instance.currentHeight = CurrentHeight;
    }
}
