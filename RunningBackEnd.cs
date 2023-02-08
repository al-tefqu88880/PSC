using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.Tilemaps;

public class RunningBackEnd : MonoBehaviour
{
    public static TilemapData tilemap;
    public EditorPainter ep;
    public int width = 136;
    public int height = 121;

    public Tilemap terrain;
    public Tilemap rabbit;

    public Tile rabbit100;
    public Tile rabbit090;
    public Tile rabbit080;
    public Tile rabbit070;
    public Tile rabbit060;
    public Tile rabbit050;


    public static TilemapData GetTilemap()
    {
        return tilemap;
    }

    [ContextMenu("Initiate Tilemap")]

    public void InitiateTilemap()
    {
        tilemap = new TilemapData(height, width, terrain, rabbit);
        //Debug.Log("Tilemap initiated");
    }

    public void SetRabbitColor()
    {
        for (int x = 0; x < tilemap.GetWidth(); x++)
        {
            for (int y = 0; y < tilemap.GetHeight(); y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                Vector3Int invertedP = new Vector3Int(y, x, 0);
                switch (tilemap.GetValue(p, "rabbit")){
                    case float n when n > 1000:
                        rabbit.SetTile(invertedP, rabbit100);
                        break;
                    case float n when n > 500:
                        rabbit.SetTile(invertedP, rabbit090);
                        break;
                    case float n when n > 200:
                        rabbit.SetTile(invertedP, rabbit080);
                        break;
                    case float n when n > 100:
                        rabbit.SetTile(invertedP, rabbit070);
                        break;
                    case float n when n > 20:
                        rabbit.SetTile(invertedP, rabbit060);
                        break;
                    case float n when n > 0:
                        rabbit.SetTile(invertedP, rabbit050);
                        break;
                }
            }
        }
    }

    void Start()
    {
        InitiateTilemap();
        DataRead dr = gameObject.AddComponent<DataRead>();
        ep.ClearAll();
        ep.PaintWater();
        dr.ApplyData();
        //SetRabbitColor();

    }

    void Update()
    {
        // to be completed
    }
}
