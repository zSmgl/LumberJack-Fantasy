using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace LumberjackFantasy
{
	/// <summary>
	/// Now Jacob is Here
	/// </summary>
	
		//gamestate enum to track the games current state
		enum GameState
		{
			start,
			pause,
			gameLoop,
			gameOver,
			exit
		}
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		List<Texture2D> testTiles;
		List<WorldTile> worldTile;

        Random rng;

		SpriteFont spriteFont;
        Texture2D playerTexture;
        Texture2D startScreenBackground;
        Texture2D starterBackground;
        Texture2D startButton;
		Texture2D exitButton;
		Texture2D camera;
		int frameskip = 0;
        KeyboardState kb = new KeyboardState();
        KeyboardState previousKbstate = new KeyboardState();

        Player player1;
        UpdateManager updateManager;
        ScreenManager screenManager;
		ScoreboardManager scoreBoardManager;
		GameState gameState;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			worldTile = new List<WorldTile>();
			graphics.PreferredBackBufferWidth = 896;
			graphics.PreferredBackBufferHeight = 896;
			graphics.ApplyChanges();
			scoreBoardManager = new ScoreboardManager();

			base.Initialize();
			gameState = GameState.start;
            
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			//loads spritefont
			spriteFont = Content.Load<SpriteFont>("spriteFont");
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
            //temporary test loadmenu, to keep code short once all textures are made will load into a list, then the loadmenu will call members of that list instead of directly loading
            
            //Starter Background Variables for Base Build
            starterBackground = Content.Load<Texture2D>("starterBackground");
            startScreenBackground = Content.Load<Texture2D>("startScreenBackground");
            testTiles = new List<Texture2D>();
            playerTexture = Content.Load<Texture2D>("lumberjackFront");
            //rng = new Random();
            //LoadTile();

            startButton = Content.Load<Texture2D>("startButton");
			exitButton = Content.Load<Texture2D>("exitButton");
			camera = Content.Load<Texture2D>("cam");

            // Managers 

			updateManager = new UpdateManager(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, camera);
            screenManager = new ScreenManager(startButton, exitButton, startScreenBackground);
            player1 = new Player(448, 448, 96, 96, playerTexture, 3, 17, 10);


        }

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

            previousKbstate = kb;
            kb = Keyboard.GetState();

            // TODO: Add your update logic here
            switch (gameState)
			{
				case GameState.start:
                    
					this.IsMouseVisible = true;					
					gameState = screenManager.UpdateTitleScreen();
					break;

				case GameState.pause:
					gameState = screenManager.UpdatePauseScreen();
					//show menu
					break;

				case GameState.gameLoop:
                    //does bears and movement etc
                    updateManager.UpdateGameScreenFields(kb, previousKbstate, gameTime);
                    updateManager.UpdatePlayer();
					updateManager.UpdateCamera();
                    //updateManager.UpdateAllBears();

					/* line of code that
					 gameState = updateManager.UpdateGameScreen
					 */
					 if (gameState == GameState.gameOver)
					 {
						scoreBoardManager.CurrentScore = updateManager.ReturnScore();
					 }
					break;

				case GameState.gameOver:
					//display score

					gameState = scoreBoardManager.UpdateGameover();
					if (gameState == GameState.start)
					{
						scoreBoardManager.CurrentScore = 0;
					}
					break;

				case GameState.exit:
					this.Exit();
					break;
			}
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			spriteBatch.Begin();
			switch (gameState)
			{
				case GameState.start:
					screenManager.DrawTitleScreen(spriteBatch);
					break;

				case GameState.pause:
					break;

				case GameState.gameLoop:
					if (frameskip == 0)
					{
						frameskip = 1;
					}
					else
					{
						spriteBatch.Draw(starterBackground, new Rectangle(0, 0, 896, 896), Color.White);
						updateManager.DrawGame(spriteBatch);
						updateManager.camera.DrawCam(spriteBatch);
						//updateManager.camera.DrawCam(spriteBatch);
					}
					break;

				case GameState.gameOver:
					scoreBoardManager.DrawGameOver(spriteBatch, spriteFont);
					break;
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}


		//Call only during textureload
		//can be extended for extra textures
		public void LoadTile()
		{
			testTiles.Add(Content.Load<Texture2D>("Tiles/bgP1"));
			testTiles.Add(Content.Load<Texture2D>("Tiles/bgP2"));
			testTiles.Add(Content.Load<Texture2D>("Tiles/bgP3"));
			testTiles.Add(Content.Load<Texture2D>("Tiles/bgP4"));
		}

        public void NewLevel()
        {
            
            // Add in correct lists into updateManager based on current level
            updateManager.BearsCurrent = worldTile[0].WorldBears;
            updateManager.TreesCurrent = worldTile[0].WorldTrees;
            updateManager.PickUpsCurrent = worldTile[0].WorldPickUps;

            // Reset other shit in updateManager if need be by calling um.NextLevel watever thais may consist of. 
            
        }

	}
}
