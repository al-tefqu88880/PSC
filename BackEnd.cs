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

        //different local characteristics 
        private float bear;
        private float lynx;
        private float vole;
        private float humidity;
        private float sunlight;
        private float biomass;

        public TileData()
        {
            this.tile = 0;
            this.bear = 0;
            this.lynx = 0;
            this.vole = 0;
            this.humidity = 0;
            this.sunlight = 0;
            this.biomass = 0;
            
        }

        public void SetTile(int value)
        {
            this.tile = value;
        }
        public void SetBear(float value)
        {
            this.bear = value;
        }
        public void SetLynx(float value)
        {
            this.lynx = value;
        }
        public void SetVole(float value)
        {
            this.vole = value;
        }
        public void SetHumidity(float value)
        {
            this.humidity = value;
        }
        public void SetSunlight(float value)
        {
            this.sunlight = value;
        }
        public void SetBiomass(float value)
        {
            this.biomass = value;
        }

        public int GetTile()
        {
            return this.tile;
        }
        public float GetHumidity()
        {
            return this.humidity;
        }
        public float GetBear()
        {
            return this.bear;
        }
        public float GetLynx()
        {
            return this.lynx;
        }
        public float GetVole()
        {
            return this.vole;
        }
        public float GetSunlight()
        {
            return this.sunlight;
        }
        public float GetBiomass()
        {
            return this.biomass;
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
        public void SetBear(Vector3Int position, float value)
        {
            tileMatrix[position.x, position.y].SetBear(value);
        }
        public void SetLynx(Vector3Int position, float value)
        {
            tileMatrix[position.x, position.y].SetLynx(value);
        }
        public void SetVole(Vector3Int position, float value)
        {
            tileMatrix[position.x, position.y].SetVole(value);
        }
        public void SetHumidity(Vector3Int position, float value)
        {
            tileMatrix[position.x, position.y].SetHumidity(value);
        }
        public void SetBiomass(Vector3Int position, float value)
        {
            tileMatrix[position.x, position.y].SetBiomass(value);
        }
        public void SetSunlight(Vector3Int position, float value)
        {
            tileMatrix[position.x, position.y].SetSunlight(value);
        }
        public int GetTile(Vector3Int position)
        {
            return tileMatrix[position.x, position.y].GetTile();
        }
        public float GetBear(Vector3Int position)
        {
            return tileMatrix[position.x, position.y].GetBear();
        }
        public float GetLynx(Vector3Int position)
        {
            return tileMatrix[position.x, position.y].GetLynx();
        }
        public float GetVole(Vector3Int position)
        {
            return tileMatrix[position.x, position.y].GetVole();
        }
        public float GetHumidity(Vector3Int position)
        {
            return tileMatrix[position.x, position.y].GetHumidity();
        }
        public float GetBiomass(Vector3Int position)
        {
            return tileMatrix[position.x, position.y].GetBiomass();
        }
        public float GetSunlight(Vector3Int position)
        {
            return tileMatrix[position.x, position.y].GetSunlight();
        }
    }
}

