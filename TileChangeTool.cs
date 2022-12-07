using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChangeTool : MonoBehaviour
{
    public Vector3Int coords;
    public Tile tile;
    Tilemap tilemap;
    


    [ContextMenu("Set tile")]
    void SetTile()
    {
        tilemap = GameObject.Find("TerrainMap").GetComponent<Tilemap>();
        tilemap.SetTile(coords, tile);
    }
}
