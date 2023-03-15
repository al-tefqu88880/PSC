using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using TMPro;

public class InputManager : MonoBehaviour
{
    private string context;
    public ChangeDataMenu cdm;
    public ActionMenu am;
    public GameObject mainCamera;
    public Transform hoverTile;
    public TileInfoDisplay tid;
    public TMP_Dropdown dropdownParameters;
    public TMP_Dropdown dropdownMode;
    
    void Start()
    {
        context = "map";
    }

    public void SetContext(string txt)
    {
        context = txt;
    }

    public string IndexToString(int index)
    {
        switch (index)
        {
            case 0:
                return "rabbit";
            case 1:
                return "lynx";
            case 2:
                return "fox";
            case 3:
                return "temperature";
            case 4:
                return "isothermality";
            case 5:
                return "summerTemperature";
            case 6:
                return "rain";
            case 7:
                return "rainVariation";
            case 8:
                return "summerRain";
            case 9:
                return "useful";
            default:
                Debug.LogWarning("Bad index");
                return "oupsi";
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int position = GridCoordinates.MouseAtTile(mainCamera);
            switch (context)
            {
                case "cheater":
                    //add a condition of "not on a menu" to get a clean interface
                    cdm.ToggleMenu(position);
                    context = "dataMenu";
                    break;
                case "brush":
                    if (dropdownMode.value == 1)
                    {
                        Brushes.ChangeBrush(position, tid.GetHoverSize(), IndexToString(dropdownParameters.value), am.GetValue());
                    }
                    else
                    {
                        Brushes.SetBrush(position, tid.GetHoverSize(), IndexToString(dropdownParameters.value), am.GetValue());
                    }
                    break;
                default:
                    break;
            }
        }
        
        

        if (Input.GetKeyDown(KeyCode.Return))
        {
            cdm.SubmitAnswers();
            am.SubmitAnswers();
        }
        if (Input.GetMouseButtonDown(1))
        {
            cdm.CloseMenu();
            am.closeAll();
            switch (context)
            {
                case "dataMenu":
                    context = "cheater";
                    break;
                case "cheater":
                    break;
                default:
                    context = "map";
                    break;
            }
        }
    }
}
