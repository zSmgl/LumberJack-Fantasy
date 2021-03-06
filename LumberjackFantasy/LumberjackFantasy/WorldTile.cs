﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections;

namespace LumberjackFantasy
{
	//test comment
	//make enum for tile type
	enum Quadrent
	{
		UL = 0,
		UR = 1,
		BL = 2,
		BR = 3
	}
	class WorldTile
	{
		//fields 
		//placeholders could be changed to enum if so chosen later
		private int upperLeft;
		private int upperRight;
		private int lowerLeft;
		private int lowerRight;
		private Random rng;
		private Rectangle upperLeftRect;
		private Rectangle upperRightRect;
		private Rectangle lowerLeftRect;
		private Rectangle lowerRightRect;
		private List<Tile> tiles;
        private List<Bear> worldBears;
        private List<Tree> worldTrees;
        private List<PickUp> worldPickups;
		private List<BackgroundTile> backgrounds;

		/// <summary>
		/// List of every bear in a level
		/// </summary>
		public List<Bear> WorldBears
        {
            get { return worldBears; }
        }
        /// <summary>
        /// List of every tree in a level
        /// </summary>
        public List<Tree> WorldTrees
        {
            get { return worldTrees; }
        }
        /// <summary>
        /// List of every pickup in a level
        /// </summary>
        public List<PickUp> WorldPickUps
        {
            get { return worldPickups; }
        }

        //that stuff that lets you access things
        public List<Tile> Tiles
		{
			get { return tiles; }
			set { tiles = value; }
		}

		// lets you access the backgrounds for each quad
		public List<BackgroundTile> Backgrounds
		{
			get { return backgrounds; }
			set { backgrounds = value; }
		}


		//random object removed in favor of random selection of tiles in the tile class
		public WorldTile(Texture2D treeTexture, Texture2D bearTexture, List<Texture2D> pickups, Texture2D background)
		{
			rng = new Random();
			tiles = new List<Tile>();
			worldBears = new List<Bear>();
			worldPickups = new List<PickUp>();
			worldTrees = new List<Tree>();
			backgrounds = new List<BackgroundTile>(); 
			upperLeft = rng.Next(0, 19);
			upperRight = rng.Next(0, 19);
			lowerLeft = rng.Next(0, 19);
			lowerRight = rng.Next(0, 19);


			upperLeftRect = new Rectangle(0, 0, 896, 896);
			upperRightRect = new Rectangle(896, 0, 896, 896);
			lowerLeftRect = new Rectangle(0, 896, 896, 896);
			lowerRightRect = new Rectangle(896, 896, 896, 896);

			tiles.Add(LoadTiles("tile_" + upperLeft +".txt", treeTexture, bearTexture, pickups, rng, Quadrent.UL));
			tiles.Add(LoadTiles("tile_" + upperRight + ".txt", treeTexture, bearTexture, pickups, rng, Quadrent.UR));
			tiles.Add(LoadTiles("tile_" + lowerLeft + ".txt", treeTexture, bearTexture, pickups, rng, Quadrent.BL));
			tiles.Add(LoadTiles("tile_" + lowerRight + ".txt", treeTexture, bearTexture, pickups, rng, Quadrent.BR));

			backgrounds.Add(new BackgroundTile(0, 0, 896, 896, background));
			backgrounds.Add(new BackgroundTile(896, 0, 896, 896, background));
			backgrounds.Add(new BackgroundTile(0, 896, 896, 896, background));
			backgrounds.Add(new BackgroundTile(896, 896, 896, 896, background));

			SetWorldTileLists();

		}

        public void SetWorldTileLists()
        {
            for(int i = 0; i < tiles.Count; i++)
            {
				if (tiles[i].BearsFromTile != null)
				{
					foreach (Bear b in tiles[i].BearsFromTile)
					{
						worldBears.Add(b);
					}
				}
				if (tiles[i].PickUpsFromTile != null)
				{
					foreach (PickUp p in tiles[i].PickUpsFromTile)
					{
						worldPickups.Add(p);
					}
				}
				if (tiles[i].TreesFromTile != null)
				{
					foreach (Tree t in tiles[i].TreesFromTile)
					{
						worldTrees.Add(t);
					}
				}
            }
        }


		public Tile LoadTiles(string toLoad, Texture2D treeTexture, Texture2D bearTexture, List<Texture2D> pickups, Random rng, Quadrent quadrent)
		{
			Tile tile = new Tile(toLoad, treeTexture, bearTexture, pickups, rng, quadrent);
			return tile;
		}



		//draws the tiles, possibly will be changed in order to allow free scrolling

		//Tiles are 896x896.
		//Bobby Will Remember This
		public void Draw(SpriteBatch spriteBatch, Texture2D textures)
		{

			//put this in if you need it back
			//GetTexture(Quadrent.UL, 9999, textures)
			//UL
			spriteBatch.Draw(textures, upperLeftRect, Color.White);
			//UR
			spriteBatch.Draw(textures, upperRightRect, Color.White);
			//BL
			spriteBatch.Draw(textures, lowerLeftRect, Color.White);
			//BR
			spriteBatch.Draw(textures, lowerRightRect, Color.White);

		}


		/*
		//returns the texture for each "quadrent"
		public Texture2D GetTexture(Quadrent quadrent, int selection, Texture2D[] textures)
		{
			Texture2D texture;
			//currently overridden to be test bois
			if (Quadrent.UL == quadrent)
			{
				if (selection == 9999)
				{
					texture = textures[0];
				}
				else { texture = textures[0]; }
			}
			else if (Quadrent.UR == quadrent)
			{
				if (selection == 9999)
				{
					texture = textures[1];
				}
				else { texture = textures[0]; }
			}
			else if (Quadrent.BL == quadrent)
			{
				if (selection == 9999)
				{
					texture = textures[2];
				}
				else { texture = textures[0]; }
			}
			else
			{
				if (selection == 9999)
				{
					texture = textures[3];
				}
				else { texture = textures[0]; }
			}
			return texture;
		}

	*/






	}
}
