using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChangeColor : MonoBehaviour
{
    public float red;
    public float green;
    public float blue;
    public float alpha;

    public int x;
    public int y;

    public Tilemap terrain;

    // Update is called once per frame
    void Update()
    {
        Vector3Int p = new Vector3Int(x, y, 0);
        Color color = new Color(red, green, blue, alpha);
        terrain.SetTileFlags(p, TileFlags.None);
        terrain.SetColor(p, color);
    }
}
