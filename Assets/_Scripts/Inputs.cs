using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inputs : MonoBehaviour {

    [Header("Sliders")]
    public Slider widthSlider;
    public Slider heightSlider;
    [Header("Buttons")]
    //public Button createButton;
    //public Button setGoalsButton;
    //public Button costButton;
    public Button randomButton;
    [Header("Toggles")]
    public Toggle showPolicy;
    public Toggle inputCosts;
    public Toggle setGoals;
    [Header("Dropdowns")]
    public Dropdown dimension;

    //private bool setCosts = false;
    //private bool showValues = false;
    //private bool calculatingValues = false;

    public CreateMap[] maps;
    private CreateMap map;

    // Use this for initialization
    void Start () {
        map = maps[0];
        maps[1].gameObject.SetActive(false);
        foreach (CreateMap m in maps)
            m.Init();
        widthSlider.onValueChanged.AddListener(delegate {
            SetMapDimensions();
            map.AdjustMap(); });
        heightSlider.onValueChanged.AddListener(delegate {
            SetMapDimensions();
            map.AdjustMap(); });
        //setGoalsButton.onClick.AddListener(delegate { GameData3D.Instance.setGoals = !GameData3D.Instance.setGoals; ButtonColorChange(setGoalsButton, GameData3D.Instance.setGoals); });
        //costButton.onClick.AddListener(delegate { setCosts = !setCosts; ButtonColorChange(costButton, setCosts); map.AdjustMap(); });
        randomButton.onClick.AddListener(delegate {
            GetComponent<ToggleGroup>().SetAllTogglesOff();
            GameData3D.Instance.RandomGrid();
            map.AdjustMap(); });
        //createButton.onClick.AddListener(delegate
        //{
        //    showValues = !showValues; if (showValues)
        //    {
        //        calculatingValues = true;
        //        GameData3D.Instance.CalculateValue();
        //        map.ShowCostTable();
        //        calculatingValues = false;
        //    } else
        //        map.AdjustMap(); ButtonColorChange(createButton, showValues); });

        showPolicy.onValueChanged.AddListener(delegate {
            ToggleColorChange(showPolicy);
            if (showPolicy.isOn)
            {
                GameData3D.Instance.calculateValues = true;
                GameData3D.Instance.CalculateValue();
                map.ShowCostTable();
                GameData3D.Instance.calculateValues = false;
            }
            else
            {
                map.AdjustMap();
            }
        });
        setGoals.onValueChanged.AddListener(delegate {
            ToggleColorChange(setGoals);
            //GameData3D.Instance.setGoals = setGoals;
        });
        inputCosts.onValueChanged.AddListener(delegate { ToggleColorChange(inputCosts); });
        dimension.onValueChanged.AddListener(delegate {
            map.gameObject.SetActive(false);
            map = maps[dimension.value];
            map.gameObject.SetActive(true);
            map.AdjustMap();
            if (showPolicy.isOn)
                map.ShowCostTable();
        });
    }

    void ToggleColorChange(Toggle t)
    {
        t.GetComponentInChildren<Image>().color = t.isOn ? Color.cyan : Color.white;
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
            return inputCosts.isOn;
        }
    }

    public bool ShowValues
    {
        get
        {
            return showPolicy.isOn;
        }
    }
        
    public void SetMapDimensions()
    {
        GameData3D.Instance.currentWidth = CurrentWidth;
        GameData3D.Instance.currentHeight = CurrentHeight;
    }
}
