using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.Tilemaps;

public class RunningBackEnd : MonoBehaviour
{
    public static TilemapData tilemap;
    public int width;
    public int height;
    public Tilemap terrain;

    public static TilemapData GetTilemap()
    {
        return tilemap;
    }

    [ContextMenu("Initiate Tilemap")]
    void InitiateTilemap()
    {
        tilemap = new TilemapData(width, height, terrain);
        Debug.Log("Tilemap initiated");
    }

    void Start()
    {
        InitiateTilemap();
    }

    void Update()
    {
        
    }
}
