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
    public Tile blankTile;
    Color emptyColor = new Color(0, 0, 0, 0);

    private int UpdateCounter;
    private float[,,] NextValues = new float[121, 121, 3];


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

    /*public void SetRabbitColor()
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
    }*/

    private float SignCheck(float x)
    {
        if (x < 0)
            return 0;
        if (x > 10000)
            return 10000;
        return x;
    }
    void UpdateTile(Vector3Int coords)
    {
        List<Vector3Int> neibourgh = GridCoordinates.GetNeighbours(coords[0], coords[1], width - 1, height - 1);
        int k = 0;
        while (k < neibourgh.Count)
        {
            //Vector3Int tmp = neibourgh[k];
            //Debug.Log(tmp[0] + " " + tmp[1]);
            if (tilemap.GetValue(neibourgh[k], "useful") < 0.5)
            {
                neibourgh.RemoveAt(k);
            }
            else
                k++;
        }
        float rabbit = tilemap.GetValue(coords, "rabbit");
        float rabbit2 = SignCheck(rabbit + rabbit * (5000 - rabbit) / 5000 / 100);
        float lynx = tilemap.GetValue(coords, "lynx");
        float lynx2 = SignCheck(lynx + lynx * (5000 - lynx) / 5000 / 10);
        float fox = tilemap.GetValue(coords, "fox");
        float fox2 = SignCheck(fox + fox * (5000 - fox) / 5000 / 100);
        //float coeff = 1;
        for (int l = 0; l < neibourgh.Count; l++)
        {
            float rabbitExte = tilemap.GetValue(neibourgh[l], "rabbit");
            rabbit2 += rabbitExte * rabbitExte * (5000 - rabbit) / 5000 / 5000 / 100;
            float lynxExte = tilemap.GetValue(neibourgh[l], "lynx");
            lynx2 += lynxExte * lynxExte * (5000 - lynx) / 5000 / 5000 / 100;
            float foxExte = tilemap.GetValue(neibourgh[l], "fox");
            fox2 += foxExte * foxExte * (5000 - fox) / 5000 / 5000 / 100;
        }
        /*tilemap.SetValue(coords, "rabbit", SignCheck(rabbit2));
        tilemap.SetValue(coords, "lynx", SignCheck(lynx2));
        tilemap.SetValue(coords, "fox", SignCheck(fox2));*/
        NextValues[coords[0], coords[1], 0] = SignCheck(rabbit2);
        NextValues[coords[0], coords[1], 1] = SignCheck(fox2);
        NextValues[coords[0], coords[1], 2] = SignCheck(lynx2);
    }

    void UpdateMapGraphics()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3Int p = new Vector3Int(i, j, 0);
                Vector3Int invertedP = new Vector3Int(j, i, 0);
                if (tilemap.GetValue(invertedP, "useful") > 0.5)
                {
                    if (tilemap.GetValue(invertedP, "rabbit") > 1)
                    {
                        Color rabbitColor = new Color(1, 0, 0, 0.1f + tilemap.GetValue(invertedP, "rabbit") / 5000.0f);
                        rabbit.SetColor(p, rabbitColor);
                    }
                    else
                    {
                        rabbit.SetColor(p, emptyColor);
                    }
                    if (tilemap.GetValue(invertedP, "fox") > 1)
                    {
                        Color foxColor = new Color(0, 0, 1, 0.1f + tilemap.GetValue(invertedP, "fox") / 5000.0f);
                        fox.SetColor(p, foxColor);
                    }
                    else
                    {
                        fox.SetColor(p, emptyColor);
                    }
                    if (tilemap.GetValue(invertedP, "lynx") > 1)
                    {
                        Color lynxColor = new Color(1, 0, 1, 0.1f + tilemap.GetValue(invertedP, "lynx") / 5000.0f);
                        lynx.SetColor(p, lynxColor);
                    }
                    else
                    {
                        lynx.SetColor(p, emptyColor);
                    }

                }

            }
        }
    }
    void ApplyChanges()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3Int coords = new Vector3Int(i, j, 0);
                tilemap.SetValue(coords, "rabbit", NextValues[i, j, 0]);
                tilemap.SetValue(coords, "fox", NextValues[i, j, 1]);
                tilemap.SetValue(coords, "lynx", NextValues[i, j, 2]);
            }
        }
    }

    void UpdateMap()
    {
        /*for (int i=0; i<width; i++)
        {
            for (int j=0; j<height; j++)
            {
                if (tilemap.GetValue(new Vector3Int(i,j,0), "useful")>0.5)
                UpdateTile(new Vector3Int(i, j, 0));
            }
        }*/
        if (UpdateCounter == 11)
        {
            ApplyChanges();
            UpdateCounter = 0;
            Debug.Log("cycle");
        }
        else
        {
            Debug.Log(UpdateCounter);
            for (int i = UpdateCounter * (width / 11); i < (UpdateCounter + 1) * (width / 11); i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (tilemap.GetValue(new Vector3Int(i, j, 0), "useful") > 0.5)
                        UpdateTile(new Vector3Int(i, j, 0));
                }
            }
            UpdateCounter++;
        }
    }

    void Start()
    {
        InitiateTilemap();
        DataRead dr = gameObject.AddComponent<DataRead>();
        ep.ClearAll();
        ep.PaintWater();
        dr.ApplyData();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                Vector3Int invertedP = new Vector3Int(y, x, 0);
                if (tilemap.GetValue(invertedP, "useful") > 0.5)
                {
                    rabbit.SetTile(p, blankTile);
                    fox.SetTile(p, blankTile);
                    lynx.SetTile(p, blankTile);
                    biomass.SetTile(p, blankTile);
                    terrain.SetTileFlags(p, TileFlags.None);
                    fox.SetTileFlags(p, TileFlags.None);
                    rabbit.SetTileFlags(p, TileFlags.None);
                    biomass.SetTileFlags(p, TileFlags.None);
                    lynx.SetTileFlags(p, TileFlags.None);
                }
            }
        }

    }

    void FixedUpdate()
    {
        UpdateMap();
        UpdateMapGraphics();
    }
}

