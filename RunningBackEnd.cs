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
    //private static float dRabbit = .3f;
    private static float cFox = 15e-5f;
    private static float dLynx = .55f;
    private static float cLynx = 2e-4f;
    private static float dFox = dLynx * cFox / cLynx;
    private static float[] moyTemperature = { 94.5f, 89.8f, 177f };
    private static float[] moyIsothermality = { 354f, 34.4f, 37f };
    private static float[] moySummerTemperature = { 164f, 165f, 243f };
    private static float[] moyRain = { 745f, 755f, 509f };
    private static float[] moyRainVariation = { 12f, 10f, 64f };
    private static float[] moySummerRain = { 190f, 196f, 23f };
    private static float[] sigmaTemperature = { 9f, 11.5f, 23f };
    private static float[] sigmaIsothermality = { 3.3f, 4.2f, 0.9f };
    private static float[] sigmaSummerTemperature = { 15f, 14f, 10f };
    private static float[] sigmaRain = { 80f, 90f, 30f };
    private static float[] sigmaRainVariation = { 7.3f, 11f, 14.5f };
    private static float[] sigmaSummerRain = { 25.7f, 33f, 12f };


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

    float gaussian(float x, float m, float sigma)
    {
        float res = (float)Math.Exp(-(x - m) * (x - m) / (2 * sigma * sigma));
        //if (res > 999)
            //Debug.Log("aled " + res);
        return res;
    }


    float Sigmoid(float x)
    {
        return (float)1 / (1 + (float)Math.Exp(20 * (x - 0.45)));
    }


    float[] ClimateAffinity(Vector3Int coords)
    {

        float rabbitAffinity = gaussian(tilemap.GetValue(coords, "temperature"), moyTemperature[0], sigmaTemperature[0]);
        rabbitAffinity +=  2* gaussian(tilemap.GetValue(coords, "isothermality"), moyIsothermality[0], sigmaIsothermality[0]);
        rabbitAffinity += 1.43f * gaussian(tilemap.GetValue(coords, "summerTemperature"), moySummerTemperature[0], sigmaSummerTemperature[0]);
        /*if (rabbitAffinity > 999)
            Debug.Log("Aff : " + rabbitAffinity);*/
        rabbitAffinity += gaussian(tilemap.GetValue(coords, "rain"), moyRain[0], sigmaRain[0]);
        rabbitAffinity += 0.3f*gaussian(tilemap.GetValue(coords, "rainVariation"), moyRainVariation[0], sigmaRainVariation[0]);
        rabbitAffinity += 0.9f*gaussian(tilemap.GetValue(coords, "summerRain"), moySummerRain[0], sigmaSummerRain[0]);
        rabbitAffinity = rabbitAffinity / 6.63f;
        float foxAffinity = gaussian(tilemap.GetValue(coords, "temperature"), moyTemperature[1], sigmaTemperature[1]);
        foxAffinity += 2.19f * gaussian(tilemap.GetValue(coords, "isothermality"), moyIsothermality[1], sigmaIsothermality[1]);
        foxAffinity += 1.86f * gaussian(tilemap.GetValue(coords, "summerTemperature"), moySummerTemperature[1], sigmaSummerTemperature[1]);
        foxAffinity += gaussian(tilemap.GetValue(coords, "rain"), moyRain[1], sigmaRain[1]);
        foxAffinity += 0.59f * gaussian(tilemap.GetValue(coords, "rainVariation"), moyRainVariation[1], sigmaRainVariation[1]);
        foxAffinity += 0.86f * gaussian(tilemap.GetValue(coords, "summerRain"), moySummerRain[1], sigmaSummerRain[1]);
        foxAffinity = foxAffinity / 7.5f;
        float lynxAffinity = gaussian(tilemap.GetValue(coords, "temperature"), moyTemperature[2], sigmaTemperature[2]);
        lynxAffinity += 2f * gaussian(tilemap.GetValue(coords, "isothermality"), moyIsothermality[2], sigmaIsothermality[2]);
        lynxAffinity += 1.43f * gaussian(tilemap.GetValue(coords, "summerTemperature"), moySummerTemperature[2], sigmaSummerTemperature[2]);
        lynxAffinity += gaussian(tilemap.GetValue(coords, "rain"), moyRain[2], sigmaRain[2]);
        lynxAffinity += 0.3f * gaussian(tilemap.GetValue(coords, "rainVariation"), moyRainVariation[2], sigmaRainVariation[2]);
        lynxAffinity += 0.9f * gaussian(tilemap.GetValue(coords, "summerRain"), moySummerRain[2], sigmaSummerRain[2]);
        lynxAffinity = lynxAffinity / 6.63f;
        float [] res = new float[3];
        res[0] = rabbitAffinity;
        res[1] = foxAffinity;
        res[2] = lynxAffinity;
        return  res ;
    }


    void UpdateClimateAffinity(Vector3Int coords)
    {
        float[] tab = ClimateAffinity(coords);
        tilemap.SetValue(coords, "rabbitClimateAffinity", tab[0]);
        tilemap.SetValue(coords, "foxClimateAffinity", tab[1]);
        tilemap.SetValue(coords, "lynxClimateAffinity", tab[2]);
    }

    
    float PreyAffinity(Vector3Int coords)
    {
        float rabbit = tilemap.GetValue(coords, "rabbit");
        float fox = tilemap.GetValue(coords, "fox");
        float lynx = tilemap.GetValue(coords, "lynx");
        if ((.3f * fox + .15f * lynx) == 0 & rabbit > 0)
            return 1;
        if ((.3f * fox + .15f * lynx) == 0)
            return 0;
        float res = (rabbit - .3f * fox - .15f * lynx) / (.3f * fox + .15f * lynx);
        if (res > 1)
            res = 1;
        if (res < 0)
            res = 0;
        //if (System.Single.IsNaN(res))
        //Debug.Log(rabbit + "" + fox + lynx + "             " + res + " " + coords);
        return res;
    } 


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
        float rabbitClimateAffinity = tilemap.GetValue(coords, "rabbitClimateAffinity");
        float foxClimateAffinity = tilemap.GetValue(coords, "foxClimateAffinity");
        float lynxClimateAffinity = tilemap.GetValue(coords, "lynxClimateAffinity");
        float preyAffinity = tilemap.GetValue(coords, "preyAffinity");


        float rabbit2 = rabbit + tickToYear * (cRabbit * rabbit * (1 - rabbit / k) - pFox * rabbit * fox - pLynx * lynx * rabbit);
        float fox2 = fox - tickToYear * (dFox * fox + cFox * rabbit * fox);
        float lynx2 = lynx - tickToYear * (dLynx * lynx + cLynx * lynx * rabbit);

        //if (coords[0] == 19 & coords[1] == 103) { 
        //    Debug.Log("rabbit : " + rabbit + " fox : " + fox + " lynx : " + lynx);
        //    Debug.Log("rabbit : " + rabbit2 + " fox : " + fox2 + " lynx : " + lynx2); 
        //}

        float fleeingRabbit = rabbit * Sigmoid(rabbitClimateAffinity) * tickToYear;
        float fleeingFox = fox * Sigmoid(foxClimateAffinity * 0.75f + preyAffinity * 0.25f) * tickToYear;
        float fleeingLynx = lynx * Sigmoid(lynxClimateAffinity * 0.75f + preyAffinity * 0.25f) * tickToYear;

        float lynxHospitality = 0;
        float foxHospitality = 0;
        float rabbitHospitality = 0;
        

        for (int j = 0; j < neibourgh.Count; j++)
        {
            float prey = tilemap.GetValue(neibourgh[j], "preyAffinity");
            float altitude = tilemap.GetValue(neibourgh[j], "altitude");
            if (altitude < 1000)
            {
                lynxHospitality += (0.75f * tilemap.GetValue(neibourgh[j], "lynxClimateAffinity") + 0.25f * prey);
                if (altitude < 500)
                {
                    rabbitHospitality += tilemap.GetValue(neibourgh[j], "rabbitClimateAffinity");
                    if (altitude <400)
                        foxHospitality += 0.75f * tilemap.GetValue(neibourgh[j], "foxClimateAffinity") + 0.25f * prey;
                }
            }
        }

        lynxHospitality *= 2;
        foxHospitality *= 2;
        rabbitHospitality *= 2;

        for (int j = 0; j < neibourgh.Count; j++)
        {
            float prey = tilemap.GetValue(neibourgh[j], "preyAffinity");
            float altitude = tilemap.GetValue(neibourgh[j], "altitude");
            if (altitude < 1000)
            {
                if (lynxHospitality > 0)
                    NextValues[neibourgh[j][0], neibourgh[j][1], 2] += SignCheck(fleeingLynx*(0.75f * tilemap.GetValue(neibourgh[j], "lynxClimateAffinity") + 0.25f * prey)/lynxHospitality);
                //if (neibourgh[j][0] == 19 & neibourgh[j][1] == 103)
                    //Debug.Log("lynx : " + tilemap.GetValue(neibourgh[j], "lynxClimateAffinity") + " " + lynxHospitality +" "+ prey);
                if (altitude < 500)
                {
                    //if (neibourgh[j][0] == 19 & neibourgh[j][1] == 103)
                    //    Debug.Log("rabbit : " + tilemap.GetValue(neibourgh[j], "rabbitClimateAffinity") + " " + rabbitHospitality);
                    if (rabbitHospitality >0)
                        NextValues[neibourgh[j][0], neibourgh[j][1], 0] += SignCheck(fleeingRabbit*(tilemap.GetValue(neibourgh[j], "rabbitClimateAffinity") / rabbitHospitality));
                    if (altitude < 400 & foxHospitality>0) { 
                    NextValues[neibourgh[j][0], neibourgh[j][1], 1] += SignCheck(fleeingFox*(0.75f * tilemap.GetValue(neibourgh[j], "foxClimateAffinity") + 0.25f * prey) / foxHospitality);
                        //if (neibourgh[j][0] == 19 & neibourgh[j][1] == 103)
                        //    Debug.Log("fox : " + tilemap.GetValue(neibourgh[j], "foxClimateAffinity") + " " + foxHospitality + " " + prey);
                    }
                }
            }
        }

        NextValues[coords[0], coords[1], 0] += SignCheck(rabbit2-fleeingRabbit);
        NextValues[coords[0], coords[1], 1] += SignCheck(fox2-fleeingFox);
        NextValues[coords[0], coords[1], 2] += SignCheck(lynx2-fleeingLynx);
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
                        Color foxColor = new Color(0, 0, 1, 0.1f + tilemap.GetValue(invertedP, "fox") / 2000.0f);
                        fox.SetColor(p, foxColor);
                    }
                    else
                    {
                        fox.SetColor(p, emptyColor);
                    }
                    if (tilemap.GetValue(invertedP, "lynx") > 1)
                    {
                        Color lynxColor = new Color(1, 0, 1, 0.1f + tilemap.GetValue(invertedP, "lynx") / 100.0f);
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
                tilemap.SetValue(coords, "rabbit", SignCheck(NextValues[i, j, 0]));
                tilemap.SetValue(coords, "fox", SignCheck(NextValues[i, j, 1]));
                tilemap.SetValue(coords, "lynx", SignCheck(NextValues[i, j, 2]));
                tilemap.SetValue(coords, "preyAffinity", PreyAffinity(coords));
                NextValues[i, j, 0] = 0;
                NextValues[i, j, 1] = 0;
                NextValues[i, j, 2] = 0;  

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
            UpdateMapData(0, width );
            UpdateCounter++;
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
                    tilemap.SetValue(p, "preyAffinity", PreyAffinity(p));
                    UpdateClimateAffinity(p);
                }
            }
        }

    }


    void FixedUpdate()
    {
        //Debug.Log(1);
        UpdateMap();
        //UpdateMapGraphics(0, height);
        //Debug.Log(tilemap.GetValue(new Vector3Int(60, 60, 0), "rabbit"));

    }
}

