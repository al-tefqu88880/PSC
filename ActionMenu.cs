using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    public Transform hoverTile;
    Vector3 hoverScale = new Vector3((float)1.2, (float)1.2, (float)1.2);

    public Button weatherToggle;
    public Button animalToggle;
    public Button cheaterToggle;

    public Button randomAction;

    public Canvas weatherCanvas;
    public Canvas animalCanvas;



    void randomActionButton()
    {
        hoverScale.x = (float)3.2;
        hoverScale.y = (float)4;
        Quaternion rotation = Quaternion.Euler(0, 0, 90);
        hoverTile.localScale = hoverScale;
        hoverTile.rotation = rotation;
        
    }

    public void closeAll()
    {
        weatherCanvas.enabled = false;
    }

    void Start()
    {
        weatherToggle.onClick.AddListener(() => weatherCanvas.enabled = true);
        randomAction.onClick.AddListener(() => randomActionButton());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
