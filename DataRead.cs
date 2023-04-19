using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;
using System;
using Common;


public class DataRead : MonoBehaviour
{
    public DataRead()
    {

    }


    List<string[]> ReadData()
    {
        List<string[]> res = new();
        var path = @"Assets\Game_data.csv";
        using (TextFieldParser csvParser = new(path))
        {
            csvParser.SetDelimiters(new string[] { ", " });

            // Skip the row with the column names
            //csvParser.ReadLine();

            while (!csvParser.EndOfData)
            {
                string[] fields = csvParser.ReadFields();
                res.Add(fields);
            }
        }
        return res;
    }



    Vector3Int ConvCoords(float x, float y)
    {
        GridCoordinates c = GridCoordinates.WorldToHexa(new Vector3((float)(3.0 * (x + 10.0)), (float)(3 * (y + -30.0)), 0));
        return new Vector3Int(c.x + 1, c.y + 1, 0);
    }


    /*     void AttributeValues()
       {
            List<string[]> data = ReadData();
            foreach (string[] line in data)
            {
                Vector3Int position = ConvCoords(float.Parse(line[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(line[2], CultureInfo.InvariantCulture.NumberFormat));
                bool success = int.TryParse(line[14], out int number);
                if (success)
                {
                    //Debug.Log(position[0]);
                    //Debug.Log(position[1]);
                    RunningBackEnd.tilemap.SetValue(position,"humidity", (float)number);
                    RunningBackEnd.tilemap.SetTile(position, 2);
                }
            }
            }
        }*/

    void AttributeValues()
    {
        List<string[]> data = ReadData();
        foreach (string[] line in data)
        {

            if (string.Equals(line[2], "1"))
            {
                Vector3Int position = new(int.Parse(line[0]), int.Parse(line[1]), 0);
                //Debug.Log(position[0]);
                //Debug.Log(position[1]); 
                RunningBackEnd.tilemap.SetValue(position, "useful", 1);
                RunningBackEnd.tilemap.SetTile(position, 4);        //debug, 4 should be 2
                RunningBackEnd.tilemap.SetValue(position, "rabbit", float.Parse(line[9], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "fox", float.Parse(line[10], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "lynx", float.Parse(line[11], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "temperature", float.Parse(line[3], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "isothermality", float.Parse(line[4], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "summerTemperature", float.Parse(line[5], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "rain", float.Parse(line[6], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "rainVariation", float.Parse(line[7], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "summerRain", float.Parse(line[8], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "tree", float.Parse(line[12], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "altitude", float.Parse(line[13], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "Temperature2040", float.Parse(line[14], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "Isothermality2040", float.Parse(line[15], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "SummerTemperature2040", float.Parse(line[16], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "varRain2040", float.Parse(line[17], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "varRainVariation2040", float.Parse(line[18], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "varSummerRain2040", float.Parse(line[19], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "Temperature2060", float.Parse(line[20], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "Isothermality2060", float.Parse(line[21], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "SummerTemperature2060", float.Parse(line[22], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "varRain2060", float.Parse(line[23], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "varRainVariation2060", float.Parse(line[24], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "varSummerRain2060", float.Parse(line[25], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "Temperature2080", float.Parse(line[26], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "Isothermality2080", float.Parse(line[27], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "SummerTemperature2080", float.Parse(line[28], CultureInfo.InvariantCulture) / 10);
                RunningBackEnd.tilemap.SetValue(position, "varRain2080", float.Parse(line[29], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "varRainVariation2080", float.Parse(line[30], CultureInfo.InvariantCulture));
                RunningBackEnd.tilemap.SetValue(position, "varSummerRain2080", float.Parse(line[31], CultureInfo.InvariantCulture));
            }
            if (string.Equals(line[2], "2"))
            {
                Vector3Int position = new(int.Parse(line[0]), int.Parse(line[1]), 0);
                //Debug.Log(position[0]);
                //Debug.Log(position[1]); 
                //RunningBackEnd.tilemap.SetValue(position, "useful", 1);
                RunningBackEnd.tilemap.SetTile(position, 3);
            }
        }
    }


    [ContextMenu("Apply Data")]

    public void ApplyData()
    {
        AttributeValues();
    }

    /*[ContextMenu("Test Data")]
    
    void testData()
    {
        RunningBackEnd.InitiateTilemap();
        EditorPainter.PaintWater();
        applyData();
    }*/


}