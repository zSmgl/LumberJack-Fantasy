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
    class ScreenPosManager
    {
        //No difficulty button put in yet
        //Game Screen Background currently commented
        //out since tile generation draws the background

        //Fields
        //Texture Fields
        private Texture2D startScreenBackground;
        private Texture2D title;
        private Texture2D startButton;
        private Texture2D exitButton;
		private Texture2D instructionsButton;
		private Texture2D quitButton;
		private Texture2D menuReturnButton;
		private Texture2D startButtonH;
		private Texture2D exitButtonH;
		private Texture2D instructionsButtonH;
		private Texture2D quitButtonH;
		private Texture2D menuReturnButtonH;
		private Texture2D health; //fossil code
        private Texture2D hiScore; //fossil code
        //private Texture2D gameScreenBackground;
        private Texture2D playerPortrait; //fossil code
        private Texture2D gameOverScreenBackground; //fossil code

        private Texture2D pauseScreenBackground;

        //Vector Fields
        private Vector2 ssBGPos; //start screen bg position
        private Vector2 tPos; //title position
        private Vector2 sbPos; //start button position
        private Vector2 ebPos; //exit button position
		private Vector2 ibPos; //instructions button position
		private Vector2 ibPos2; //instructions button position for instructions menu
        private Vector2 hPos; //fossil code
        private Vector2 hsPos; //fossil code
        //private Vector2 gsBGPos;
        private Vector2 ppPos; //fossil code
        private Vector2 gosBGPos; //fossil code
        private Vector2 mrbPos; //return button position
        private Vector2 psBGPos; //pause screen background position
		private Vector2 quitButtonPos; //pause screen quick button position

		//bool fields for whether the button is currently hovered
		private bool startHover;
		private bool exitHover;
		private bool instructHover;
		private bool returnHover;
		private bool quitHover;

        //property rectangles for button hitboxes
        public Rectangle StartButton { get { return new Rectangle((int)sbPos.X, (int)sbPos.Y, startButton.Width, startButton.Height); } }
        public Rectangle ExitButton { get { return new Rectangle((int)ebPos.X, (int)ebPos.Y, exitButton.Width, exitButton.Height); } }
		public Rectangle InstructButton { get { return new Rectangle((int)ibPos.X, (int)ibPos.Y, instructionsButton.Width, instructionsButton.Height); } }
		public Rectangle InstructButton2 { get { return new Rectangle((int)ibPos2.X, (int)ibPos2.Y, instructionsButton.Width, instructionsButton.Height); } }
        public Rectangle MenuReturnButton { get { return new Rectangle((int)mrbPos.X, (int)mrbPos.Y, menuReturnButton.Width, menuReturnButton.Height); } }
		public Rectangle QuitButton { get { return new Rectangle((int)quitButtonPos.X, (int)quitButtonPos.Y, quitButton.Width, quitButton.Height); } }

		//properties to get and set being hovered
		public bool StartHover { get { return startHover; } set { startHover = value; } }
		public bool ExitHover { get { return exitHover; } set { exitHover = value; } }
		public bool InstructHover { get { return instructHover; } set { instructHover = value; } }
		public bool ReturnHover { get { return returnHover; } set { returnHover = value; } }
		public bool QuitHover { get { return quitHover; } set { quitHover = value; } }
		//Constructor

		public ScreenPosManager(Texture2D start, Texture2D exit, Texture2D instruct, Texture2D ssBG, Texture2D pauseBG, Texture2D returnButton, Texture2D quit,
			Texture2D startH , Texture2D exitH, Texture2D instructH, Texture2D returnH, Texture2D quitH)
        {
            startScreenBackground = ssBG;
            ssBGPos.X = 0;
            ssBGPos.Y = 0;

            startButton = start;
			startButtonH = startH;
			startHover = false;
            sbPos.X = 243;
            sbPos.Y = 300;

			instructionsButton = instruct;
			instructionsButtonH = instructH;
			instructHover = false;
			ibPos.X = 243;
			ibPos.Y = 450;
			ibPos2.X = 243;
			ibPos2.Y = 766;

            exitButton = exit;
			exitButtonH = exitH;
            ebPos.X = 243;
            ebPos.Y = 600;
			exitHover = false;

			pauseScreenBackground = pauseBG;
			psBGPos.X = 0;
			psBGPos.Y = 0;

			menuReturnButton = returnButton;
			menuReturnButtonH = returnH;
			mrbPos.X = 243;
			mrbPos.Y = 318;

			quitButton = quit;
			quitButtonH = quitH;
			quitButtonPos.X = 243;
			quitButtonPos.Y = 766;
        }
        //REMINDER: Game screen background removed from parameters
		
      
        //Methods
        public void DrawStartScreen(SpriteBatch spriteBatch)
        {
            //Calls the parameter spritebatch and then draws
            //the background, title, and buttons
            spriteBatch.Draw(startScreenBackground, ssBGPos, Color.White);
            //spriteBatch.Draw(title, tPos, Color.White);
            
        }

        public void DrawStartButton(SpriteBatch spriteBatch)
        {
            //draws the start button
            spriteBatch.Draw(startButton, sbPos, Color.White);
        }

        public void DrawStartHover(SpriteBatch spriteBatch)
        {
            //draws hovered over start button
            spriteBatch.Draw(startButtonH, sbPos, Color.White);
        }

        public void DrawExitButton(SpriteBatch spriteBatch)
        {
            //draws the exit button
            spriteBatch.Draw(exitButton, ebPos, Color.White);
        }

        public void DrawExitHover(SpriteBatch spriteBatch)
        {
            //draws the hovered over exit button
            spriteBatch.Draw(exitButtonH, ebPos, Color.White);
        }

        public void DrawMenuReturnButton(SpriteBatch spriteBatch)
        {
            //draws the menu return button
            spriteBatch.Draw(menuReturnButton, mrbPos, Color.White);
        }

        public void DrawMenuReturnHover(SpriteBatch spriteBatch)
        {
            //draws the hovered over menu return button
            spriteBatch.Draw(menuReturnButtonH, mrbPos, Color.White);
        }

		public void DrawQuitButton(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(quitButton, quitButtonPos, Color.White);
		}

		public void DrawQuitHover(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(quitButtonH, quitButtonPos, Color.White);
		}

		public void DrawPause(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(pauseScreenBackground, psBGPos, Color.White);
		}

		public void DrawInstructButton(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(instructionsButton, ibPos, Color.White);
		}

		public void DrawInstructHover(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(instructionsButtonH, ibPos, Color.White);
		}

		public void DrawInstructButton2(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(instructionsButton, ibPos2, Color.White);
		}

		public void DrawInstructHover2(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(instructionsButtonH, ibPos2, Color.White);
		}

		/*public void DrawGameScreenElements(SpriteBatch spriteBatch)
        {
            //Draws the in-game screen UI elements
            spriteBatch.Draw(health, hPos, Color.White);
            spriteBatch.Draw(hiScore, hsPos, Color.White);
            spriteBatch.Draw(playerPortrait, ppPos, Color.White);
        }
		

        public void DrawGameOverScreen(SpriteBatch spriteBatch)
        {
            //Draws GameOver Screen and buttons
            spriteBatch.Draw(gameOverScreenBackground, gosBGPos, Color.White);
            
            
        }

        public void DrawPauseScreen(SpriteBatch spriteBatch)
        {
            //Draw Pause Screen and Buttons
            spriteBatch.Draw(pauseScreenBackground, psBGPos, Color.White);
            
        }
		*/
	}
}
