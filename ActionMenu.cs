using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class ActionMenu : MonoBehaviour
{
    public InputManager inputManager;
    public ChangeDataMenu cdm;
    public Brushes brushes;

    public Transform hoverTile;
    Vector3 hoverScale = new Vector3((float)1.2, (float)1.2, (float)1.2);

    public Button weatherToggle;
    public Button overlayToggle;
    public Button cheaterToggle;

    public Button randomAction;
    public Button rabbitToggle;
    public Button lynxToggle;
    public Button foxToggle;
    public Button biomassToggle;

    public Canvas weatherCanvas;
    public Canvas overlayCanvas;

    private bool isCheater = false;
    private bool isRabbit = false;
    private bool isLynx = false;
    private bool isFox = false;
    private bool isBiomass = true;
    private bool isWeatherPanel = false;
    private bool isOverlayPanel = false;
    private Color buttonColor = new Color(0.56f, 0.93f, 0.56f, 0.5f);

    public TilemapRenderer rabbitRenderer;
    public TilemapRenderer lynxRenderer;
    public TilemapRenderer foxRenderer;
    public TilemapRenderer biomassRenderer;



    void randomActionButton()
    {
        hoverScale.x = (float)3.2;
        hoverScale.y = (float)4;
        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        hoverTile.localScale = hoverScale;
        hoverTile.rotation = rotation;
        
    }

    void cheaterButton()
    {
        if (isCheater)
        {
            inputManager.SetContext("map");
            isCheater= false;
            cheaterToggle.GetComponent<Image>().color = Color.white;
            cdm.CloseMenu();
        }
        else
        {
            inputManager.SetContext("cheater");
            isCheater= true;
            cheaterToggle.GetComponent<Image>().color = buttonColor;
        }
    }
    void rabbitButton()
    {
        if (isRabbit)
        {
            isRabbit = false;
            rabbitToggle.GetComponent<Image>().color = Color.white;
            rabbitRenderer.enabled = false;
        }
        else
        {
            isRabbit = true;
            rabbitToggle.GetComponent<Image>().color = buttonColor;
            rabbitRenderer.enabled = true;
        }
    }

    void lynxButton()
    {
        if (isLynx)
        {
            isLynx = false;
            lynxToggle.GetComponent<Image>().color = Color.white;
            lynxRenderer.enabled = false;
        }
        else
        {
            isLynx = true;
            lynxToggle.GetComponent<Image>().color = buttonColor;
            lynxRenderer.enabled = true;
        }
    }

    void foxButton()
    {
        if (isFox)
        {
            isFox = false;
            foxToggle.GetComponent<Image>().color = Color.white;
            foxRenderer.enabled = false;
        }
        else
        {
            isFox = true;
            foxToggle.GetComponent<Image>().color = buttonColor;
            foxRenderer.enabled = true;
        }
    }

    void biomassButton()
    {
        if (isBiomass)
        {
            isBiomass = false;
            biomassToggle.GetComponent<Image>().color = Color.white;
            biomassRenderer.enabled = false;
        }
        else
        {
            isBiomass = true;
            biomassToggle.GetComponent<Image>().color = buttonColor;
            biomassRenderer.enabled = true;
        }
    }

    void weatherToggleButton()
    {
        if (isWeatherPanel)
        {
            isWeatherPanel= false;
            weatherCanvas.enabled = false;
        }
        else
        {
            isWeatherPanel= true;
            weatherCanvas.enabled = true;
        }
    }

    void overlayToggleButton()
    {
        if (isOverlayPanel)
        {
            isOverlayPanel= false;
            overlayCanvas.enabled = false;
        }
        else
        {
            isOverlayPanel= true;
            overlayCanvas.enabled = true;
        }
    }

    public void closeAll()
    {
        weatherCanvas.enabled = false;
        overlayCanvas.enabled = false;
        isOverlayPanel = false;
        isWeatherPanel = false;
    }

    void Start()
    {
        overlayToggle.onClick.AddListener(()=> overlayToggleButton());
        weatherToggle.onClick.AddListener(() => weatherToggleButton());
        randomAction.onClick.AddListener(() => randomActionButton());
        cheaterToggle.onClick.AddListener(() => cheaterButton());
        rabbitToggle.onClick.AddListener(() => rabbitButton());
        lynxToggle.onClick.AddListener(() => lynxButton());
        foxToggle.onClick.AddListener(() => foxButton());
        biomassToggle.onClick.AddListener(() => biomassButton());
        biomassToggle.GetComponent<Image>().color = buttonColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
