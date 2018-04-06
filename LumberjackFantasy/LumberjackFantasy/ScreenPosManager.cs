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
        private Texture2D health;
        private Texture2D hiScore;
        //private Texture2D gameScreenBackground;
        private Texture2D playerPortrait;
        private Texture2D gameOverScreenBackground;
        private Texture2D menuReturnButton;
        private Texture2D pauseScreenBackground;

        //Vector Fields
        private Vector2 ssBGPos;
        private Vector2 tPos;
        private Vector2 sbPos;
        private Vector2 ebPos;
        private Vector2 hPos;
        private Vector2 hsPos;
        //private Vector2 gsBGPos;
        private Vector2 ppPos;
        private Vector2 gosBGPos;
        private Vector2 mrbPos;
        private Vector2 psBGPos;

		//bool fields for whether the button is currently hovered
		private bool startHover;
		private bool exitHover;
		private bool returnHover;

        //property rectangles for button hitboxes
        public Rectangle StartButton { get { return new Rectangle((int)sbPos.X, (int)sbPos.Y, startButton.Width, startButton.Height); } }
        public Rectangle ExitButton { get { return new Rectangle((int)ebPos.X, (int)ebPos.Y, exitButton.Width, exitButton.Height); } }
        public Rectangle MenuReturnButton { get { return new Rectangle((int)mrbPos.X, (int)mrbPos.Y, menuReturnButton.Width, menuReturnButton.Height); } }

		//properties to get and set being hovered
		public bool StartHover { get { return startHover; } set { startHover = value; } }
		public bool ExitHover { get { return exitHover; } set { exitHover = value; } }

        //Constructor

        public ScreenPosManager(Texture2D start, Texture2D exit, Texture2D ssBG)
        {
            startScreenBackground = ssBG;
            ssBGPos.X = 0;
            ssBGPos.Y = 0;

            startButton = start;
			startHover = false;
            sbPos.X = 287;
            sbPos.Y = 300;

            exitButton = exit;
            ebPos.X = 287;
            ebPos.Y = 600;
			exitHover = false;
        }
        //REMINDER: Game screen background removed from parameters
		
        public ScreenPosManager(Texture2D ssBG, Texture2D t, Texture2D sb, Texture2D eb,
            Texture2D h, Texture2D hs, Texture2D pp,
            Texture2D gosBG, Texture2D mrb, Texture2D psBG)
        {
            //Vector Positions are filler for now
            //Start Screen
            startScreenBackground = ssBG;
            ssBGPos.X = 0;
            ssBGPos.Y = 0;

            //Title
            title = t;
            tPos.X = 0;
            tPos.Y = 0;

            //Start Button
            startButton = sb;
            sbPos.X = 0;
            sbPos.Y = 0;
			startHover = false;

            //Exit Button
            exitButton = eb;
            ebPos.X = 0;
            ebPos.Y = 0;
			exitHover = false;

            //Health
            health = h;
            hPos.X = 0;
            hPos.Y = 0;

            //HiScore
            hiScore = hs;
            hsPos.X = 0;
            hsPos.Y = 0;

            //Game Screen Background
            //gameScreenBackground = gsBG;
            //gsBGPos.X = 0;
            //gsBGPos.Y = 0;

            //Player Portrait
            playerPortrait = pp;
            ppPos.X = 0;
            ppPos.Y = 0;

            //Game Over Screen Background
            gameOverScreenBackground = gosBG;
            gosBGPos.X = 0;
            gosBGPos.Y = 0;

            //Menu Return Button
            menuReturnButton = mrb;
            mrbPos.X = 0;
            mrbPos.Y = 0;

            //Pause Screen Background
            pauseScreenBackground = psBG;
            psBGPos.X = 0;
            psBGPos.Y = 0;
        }
		
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
            spriteBatch.Draw(startButton, sbPos, Color.Firebrick);
        }

        public void DrawExitButton(SpriteBatch spriteBatch)
        {
            //draws the exit button
            spriteBatch.Draw(exitButton, ebPos, Color.White);
        }

        public void DrawExitHover(SpriteBatch spriteBatch)
        {
            //draws the hovered over exit button
            spriteBatch.Draw(exitButton, ebPos, Color.Firebrick);
        }

        public void DrawMenuReturnButton(SpriteBatch spriteBatch)
        {
            //draws the menu return button
            spriteBatch.Draw(menuReturnButton, mrbPos, Color.White);
        }

        public void DrawMenuReturnHover(SpriteBatch spriteBatch)
        {
            //draws the hovered over menu return button
            spriteBatch.Draw(menuReturnButton, mrbPos, Color.Firebrick);
        }

        public void DrawGameScreenElements(SpriteBatch spriteBatch)
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
    }
}
