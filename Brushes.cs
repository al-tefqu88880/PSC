using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System;


public class Brushes : MonoBehaviour
{

    public static bool IsInMap(Vector3Int position)
    {
        if (position.x<0 | position.x >= RunningBackEnd.tilemap.GetWidth() | position.y<0 | position.y >= RunningBackEnd.tilemap.GetHeight())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static List<Vector3Int> GetArea(Vector3Int center, int radius)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        for (int i=center.x-radius; i < center.x + radius + 1; i++)
        {
            for (int j = center.y - radius + (int)(Math.Abs(i - center.x) / 2); j < center.y + radius + 1 - (int)((Math.Abs(i - center.x) + 1) / 2); j++)
            {
                Vector3Int temp;
                if(center.x%2==1 & i % 2 == 0)
                {
                    temp = new Vector3Int(i, j+1, 0);
                }
                else
                {
                    temp = new Vector3Int(i, j, 0);
                }
                if (IsInMap(temp))
                {
                    result.Add(temp);
                }
            }
        }
        return result;
    }

    public static void ChangeBrush(Vector3Int position, int radius, string name, float change)
    {
        foreach (Vector3Int pos in GetArea(position, radius))
        {
            RunningBackEnd.tilemap.ChangeValue(pos, name, change);
        }
    }

    public static void SetBrush(Vector3Int position, int radius, string name, float change)
    {
        foreach (Vector3Int pos in GetArea(position, radius))
        {
            RunningBackEnd.tilemap.SetValue(pos, name, change);
        }
    }

    public static float GetAverageBrush(Vector3Int position, int radius, string name)
    {
        float result = 0;
        int count = 0;
        foreach (Vector3Int pos in GetArea(position, radius))
        {
            result += RunningBackEnd.tilemap.GetValue(pos, name);
            count++;
        }
        return result / count;
    }

    public static float GetTotalBrush(Vector3Int position, int radius, string name)
    {
        float result = 0;
        foreach (Vector3Int pos in GetArea(position, radius))
        {
            result += RunningBackEnd.tilemap.GetValue(pos, name);
        }
        return result;
    }

}
