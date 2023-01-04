using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class InputManager : MonoBehaviour
{
    private string context;
    public ChangeDataMenu cdm;
    public GameObject mainCamera;

    void Start()
    {
        context = "map";
    }

    void DoNothing() { }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (context)
            {
                case "map":
                    Vector3Int position = GridCoordinates.MouseAtTile(mainCamera);
                    cdm.ToggleMenu(position);
                    context = "dataMenu";
                    break;
                default:
                    break;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (context)
            {
                case "dataMenu":
                    cdm.CloseMenu();
                    context = "map";
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
    }
}
