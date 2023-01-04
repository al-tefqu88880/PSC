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
        var path = @"C:\Users\alexa\Jeu PSC\CSV donn�es environnementales reduit(1).csv";
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
        return new Vector3Int((int)(10.0 * x)-300, (int)(10.0 * y)+100, 0);
    }


    void attributeValues(List<string[]> data)
    {
        foreach (string[] line in data)
        {
            Vector3Int position = convCoords(float.Parse(line[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(line[1], CultureInfo.InvariantCulture.NumberFormat));
            int number;
            bool success = int.TryParse(line[14], out number);
            if (success)
            {
                Debug.Log(position[0]);
                Debug.Log(position[1]);
                RunningBackEnd.tilemap.SetHumidity(position, (float)number);
                RunningBackEnd.tilemap.SetTile(position, 2);
            }
        }
    }

    [ContextMenu("Apply Data")]

    void applyData()
    {
        attributeValues(readData());
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

