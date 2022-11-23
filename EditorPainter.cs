using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Microsoft.VisualBasic.FileIO;


public class EditorPainter : MonoBehaviour
{
    public Tilemap terrain;
    public  Tile desert;
    public  Tile grass;
    public  Tile water;

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

    (int, int, int) convCoords(float x,float y)
    {
        return (0,0,0);
    }


    null attributeValues(List<string[]>)
    {

    }



[ContextMenu("Paint")]
    void Paint()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                int index = Random.Range(0, 3);
                if (index == 0)
                {
                    terrain.SetTile(p, desert);
                }
                else
                {
                    if (index == 1)
                    {
                        terrain.SetTile(p, grass);
                    }
                    else
                    {
                        terrain.SetTile(p, water);
                    }
                }
                
            }
        }
        
    }
}
