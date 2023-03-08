using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


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
                case "rabbit":
                    return 0;
                case "lynx":
                    return 1;
                case "fox":
                    return 2;
                case "temperature":
                    return 3;
                case "isothermality":
                    return 4;
                case "summerTemperature":
                    return 5;
                case "rain":
                    return 6;
                case "rainVariation":
                    return 7;
                case "summerRain":
                    return 8;
                case "useful":
                    return 9;
                default:
                    Debug.Log("Invalid characteristic name");
                    return -1;
            }
        }

        public TileData()
        {
            this.tile = 0;
            this.characteristics = new float[10];
            this.characteristics[9] = 0;
        }

        public void SetTile(int value)
        {
            this.tile = value;
        }

        public void SetValue(string name, float value)      //set the "name" characteristics to value
        {
            this.characteristics[StringToIndex(name)] = value;
        }

        public void ChangeValue(string name, float change)      //does +change on the "name" characteristics
        {
            this.characteristics[StringToIndex(name)] += change;
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
        public static Tile species100 = (Tile)Resources.Load("species100");
        public static Tile species090 = (Tile)Resources.Load("species090");
        public static Tile species080 = (Tile)Resources.Load("species080");
        public static Tile species070 = (Tile)Resources.Load("species070");
        public static Tile species060 = (Tile)Resources.Load("species060");
        public static Tile species050 = (Tile)Resources.Load("species050");
        public static Tile speciesBis100 = (Tile)Resources.Load("speciesBis100");
        public static Tile speciesBis090 = (Tile)Resources.Load("speciesBis090");
        public static Tile speciesBis080 = (Tile)Resources.Load("speciesBis080");
        public static Tile speciesBis070 = (Tile)Resources.Load("speciesBis070");
        public static Tile speciesBis060 = (Tile)Resources.Load("speciesBis060");
        public static Tile speciesBis050 = (Tile)Resources.Load("speciesBis050");
        public static Tile speciesTer100 = (Tile)Resources.Load("speciesTer100");
        public static Tile speciesTer090 = (Tile)Resources.Load("speciesTer090");
        public static Tile speciesTer080 = (Tile)Resources.Load("speciesTer080");
        public static Tile speciesTer070 = (Tile)Resources.Load("speciesTer070");
        public static Tile speciesTer060 = (Tile)Resources.Load("speciesTer060");
        public static Tile speciesTer050 = (Tile)Resources.Load("speciesTer050");
        public static Tile lynxTile = (Tile)Resources.Load("lynx");


        private TileData[,] tileMatrix;
        private int width;
        private int height;
        private Tilemap terrain;
        private Tilemap rabbit;
        private Tilemap fox;
        private Tilemap lynx;
        private Tilemap biomass;

        public TilemapData(int x_width, int y_width, Tilemap terrain, Tilemap rabbit, Tilemap lynx, Tilemap fox, Tilemap biomass)
        {
            this.tileMatrix = new TileData[x_width, y_width];
            this.width = x_width;
            this.height = y_width;
            this.terrain = terrain;
            this.rabbit = rabbit;
            this.lynx = lynx;
            this.fox = fox;
            this.biomass = biomass;
            for (int x = 0; x < x_width; x++)
            {
                for (int y = 0; y < y_width; y++)
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
            int temp = position.x;                      //inversion entre x/y backend et x/y affiché
            position.x = position.y;
            position.y = temp;
            terrain.SetTile(position, displayTile);
        }

        public void SetValue(Vector3Int position, string name, float value)
        {
            tileMatrix[position.x, position.y].SetValue(name, value);
            Vector3Int invertedP = new Vector3Int(position.y, position.x, 0);
            switch (name)
            {
                case "rabbit":
                    switch (value)      //updates the rabbit map display
                    {
                        case float n when n > 1000:
                            rabbit.SetTile(invertedP, species100);
                            break;
                        case float n when n > 500:
                            rabbit.SetTile(invertedP, species090);
                            break;
                        case float n when n > 200:
                            rabbit.SetTile(invertedP, species080);
                            break;
                        case float n when n > 100:
                            rabbit.SetTile(invertedP, species070);
                            break;
                        case float n when n > 20:
                            rabbit.SetTile(invertedP, species060);
                            break;
                        case float n when n > 0:
                            rabbit.SetTile(invertedP, species050);
                            break;
                        default:
                            rabbit.SetTile(invertedP, null);
                            break;
                    }
                    break;
                case "lynx":
                    switch (value)      //updates the lynx map display
                    {
                        /*
                        case float n when n > 20:
                            lynx.SetTile(invertedP, speciesTer100);
                            break;
                        case float n when n > 15:
                            lynx.SetTile(invertedP, speciesTer090);
                            break;
                        case float n when n > 10:
                            lynx.SetTile(invertedP, speciesTer080);
                            break;
                        case float n when n > 5:
                            lynx.SetTile(invertedP, speciesTer070);
                            break;
                        case float n when n > 2:
                            lynx.SetTile(invertedP, speciesTer060);
                            break;
                        */
                        case float n when n > 0:
                            lynx.SetTile(invertedP, lynxTile);
                            break;
                        default:
                            lynx.SetTile(invertedP, null);
                            break;
                    }
                    break;
                case "fox":
                    switch (value)      //updates the fox map display
                    {
                        case float n when n > 1000:
                            fox.SetTile(invertedP, speciesBis100);
                            break;
                        case float n when n > 500:
                            fox.SetTile(invertedP, speciesBis090);
                            break;
                        case float n when n > 200:
                            fox.SetTile(invertedP, speciesBis080);
                            break;
                        case float n when n > 100:
                            fox.SetTile(invertedP, speciesBis070);
                            break;
                        case float n when n > 20:
                            fox.SetTile(invertedP, speciesBis060);
                            break;
                        case float n when n > 0:
                            fox.SetTile(invertedP, speciesBis050);
                            break;
                        default:
                            fox.SetTile(invertedP, null);
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        public void ChangeValue(Vector3Int position, string name, float change)
        {
            tileMatrix[position.x, position.y].ChangeValue(name, change);
            Vector3Int invertedP = new Vector3Int(position.y, position.x, 0);
            switch (name)
            {
                case "rabbit":
                    switch (tileMatrix[position.x, position.y].GetValue(name))      //updates the rabbit map display
                    {
                        case float n when n > 1000:
                            rabbit.SetTile(invertedP, species100);
                            break;
                        case float n when n > 500:
                            rabbit.SetTile(invertedP, species090);
                            break;
                        case float n when n > 200:
                            rabbit.SetTile(invertedP, species080);
                            break;
                        case float n when n > 100:
                            rabbit.SetTile(invertedP, species070);
                            break;
                        case float n when n > 20:
                            rabbit.SetTile(invertedP, species060);
                            break;
                        case float n when Math.Round(n) > 0:
                            rabbit.SetTile(invertedP, species050);
                            break;
                        default:
                            rabbit.SetTile(invertedP, null);
                            break;
                    }
                    break;
                case "lynx":
                    switch (tileMatrix[position.x, position.y].GetValue(name))      //updates the lynx map display
                    {
                        /*
                        case float n when n > 20:
                            lynx.SetTile(invertedP, speciesTer100);
                            break;
                        case float n when n > 15:
                            lynx.SetTile(invertedP, speciesTer090);
                            break;
                        case float n when n > 10:
                            lynx.SetTile(invertedP, speciesTer080);
                            break;
                        case float n when n > 5:
                            lynx.SetTile(invertedP, speciesTer070);
                            break;
                        case float n when n > 2:
                            lynx.SetTile(invertedP, speciesTer060);
                            break;
                        */
                        case float n when n > 0:
                            lynx.SetTile(invertedP, lynxTile);
                            break;
                        default:
                            lynx.SetTile(invertedP, null);
                            break;
                    }
                    break;
                case "fox":
                    switch (tileMatrix[position.x, position.y].GetValue(name))      //updates the fox map display
                    {
                        case float n when n > 1000:
                            fox.SetTile(invertedP, speciesBis100);
                            break;
                        case float n when n > 500:
                            fox.SetTile(invertedP, speciesBis090);
                            break;
                        case float n when n > 200:
                            fox.SetTile(invertedP, speciesBis080);
                            break;
                        case float n when n > 100:
                            fox.SetTile(invertedP, speciesBis070);
                            break;
                        case float n when n > 20:
                            fox.SetTile(invertedP, speciesBis060);
                            break;
                        case float n when n > 0:
                            fox.SetTile(invertedP, speciesBis050);
                            break;
                        default:
                            fox.SetTile(invertedP, null);
                            break;
                    }
                    break;
                default:
                    break;
            }
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