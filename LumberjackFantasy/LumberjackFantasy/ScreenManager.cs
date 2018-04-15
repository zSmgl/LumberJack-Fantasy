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
    class ScreenManager
    {
        //The purpose of this class is to handle all the updates of each Screen State (other than the playing game.)
        // This means the Menu, Pause, and End Screen

        // Fields

        private ScreenPosManager menu; // holds the position of everything for each menu
        private MouseState currentMS;                   // Holds the current Mouse State
        private MouseState previousMS;                  // Holds the previous Mouse State (if needed)
		private KeyboardState currentKB;				// Holds the current keyboard State
		private KeyboardState previousKB;				// Holds the previous keyboard state (if needed)

        public ScreenManager(Texture2D startButton, Texture2D exitButton, Texture2D ssBG, Texture2D overlay, Texture2D continueButton, Texture2D quit, Texture2D startHover, Texture2D exitHover, Texture2D continueHover, Texture2D quitHover)
        {
            LoadMenus(startButton, exitButton, ssBG, overlay, continueButton, quit, startHover, exitHover, continueHover, quitHover);
        }

        // ------------------------------------------------------------------- UPDATE METHODS FOR GAMESCREENS ---------------------------------------------------------------------

        /// <summary>
		/// Update method called when the game is at the Title Screen
		/// </summary>
		public GameState UpdateTitleScreen()
        {
            currentMS = Mouse.GetState();
            GameState toReturn = GameState.start;

            menu.StartHover = IsHovering(menu.StartButton);
            menu.ExitHover = IsHovering(menu.ExitButton);
            if (currentMS.LeftButton == ButtonState.Pressed)
            {
                if (menu.StartHover)
                {
                    toReturn = GameState.loadLevel;
                }
                else if (menu.ExitHover)
                {
                    toReturn = GameState.exit;
                }
            }
            previousMS = currentMS;
            return toReturn;
        }

        /// <summary>
        /// Update Method called when the game is at a Pause Screen
        /// </summary>
        public GameState UpdatePauseScreen()
        {
			currentMS = Mouse.GetState();
			currentKB = Keyboard.GetState();
			GameState toReturn = GameState.pause;
			menu.ReturnHover = IsHovering(menu.MenuReturnButton);
			menu.QuitHover = IsHovering(menu.QuitButton);

			if (currentKB.IsKeyDown(Keys.C))
			{
				toReturn = GameState.gameLoop;
			}

			if (currentKB.IsKeyDown(Keys.Q))
			{
				toReturn = GameState.start;
			}

			if (currentMS.LeftButton == ButtonState.Pressed)
			{
				if (menu.ReturnHover)
				{
					toReturn = GameState.gameLoop;
				}
				else if (menu.QuitHover)
				{
					toReturn = GameState.start;
				}
			}

			previousKB = currentKB;
			previousMS = currentMS;
			return toReturn;
        }

        /// <summary>
        /// Update Method called when the game is at a Game Over Screen
        /// </summary>
        public void UpdateEndGameScreen()
        {

        }



        // ----------------------------------------------------------------------- UPDATE SCREEN !FIELDS! METHODS ---------------------------------------------------------
        public void UpdateTitleScreenFields()
        {

        }

        public void UpdatePauseScreenFields()
        {

        }

        public void UpdateEndGameScreenFields()
        {

        }
        // ---------------------------------------------------------------------------- Menus Logic -----------------------------------------------------------------------
        //loadmenu command to create the screenmanager to be called in loadContent, method is in its test state as not all textures are created
        public void LoadMenus(Texture2D startButton, Texture2D exitButton, Texture2D ssBG, Texture2D overlay, Texture2D continueButton, Texture2D quit, Texture2D startHover, Texture2D exitHover, Texture2D continueHover, Texture2D quitHover)
        {
            menu = new ScreenPosManager(startButton, exitButton, ssBG, overlay, continueButton, quit, startHover, exitHover, continueHover, quitHover);
        }

        //command to see if a button is being hovered over
        public bool IsHovering(Rectangle box)
        {
            if (box.Contains(currentMS.Position))
            {
                return true;
            }
            return false;
        }

        // ---------------------------------------------------------------------------- DRAW SCREENS METHODS ------------------------------------------------------------

        /// <summary>
        /// Draw method called when the game is at the Title Screen
        /// </summary>
        public void DrawTitleScreen(SpriteBatch spriteBatch)
        {
            menu.DrawStartScreen(spriteBatch);

            if (menu.StartHover)
            {
                menu.DrawStartHover(spriteBatch);
            }
            else
            {
                menu.DrawStartButton(spriteBatch);
            }

            if (menu.ExitHover)
            {
                menu.DrawExitHover(spriteBatch);
            }
            else
            {
                menu.DrawExitButton(spriteBatch);
            }
        }
		public void DrawPauseScreen(SpriteBatch spriteBatch)
		{
			menu.DrawPause(spriteBatch);
			if(menu.ReturnHover)
			{
				menu.DrawMenuReturnHover(spriteBatch);

			}
			else
			{
				menu.DrawMenuReturnButton(spriteBatch);
			}
			if (menu.QuitHover)
			{
				menu.DrawQuitHover(spriteBatch);
			}
			else
			{
				menu.DrawQuitButton(spriteBatch);
			}
		}

    }
}
