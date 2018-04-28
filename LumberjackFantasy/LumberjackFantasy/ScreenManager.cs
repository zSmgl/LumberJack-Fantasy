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
		private KeyboardState previousKB;               // Holds the previous keyboard state (if needed)
		private bool instructionsOn;                    // Holds whether or not instructions is being displayed
		private Vector2 topLeftCenter;					//Holds the position of instruction text

        public ScreenManager(Texture2D startButton, Texture2D exitButton, Texture2D instructButton, Texture2D ssBG, Texture2D overlay, Texture2D continueButton, 
			Texture2D quit, Texture2D startHover, Texture2D exitHover, Texture2D instructHover, Texture2D continueHover, Texture2D quitHover, Texture2D isBG)
        {
			instructionsOn = false;
			topLeftCenter = new Vector2(98, 0);
            LoadMenus(startButton, exitButton, instructButton, ssBG, overlay, continueButton, quit, startHover, exitHover, instructHover, continueHover, quitHover, isBG);
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
			menu.InstructHover = IsHovering(menu.InstructButton);
			if (instructionsOn)
			{
				menu.InstructHover = IsHovering(menu.InstructButton2);
			}
			if (singleLeftClick())
            {
                if (menu.StartHover)
                {
					if (!instructionsOn)
					{
						toReturn = GameState.loadLevel;
					}
                }
				else if (menu.InstructHover)
				{
					if (instructionsOn)
					{
						instructionsOn = false;
					}
					else
					{
						instructionsOn = true;
					}
				}
                else if (menu.ExitHover)
                {
					if (!instructionsOn)
					{
						toReturn = GameState.exit;
					}
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

			if (singleLeftClick())
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

        // ---------------------------------------------------------------------------- Menus Logic -----------------------------------------------------------------------
        //loadmenu command to create the screenmanager to be called in loadContent, method is in its test state as not all textures are created
        public void LoadMenus(Texture2D startButton, Texture2D exitButton, Texture2D instructButton, Texture2D ssBG, Texture2D overlay, 
			Texture2D continueButton, Texture2D quit, Texture2D startHover, Texture2D exitHover, Texture2D instructHover, Texture2D continueHover, Texture2D quitHover, Texture2D isBG)
        {
            menu = new ScreenPosManager(startButton, exitButton, instructButton, ssBG, overlay, continueButton, quit, startHover, exitHover, 
				instructHover, continueHover, quitHover, isBG);
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
        public void DrawTitleScreen(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
           
			if (!instructionsOn)
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

				if (menu.InstructHover)
				{
					menu.DrawInstructHover(spriteBatch);
				}
				else
				{
					menu.DrawInstructButton(spriteBatch);
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
			else
			{
                menu.DrawInstructionScreen(spriteBatch);
                spriteBatch.DrawString(spriteFont, "Welcome To LumberJack Fantasy!\n" +
					"WASD Keys To Move\n" +
					"Arrow Keys To Attack\n\n" +
					"Collect the Required Number \n" +
					"Of Logs Per Level\n" +
					"Cut Down Trees and Collect Syrup\n" +
					"to Gain points\n" +
					"Killing bears deducts points,\n" +
					"unless it is Open Season\n\n" +
					"Collect Apples To regain Health\n" +
					"Grab a Shotgun for Open Season\n" +
					"and Shoot Away!", topLeftCenter, Color.White);
				if (menu.InstructHover)
				{
					menu.DrawInstructHover2(spriteBatch);
				}
				else
				{
					menu.DrawInstructButton2(spriteBatch);
				}
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

		public bool singleLeftClick() //Prevents InfiniLoops.
		{
			if (currentMS.LeftButton == ButtonState.Pressed)
			{
				if (previousMS.LeftButton == ButtonState.Pressed)
				{
					return false;
				}
				return true;
			}
			return false;
		}
	}
}
