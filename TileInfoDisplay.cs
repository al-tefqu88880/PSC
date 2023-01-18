using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Common;
using BackEnd;

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

    public Transform hoverTile;
    Vector3 hoverPos = new Vector3();

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
                string bear = "Bears : " + RunningBackEnd.tilemap.GetBear(position);
                string lynx = "Lynx : " + RunningBackEnd.tilemap.GetLynx(position);
                string vole = "Voles : " + RunningBackEnd.tilemap.GetVole(position);
                string biomass = "Biomass : " + RunningBackEnd.tilemap.GetBiomass(position);
                string humidity = "Humidity : " + RunningBackEnd.tilemap.GetHumidity(position);
                string sunlight = "Sunlight : " + RunningBackEnd.tilemap.GetSunlight(position);
                string newLine = System.Environment.NewLine;
                text.SetText(coord + newLine + tile + newLine + bear + newLine + lynx + newLine + vole + newLine + biomass + newLine + humidity + newLine + sunlight);
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
    }
}
