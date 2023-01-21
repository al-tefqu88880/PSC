using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using BackEnd;


public class EditorPainter : MonoBehaviour
{
    public Tilemap terrain;
    public Tilemap rabbit;

    public EditorPainter() { }

    [ContextMenu("Paint")]
    void Paint()
    {
        Tile error = (Tile)Resources.Load("error");
        Tile water = (Tile)Resources.Load("water");
        Tile desert = (Tile)Resources.Load("desert");
        Tile plain = (Tile)Resources.Load("plain");
        Tile rabbit100 = (Tile)Resources.Load("rabbit100");
        Tile rabbit090 = (Tile)Resources.Load("rabbit090");
        Tile rabbit080 = (Tile)Resources.Load("rabbit080");
        Tile rabbit070 = (Tile)Resources.Load("rabbit070");
        Tile rabbit060 = (Tile)Resources.Load("rabbit060");
        Tile rabbit050 = (Tile)Resources.Load("rabbit050");


        TilemapData tilemap = RunningBackEnd.GetTilemap();
        for (int x = 0; x < tilemap.GetWidth(); x++)
        {
            for (int y = 0; y < tilemap.GetHeight(); y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                int index = Random.Range(0, 3);
                if (index == 0)
                {
                    terrain.SetTile(p, desert);
                    rabbit.SetTile(p, rabbit050);
                }
                else
                {
                    if (index == 1)
                    {
                        terrain.SetTile(p, plain);
                        rabbit.SetTile(p, rabbit080);
                    }
                    else
                    {
                        terrain.SetTile(p, water);
                        rabbit.SetTile(p, rabbit100);
                    }
                }
                
            }
        }
        
    }
    [ContextMenu("Paint from BackEnd")]
    void PaintFromBackEnd()
    {
        Tile error = (Tile)Resources.Load("error");
        Tile water = (Tile)Resources.Load("water");
        Tile desert = (Tile)Resources.Load("desert");
        Tile plain = (Tile)Resources.Load("plain");

        TilemapData tilemap = RunningBackEnd.GetTilemap();
        for (int x = 0; x < tilemap.GetWidth(); x++)
        {
            for(int y=0; y < tilemap.GetHeight(); y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                Tile displayTile = tilemap.GetTile(p) switch
            {
                0 => water,
                1 => desert,
                2 => plain,
                _ => error,
            };
            terrain.SetTile(p, displayTile);
            }
        }
    }


    [ContextMenu("Paint with water")]
    public void PaintWater()
    {

        Tile water = (Tile)Resources.Load("water");

        TilemapData tilemap = RunningBackEnd.GetTilemap();
        for (int x = 0; x < tilemap.GetWidth(); x++)
        {
            for (int y = 0; y < tilemap.GetHeight(); y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                RunningBackEnd.tilemap.SetTile(p, 0);
            }
        }
    }


    [ContextMenu("Clear All")]
    public void ClearAll()
    {
        terrain.ClearAllTiles();
        rabbit.ClearAllTiles();
    }
}
