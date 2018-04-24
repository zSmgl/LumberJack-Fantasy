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
	
	class AnimationManager
	{
		// Variables
		private int currentFrame;
		private double fps;
		private double secondsPerFrame;
		private double timeCounter;
		private SpriteBatch spriteBatch;

		// Constructor
		public AnimationManager(SpriteBatch sprite)
		{
			currentFrame = 1;
			fps = 30.0;
			secondsPerFrame = 1.0f / fps;
			timeCounter = 0;
			spriteBatch = sprite;
			
		}

		// Methods

		private bool UpdateAnimation(GameTime gameTime)
		{
			// Add to the time counter (need TOTALSECONDS here)
			timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

			// Has enough time gone by to actually flip frames?
			if (timeCounter >= secondsPerFrame)
			{
				// Remove one "frame" worth of time
				timeCounter -= secondsPerFrame; // tell what is asking that it may update the frame
				return true;
			}
			return false;
			 
		}


		private void Animate(SpriteBatch spriteBatch, Texture2D aniTexture, Vector2 vector2, int widthOfSingleSprite, SpriteEffects flip) //fossil code
		{
			spriteBatch.Draw(
				aniTexture,
				vector2,
				new Rectangle(widthOfSingleSprite * currentFrame, 0, widthOfSingleSprite, aniTexture.Height),
				Color.White,
				0.0f,
				Vector2.Zero,
				1.0f,
				flip,
				0.0f);
		}
	}
}
