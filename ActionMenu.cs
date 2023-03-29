using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;
using BackEnd;

public class ActionMenu : MonoBehaviour
{
    public InputManager inputManager;
    public ChangeDataMenu cdm;

    public Transform hoverTile;
    Vector3 hoverScale = new Vector3((float)1.2, (float)1.2, (float)1.2);

    public Button brushToggle;
    public Button overlayToggle;
    public Button cheaterToggle;

    public Button brushActionToggle;
    public Button rabbitToggle;
    public Button lynxToggle;
    public Button foxToggle;
    public Button biomassToggle;

    public Canvas brushCanvas;
    public Canvas overlayCanvas;

    private bool isCheater = false;
    private bool isBrushAction = false;
    private bool isRabbit = false;
    private bool isLynx = false;
    private bool isFox = false;
    private bool isBiomass = true;
    private bool isBrushPanel = false;
    private bool isOverlayPanel = false;
    private Color buttonColor = new Color(0.56f, 0.93f, 0.56f, 0.5f);

    
    private int rabbitLayer = 2;
    private int lynxLayer = 3;
    private int foxLayer = 4;
    
    
    public TilemapRenderer rabbitRenderer;
    public TilemapRenderer lynxRenderer;
    public TilemapRenderer foxRenderer;
    public TilemapRenderer biomassRenderer;
    /*
    public Tilemap rabbitTilemap;
    public Tilemap lynxTilemap;
    public Tilemap foxTilemap;
    
    public Tile blankFull;
    public Tile blankLeft;
    public Tile blankRight;
    public Tile blankTopLeft;
    public Tile blankTopRight;
    public Tile blankBottom;
    */
    public TMP_Text valueQuestion;
    public TMP_InputField valueAnswer;
    public TMP_Text brushOn;

    //private TilemapData tilemap;

    private float value;

    public float GetValue()
    {
        return value;
    }

    void brushActionButton()
    {
        if (isBrushAction)
        {
            inputManager.SetContext("map");
            isBrushAction = false;
            brushOn.text = "Brush Off";
            brushActionToggle.GetComponent<Image>().color = Color.white;
        }
        else
        {
            inputManager.SetContext("brush");
            isBrushAction = true;
            brushOn.text = "Brush On";
            brushActionToggle.GetComponent<Image>().color = buttonColor;
        }
    }

