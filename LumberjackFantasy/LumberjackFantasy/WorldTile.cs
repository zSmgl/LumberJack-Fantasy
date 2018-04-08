using System;
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
		private UpdateManager udManager;

		//that stuff that lets you access things
		public List<Tile> Tiles
		{
			get { return tiles; }
			set { tiles = value; }
		}


		//random object removed in favor of random selection of tiles in the tile class
		public WorldTile(Texture2D treeTexture, Texture2D bearTexture, List<Texture2D> pickups, UpdateManager updateManager)
		{
			rng = new Random();
			udManager = updateManager;

			upperLeft = rng.Next(0, 19);
			upperRight = rng.Next(0, 19);
			lowerLeft = rng.Next(0, 19);
			lowerRight = rng.Next(0, 19);


			upperLeftRect = new Rectangle(0, 0, 896, 896);
			upperRightRect = new Rectangle(896, 0, 896, 896);
			lowerLeftRect = new Rectangle(0, 896, 896, 896);
			lowerRightRect = new Rectangle(896, 896, 896, 896);

			tiles.Add(LoadTiles("tile_" + upperLeft, treeTexture, bearTexture, pickups, rng, Quadrent.UL));
			tiles.Add(LoadTiles("tile_" + upperRight, treeTexture, bearTexture, pickups, rng, Quadrent.UR));
			tiles.Add(LoadTiles("tile_" + lowerLeft, treeTexture, bearTexture, pickups, rng, Quadrent.BL));
			tiles.Add(LoadTiles("tile_" + lowerRight, treeTexture, bearTexture, pickups, rng, Quadrent.BR));

		}


		public Tile LoadTiles(string toLoad, Texture2D treeTexture, Texture2D bearTexture, List<Texture2D> pickups, Random rng, Quadrent quadrent)
		{
			Tile tile = new Tile(toLoad, treeTexture, bearTexture, pickups, rng, quadrent, udManager);
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
