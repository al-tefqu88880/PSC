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



[ContextMenu("Paint")]
    void Paint()
    {
        Tile error = (Tile)Resources.Load("error");
        Tile water = (Tile)Resources.Load("water");
        Tile desert = (Tile)Resources.Load("desert");
        Tile plain = (Tile)Resources.Load("plain");

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                int index = Random.Range(0, 3);
                if (index == 0)
                {
                    terrain.SetTile(p, desert);
                }
                else
                {
                    if (index == 1)
                    {
                        terrain.SetTile(p, plain);
                    }
                    else
                    {
                        terrain.SetTile(p, water);
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
    void PaintWater()
    {

        Tile water = (Tile)Resources.Load("water");

        TilemapData tilemap = RunningBackEnd.GetTilemap();
        for (int x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                terrain.SetTile(p, water);
            }
        }
    }


    [ContextMenu("Clear All")]
    void ClearAll()
    {
        terrain.ClearAllTiles();
    }
}
