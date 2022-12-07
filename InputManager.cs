using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private string context;
    public ChangeDataMenu cdm;

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
                    cdm.ToggleMenu();
                    break;
                default:
                    DoNothing();
                    break;
            }
        }
    }
}