    public void SubmitAnswers()
    {
        if (valueAnswer.text != "")
        {
            value = int.Parse(valueAnswer.text);
            valueAnswer.text = "";
            valueQuestion.SetText("Value : " + value.ToString());
        }
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
   //     Previous definition of overlays, with overlappling
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
            rabbitRenderer.sortingOrder = 4;
            rabbitLayer = 4;
            if (foxLayer == 4)
            {
                foxLayer = 3;
                foxRenderer.sortingOrder = 3;
                lynxLayer = 2;
                lynxRenderer.sortingOrder = 2;
            }
            if (lynxLayer == 4)
            {
                lynxLayer = 3;
                lynxRenderer.sortingOrder = 3;
                foxLayer = 2;
                foxRenderer.sortingOrder = 2;
            }
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
            lynxRenderer.sortingOrder = 4;
            lynxLayer = 4;
            if (foxLayer == 4)
            {
                foxLayer = 3;
                foxRenderer.sortingOrder = 3;
                rabbitLayer = 2;
                rabbitRenderer.sortingOrder = 2;
            }
            if (rabbitLayer == 4)
            {
                rabbitLayer = 3;
                rabbitRenderer.sortingOrder = 3;
                foxLayer = 2;
                foxRenderer.sortingOrder = 2;
            }
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
            foxRenderer.sortingOrder = 4;
            foxLayer = 4;
            if (rabbitLayer == 4)
            {
                rabbitLayer = 3;
                rabbitRenderer.sortingOrder = 3;
                lynxLayer = 2;
                lynxRenderer.sortingOrder = 2;
            }
            if (lynxLayer == 4)
            {
                lynxLayer = 3;
                lynxRenderer.sortingOrder = 3;
                rabbitLayer = 2;
                rabbitRenderer.sortingOrder = 2;
            }
            foxRenderer.enabled = true;
        }
    }
    /*

    void rabbitButton()
    {
        if (isRabbit)
        {
            isRabbit = false;
            rabbitToggle.GetComponent<Image>().color = Color.white;
            updateLayers();
            rabbitRenderer.enabled = false;
        }
        else
        {
            isRabbit = true;
            rabbitToggle.GetComponent<Image>().color = buttonColor;
            updateLayers();
            rabbitRenderer.enabled = true;
        }
    }

    void foxButton()
    {
        if (isFox)
        {
            isFox = false;
            foxToggle.GetComponent<Image>().color = Color.white;
            updateLayers();
            foxRenderer.enabled = false;
        }
        else
        {
            isFox = true;
            foxToggle.GetComponent<Image>().color = buttonColor;
            updateLayers();
            foxRenderer.enabled = true;
        }
    }

    void lynxButton()
    {
        if (isLynx)
        {
            isLynx = false;
            lynxToggle.GetComponent<Image>().color = Color.white;
            updateLayers();
            lynxRenderer.enabled = false;
        }
        else
        {
            isLynx = true;
            lynxToggle.GetComponent<Image>().color = buttonColor;
            updateLayers();
            lynxRenderer.enabled = true;
        }
    }



    void updateLayers()
    {
        if (isRabbit)
        {
            if (isLynx)
            {
                if (isFox)
                {
                    for (int i=0; i<121; i++)
                    {
                        for (int j=0; j < 121; j++)
                        {
                            if (RunningBackEnd.tilemap.GetValue(new Vector3Int(j, i, 0), "useful") > 0.5)
                            {
                                rabbitTilemap.SetTile(new Vector3Int(i, j, 0), blankTopLeft);
                                rabbitTilemap.SetTileFlags(new Vector3Int(i,j,0), TileFlags.None);
                                foxTilemap.SetTile(new Vector3Int(i,j,0), blankTopRight);
                                foxTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                                lynxTilemap.SetTile(new Vector3Int(i, j, 0), blankBottom);
                                lynxTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 121; i++)
                    {
                        for (int j = 0; j < 121; j++)
                        {
                            if (RunningBackEnd.tilemap.GetValue(new Vector3Int(j, i, 0), "useful") > 0.5)
                            {
                                rabbitTilemap.SetTile(new Vector3Int(i, j, 0), blankLeft);
                                rabbitTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                                lynxTilemap.SetTile(new Vector3Int(i, j, 0), blankRight);
                                lynxTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                            }
                        }
                    }
                }
            }
            else
            {
                if (isFox)
                {
                    for (int i = 0; i < 121; i++)
                    {
                        for (int j = 0; j < 121; j++)
                        {
                            if (RunningBackEnd.tilemap.GetValue(new Vector3Int(j, i, 0), "useful") > 0.5)
                            {
                                rabbitTilemap.SetTile(new Vector3Int(i, j, 0), blankLeft);
                                rabbitTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                                foxTilemap.SetTile(new Vector3Int(i, j, 0), blankRight);
                                foxTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 121; i++)
                    {
                        for (int j = 0; j < 121; j++)
                        {
                            if (RunningBackEnd.tilemap.GetValue(new Vector3Int(j, i, 0), "useful") > 0.5)
                            {
                                rabbitTilemap.SetTile(new Vector3Int(i, j, 0), blankFull);
                                rabbitTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (isLynx)
            {
                if (isFox)
                {
                    for (int i = 0; i < 121; i++)
                    {
                        for (int j = 0; j < 121; j++)
                        {
                            if (RunningBackEnd.tilemap.GetValue(new Vector3Int(j, i, 0), "useful") > 0.5)
                            {
                                foxTilemap.SetTile(new Vector3Int(i, j, 0), blankLeft);
                                foxTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                                lynxTilemap.SetTile(new Vector3Int(i, j, 0), blankRight);
                                lynxTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 121; i++)
                    {
                        for (int j = 0; j < 121; j++)
                        {
                            if (RunningBackEnd.tilemap.GetValue(new Vector3Int(j, i, 0), "useful") > 0.5)
                            {
                                lynxTilemap.SetTile(new Vector3Int(i, j, 0), blankFull);
                                lynxTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                            }
                        }
                    }
                }
            }
            else
            {
                if (isFox)
                {
                    for (int i = 0; i < 121; i++)
                    {
                        for (int j = 0; j < 121; j++)
                        {
                            if (RunningBackEnd.tilemap.GetValue(new Vector3Int(j, i, 0), "useful") > 0.5)
                            {
                                foxTilemap.SetTile(new Vector3Int(i, j, 0), blankFull);
                                foxTilemap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                            }
                        }
                    }
                }
            }
        }
    }*/

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

    void brushToggleButton()
    {
        if (isBrushPanel)
        {
            isBrushPanel = false;
            brushCanvas.enabled = false;
        }
        else
        {
            isBrushPanel = true;
            valueQuestion.SetText("Value : " + value.ToString());
            brushCanvas.enabled = true;
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
        brushCanvas.enabled = false;
        overlayCanvas.enabled = false;
        isOverlayPanel = false;
        isBrushPanel = false;
        isBrushAction = false;
        brushActionToggle.GetComponent<Image>().color = Color.white;
    }

    void Start()
    {
        overlayToggle.onClick.AddListener(()=> overlayToggleButton());
        brushToggle.onClick.AddListener(() => brushToggleButton());
        brushActionToggle.onClick.AddListener(() => brushActionButton());
        cheaterToggle.onClick.AddListener(() => cheaterButton());
        rabbitToggle.onClick.AddListener(() => rabbitButton());
        lynxToggle.onClick.AddListener(() => lynxButton());
        foxToggle.onClick.AddListener(() => foxButton());
        biomassToggle.onClick.AddListener(() => biomassButton());
        biomassToggle.GetComponent<Image>().color = buttonColor;
        value = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
