using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;


public class Brushes : MonoBehaviour
{

    bool IsInMap(Vector3Int position)
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

    void GetAreaRecursive(Vector3Int center, int radius, HashSet<Vector3Int> buffer, string accessedFrom)
    {
        if (radius > 0 & radius<12)
        {
            Vector3Int left = new Vector3Int(center.x, center.y - 1, 0);
            Vector3Int right = new Vector3Int(center.x, center.y + 1, 0);
            Vector3Int topLeft;
            Vector3Int topRight;
            Vector3Int bottomLeft;
            Vector3Int bottomRight;
            if (center.x % 2 == 0)     //on even lines 
            {
                topLeft = new Vector3Int(center.x + 1, center.y - 1, 0);
                topRight = new Vector3Int(center.x + 1, center.y, 0);
                bottomLeft = new Vector3Int(center.x - 1, center.y - 1, 0);
                bottomRight = new Vector3Int(center.x - 1, center.y, 0);
            }
            else     //on odd lines
            {
                topLeft = new Vector3Int(center.x + 1, center.y, 0);
                topRight = new Vector3Int(center.x + 1, center.y + 1, 0);
                bottomLeft = new Vector3Int(center.x - 1, center.y, 0);
                bottomRight = new Vector3Int(center.x - 1, center.y + 1, 0);
            }

            switch (accessedFrom)
            {
                case "origin":
                    if (IsInMap(left))
                    {
                        buffer.Add(left);
                        GetAreaRecursive(left, radius - 1, buffer, "right");
                    }
                    if (IsInMap(topLeft))
                    {
                        buffer.Add(topLeft);
                        GetAreaRecursive(topLeft, radius - 1, buffer, "bottomRight");
                    }
                    if (IsInMap(topRight))
                    {
                        buffer.Add(topRight);
                        GetAreaRecursive(topRight, radius - 1, buffer, "bottomLeft");
                    }
                    if (IsInMap(right))
                    {
                        buffer.Add(right);
                        GetAreaRecursive(right, radius - 1, buffer, "left");
                    }
                    if (IsInMap(bottomRight))
                    {
                        buffer.Add(bottomRight);
                        GetAreaRecursive(bottomRight, radius - 1, buffer, "topLeft");
                    }
                    if (IsInMap(bottomLeft))
                    {
                        buffer.Add(bottomLeft);
                        GetAreaRecursive(bottomLeft, radius - 1, buffer, "topRight");
                    }
                    break;
                case "left":
                    if (IsInMap(topRight))
                    {
                        buffer.Add(topRight);
                        GetAreaRecursive(topRight, radius - 1, buffer, "bottomLeft");
                    }
                    if (IsInMap(right))
                    {
                        buffer.Add(right);
                        GetAreaRecursive(right, radius - 1, buffer, "left");
                    }
                    if (IsInMap(bottomRight))
                    {
                        buffer.Add(bottomRight);
                        GetAreaRecursive(bottomRight, radius - 1, buffer, "topLeft");
                    }
                    break;
                case "topLeft":
                    if (IsInMap(bottomRight))
                    {
                        buffer.Add(bottomRight);
                        GetAreaRecursive(bottomRight, radius - 1, buffer, "topLeft");
                    }
                    if (IsInMap(right))
                    {
                        buffer.Add(right);
                        GetAreaRecursive(right, radius - 1, buffer, "left");
                    }
                    if (IsInMap(bottomLeft))
                    {
                        buffer.Add(bottomLeft);
                        GetAreaRecursive(bottomLeft, radius - 1, buffer, "topRight");
                    }
                    break;
                case "topRight":
                    if (IsInMap(bottomRight))
                    {
                        buffer.Add(bottomRight);
                        GetAreaRecursive(bottomRight, radius - 1, buffer, "topLeft");
                    }
                    if (IsInMap(bottomLeft))
                    {
                        buffer.Add(bottomLeft);
                        GetAreaRecursive(bottomLeft, radius - 1, buffer, "topRight");
                    }
                    if (IsInMap(left))
                    {
                        buffer.Add(left);
                        GetAreaRecursive(left, radius - 1, buffer, "right");
                    }
                    break;
                case "right":
                    if (IsInMap(left))
                    {
                        buffer.Add(left);
                        GetAreaRecursive(left, radius - 1, buffer, "right");
                    }
                    if (IsInMap(bottomLeft))
                    {
                        buffer.Add(bottomLeft);
                        GetAreaRecursive(bottomLeft, radius - 1, buffer, "topRight");
                    }
                    if (IsInMap(topLeft))
                    {
                        buffer.Add(topLeft);
                        GetAreaRecursive(topLeft, radius - 1, buffer, "bottomRight");
                    }
                    break;
                case "bottomRight":
                    if (IsInMap(left))
                    {
                        buffer.Add(left);
                        GetAreaRecursive(left, radius - 1, buffer, "right");
                    }
                    if (IsInMap(topLeft))
                    {
                        buffer.Add(topLeft);
                        GetAreaRecursive(topLeft, radius - 1, buffer, "bottomRight");
                    }
                    if (IsInMap(topRight))
                    {
                        buffer.Add(topRight);
                        GetAreaRecursive(topRight, radius - 1, buffer, "bottomLeft");
                    }
                    break;
                case "bottomLeft":
                    if (IsInMap(topRight))
                    {
                        buffer.Add(topRight);
                        GetAreaRecursive(topRight, radius - 1, buffer, "bottomLeft");
                    }
                    if (IsInMap(topLeft))
                    {
                        buffer.Add(topLeft);
                        GetAreaRecursive(topLeft, radius - 1, buffer, "bottomRight");
                    }
                    if (IsInMap(right))
                    {
                        buffer.Add(right);
                        GetAreaRecursive(right, radius - 1, buffer, "left");
                    }
                    break;
                default:
                    Debug.Log("Error in Brushes.GetAreaRecursive accessedFromVariable");
                    break;

            }
        }
        if (radius > 11)
        {
            Debug.Log("GetArea (brush) called on too big of a radius. Max allowed is 11");
        }
    }

    HashSet<Vector3Int> GetArea(Vector3Int center, int radius)
    {
        HashSet<Vector3Int> result = new HashSet<Vector3Int>();
        result.Add(center);
        GetAreaRecursive(center, radius, result, "origin" );
        return result;
    }

    public void ChangeBrush(Vector3Int position, int radius, string name, float change)
    {
        foreach (Vector3Int pos in GetArea(position, radius))
        {
            RunningBackEnd.tilemap.ChangeValue(pos, name, change);
        }
    }

}
