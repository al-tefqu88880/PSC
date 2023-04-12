using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.Tilemaps;
using Common;
using System;
using Unity.VisualScripting;

public class RunningBackEnd : MonoBehaviour
{
    public static TilemapData tilemap;
    public EditorPainter ep;
    public static int width = 121;
    public static int height = 121;

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

    private static float tickToYear = 0.00462962962f;
    private static float cRabbit = 3f;
    private static float k = 5000f;
    private static float pFox = 4e-5f;
    private static float pLynx = 8e-4f;
    private static float dRabbit = .3f;
    private static float dFox = .11f;
    private static float cFox = 4e-5f;
    private static float dLynx = .55f;
    private static float cLynx = 2e-4f;




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
        int l = 0;
        while (l < neibourgh.Count)
        {
            if (tilemap.GetValue(neibourgh[l], "useful") < 0.5)
            {
                neibourgh.RemoveAt(l);
            }
            else
                l++;
        }
        float rabbit = tilemap.GetValue(coords, "rabbit");
        float fox = tilemap.GetValue(coords, "fox");
        float lynx = tilemap.GetValue(coords, "lynx");
        /*float rabbit2 = SignCheck(rabbit + rabbit * (5000 - rabbit) / 5000 / 100);
        float lynx2 = SignCheck(lynx + lynx * (5000 - lynx) / 5000 / 10);
        float fox2 = SignCheck(fox + fox * (5000 - fox) / 5000 / 100);*/

        float rabbit2 = rabbit + tickToYear*(cRabbit*rabbit*(1-rabbit/k) - pFox*rabbit*fox-pLynx*lynx*rabbit-dRabbit*rabbit);
        float fox2 = fox - tickToYear*(dFox * cFox / cLynx * fox + cFox * rabbit * fox);
        float lynx2 = lynx - tickToYear*(dLynx * lynx + cLynx * lynx * rabbit);

        /*for (int l = 0; l < neibourgh.Count; l++)
        {
            float rabbitExte = tilemap.GetValue(neibourgh[l], "rabbit");
            rabbit2 += rabbitExte * rabbitExte * (5000 - rabbit) / 5000 / 5000 / 100;
            float lynxExte = tilemap.GetValue(neibourgh[l], "lynx");
            lynx2 += lynxExte * lynxExte * (5000 - lynx) / 5000 / 5000 / 100;
            float foxExte = tilemap.GetValue(neibourgh[l], "fox");
            fox2 += foxExte * foxExte * (5000 - fox) / 5000 / 5000 / 100;
        }*/

        NextValues[coords[0], coords[1], 0] = SignCheck(rabbit2);
        NextValues[coords[0], coords[1], 1] = SignCheck(fox2);
        NextValues[coords[0], coords[1], 2] = SignCheck(lynx2);
    }




    public void UpdateMapGraphics(int MinI, int MaxI)
    {
        for (int i = MinI; i < MaxI; i++)
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




    void UpdateMapData(int MinI, int MaxI)
    {
        for (int i = MinI; i < MaxI; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (tilemap.GetValue(new Vector3Int(i, j, 0), "useful") > 0.5)
                    UpdateTile(new Vector3Int(i, j, 0));
            }
        }
    }


    void ApplyChanges(int MinI, int MaxI)
    {
        for (int i = MinI; i < MaxI; i++)
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

        if (UpdateCounter == 0)
        {

            //for (int i = UpdateCounter * (width / 11); i < (UpdateCounter + 1) * (width / 11); i++)
            UpdateMapData(0, height);
            UpdateCounter++;
            //Debug.Log(NextValues[74, 53, 0]);
            //Debug.Log(tilemap.GetValue(new Vector3Int(74, 53, 0), "rabbit"));


        }
        else 
        {
            ApplyChanges((UpdateCounter-1) * (width / 11), UpdateCounter * (width / 11));
            UpdateMapGraphics((UpdateCounter - 1) * (width / 11), UpdateCounter * (width / 11));
            UpdateCounter++;
            if (UpdateCounter == 12)
                UpdateCounter = 0;
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
                    Color biomassColor = new Color(0.2f, 0.8f, 0, tilemap.GetValue(invertedP, "tree") / 8.0f);
                    biomass.SetColor(p, biomassColor); 
                }
            }
        }

    }

    void FixedUpdate()
    {
        //Debug.Log(1);
        UpdateMap();
<<<<<<< HEAD
        UpdateMapGraphics(0, height);
=======
        
>>>>>>> 0110f47ea3a4ab2bc79b2ba6e0265a582b6b708c
    }
}

