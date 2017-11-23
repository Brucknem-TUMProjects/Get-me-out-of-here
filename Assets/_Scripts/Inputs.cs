using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public Button stepButton;
    [Header("Toggles")]
    public Toggle showPolicy;
    //public Toggle inputCosts;
    //public Toggle setGoals;
    public Toggle help;
    public Toggle setStart;
    [Header("Dropdowns")]
    public Dropdown dimension;
    public Dropdown mode;
    public Dropdown allOrStep;
    [Header("Camera")]
    public MoveCamera Viewer;

    private ToggleGroup toggleGroup;

    public CreateMap[] maps;
    private CreateMap map;

    public int Width { get { return (int)widthSlider.value; } }
    public int Height { get { return (int)heightSlider.value; } }

    private class PreventCameraMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        MoveCamera cam;
        public void SetCamera(MoveCamera c)
        {
            cam = c;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            cam.isEnabled = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cam.isEnabled = true;
        }
    }

    private class HoldStepButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        Inputs inputs;
        public bool holding = false;
        float lastStep = 0;
        float stepTime = 0.05f;

        public void SetInputs(Inputs i)
        {
            inputs = i;
        }

        private void Update()
        {
            if (holding && Time.time - lastStep > stepTime)
            {
                inputs.OnStepButton();
                lastStep = Time.time;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            holding = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            holding = false;
        }
    }

    // Use this for initialization
    void Start () {
        PreventCameraMove pcm = gameObject.AddComponent<PreventCameraMove>();
        pcm.SetCamera(Viewer);

        GameData.Instance.MaxWidth = (int)widthSlider.maxValue;
        GameData.Instance.MaxHeight = (int)heightSlider.maxValue;

        toggleGroup = GetComponent<ToggleGroup>();
        map = maps[0];
        maps[1].gameObject.SetActive(false);

        GameData.Instance.Initialize(Width, Height);
        GameData.Instance.InitDynamicProgrammingSingleStep();

        foreach (CreateMap m in maps)
            m.Init();

        widthSlider.onValueChanged.AddListener(delegate {
            OnSliderValueChange();
        });

        heightSlider.onValueChanged.AddListener(delegate {
            OnSliderValueChange();
        });
        
        randomButton.onClick.AddListener(delegate {
            //preventInputChange = true;
            GetComponent<ToggleGroup>().SetAllTogglesOff();
            GameData.Instance.RandomGrid();
            GameData.Instance.InitDynamicProgrammingSingleStep();
            GameData.Instance.InitAStarSingleStep();
            AllOrStepChange();
            map.AdjustMap();
            //preventInputChange = false;
        });

        showPolicy.onValueChanged.AddListener(delegate {
            ToggleColorChange(showPolicy);
            //preventInputChange = true;
            if (showPolicy.isOn)
            {
                GameData.Instance.CalculatePolicy();
                map.ShowCostTable();
            }
            else
            {
                map.AdjustMap();
            }
            //preventInputChange = false;
        });

        dimension.onValueChanged.AddListener(delegate
        {
            //preventInputChange = true;
            map.gameObject.SetActive(false);
            map = maps[dimension.value];
            map.gameObject.SetActive(true);
            map.AdjustMap();
            if (showPolicy.isOn)
            {
                //preventInputChange = true;
                map.ShowCostTable();
                //preventInputChange = false;
            }
            Viewer.isEnabled = dimension.value == 1;
        });

        resetButton.onClick.AddListener(delegate {
            toggleGroup.SetAllTogglesOff();
            mode.value = 0;
            GameData.Instance.Initialize(Width, Height);
            GameData.Instance.InitDynamicProgrammingSingleStep();
            GameData.Instance.InitAStarSingleStep();
            AllOrStepChange();
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

            OnModeChange();
        });

        allOrStep.onValueChanged.AddListener(delegate
        {
            AllOrStepChange();
        });

        HoldStepButton hsb = stepButton.gameObject.AddComponent<HoldStepButton>();
        hsb.SetInputs(GetComponent<Inputs>());

        //stepButton.onClick.AddListener(delegate
        //{
        //    OnStepButton();
        //});
    }

    private void OnStepButton()
    {
        //In Dynamic programming mode
        if(mode.value == 0)
        {
            if (GameData.Instance.DynamicProgrammingSingleStep())
                allOrStep.value = 0;
            map.ShowCostTable();
        }
        else
        {
            if (GameData.Instance.AStarSingleStep())
                allOrStep.value = 0;
            map.Redraw();
        }
    }

    private void AllOrStepChange()
    {
        GameData.Instance.ResetDynamicProgramming();
        if(allOrStep.value == 0)
        {
            stepButton.GetComponent<HoldStepButton>().holding = false;
            OnModeChange();
        }
        else
        {
            if(mode.value == 0)
            {
                stepButton.gameObject.SetActive(true);
                showPolicy.gameObject.SetActive(false);
                setStart.gameObject.SetActive(false);
                //showPolicy.isOn = true;
                map.Redraw();

                setStart.isOn = false;
                GameData.Instance.InitDynamicProgrammingSingleStep();
            }
            else
            {
                //GameData.Instance.RemoveShortestPath();
                GameData.Instance.ResetAStar();
                map.Redraw();
                stepButton.gameObject.SetActive(false);
                showPolicy.gameObject.SetActive(false);
                setStart.gameObject.SetActive(true);
                setStart.isOn = true;
            }
        }
    }

    private void OnModeChange()
    {
        allOrStep.value = 0;
        if (showPolicy.isOn)
        {
            GameData.Instance.CalculatePolicy();
        }
        map.Redraw();


        if (mode.value == 0)
        {
            setStart.gameObject.SetActive(false);
            showPolicy.gameObject.SetActive(true);
            stepButton.gameObject.SetActive(false);
            setStart.isOn = false;
        }
        else
        {
            //GameData.Instance.RemoveShortestPath();
            //if (showPolicy.isOn)
            //    map.ShowCostTable();
            //else
            //    map.AdjustMap();
            setStart.gameObject.SetActive(true);
            showPolicy.gameObject.SetActive(false);
            stepButton.gameObject.SetActive(false);
            setStart.isOn = true;
        }
    }
    
    private void OnSliderValueChange()
    {
        SetMapDimensions();
        GameData.Instance.Initialize(Width, Height);
        showPolicy.isOn = false;
        //setStart.isOn = false;
        //GameData.Instance.RemoveShortestPath();
        GameData.Instance.ResetAStar();
        toggleGroup.SetAllTogglesOff();
        map.AdjustMap();
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

    public bool ShowValues
    {
        get
        {
            return showPolicy.isOn;
        }
    }
        
    public void SetMapDimensions()
    {
        GameData.Instance.currentWidth = Width;
        GameData.Instance.currentHeight = Height;
    }
}
