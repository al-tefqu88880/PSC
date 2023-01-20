using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.Tilemaps;

public class RunningBackEnd : MonoBehaviour
{
    public static TilemapData tilemap;
    public int width = 136;
    public int height = 121;
    public Tilemap terrain;

    public static TilemapData GetTilemap()
    {
        return tilemap;
    }

    [ContextMenu("Initiate Tilemap")]
    public void InitiateTilemap()
    {
        tilemap = new TilemapData(height, width, terrain);
        //Debug.Log("Tilemap initiated");
    }

    void Start()
    {
        InitiateTilemap();
        DataRead dr = gameObject.AddComponent<DataRead>();
        EditorPainter ep = gameObject.AddComponent<EditorPainter>();
        ep.PaintWater();
        dr.ApplyData();
    }

    void Update()
    {
        // to be completed
    }
}
