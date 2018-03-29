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
		enum GameState { start, pause, gameLoop, gameOver, exit}
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		List<Texture2D> testTiles;
		List<WorldTile> worldTile;

        Texture2D playerTexture;
        Texture2D starterBackground;
        Texture2D startButton;
		Texture2D exitButton;

        Player player1;
        UpdateManager updateManager;
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

			base.Initialize();
			gameState = GameState.start;
            
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
            //temporary test loadmenu, to keep code short once all textures are made will load into a list, then the loadmenu will call members of that list instead of directly loading
            
            //Starter Background Variables for Base Build
            starterBackground = Content.Load<Texture2D>("starterBackground");

            //Texture Loading
            //Player Creation
            playerTexture = Content.Load<Texture2D>("lumberjackFront");
            player1 = new Player(448, 448, 96, 96, playerTexture, 3, 7, 10);

            startButton = Content.Load<Texture2D>("startButton");
			exitButton = Content.Load<Texture2D>("exitButton");
			updateManager = new UpdateManager(startButton, exitButton);
			//LoadTile(); -- BROKEN WON'T FIX

			
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

			// TODO: Add your update logic here
			switch (gameState)
			{
				case GameState.start:
					this.IsMouseVisible = true;
					gameState = updateManager.UpdateTitleScreen();
					break;

				case GameState.pause:
					//show menu
					break;

				case GameState.gameLoop:
                    //does bears and movement etc
					//updateManager.UpdatePlayer();
					break;

				case GameState.gameOver:
					//kill u
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
					updateManager.DrawTitleScreen(spriteBatch);
					break;

				case GameState.pause:
					break;

				case GameState.gameLoop:
                    spriteBatch.Draw(starterBackground, new Rectangle(0, 0, 896, 896), Color.White);
                    player1.Draw(spriteBatch);
					break;

				case GameState.gameOver:
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

	}
}
