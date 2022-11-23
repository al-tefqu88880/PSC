using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditorPainter : MonoBehaviour
{
    public Tilemap terrain;
    public  Tile desert;
    public  Tile grass;
    public  Tile water;


    [ContextMenu("Paint")]
    void Paint()
    {
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
                        terrain.SetTile(p, grass);
                    }
                    else
                    {
                        terrain.SetTile(p, water);
                    }
                }
                
            }
        }
        
    }
}
