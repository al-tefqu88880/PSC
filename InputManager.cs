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
            Vector3 hoverScale = new Vector3((float)1.2, (float)1.2, (float)1.2);
            hoverTile.localScale = hoverScale;
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            hoverTile.rotation = rotation;
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
