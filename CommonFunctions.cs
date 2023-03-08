using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Common
{
    public class GridCoordinates
    {
        public int x;
        public int y;

        public GridCoordinates(int row, int column)
        {
            this.x = row;
            this.y = column;
        }

        public static GridCoordinates WorldToHexa(Vector3 coordinates)
        {
            var x = coordinates.x;
            var y = coordinates.y;
            var doubleRow = (int) Math.Floor(y / 1.5);
            var column = (int) Math.Floor(x);
            var newx = x - column;
            var newy = y - doubleRow * 1.5;
            var row=0;
            
            if (newx < 0.5)
            {
                if (newy > 0.846 + 0.578 * newx)
                {
                    column--;
                    row = 2 * doubleRow + 1;
                }
                else
                {
                    if (newy < 0.289 - 0.578 * newx)
                    {
                        row = doubleRow * 2 - 1;
                        column--;
                    }
                    else
                    {
                        row = doubleRow * 2;
                    }
                }
            }
            else
            {
                if (newy > 1.424 - 0.578 * newx)
                {
                    row = doubleRow * 2 + 1;
                }
                else
                {
                    if (newy < 0.578 * newx - 0.289)
                    {
                        row = doubleRow * 2 - 1;
                    }
                    else
                    {
                        row = doubleRow * 2;
                    }
                }
            }

            return new GridCoordinates(row, column);
        }

        public Vector3Int V3I()
        {
            return new Vector3Int(x, y, 0);
        }

        public static Vector3Int MouseAtTile(GameObject camera)
        {
            var screenPoint = (Input.mousePosition);
            screenPoint.z = (-1) * camera.GetComponent<Transform>().position.z ; //distance of the plane from the camera
            Vector3 mouseCoordinates = Camera.main.ScreenToWorldPoint(screenPoint);
            GridCoordinates gridCoord = GridCoordinates.WorldToHexa(mouseCoordinates);
            return gridCoord.V3I();
        }

        override
        public String ToString()
        {
            return ("(" + this.x.ToString() + "," + this.y.ToString() + ")");
        }

        public static List<Vector3Int> Neibourghs(int i, int j, int maxI, int maxJ)
        {
            List<Vector3Int> res = new ();
            if (i + 1 < maxI)
                res.Add(new Vector3Int(i + 1, j, 0));
            if (i - 1 >= 0)
                res.Add(new Vector3Int(i - 1, j, 0));
            if (j + 1 < maxJ)
                res.Add(new Vector3Int(i, j + 1, 0));
            if (j - 1 >= 0)
                res.Add(new Vector3Int(i, j - 1, 0));
            if (j + (i % 2) < maxJ && i + 1 < maxI)
                res.Add(new Vector3Int(i + 1, j + (i % 2), 0));
            if (j + (i % 2) < maxJ && i -1 >= 0)
                res.Add(new Vector3Int(i - 1, j + (i % 2), 0));
            return res;
        }
    }
}