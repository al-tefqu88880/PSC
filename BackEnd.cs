using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//in TilemapData, need to implement the initialization from a .csv file


namespace BackEnd
{
    public class Species
    {
        public static string[] listOfSpecies = { "otter", "bear", "lynx", "vole" };
        private int amount;
        private string name;
        //data on how to update it

        public Species(string name)
        {
            this.amount = 0;
            this.name = name;
        }

        public void setAmount(int value)
        {
            this.amount = value;
        }

        public int getAmount()
        {
            return amount;
        }
    }
    public class TileData
    {
        private Tile tile;  //the displayed tile in the game
        private Species[] speciesArray;  //all of the species in the game

        //different local characteristics 
        private float humidity;
        private float sunlight;
        private float biomass;

        public TileData(Tile tile)
        {
            //create the instances of species on this tile
            Species[] species = new Species[Species.listOfSpecies.Length];
            for (int i=0; i<Species.listOfSpecies.Length; i++)
            {
                species[i] = new Species(Species.listOfSpecies[i]);
            }
            this.speciesArray = species;

            this.tile = tile;
            this.humidity = 0;
            this.sunlight = 0;
            this.biomass = 0;
        }

        public void setTile(Tile value)
        {
            this.tile = value;
        }
        public void setHumidity(float value)
        {
            this.humidity = value;
        }
        public void setSunlight(float value)
        {
            this.sunlight = value;
        }
        public void setBiomass(float value)
        {
            this.biomass = value;
        }

        public Tile getTile()
        {
            return this.tile;
        }
        public float getHumidity()
        {
            return this.humidity;
        }
        public float getSunlight()
        {
            return this.sunlight;
        }
        public float getBiomass()
        {
            return this.biomass;
        }

    }
    public class TilemapData
    {
        private TileData[,] tileMatrix;
        public TilemapData(int x_width,int y_width)
        {
            this.tileMatrix = new TileData[x_width, y_width];
        }

        public void setTile(Vector3Int position, Tile tile)
        {
            tileMatrix[position.x, position.y].setTile(tile);
        }
    }
}

