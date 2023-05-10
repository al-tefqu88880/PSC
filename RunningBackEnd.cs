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

    private int UpdateCounter ;
    public static float[,,] NextValues = new float[121, 121, 3];

    private static float baseTickToYear = 0.00462962962f;
    public int gameSpeed = 1;
    private float currentTickToYear = baseTickToYear*10;
    private float timeCounter = 0;

    private static float cRabbit = (float)Math.Log(3);
    private static float k = 5000f;
    private static float pFox = 4e-5f;
    private static float pLynx = 8e-4f;
    //private static float dRabbit = .3f;
    private static float cFox = 4e-5f;
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

    private float rabbitAffinityMultiplier = 1;
    private float foxAffinityMultiplier = 1;
    private float lynxAffinityMultiplier = 1;


    public static TilemapData GetTilemap()
    {
        return tilemap;
    }

    [ContextMenu("Initiate Tilemap")]

    public void InitiateTilemap()
    {
        tilemap = new TilemapData(height, width, terrain, rabbit, lynx, fox, biomass);
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
        return res;
    }


    float LogSigmoid(float x)
    {
        return (float) - Math.Log(1 - 0.1 / (1 + (float)Math.Exp(20 * (x - 0.45))));
    }


    float[] ClimateAffinity(Vector3Int coords)
    {

        float rabbitAffinity = gaussian(tilemap.GetValue(coords, "temperature"), moyTemperature[0], sigmaTemperature[0]);
        rabbitAffinity +=  2* gaussian(tilemap.GetValue(coords, "isothermality"), moyIsothermality[0], sigmaIsothermality[0]);
        rabbitAffinity += 1.43f * gaussian(tilemap.GetValue(coords, "summerTemperature"), moySummerTemperature[0], sigmaSummerTemperature[0]);
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
        res[0] = rabbitAffinity*rabbitAffinityMultiplier;
        res[1] = foxAffinity*foxAffinityMultiplier;
        res[2] = lynxAffinity*lynxAffinityMultiplier;
        return  res ;
    }


    void UpdateClimateAffinity(Vector3Int coords)
    {
        float[] tab = ClimateAffinity(coords);
        tilemap.SetValue(coords, "rabbitClimateAffinity", tab[0]);
        tilemap.SetValue(coords, "foxClimateAffinity", tab[1]);
        tilemap.SetValue(coords, "lynxClimateAffinity", tab[2]);
        //Debug.Log(coords + "   " + tab[0] + "   " + tab[1] + "   " + tab[2]);
    }


    void UpdateAffinityMultipliers()
    {
        float rabbitMax = 0;
        float foxMax = 0;
        float lynxMax = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int coords = new Vector3Int(x, y, 0);
                if (tilemap.GetValue(coords, "rabbitClimateAffinity") > rabbitMax)
                    rabbitMax = tilemap.GetValue(coords, "rabbitClimateAffinity");
                if (tilemap.GetValue(coords, "foxClimateAffinity") > foxMax)
                    foxMax = tilemap.GetValue(coords, "foxClimateAffinity"); 
                if (tilemap.GetValue(coords, "lynxClimateAffinity") > lynxMax)
                    lynxMax = tilemap.GetValue(coords, "lynxClimateAffinity");
            }
        }
        rabbitAffinityMultiplier = rabbitAffinityMultiplier / rabbitMax;
        foxAffinityMultiplier = foxAffinityMultiplier / foxMax;
        lynxAffinityMultiplier = lynxAffinityMultiplier / lynxMax;
    }

    
    float PreyAffinity(Vector3Int coords)
    {
        float rabbit = tilemap.GetValue(coords, "rabbit");
        float fox = tilemap.GetValue(coords, "fox");
        float lynx = tilemap.GetValue(coords, "lynx");
        if ( rabbit == 0)
            return 0;
        if ((.3f * fox + .15f * lynx) == 0)
            return 1;
        float res = rabbit / (5*(.3f * fox + .15f * lynx));
        if (res > 1)
            res = 1;
        if (res < 0)
            res = 0;
        return res;
    }


    float SignCheck(float x)
    {
        if (x < 0) { 
        //Debug.Log("neg : " + x);
        return 0; }
        if (x > 10000)
            return 10000;
        return x;
    }


    void UpdateTile(Vector3Int coords)
    {
        UpdateTileSpeciesData(coords);
    }


    void UpdateTileSpeciesData(Vector3Int coords)
    {
        List<Vector3Int> neibourgh = GridCoordinates.GetNeighbours(coords[0], coords[1], width , height);
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


        float rabbit2 = rabbit + currentTickToYear * rabbit*(cRabbit  * (1 - rabbit / k) - pFox * fox - pLynx * lynx - LogSigmoid(rabbitClimateAffinity));
        float fox2 = fox + currentTickToYear * fox * (- dFox + cFox * rabbit - LogSigmoid(foxClimateAffinity));
        float lynx2 = lynx + currentTickToYear * lynx * (- dLynx + cLynx * rabbit - LogSigmoid(lynxClimateAffinity));

        float fleeingRabbit = rabbit * LogSigmoid(rabbitClimateAffinity) * currentTickToYear;
        //if (coords[0] == 54 && coords[1] == 11) 
        //    Debug.Log(fleeingRabbit + " rabbits " + rabbit + " pop " + LogSigmoid(rabbitClimateAffinity));
        float fleeingFox = SignCheck(fox * LogSigmoid(foxClimateAffinity * 0.75f + preyAffinity * 0.25f) * currentTickToYear);
        float fleeingLynx = SignCheck(lynx * LogSigmoid(lynxClimateAffinity * 0.75f + preyAffinity * 0.25f) * currentTickToYear);

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
                    //if (coords[0] == 54 && coords[1] == 11)
                    //{
                    //    Debug.Log(neibourgh[j] + "         " + tilemap.GetValue(neibourgh[j], "rabbitClimateAffinity"));
                    //}
                    rabbitHospitality += tilemap.GetValue(neibourgh[j], "rabbitClimateAffinity");
                    if (altitude <400)
                        foxHospitality += 0.75f * tilemap.GetValue(neibourgh[j], "foxClimateAffinity") + 0.25f * prey;
                }
            }
        }

        for (int j = 0; j < neibourgh.Count; j++)
        {
            float prey = tilemap.GetValue(neibourgh[j], "preyAffinity");
            float altitude = tilemap.GetValue(neibourgh[j], "altitude");
            if (altitude < 1000)
            {
                if (lynxHospitality > 0)
                    NextValues[neibourgh[j][0], neibourgh[j][1], 2] += SignCheck(fleeingLynx*(0.75f * tilemap.GetValue(neibourgh[j], "lynxClimateAffinity") + 0.25f * prey)/lynxHospitality);
                if (altitude < 500)
                {
                    //if (coords[0] == 54 && coords[1] == 11)
                    //{
                    //    Debug.Log(rabbitHospitality + "  " + neibourgh[j] + "         " + (fleeingRabbit * (tilemap.GetValue(neibourgh[j], "rabbitClimateAffinity") / rabbitHospitality)));
                    //}
                    if (rabbitHospitality >0)
                        NextValues[neibourgh[j][0], neibourgh[j][1], 0] += SignCheck(fleeingRabbit*(tilemap.GetValue(neibourgh[j], "rabbitClimateAffinity") / rabbitHospitality));
                    if (altitude < 400 & foxHospitality>0) { 
                    NextValues[neibourgh[j][0], neibourgh[j][1], 1] += SignCheck(fleeingFox*(0.75f * tilemap.GetValue(neibourgh[j], "foxClimateAffinity") + 0.25f * prey) / foxHospitality);
                    }
                }
            }
        }

        NextValues[coords[0], coords[1], 0] += SignCheck(rabbit2-fleeingRabbit);
        NextValues[coords[0], coords[1], 1] += SignCheck(fox2-fleeingFox);
        NextValues[coords[0], coords[1], 2] += SignCheck(lynx2-fleeingLynx);
    }


    void UpdateTileClimateData(Vector3Int coords)
    {
        if (timeCounter < 20)
        {
            tilemap.ChangeValue(coords, "temperature", (tilemap.GetValue(coords, "temperature2040") - tilemap.GetValue(coords, "temperature2020")) / 20 * currentTickToYear);
            tilemap.ChangeValue(coords, "isothermality", (tilemap.GetValue(coords, "isothermality2040") - tilemap.GetValue(coords, "isothermality2020")) / 20 * currentTickToYear);
            tilemap.ChangeValue(coords, "summerTemperature", (tilemap.GetValue(coords, "summerTemperature2040") - tilemap.GetValue(coords, "summerTemperature2020")) / 20 * currentTickToYear);
            tilemap.ChangeValue(coords, "rain", (tilemap.GetValue(coords, "rain2040") - tilemap.GetValue(coords, "rain2020")) / 20 * currentTickToYear);
            tilemap.ChangeValue(coords, "rainVariation", (tilemap.GetValue(coords, "rainVariation2040") - tilemap.GetValue(coords, "rainVariation2020")) / 20 * currentTickToYear);
            tilemap.ChangeValue(coords, "summerRain", (tilemap.GetValue(coords, "summerRain2040") - tilemap.GetValue(coords, "summerRain2020")) / 20 * currentTickToYear);
        }
        UpdateClimateAffinity(coords);
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
                //UpdateTileClimateData(coords);
            }
        }
    }


    void UpdateMap()
    {
        if (UpdateCounter == 0)
        {
            //Debug.Log(timeCounter);
            UpdateMapData(0, width );
            UpdateCounter++;
            timeCounter += currentTickToYear;
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
                Vector3Int invertedP = new Vector3Int(y, x, 0);
                if (tilemap.GetValue(invertedP, "useful") > 0.5)
                {
                    UpdateClimateAffinity(invertedP);
                }
            }
        }
        UpdateAffinityMultipliers();
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
                    tilemap.SetValue(invertedP, "preyAffinity", PreyAffinity(invertedP));
                    UpdateClimateAffinity(invertedP);
                }
            }
        }
        Debug.Log("57, 56 : " + tilemap.GetValue(new Vector3Int(57, 56, 0), "useful"));
        List<Vector3Int> neibourgh = GridCoordinates.GetNeighbours(57, 56, width, height);
        for (int i = 0; i < neibourgh.Count; i++)
            Debug.Log(neibourgh[i]);
    }


    void FixedUpdate()
    {
        UpdateMap();
        //UpdateMapGraphics(0, height);

    }
}

