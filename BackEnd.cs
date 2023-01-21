using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//in TilemapData, need to implement the initialization from a .csv file


namespace BackEnd
{
    public class TileData
    {
        private int tile;  //the displayed tile in the game will be selected depending on this int (0=water, 1=desert...)
        private float[] characteristics;

        private static int StringToIndex(string name)
        {
            switch (name)
            {
                case "bear":
                    return 0;
                case "lynx":
                    return 1;
                case "vole":
                    return 2;
                case "biomass":
                    return 3;
                case "humidity":
                    return 4;
                case "sunlight":
                    return 5;
                default:
                    Debug.Log("Invalid characteristic name");
                    return -1;
            }
        }

        public TileData()
        {
            this.tile = 0;
            this.characteristics = new float[6];
        }

        public void SetTile(int value)
        {
            this.tile = value;
        }
        
        public void SetValue(string name, float value)
        {
            this.characteristics[StringToIndex(name)] = value;
        }

        public int GetTile()
        {
            return this.tile;
        }
        
        public float GetValue(string name)
        {
            return this.characteristics[StringToIndex(name)];
        }

    }
    public class TilemapData
    {
        public static Tile error = (Tile)Resources.Load("error");
        public static Tile water = (Tile)Resources.Load("water");
        public static Tile desert = (Tile)Resources.Load("desert");
        public static Tile plain = (Tile)Resources.Load("plain");

        private TileData[,] tileMatrix;
        private int width;
        private int height;
        private Tilemap terrain;

        public TilemapData(int x_width,int y_width, Tilemap terrain)
        {
            this.tileMatrix = new TileData[x_width, y_width];
            this.width = x_width;
            this.height = y_width;
            this.terrain = terrain;
            for (int x=0; x< x_width; x++)
            {
                for (int y = 0; y< y_width; y++)
                {
                    tileMatrix[x, y] = new TileData();
                }
            }
        }

        public int GetHeight()
        {
            return height;
        }
        public int GetWidth()
        {
            return width;
        }

        public void SetTile(Vector3Int position, int tile)
        {
            tileMatrix[position.x, position.y].SetTile(tile);
            Tile displayTile = tile switch
            {
                0 => water,
                1 => desert,
                2 => plain,
                _ => error,
            };
            int temp = position.x;
            position.x = position.y;
            position.y = temp;
            terrain.SetTile(position, displayTile);
        }

        public void SetValue(Vector3Int position, string name, float value)
        {
            tileMatrix[position.x,position.y].SetValue(name,value);
        }
        
        public int GetTile(Vector3Int position)
        {
            return tileMatrix[position.x, position.y].GetTile();
        }
        
        public float GetValue(Vector3Int position, string name)
        {
            return tileMatrix[position.x, position.y].GetValue(name);
        }
    }
}

