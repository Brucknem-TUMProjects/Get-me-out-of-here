using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inputs : MonoBehaviour
{

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
    public Button quitButton;
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
    [Header("Labels")]
    public GameObject iterations;
    [Header("Camera")]
    public MoveCamera Viewer;
    [Header("Debug Texts")]
    public Text width;
    public Text height;

    private DateTime startTime;


    private ToggleGroup toggleGroup;

    public CreateMap[] maps;
    private CreateMap map;

    public int Width { get { return (int)widthSlider.value; } }
    public int Height { get { return (int)heightSlider.value; } }

    public bool threading = false;

    //private bool firstOn3D = true;

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

    private float oldH;
    //private float oldW;

    //private void Update()
    //{
    //    float h = maps[0].GetComponent<RectTransform>().rect.height;
    //    //float w = maps[0].GetComponent<RectTransform>().rect.height;
    //    if (oldH != h /*|| oldW != w*/)
    //    {
    //        print(h);
    //        height.text = h.ToString();// Screen.height.ToString();
    //        //width.text = w.ToString();

    //        maps[0].GetComponent<CreateMap>().mapHeight = (int)h - 5; /*Screen.height + plus;*/
    //        heightSlider.maxValue = (int)((h) / 30.0f) + 5;
    //        GameData.Instance.MaxHeight = (int)((h) / 30.0f) + 5;

    //        //maps[0].GetComponent<CreateMap>().mapWidth = (int)w - 5; /*Screen.height + plus;*/
    //        //widthSlider.maxValue = (int)((w) / 30.0f) + 5;
    //        //GameData.Instance.MaxWidth = (int)((w) / 30.0f) + 5;

    //        OnSliderValueChange();
    //        oldH = h;
    //    }
    //}

    // Use this for initialization
    void Start()
    {
        startTime = DateTime.Now;

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

        quitButton.onClick.AddListener(delegate
        {
            Application.Quit();
        });

        widthSlider.onValueChanged.AddListener(delegate
        {
            OnSliderValueChange();
        });

        heightSlider.onValueChanged.AddListener(delegate
        {
            OnSliderValueChange();
        });

        randomButton.onClick.AddListener(delegate
        {
            //preventInputChange = true;
            GetComponent<ToggleGroup>().SetAllTogglesOff();
            GameData.Instance.RandomGrid();
            //GameData.Instance.InitDynamicProgrammingSingleStep();
            //GameData.Instance.InitAStarSingleStep();
            //mode.value = 0;
            allOrStep.value = 0;
            showPolicy.isOn = false;
            map.AdjustMap();
            //preventInputChange = false;
        });

        showPolicy.onValueChanged.AddListener(delegate
        {
            ToggleColorChange(showPolicy);
            //preventInputChange = true;
            if (showPolicy.isOn)
            {
                if (mode.value == 0)
                    CalculateThreaded(GameData.Instance.CalculateDynamicProgramming);
                else if (mode.value == 2)
                    CalculateThreaded(GameData.Instance.CalculateMyOwnImplementation);
            }
            else
            {
                map.AdjustMap();
                ShowIterations();
            }

            //preventInputChange = false;
        });

        dimension.onValueChanged.AddListener(delegate
        {
            //preventInputChange = true;
            DimensionSwitch();

            //if (firstOn3D)
            //{
            //    firstOn3D = false;
            //    dimension.value = 0;
            //    dimension.value = 1;
            //}
        });

        resetButton.onClick.AddListener(delegate
        {
            toggleGroup.SetAllTogglesOff();
            mode.value = 0;
            allOrStep.value = 0;
            showPolicy.isOn = false;

            GameData.Instance.Initialize(Width, Height);
            //GameData.Instance.InitDynamicProgrammingSingleStep();
            //GameData.Instance.InitAStarSingleStep();
            //AllOrStepChange();
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

        GameData.Instance.currentWidth = (int)widthSlider.value;
        GameData.Instance.currentHeight = (int)heightSlider.value;
        map.AdjustMap();
    }

    int screenCounter = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            MakeScreenshot();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameData.Instance.goals = new List<Vector2>();
            map.Redraw();
        }
    }

    void MakeScreenshot()
    {
        try
        {
            print(Directory.GetDirectories(Application.dataPath + "/Screenshots/" + startTime.ToString("ddMMyyyy_HHmmss")));
        }
        catch (Exception)
        {
            Directory.CreateDirectory(Application.dataPath + "/Screenshots/" + startTime.ToString("ddMMyyyy_HHmmss"));
        }
        finally
        {
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + startTime.ToString("ddMMyyyy_HHmmss") + "/screenshot (" + screenCounter++ + ").png", 2);
        };
    }

    private void DimensionSwitch()
    {
        print("switching dimension");
        map.gameObject.SetActive(false);
        map = maps[dimension.value];
        map.gameObject.SetActive(true);
        map.AdjustMap();
        if (showPolicy.isOn || (allOrStep.value == 1 && mode.value != 1))
        {
            print("Show Cost Table");
            //preventInputChange = true;
            map.ShowCostTable();
            //preventInputChange = false;
        }
        Viewer.isEnabled = dimension.value == 1;
    }

    public void CalculateThreaded(Action algorithm)
    {
        StartCoroutine(CalculationThread(algorithm));
    }

    private IEnumerator<WaitForSeconds> CalculationThread(Action algorithm)
    {
        Thread t = new Thread(() => {
            algorithm();
        });
        showPolicy.GetComponentInChildren<Text>().text = "Calculating..";
        SetButtonsInteractive(false);
        map.SetInteractable(false);
        threading = true;
        t.Start();
        while (t.IsAlive)
            yield return null;
        //print(GameData.Instance.AlgorithmIterations);
        map.ShowCostTable();
        ShowIterations();
        SetButtonsInteractive(true);
        map.SetInteractable(true);

        threading = false;
    }

    private void SetButtonsInteractive(bool active)
    {
        widthSlider.interactable = active;
        heightSlider.interactable = active;
        showPolicy.interactable = active;
        resetButton.interactable = active;
        randomButton.interactable = active;
        mode.interactable = active;
        dimension.interactable = active;
        allOrStep.interactable = active;
    }

    private void OnStepButton()
    {
        //In Dynamic programming mode
        if (mode.value == 1)
        {
            if (GameData.Instance.AStarSingleStep())
                allOrStep.value = 0;
            map.AStarHighlight();
        }
        else
        {
            if(Input.GetKey(KeyCode.S))
                MakeScreenshot();
            if (mode.value == 0)
            {
                if (GameData.Instance.DynamicProgrammingSingleStep())
                {
                    allOrStep.value = 0;
                    showPolicy.isOn = true;
                    //StartCoroutine(ShowIterations());
                    ShowIterations();
                }
                else
                {
                    map.ShowCostTable_DynamicProgrammingHighlight();
                    //map.ShowCostTable();
                }
            }
            else
            {
                if (GameData.Instance.MyOwnImplementationSingleStep())
                {
                    allOrStep.value = 0;
                    showPolicy.isOn = true;
                    //StartCoroutine(ShowIterations());
                    ShowIterations();
                }
                else
                {
                    map.ShowCostTable_MyOwnImplementationHighlight();
                    //map.ShowCostTable();
                }
            }
        }
    }

    private void AllOrStepChange()
    {
        print("AllOrStepChange");
        GameData.Instance.ResetDynamicProgramming();

        if (allOrStep.value == 0)
        {
            stepButton.GetComponent<HoldStepButton>().holding = false;
            OnModeChange();
        }
        else
        {
            if (mode.value == 1)
            {
                //GameData.Instance.RemoveShortestPath();
                GameData.Instance.ResetAStar();
                map.Redraw();
                stepButton.gameObject.SetActive(false);
                showPolicy.gameObject.SetActive(false);
                setStart.gameObject.SetActive(true);
                setStart.isOn = true;
            }
            else
            {
                print("Here");
                stepButton.gameObject.SetActive(true);
                showPolicy.gameObject.SetActive(false);
                setStart.gameObject.SetActive(false);
                //showPolicy.isOn = true;

                setStart.isOn = false;

                if (mode.value == 0)
                    GameData.Instance.InitDynamicProgrammingSingleStep();
                else if (mode.value == 2)
                {
                    GameData.Instance.InitMyOwnImplementationSingleStep();
                    print("Inner");
                }
                map.ShowCostTable();
                //if (mode.value == 0)
                //    map.ResetForSingleSteps();
                //else if (mode.value == 2)
                //    map.ShowCostTable();
            }
        }
    }

    private void OnModeChange()
    {
        allOrStep.value = 0;
        if (showPolicy.isOn)
        {
            if (mode.value == 0)
                CalculateThreaded(GameData.Instance.CalculateDynamicProgramming);
            else
                CalculateThreaded(GameData.Instance.CalculateMyOwnImplementation);

            ShowIterations();
        }
        map.Redraw();


        if (mode.value == 1)
        {
            setStart.gameObject.SetActive(true);
            showPolicy.gameObject.SetActive(false);
            stepButton.gameObject.SetActive(false);
            setStart.isOn = true;
        }
        else 
        {
            setStart.gameObject.SetActive(false);
            showPolicy.gameObject.SetActive(true);
            stepButton.gameObject.SetActive(false);
            setStart.isOn = false;
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
    
    public void ShowIterations()
    {
        if (showPolicy.isOn)
        {
            showPolicy.GetComponentInChildren<Text>().text = "Iterations: " + GameData.Instance.AlgorithmIterations;
        }
        else
            showPolicy.GetComponentInChildren<Text>().text = "Show Policy";
    }
}
