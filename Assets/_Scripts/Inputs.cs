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
    [Header("Toggles")]
    public Toggle showPolicy;
    //public Toggle inputCosts;
    //public Toggle setGoals;
    public Toggle help;
    public Toggle setStart;
    [Header("Dropdowns")]
    public Dropdown dimension;
    public Dropdown mode;

    private ToggleGroup toggleGroup;

    public CreateMap[] maps;
    private CreateMap map;
   
    // Use this for initialization
    void Start () {
        toggleGroup = GetComponent<ToggleGroup>();
        map = maps[0];
        maps[1].gameObject.SetActive(false);

        foreach (CreateMap m in maps)
            m.Init();

        widthSlider.onValueChanged.AddListener(delegate {
            SetMapDimensions();
            map.AdjustMap();
            toggleGroup.SetAllTogglesOff();
        });

        heightSlider.onValueChanged.AddListener(delegate {
            SetMapDimensions();
            map.AdjustMap();
            toggleGroup.SetAllTogglesOff();
        });

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
            toggleGroup.SetAllTogglesOff();
            mode.value = 0;
            GameData.Instance.InitGrid<int>(ref GameData.Instance.grid, 1);
            GameData.Instance.goals = new List<Vector2>();
            GameData.Instance.shortestPath = new List<Vector2>();
            map.AdjustMap();
        });

        help.onValueChanged.AddListener(delegate
        {
            ToggleColorChange(help);
            help.transform.GetChild(1).gameObject.SetActive(help.isOn);
        });

        setStart.onValueChanged.AddListener(delegate
        {
            //GameData.Instance.setStart = setStart.isOn;
            ToggleColorChange(setStart);
        });

        mode.onValueChanged.AddListener(delegate
        {
            //toggleGroup.SetAllTogglesOff();

            if (mode.value == 0)
            {
                setStart.gameObject.SetActive(false);
                showPolicy.gameObject.SetActive(true);
            }
            else
            {
                setStart.gameObject.SetActive(true);
                showPolicy.gameObject.SetActive(false);
                setStart.isOn = true;
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
