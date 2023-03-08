using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.Tilemaps;
using Common;
using System;

public class RunningBackEnd : MonoBehaviour
{
    public static TilemapData tilemap;
    public EditorPainter ep;
    public int width = 121;
    public int height = 121;

    public Tilemap terrain;
    public Tilemap rabbit;
    public Tilemap lynx;
    public Tilemap fox;
    public Tilemap biomass;

    public Tile species100;
    public Tile species090;
    public Tile species080;
    public Tile species070;
    public Tile species060;
    public Tile species050;


    public static TilemapData GetTilemap()
    {
        return tilemap;
    }

    [ContextMenu("Initiate Tilemap")]

    public void InitiateTilemap()
    {
        tilemap = new TilemapData(height, width, terrain, rabbit, lynx, fox, biomass);
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
                        rabbit.SetTile(invertedP, species100);
                        break;
                    case float n when n > 500:
                        rabbit.SetTile(invertedP, species090);
                        break;
                    case float n when n > 200:
                        rabbit.SetTile(invertedP, species080);
                        break;
                    case float n when n > 100:
                        rabbit.SetTile(invertedP, species070);
                        break;
                    case float n when n > 20:
                        rabbit.SetTile(invertedP, species060);
                        break;
                    case float n when n > 0:
                        rabbit.SetTile(invertedP, species050);
                        break;
                }
            }
        }
    }
    
    private float SignCheck(float x)
    {
        if (x < 0)
            return 0;
        if (x > 5000)
            return 10000;
        return x;
    }
    void UpdateTile(int i, int j)
    {
        List<Vector3Int> neibourgh = GridCoordinates.Neibourghs(i, j, width, height);
        for (int k = 0; k < neibourgh.Count; k++)
        {
            if (tilemap.GetValue(neibourgh[k], "useful") < 0.5)
            {
                neibourgh.RemoveAt(k);
            }
        }
        Vector3Int coords = new (i, j, 0);
        float rabbit = tilemap.GetValue(coords, "rabbit");
        float rabbit2 = SignCheck(rabbit + rabbit * (5000 - rabbit) / 5000 / 100);
        //float coeff = 1;
        for (int k = 0; k < neibourgh.Count; k++)
        {
            float rabbitExte = tilemap.GetValue(neibourgh[k], "rabbit");
            rabbit2 +=  rabbitExte*rabbitExte * (5000 - rabbit)/5000/5000 /100;
        }
        tilemap.SetValue(coords, "rabbit", SignCheck(rabbit2));
    }

    void UpdateMap()
    {
        for (int i=0; i<width; i++)
        {
            for (int j=0; j<height; j++)
            {
                if (tilemap.GetValue(new Vector3Int(i,j,0), "useful")>0.5)
                UpdateTile(i, j);
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

    void FixedUpdate()
    {
        UpdateMap();
    }
}
