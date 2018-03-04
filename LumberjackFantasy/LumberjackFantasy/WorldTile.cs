using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LumberjackFantasy
{
	//make enum for tile type
	enum TileType
	{

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


		//passes in a random object so fast generation of tiles will not be an issue
		public WorldTile(int upLeft, int upRight, int dwnLeft, int dwnRight, Random random)
		{
			upperLeft = upLeft;
			upperRight = upRight;
			lowerLeft = dwnLeft;
			lowerRight = dwnRight;
			rng = random;
		}



		//draws the tiles, possibly will be changed in order to allow free scrolling
		public void Draw(SpriteBatch spriteBatch)
		{
			//can't be implimented without tiles

			//spriteBatch.Draw()
		}







	}
}
