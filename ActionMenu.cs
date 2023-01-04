using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    public Transform hoverTile;
    public Button weatherToggle;
    public Button animalToggle;
    public Button cheaterToggle;

    public Button randomAction;

    public Canvas weatherCanvas;
    public Canvas animalCanvas;


    void Start()
    {
        weatherToggle.onClick.AddListener(() => weatherCanvas.enabled = true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
