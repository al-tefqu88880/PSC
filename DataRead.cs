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


    List<string[]> ReadDataClimate()
    {
        List<string[]> res = new();
        var path = @"Assets\base_environnement.csv";
        using (TextFieldParser csvParser = new(path))
        {
            csvParser.SetDelimiters(new string[] { "," });

            // Skip the row with the column names
            csvParser.ReadLine();

            while (!csvParser.EndOfData)
            {
                string[] fields = csvParser.ReadFields();
                res.Add(fields);
            }
        }
        return res;
    }

    List<string[]> ReadDataRabbit()
    {
        List<string[]> res = new List<string[]>();
        var path = @"Assets\occurrenceslapin reduit.csv";
        using (TextFieldParser csvParser = new(path))
        {
            csvParser.SetDelimiters(new string[] { "	" });

            // Skip the row with the column names
            csvParser.ReadLine();

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
        return new Vector3Int(c.x+1, c.y+1 , 0);
    }


    void AttributeValues()
    {
        List<string[]> data = ReadDataClimate();
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

        data = ReadDataRabbit();
        foreach (string[] line in data)
        {
            //Debug.Log(line[0]);
            Vector3Int position = ConvCoords(float.Parse(line[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(line[0], CultureInfo.InvariantCulture.NumberFormat));
            //Debug.Log(position[0]);
            //Debug.Log(position[1]);
            RunningBackEnd.tilemap.SetValue(position,"bear", RunningBackEnd.tilemap.GetValue(position,"bear")+1);
            RunningBackEnd.tilemap.SetTile(position, 1);
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

