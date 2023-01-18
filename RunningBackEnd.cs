using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.Tilemaps;

public class RunningBackEnd : MonoBehaviour
{
    public static TilemapData tilemap;
    public int width = 500;
    public int height = 500;
    public Tilemap terrain;

    public static TilemapData GetTilemap()
    {
        return tilemap;
    }

    [ContextMenu("Initiate Tilemap")]
    public void InitiateTilemap()
    {
        tilemap = new TilemapData(width, height, terrain);
        //Debug.Log("Tilemap initiated");
    }

    void Start()
    {
        InitiateTilemap();
        DataRead dr = new DataRead();
        dr.applyData();
    }

    void Update()
    {
        
    }
}
