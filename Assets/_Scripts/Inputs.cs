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
    public Button resetButton;
    public Button setStartButton;
    [Header("Toggles")]
    public Toggle showPolicy;
    //public Toggle inputCosts;
    //public Toggle setGoals;
    public Toggle help;
    [Header("Dropdowns")]
    public Dropdown dimension;
    public Dropdown mode;

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
        randomButton.onClick.AddListener(delegate {
            GetComponent<ToggleGroup>().SetAllTogglesOff();
            GameData.Instance.RandomGrid();
            map.AdjustMap(); });

        showPolicy.onValueChanged.AddListener(delegate {
            ToggleColorChange(showPolicy);
            if (showPolicy.isOn)
            {
                GameData.Instance.calculateValues = true;
                GameData.Instance.CalculateValue();
                map.ShowCostTable();
                GameData.Instance.calculateValues = false;
            }
            else
            {
                map.AdjustMap();
            }
        });
        dimension.onValueChanged.AddListener(delegate
        {
            GameData.Instance.calculateValues = true;
            map.gameObject.SetActive(false);
            map = maps[dimension.value];
            map.gameObject.SetActive(true);
            map.AdjustMap();
            if (showPolicy.isOn)
            {
                GameData.Instance.calculateValues = true;
                map.ShowCostTable();
                GameData.Instance.calculateValues = false;
            }
        });
        resetButton.onClick.AddListener(delegate {
            showPolicy.isOn = false;
            GameData.Instance.InitGrid<int>(ref GameData.Instance.grid, 1);
            GameData.Instance.goals = new List<Vector2>();
            map.AdjustMap();
        });
        help.onValueChanged.AddListener(delegate
        {
            ToggleColorChange(help);
            help.transform.GetChild(1).gameObject.SetActive(help.isOn);
        });
        setStartButton.onClick.AddListener(delegate
        {
            GameData.Instance.setStart = true;

        });

        mode.onValueChanged.AddListener(delegate
        {
            if(mode.value == 0)
            {
                setStartButton.gameObject.SetActive(false);
                showPolicy.gameObject.SetActive(true);
            }
            else
            {
                setStartButton.gameObject.SetActive(true);
                showPolicy.gameObject.SetActive(false);
            }
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

    //public bool SetCosts
    //{
    //    get
    //    {
    //        return inputCosts.isOn;
    //    }
    //}

    public bool ShowValues
    {
        get
        {
            return showPolicy.isOn;
        }
    }
        
    public void SetMapDimensions()
    {
        GameData.Instance.currentWidth = CurrentWidth;
        GameData.Instance.currentHeight = CurrentHeight;
    }
}
