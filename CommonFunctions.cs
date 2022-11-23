using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Common
{
    public class GridCoordinates
    {
        int row;
        int column;

        public GridCoordinates(int row, int column)
        {
            this.row = row;
            this.column = column;
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

        public String ToString()
        {
            return ("(" + this.column.ToString() + "," + this.row.ToString() + ")");
        }
    }
}