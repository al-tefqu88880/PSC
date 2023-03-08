using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Common;
using BackEnd;
using System;

public class TileInfoDisplay : MonoBehaviour
{
    TMP_Text text;
    public GameObject mainCamera;
    public GridLayout grid;
    public Vector3Int position;
    TilemapData tilemap;
    GridCoordinates previousCoord = new GridCoordinates(-1,-1);
    int width;
    int height;
    bool firstUpdate = true;

    private int hoverSize = 0;
    private int maxHoverSize = 7;
    private float[] hoverScaleX = { 1.2f, 3.0f, 5.0f, 7.1f, 9.1f, 11.1f, 13.2f, 15.2f };
    private float[] hoverScaleY = { 1.2f, 4.0f, 6.7f, 9.3f, 12.0f, 14.7f, 17.3f, 20.0f };
    private Quaternion rotated = Quaternion.Euler(0, 0, 90);
    private Quaternion flat = Quaternion.Euler(0, 0, 0);
    Vector3 hoverScale = new Vector3((float)1.2, (float)1.2, (float)1.2);

    public Transform hoverTile;
    Vector3 hoverPos = new Vector3();

    public int GetHoverSize()
    {
        return hoverSize;
    }

    void HoverSizeUp()
    {
        hoverSize++;
        hoverScale.x = hoverScaleX[hoverSize];
        hoverScale.y = hoverScaleY[hoverSize];
        hoverTile.localScale = hoverScale;
        hoverTile.rotation = rotated;
    }

    void HoverSizeDown()
    {
        hoverSize--;
        hoverScale.x = hoverScaleX[hoverSize];
        hoverScale.y = hoverScaleY[hoverSize];
        hoverTile.localScale = hoverScale;
        if (hoverSize == 0)
        {
            hoverTile.rotation = flat;
        }
    }

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (firstUpdate){
            width = RunningBackEnd.GetTilemap().GetWidth();
            height = RunningBackEnd.tilemap.GetHeight();
            firstUpdate = false;
        }
        var screenPoint = (Input.mousePosition);
        screenPoint.z = (-1) * mainCamera.GetComponent<Transform>().position.z; //distance of the plane from the camera
        Vector3 mouseCoordinates = Camera.main.ScreenToWorldPoint(screenPoint);
        GridCoordinates gridCoord = GridCoordinates.WorldToHexa(mouseCoordinates);  //(x,y) values of the targeted tile

        if (gridCoord != previousCoord)
        {
            previousCoord = gridCoord;
            if (gridCoord.x < 0 | gridCoord.x >= width | gridCoord.y < 0 | gridCoord.y >= height)
            {
                text.SetText("");
            }
            else
            {
                Vector3Int position = gridCoord.V3I();
                string coord = "Location : " + gridCoord.ToString();
                string tile = "Tile : " + RunningBackEnd.tilemap.GetTile(position) switch
                {
                    0 => "Water",
                    1 => "Desert",
                    _ => "Plain",
                };
                string rabbit = "Rabbits : " + (int)Math.Round(RunningBackEnd.tilemap.GetValue(position, "rabbit"), 0);
                string lynx = "Lynx : " + (int)Math.Round((Decimal)RunningBackEnd.tilemap.GetValue(position, "lynx"), 0);
                string fox = "Foxes : " + (int)Math.Round((Decimal)RunningBackEnd.tilemap.GetValue(position, "fox"), 0);
                string temperature = "Temperature : " + (float)Math.Round((Decimal)RunningBackEnd.tilemap.GetValue(position, "temperature"), 1); 
                string rain = "Precipitation : " + (float)Math.Round((Decimal)RunningBackEnd.tilemap.GetValue(position, "rain"), 0); 
                string newLine = System.Environment.NewLine;
                text.SetText(coord + newLine + tile + newLine + rabbit + newLine + lynx + newLine + fox + newLine + temperature + newLine + rain);
                hoverPos.y = (float)(gridCoord.x * 0.75 + 0.35);
                if (gridCoord.x % 2 == 1)
                {
                    hoverPos.x = (float) (gridCoord.y + 1.01);
                }
                else
                {
                    hoverPos.x = (float) (gridCoord.y + 0.51);
                }
                hoverTile.position = hoverPos;
            }
        }

        if (Input.GetKey(KeyCode.LeftControl) & Input.mouseScrollDelta.y < 0 & hoverSize < maxHoverSize){
            HoverSizeUp();
        }
        if (Input.GetKey(KeyCode.LeftControl) & Input.mouseScrollDelta.y > 0 & hoverSize > 0){
            HoverSizeDown();
        }
    }
}
