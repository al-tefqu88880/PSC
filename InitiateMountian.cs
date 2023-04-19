using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class InitiateMountian : MonoBehaviour
{
    public Tilemap terrain;
    public Tile moutainTile;

    private void InitiateMountain()
    {
        for (int i=0; i<RunningBackEnd.tilemap.GetHeight(); i++)
        {
            for (int j = 0; j < RunningBackEnd.tilemap.GetWidth(); j++)
            {

                Vector3Int p = new Vector3Int(i, j, 0);
                Vector3Int invertedP = new Vector3Int(j, i, 0);
                if (RunningBackEnd.tilemap.GetValue(invertedP, "useful") > 0.5)
                {
                    float altitude = RunningBackEnd.tilemap.GetValue(invertedP, "altitude");
                    if (altitude > 100)
                    {
                        terrain.SetTile(p, moutainTile);
                        terrain.SetTileFlags(p, TileFlags.None);
                        float altitudeNormalized = Math.Min(altitude / 2500.0f,1.0f);
                        terrain.SetColor(p, new Color(1-altitudeNormalized, 1-altitudeNormalized, 1-altitudeNormalized, 1));

                    }
                }
            }
        }
    }
    private void Start()
    {
        InitiateMountain();
    }
}
