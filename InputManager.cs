using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class InputManager : MonoBehaviour
{
    private string context;
    public ChangeDataMenu cdm;
    public ActionMenu am;
    public GameObject mainCamera;
    public Transform hoverTile;

    void Start()
    {
        context = "map";
    }

    public void SetContext(string txt)
    {
        context = txt;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (context)
            {
                case "cheater":
                    //add a condition of "not on a menu" to get a clean interface
                    Vector3Int position = GridCoordinates.MouseAtTile(mainCamera);
                    //Debug.Log(position[0]);
                    //Debug.Log(position[1]);
                    cdm.ToggleMenu(position);
                    context = "dataMenu";
                    break;
                default:
                    break;
            }
        }
        
        

        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (context)
            {
                case "dataMenu":
                    cdm.SubmitAnswers();
                    break;
                default:
                    break;
            }
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
