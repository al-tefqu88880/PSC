using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;

public class DataRead : MonoBehaviour
{

    List<string[]> readData()
    {
        List<string[]> res = new List<string[]>();
        var path = @"C:\Users\alexa\Jeu PSC\CSV données environnementales réduit.csv";
        using (TextFieldParser csvParser = new TextFieldParser(path))
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

    Vector3Int convCoords(float x, float y)
    {
        return new Vector3Int((int)(1.0 * x), (int)(1.0 * y), 0);
    }


    void attributeValues(List<string[]> data)
    {
        foreach (string[] line in data)
        {
            Vector3Int position = convCoords(float.Parse(line[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(line[2], CultureInfo.InvariantCulture.NumberFormat));
            RunningBackEnd.tilemap.SetHumidity(position, float.Parse(line[14]));
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

