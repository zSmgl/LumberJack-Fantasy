using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace LumberjackFantasy
{
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


		//random object removed in favor of random selection of tiles in the tile class
		public WorldTile(int upLeft, int upRight, int dwnLeft, int dwnRight)
		{
			upperLeft = upLeft;
			upperRight = upRight;
			lowerLeft = dwnLeft;
			lowerRight = dwnRight;
		

			upperLeftRect = new Rectangle(0, 0, 896, 896);
			upperRightRect = new Rectangle(896, 0, 896, 896);
			lowerLeftRect = new Rectangle(0, 896, 896, 896);
			lowerRightRect = new Rectangle(896, 896, 896, 896);
		}



		//draws the tiles, possibly will be changed in order to allow free scrolling

		//Tiles are 896x896.
		//Bobby Will Remember This
		public void Draw(SpriteBatch spriteBatch, Texture2D[] textures)
		{
			//UL
			spriteBatch.Draw(getTexture(Quadrent.UL, 9999, textures), upperLeftRect, Color.White);
			//UR
			spriteBatch.Draw(getTexture(Quadrent.UR, 9999, textures), upperRightRect, Color.White);
			//BL
			spriteBatch.Draw(getTexture(Quadrent.BL, 9999, textures), lowerLeftRect, Color.White);
			//BR
			spriteBatch.Draw(getTexture(Quadrent.BR, 9999, textures), lowerRightRect, Color.White);

		}

		//returns the texture for each "quadrent"
		public Texture2D getTexture(Quadrent quadrent, int selection, Texture2D[] textures)
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








	}
}
