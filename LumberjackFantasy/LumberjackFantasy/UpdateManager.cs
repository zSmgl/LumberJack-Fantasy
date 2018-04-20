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
	class UpdateManager
	{
        // Basically Everyone has modified this at one point to assure we dont break each other's code. 
        // hello

		private Random rng;                             // Random Number Generator used for bears and bear speeds.
		private GameTime gameTime;                      // Holds the current GameTime
		private Player pCurrent;                        // Holds the player's values
		private List<Bear> bearsCurrent;                // Holds all of the bears in the game
		private List<Tree> treesCurrent;                // Holds all of the treesInTheGame
        private List<PickUp> pickUpsCurrent;            // Holds all the pickups in the game
		private KeyboardState currentKB;                // Holds the current Kb State
		private KeyboardState previousKB;               // Holds the previous Kb State (if needed)
		private PathManager pM;                         // Manager that determines pathing for the bears. 
		private Rectangle[] healthCords;                // Holds location of the helath hearts.
		private Vector2 hsCord;							// Holds location of the high score
        private OpenSeasonManager oS;                   // Manages what occurs if the open season state occurs
        private bool runOpenSeason;                     // Determines if OpenSeason needs to be set, updated, or return to default state
        public Camera camera;                           // Holds the cameras positions
        private int currentLevel;                       // Holds the current Level (-1) (Makes sense in context of # of trees to cut this level)
        private int gameMaxLevel;                           // Max Amount of Levels in the game (use in # trees to cut this level logic)
        private int totalTreesToCut;                    // Total Trees to cut this level! (Determined based on Trees Currnent, current level, and max level!)
        private List<Texture2D> uiTextures;             // Holds the textures used in the UI


		VelocityManager velocityManager = new VelocityManager(0);
        CollisionManager collisionManager;

		// Properties
		public List<Bear> BearsCurrent
		{
			get { return bearsCurrent; }
			set { bearsCurrent = value; }
		}
		public List<Tree> TreesCurrent
		{
			get { return treesCurrent; }
			set { treesCurrent = value; }
		}
		public List<PickUp> PickUpsCurrent
		{
			get { return pickUpsCurrent; }
			set { pickUpsCurrent = value; }
		}



		/// <summary>
		/// Constructor - Leave Blank. Update Manager should recieve data based on it's data retrieving methods 
		/// </summary>
		public UpdateManager(int screenWidthMax, int screenHeightMax, Texture2D camera, int gameMaxLevel, List<Texture2D> textures)
		{
            uiTextures = textures;
            collisionManager = new CollisionManager(screenWidthMax, screenHeightMax);
			pM = new PathManager();
			rng = new Random();
            this.camera = new Camera(10, camera); //instantiate the camera, 10 is a placeholder value
			healthCords = new Rectangle[5] {new Rectangle(178,16, 23, 18), new Rectangle(156,16,23,18), new Rectangle(133, 16,23,18), new Rectangle(111, 16, 23, 18), new Rectangle(88, 16, 23, 18) };
			hsCord = new Vector2(777, 14);
            oS = new OpenSeasonManager();
            runOpenSeason = false;
            this.gameMaxLevel = gameMaxLevel;
		}


        #region uM Universal Stuff
        // --------------------------------------------------------------------- Universal Updates and Draws for  Screen State ------------------------------------------------------

        /// <summary>
        /// Update method called when the game is actually being played
        /// </summary>
        public GameState UpdateGameScreen()
		{
			GameState toreturn = GameState.gameLoop;


           
            if(runOpenSeason == true)
            {
                DetermineOpenSeason();
            }

			UpdatePlayer();
            UpdateCamera();
            UpdateAllBears();
            UpdatePlayerBearInteraction();
			RemoveStuffFromStoredLists();


			if (pCurrent.Health <= 0)
			{
				toreturn = GameState.gameOver;
			}
            if(totalTreesToCut <= 0)
            {
                toreturn = GameState.loadLevel;
                if(gameMaxLevel == (currentLevel + 1))
                {
                    toreturn = GameState.gameOver;
                }
            }
			if (currentKB.IsKeyDown(Keys.P))
			{
				toreturn = GameState.pause;
			}
			return toreturn;
		}

		///<summary>
		///Draw method called during the gameLoop
		///</summary>
		public void DrawGame(SpriteBatch spriteBatch, SpriteFont spriteFont)
		{
			//draw player
			pCurrent.Draw(spriteBatch, camera.CameraPosition.Location.ToVector2());

			//draws all trees on screen
			if (treesCurrent != null)
			{
				foreach (Tree thisObject in treesCurrent)
				{
					if (thisObject.OnScreen)
					{
						thisObject.Draw(spriteBatch, camera.CameraPosition.Location.ToVector2());
					}
				}
			}
			//draw all bears on screen
			if (bearsCurrent != null)
			{
				foreach (Bear thisObject in bearsCurrent)
				{
					if (thisObject.OnScreen)
					{
						thisObject.Draw(spriteBatch, camera.CameraPosition.Location.ToVector2());
					}
				}
			}
			//draw all pickups on screen
			if (pickUpsCurrent != null)
			{
				foreach (PickUp thisObject in pickUpsCurrent)
				{
					if (thisObject.OnScreen)
					{
						thisObject.Draw(spriteBatch, camera.CameraPosition.Location.ToVector2());
					}
				}
			}

			//draws score
			spriteBatch.Draw(uiTextures[7], new Vector2(680, 18), Color.White);

			//draws highscore
			spriteBatch.DrawString(spriteFont, pCurrent.TotalScore.ToString(), hsCord, Color.White);

            //draws HP
            spriteBatch.Draw(uiTextures[5], new Vector2(45, 18), Color.White);

			//draws log
			spriteBatch.Draw(uiTextures[4], new Vector2(768, 42), Color.White);

			//draws number of tocut left
			spriteBatch.DrawString(spriteFont, totalTreesToCut.ToString(), new Vector2(807, 37), Color.White);

			//draws health
			switch (pCurrent.Health)
			{
				case 5:
					spriteBatch.Draw(uiTextures[1], healthCords[0], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[1], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[2], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[3], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[4], Color.White);
					break;

				case 4:
					spriteBatch.Draw(uiTextures[0], healthCords[0], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[1], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[2], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[3], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[4], Color.White);
					break;

				case 3:
					spriteBatch.Draw(uiTextures[0], healthCords[0], Color.White);
					spriteBatch.Draw(uiTextures[0], healthCords[1], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[2], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[3], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[4], Color.White);
					break;

				case 2:
					spriteBatch.Draw(uiTextures[0], healthCords[0], Color.White);
					spriteBatch.Draw(uiTextures[0], healthCords[1], Color.White);
					spriteBatch.Draw(uiTextures[0], healthCords[2], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[3], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[4], Color.White);
					break;

				case 1:
					spriteBatch.Draw(uiTextures[0], healthCords[0], Color.White);
					spriteBatch.Draw(uiTextures[0], healthCords[1], Color.White);
					spriteBatch.Draw(uiTextures[0], healthCords[2], Color.White);
					spriteBatch.Draw(uiTextures[0], healthCords[3], Color.White);
					spriteBatch.Draw(uiTextures[1], healthCords[4], Color.White);
					break;
			}

			//draws weapon text
			spriteBatch.Draw(uiTextures[8], new Vector2(246, 18), Color.White);

			//draws the correct weapon
			if (pCurrent.Attack == AttackVariation.axe)
			{
				spriteBatch.Draw(uiTextures[2], new Vector2(349, 9), Color.White);
			}
			else
			{
				spriteBatch.Draw(uiTextures[3], new Vector2(352, 20), Color.White);
			}

			//draws openSeason
			if (runOpenSeason == true)
			{
				spriteBatch.Draw(uiTextures[6], new Vector2(465, 18), Color.White);
			}

		}

        #endregion uM Universal Stuff
        #region Add & Remove Stuff from UM Lists

        // ----------------------------------------------------------------------- Add / Remove Stuff from Lists ---------------------------------------------------------

        /// <summary>
        /// Set's update Manager up for the next level
        /// </summary>
        /// <param name="p"></param>
        /// <param name="trees"></param>
        /// <param name="bears"></param>
        /// <param name="pickUps"></param>
        public void NextLevel(Player p, List<Tree> trees, List<Bear> bears, List<PickUp> pickUps, int currentLevel)
        {
            pCurrent = p;
            treesCurrent = trees;
            bearsCurrent = bears;
			pickUpsCurrent = pickUps;
            this.currentLevel = currentLevel;
            totalTreesToCut = (treesCurrent.Count / (gameMaxLevel - (currentLevel)));     // Current Trees in level / (Max level - current Level)
                                                                                          // IE: Current trees = 28, Max Level = 5, current level = 0(+1) [As level 0 is really lvl 1]
                                                                                          // So Level 0 [1] has 7 trees total to cut before passing. 

            // Handles if Game was in Open Season when Level Ended

            if (runOpenSeason == true)
            {
                oS.ResetOpenSeasonTimer();
                pCurrent = oS.EndOpenSeasonPlayer(pCurrent);
                oS.BearMax = bearsCurrent[0].MaxSpeed;
                camera = oS.OpenSeasonCamera(camera);
            }

            runOpenSeason = false;

        }
		
        /// <summary>
		/// Sets the neccessary fields for when the game is in Update Game Screen
		/// </summary>
		/// <param name="p"> the player</param>
		/// <param name="bears"> the bears List</param>
		/// <param name="trees"> the trees List</param>
		/// <param name="pickUps"> the pickUps List</param>
		public void UpdateGameScreenFields(KeyboardState currentKB, KeyboardState previousKB, GameTime gameTime)
		{
            this.currentKB = currentKB;
            this.previousKB = previousKB;
            this.gameTime = gameTime;
		}

        /// <summary>
        /// Returns the Player
        /// </summary>
        /// <returns></returns>
        public Player ReturnPlayer()
        {
            return pCurrent;
        }

		/// <summary>
		/// Determines stuff to remove from Lists (Tree, Bear, PickUp)
		/// </summary>
		public void RemoveStuffFromStoredLists()
		{
			//Removes all trees from the list that have less than or equal to 0 health
			for (int i = 0; i < treesCurrent.Count; i++)
			{
				if (treesCurrent[i].Health < 0 || treesCurrent[i].Health == 0)
				{
					treesCurrent.RemoveAt(i);
                    totalTreesToCut--; // Decreases total Trees to cut since the tree is now dead
                    pCurrent.TotalScore += 3;
                }
			}

			//Removes all bears from the list that have less than or equal to 0 health
			for (int i = 0; i < bearsCurrent.Count; i++)
			{
				if (bearsCurrent[i].Health < 0 || bearsCurrent[i].Health == 0)
				{
                    pCurrent.TotalScore += bearsCurrent[i].ScoreValue;
					bearsCurrent.RemoveAt(i);
				}
			}


			//Removes all the pickups from the list when item is retrieved
			for (int i = 0; i < pickUpsCurrent.Count; i++)
			{
				if (pickUpsCurrent[i].ItemState == ItemState.Retrieved)
				{
					pickUpsCurrent.RemoveAt(i);
				}
			}
		}

        #endregion Add & Remove Stuff from UM Lists
        #region Updating Player
        // ------------------------------------------------------------------------ Player Specific Methods ---------------------------------------------------------------


        /// <summary>
        /// Responsible for Updating all neccessary variables of the player
        /// </summary>
        public void UpdatePlayer()
		{

			// Creates a Player Velocity Manager
			velocityManager = new VelocityManager(pCurrent.MaxSpeed)
			{
				// Sets the current X and Y speed that should be in pl
				VelocityX = pCurrent.SpeedX,
				VelocityY = pCurrent.SpeedY
			};


			// 0 - Saves player's UnAdjusted State to be compared to through-out Update Player

			Player oldPos = new Player(pCurrent);

			// 1 - Finds Players "Un-Collided" Position with new Speed 

			UpdatePlayerMovement();

			// 2 - Check for Collisions with Trees in Game. Check for collision with side of screen. Adjust Speed and Pos Accordingly if needed.

			UpdatePlayerPosition(oldPos);

            // 3 - Determines if the Player should now Attack

            UpdatePlayerAttack(oldPos);

			// 4 - Updates the Animations of the player

			UpdatePlayerMovementAnimation(oldPos);

            // 5 - Update PickUps

            UpdatePickUps();           

            // temp code to check if player has pressing keys to move. runs after updating all the movement.
            // Do not delete this. may use in future.  FAIL-SAFE TO DEACCELERATION STUFF
            /*
            var keys = currentKB.GetPressedKeys();

          
            if (keys.Length == 0)
            {
                pCurrent.SpeedX = 0;
                pCurrent.SpeedY = 0;
            }
            */
            
            




        }

		/// <summary>
		/// Calculates the new position of the Player & Field of Vision, and updates the Players current Speed in X and Y Directions
		/// </summary>
		public void UpdatePlayerMovement()
		{
			// Adds Speed to the Velocity Manager Based on the Current Keys Pressed
			if (currentKB.IsKeyDown(Keys.W) == true)
			{
                if (velocityManager.VelocityY > 0)
                {
                    velocityManager.Decelerate(1);
                }
				velocityManager.AddVelocity(0, -1 * (pCurrent.MaxSpeed / 16));
			}
			if (currentKB.IsKeyDown(Keys.S) == true)
			{
                if (velocityManager.VelocityY < 0)
                {
                    velocityManager.Decelerate(1);
                }
                velocityManager.AddVelocity(0, (pCurrent.MaxSpeed / 16));
			}
            if (currentKB.IsKeyDown(Keys.S) == true && currentKB.IsKeyDown(Keys.W) == true)
            {
                velocityManager.VelocityY = 0;
            }
            if (currentKB.IsKeyDown(Keys.W) != true && currentKB.IsKeyDown(Keys.S) != true)
            {
                velocityManager.Decelerate(1);
            }
			if (currentKB.IsKeyDown(Keys.A) == true)
			{
                if (velocityManager.VelocityX > 0)
                {
                    velocityManager.Decelerate(0);
                }
                velocityManager.AddVelocity(-1 * (pCurrent.MaxSpeed / 16), 0);
			}
			if (currentKB.IsKeyDown(Keys.D) == true)
			{
				velocityManager.AddVelocity((pCurrent.MaxSpeed / 16), 0);
                if (velocityManager.VelocityX < 0)
                {
                    velocityManager.Decelerate(0);
                }
            }
            if (currentKB.IsKeyDown(Keys.A) == true && currentKB.IsKeyDown(Keys.D) == true)
            {
                velocityManager.VelocityY = 0;
            }
            if (currentKB.IsKeyDown(Keys.A) != true && currentKB.IsKeyDown(Keys.D) != true)
            {
                velocityManager.Decelerate(0);
            }

            // Sets the new Sprite Location & Player Field of Vision
            pCurrent.ObjectCollisionBox = velocityManager.UpdatePosition(pCurrent.ObjectCollisionBox);
			pCurrent.PlayerVision = velocityManager.UpdatePosition(pCurrent.PlayerVision);

			// Updates the Current Speed of the Player within the Player from the Calculated speed in Players VM
			pCurrent.SpeedX = velocityManager.VelocityX;
			pCurrent.SpeedY = velocityManager.VelocityY;
		}

        public void UpdatePlayerAttack(Player oldPos)
        {
            // If the player was not attacking previously or was attacking but hit the 4th frame
            if ((oldPos.IsAttacking == true && oldPos.AttackAnimationF >= 4) || (oldPos.IsAttacking == false))
            {
                // Will Attack Up
                if (currentKB.IsKeyDown(Keys.Up) == true)
                {
                    pCurrent.IsAttacking = true; // Player is now attacking!
                    pCurrent.AttackAnimationF = 1; // Player must draw the attacking first frame
                    pCurrent.AttackDirection = PlayerAttackDirection.up; // Gives a direction of the hitbox and animation
                }
                // Will Attack Up
                else if (currentKB.IsKeyDown(Keys.Left) == true)
                {
                    pCurrent.IsAttacking = true; // Player is now attacking!
                    pCurrent.AttackAnimationF = 1; // Player must draw the attacking first frame
                    pCurrent.AttackDirection = PlayerAttackDirection.left; // Gives a direction of the hit box and animation
                }
                // Will Attack Down
                else if (currentKB.IsKeyDown(Keys.Down) == true)
                {
                    pCurrent.IsAttacking = true; // Player is now attacking!
                    pCurrent.AttackAnimationF = 1; // Player must draw the attacking first frame 
                    pCurrent.AttackDirection = PlayerAttackDirection.down; // Gives a direction of the hit box and animation
                }
                // Will AttackRight
                else if (currentKB.IsKeyDown(Keys.Right) == true)
                {
                    pCurrent.IsAttacking = true; // Player is now attacking!
                    pCurrent.AttackAnimationF = 1; // Player must draw the attacking first frame 
                    pCurrent.AttackDirection = PlayerAttackDirection.right; // Gives a direction of the hit box and animation
                }
                // If player was attacking and is now [what would be] a frame over his animation for attacking,
                // make him no longer attacking and he is now back to frame 1
                else if (oldPos.AttackAnimationF >= 9 && oldPos.IsAttacking == true)
                {
                    pCurrent.IsAttacking = false; // Player is no longer Attacking!!!!
                }
            }
        }

		/// <summary>
		/// Updates the Player's direction enum to be properly set. Used to determine what animation of Player should be drawn
		/// </summary>
		/// <param name="oldPos">The old player</param>
		public void UpdatePlayerMovementAnimation(Player oldPos)
		{

			// Updates Direction Animation 

			if (oldPos.PosX > pCurrent.PosX && oldPos.PosY == pCurrent.PosY)         // Player walking in Left Direction
			{
				pCurrent.PlayerDirection = PlayerDirection.left;
			}
			else if (oldPos.PosX < pCurrent.PosX && oldPos.PosY == pCurrent.PosY)   // Player walking in the Right Direction
			{
				pCurrent.PlayerDirection = PlayerDirection.right;
			}
			else if (oldPos.PosX == pCurrent.PosX && oldPos.PosY > pCurrent.PosY)   // Player walking Up Direction
			{
				pCurrent.PlayerDirection = PlayerDirection.up;
			}
			else if (oldPos.PosX == pCurrent.PosX && oldPos.PosY < pCurrent.PosY)   // Player walking Down Direction
			{
				pCurrent.PlayerDirection = PlayerDirection.down;
			}
			else if (oldPos.PosX > pCurrent.PosX && oldPos.PosY > pCurrent.PosY)   // Player is walking Up-Left
			{
				pCurrent.PlayerDirection = PlayerDirection.upleft;
			}
			else if (oldPos.PosX < pCurrent.PosX && oldPos.PosY > pCurrent.PosY)   // Player is walking Up-Right
			{
				pCurrent.PlayerDirection = PlayerDirection.upright;
			}
			else if (oldPos.PosX > pCurrent.PosX && oldPos.PosY < pCurrent.PosY)   // Player is walking Down-Left
			{
				pCurrent.PlayerDirection = PlayerDirection.downleft;
			}
			else if (oldPos.PosX < pCurrent.PosX && oldPos.PosY < pCurrent.PosY)   // Player is walking Down-Right
			{
				pCurrent.PlayerDirection = PlayerDirection.downright;
			}
			else                                                                    // Player Stood Still
			{
				pCurrent.PlayerDirection = oldPos.PlayerDirection;
			}
		}

		/// <summary>
		/// Adjusts Players Rectangle Fields and Pos if a collision with a tree has occured.
		/// </summary>
		/// <param name="oldPos"></param>
		public void UpdatePlayerPosition(Player oldPos)
		{

            // -- Collision With Trees --

			// @ pos 0 = X Value Adjust, @ pos 1 = Y Value Adjust

			int[] adjustPosValues = collisionManager.PosAdjust(pCurrent, oldPos, treesCurrent);

			// If any sort of adjustment value was found, then the object collided with something.
			// If no adjustment was found, then nothing needs to be offset and the speed doesnt need to be adjusted

			if (adjustPosValues[0] != 0 || adjustPosValues[1] != 0)
			{
                // Makes the player and trees collision no longer occur and sets all player rectangles equally offset
                pCurrent.ObjectCollisionBox = new Rectangle(pCurrent.PosX + adjustPosValues[0], pCurrent.PosY + adjustPosValues[1], pCurrent.Width, pCurrent.Height);
                pCurrent.PlayerVision = new Rectangle(pCurrent.PlayerVision.X + adjustPosValues[0], pCurrent.PlayerVision.Y + adjustPosValues[1], 
                    pCurrent.PlayerVision.Width, pCurrent.PlayerVision.Height);

                // Changes the speed to 0 in the direction of which a potential collision has now occured. 
                if (adjustPosValues[0] != 0)
				{
					pCurrent.SpeedX = 0;
				}
				if (adjustPosValues[1] != 0)
				{
					pCurrent.SpeedY = 0;
				}
			}

            // -- MAKE IT STAY ON SCREEN -- 

            // Changes adjustPosValues to fixed values of placement if tries to go off screen;
            adjustPosValues = collisionManager.StayOnScreen(pCurrent);

            // gets new Absolute X and Y cords 

            // Only Adjusts Cords if the object has touched the side of the screen

            // Four Cases: A ) X & Y == 0 (nothing changes, nothing needs to run)
            // B) X Changes, Y doesnt == X speed needs set to 0 and all rectangles need changed
            // C) Y changes, X doesnt == Y speed needs set to 0 and all rectangles need changed
            // D) IT ALL DAMN CHANGES (x && y both change, both speeds set to 0, both x and y cords of all rectangles need changed)

            if (adjustPosValues[0] != 0 || adjustPosValues[1] != 0)
            {
                
                // X value is changed and Y is not
                if (adjustPosValues[0] != 0 && adjustPosValues[1] == 0)
                {
                    // Position
                    pCurrent.ObjectCollisionBox = new Rectangle(adjustPosValues[0], pCurrent.PosY, pCurrent.Width, pCurrent.Height);
                }
                // Y value is changed and X is not
                else if (adjustPosValues[0] == 0 && adjustPosValues[1] != 0)
                {
                    // Position
                    pCurrent.ObjectCollisionBox = new Rectangle(pCurrent.PosX, adjustPosValues[1], pCurrent.Width, pCurrent.Height);
                }
                // Both X value and Y value have changed
                else if((adjustPosValues[0] != 0 && adjustPosValues[1] != 0))
                {
                    // Position
                    pCurrent.ObjectCollisionBox = new Rectangle(adjustPosValues[0], adjustPosValues[1], pCurrent.Width, pCurrent.Height);
                }

                // Player vision based on new Position
                pCurrent.PlayerVision = new Rectangle(pCurrent.ObjectCollisionBox.X - pCurrent.VisionStandard, pCurrent.ObjectCollisionBox.Y - pCurrent.VisionStandard,
                    pCurrent.PlayerVision.Width, pCurrent.PlayerVision.Height);

                // Changes X speed if it had to be modified along the X axis
                // Changes Y speed if it had to be modified along the Y axis.

                if (adjustPosValues[0] != 0)
                {
                    pCurrent.SpeedX = 0;
                }
                if (adjustPosValues[1] != 0)
                {
                    pCurrent.SpeedY = 0;
                }
            }

            
        }

		public int ReturnScore() //method that returns player score
		{
			return pCurrent.TotalScore;
		}


        #endregion Updating Player
        #region Updating Bear
        // -------------------------------------------------------------------------- Bear Specific Methods ------------------------------------------------------------


        /// <summary>
        /// Update Responsible for updating all parts of every bear for each bear.
        /// </summary>
        public void UpdateAllBears()
		{
			// Sets tile of which player is on
			pM.FindTarget(pCurrent);

			for (int i = 0; i < bearsCurrent.Count; i++)
			{
                // ONLY UPDATES BEARS THAT ARE NOW INTERSECTING WITH THE UPSREEN. THIS CUTS DOWN THE TOTAL AMOUNT OF UPDATING THAT NEEDS TO OCCUR
                // THIS ALSO MAKES IT SO WE ARNT CHECKING COLLISIONS FOR EVERY DAMN BEAR IN THE GAME. AMEN!
                if (bearsCurrent[i].UPScreen == true)
                {
                    // Creates a Bear Velocity Manager
                    velocityManager = new VelocityManager(bearsCurrent[i].MaxSpeed)
                    {
                        // Sets the current X and Y speed
                        VelocityX = bearsCurrent[i].SpeedX,
                        VelocityY = bearsCurrent[i].SpeedY
                    };


                    // 0 - Saves bears UnAdjusted State to be compared to through-out UpdateAllBears

                    Bear oldPos = new Bear(bearsCurrent[i]);

                    // 1 - Compares Old Bear to New Bear and Decideds New Bear's Type of Movement. - Increments Timers for Statationary / Looking Accordingly.

                    // FIRST - DECIDES TO INCREMENT TIMER IF UPDATED PLAYERVISION BOX DOESN'T INTERACT WITH BEAR FIELD OF VISION
                    // SECOND - DETERMINES THE NEW STATE OF THE BEAR
                    // THIRD - DECIDES ON THE MOVEMENT OF THE NEW BEAR AND TO RESET TIMERS BASED ON OLD AND NEW STATE
                    // TYPES OF MOVEMENT: FOLLOW()  - Determines the path from the bear to the player in the quickest way
                    //                    LOOKING() - Determines the random path which the bear will walk for the bear's timer
                    // OTHER FUNCTIONS:   Bear.ResetTimer() - Will reset all the timers within the bear. (Timer for how long to stay still, and how long it should walk)

                    BearMovement(oldPos, i);

                    // 2 - Check for Collisions with Trees in Game. Adjust Speed and Pos Accordingly if needed.

                    UpdateBearsPosition(oldPos, i);

                    // 3 - Updates the Animations of the bear

                    UpdateBearAnimation(oldPos, i);


                }
            }

		}

		/// <summary>
		/// Adjusts Bear's Rectangle Fields and Pos if a collision with a tree has occured.
		/// </summary>
		/// <param name="oldPos">The old bear</param>
		/// <param name="i">The # of bear from it's list</param>
		public void UpdateBearsPosition(Bear oldPos, int i)
		{

            // -- Collision With Trees --

            // @ pos 0 = X Value Adjust, @ pos 1 = Y Value Adjust

            int[] adjustPosValues = collisionManager.PosAdjust(bearsCurrent[i], oldPos, treesCurrent);

			// If any sort of adjustment value was found, then the object collided with something.
			// If no adjustment was found, then nothing needs to be offset and the speed doesnt need to be adjusted

			if (adjustPosValues[0] != 0 || adjustPosValues[1] != 0)
			{
                // Makes the bear and trees collision no longer occur and sets all bear rectangles equally offset
                bearsCurrent[i].ObjectCollisionBox = new Rectangle(bearsCurrent[i].PosX + adjustPosValues[0], bearsCurrent[i].PosY + adjustPosValues[1], bearsCurrent[i].Width, bearsCurrent[i].Height);

                bearsCurrent[i].BearVision = new Rectangle(bearsCurrent[i].BearVision.X + adjustPosValues[0], bearsCurrent[i].BearVision.Y + adjustPosValues[1], 
                    bearsCurrent[i].BearVision.Width, bearsCurrent[i].BearVision.Height);

                bearsCurrent[i].FieldOfAttack = new Rectangle(bearsCurrent[i].FieldOfAttack.X + adjustPosValues[0], bearsCurrent[i].FieldOfAttack.Y + adjustPosValues[1],
                    bearsCurrent[i].FieldOfAttack.Width, bearsCurrent[i].FieldOfAttack.Height);

                // Changes the speed to 0 in the X and/or Y direction of which a potential collision has now occured. 
                if (adjustPosValues[0] != 0)
				{
					bearsCurrent[i].SpeedX = 0;
				}
				if (adjustPosValues[1] != 0)
				{
					bearsCurrent[i].SpeedY = 0;
				}
			}

            // -- MAKE IT STAY ON SCREEN --

            // Changes adjustPosValues to fixed values of placement if tries to go off screen;
            adjustPosValues = collisionManager.StayOnScreen(bearsCurrent[i]);

            // gets new Absolute X and Y cords 

            // Only Adjusts Cords if the object has touched the side of the screen

            // Four Cases: A ) X & Y == 0 (nothing changes, nothing needs to run)
            // B) X Changes, Y doesnt == X speed needs set to 0 and all rectangles need changed
            // C) Y changes, X doesnt == Y speed needs set to 0 and all rectangles need changed
            // D) IT ALL DAMN CHANGES (x && y both change, both speeds set to 0, both x and y cords of all rectangles need changed)

            if (adjustPosValues[0] != 0 || adjustPosValues[1] != 0)
            {

                // X value is changed and Y is not
                if (adjustPosValues[0] != 0 && adjustPosValues[1] == 0)
                {

                    // Position
                    bearsCurrent[i].ObjectCollisionBox = new Rectangle(adjustPosValues[0], bearsCurrent[i].PosY, 
                        bearsCurrent[i].Width, bearsCurrent[i].Height);
                }
                // Y value is changed and X is not
                else if (adjustPosValues[0] == 0 && adjustPosValues[1] != 0)
                {
                    // Position
                    bearsCurrent[i].ObjectCollisionBox = new Rectangle(bearsCurrent[i].PosX, adjustPosValues[1],
                        bearsCurrent[i].Width, bearsCurrent[i].Height);
                }
                // Both X value and Y value have changed
                else if ((adjustPosValues[0] != 0 && adjustPosValues[1] != 0))
                {
                    // Position
                    bearsCurrent[i].ObjectCollisionBox = new Rectangle(adjustPosValues[0], adjustPosValues[1],
                        bearsCurrent[i].Width, bearsCurrent[i].Height);
                }

                // Vision Rectangle based on new Position
                bearsCurrent[i].BearVision = new Rectangle(bearsCurrent[i].ObjectCollisionBox.X - bearsCurrent[i].VisionStandard,
                    bearsCurrent[i].ObjectCollisionBox.Y - bearsCurrent[i].VisionStandard,
                    bearsCurrent[i].BearVision.Width, bearsCurrent[i].BearVision.Height);

                // Field of Attack based on new Position
                bearsCurrent[i].FieldOfAttack = new Rectangle(bearsCurrent[i].ObjectCollisionBox.X - bearsCurrent[i].FoAStandard,
                    bearsCurrent[i].ObjectCollisionBox.Y - bearsCurrent[i].FoAStandard,
                    bearsCurrent[i].FieldOfAttack.Width, bearsCurrent[i].FieldOfAttack.Height);

                // Changes X speed if it had to be modified along the X axis
                // Changes Y speed if it had to be modified along the Y axis.

                if (adjustPosValues[0] != 0)
                {
                    pCurrent.SpeedX = 0;
                }
                if (adjustPosValues[1] != 0)
                {
                    pCurrent.SpeedY = 0;
                }
            }


        }

		/// <summary>
		/// Updates the Bear's direction enum to be properly set. Used to determine what animation of Bear should be drawn
		/// </summary>
		/// <param name="oldPos">The old bear</param>
		/// <param name="i">The # of bear from it's list</param>
		public void UpdateBearAnimation(Bear oldPos, int i)
		{
			if (oldPos.PosX > bearsCurrent[i].PosX && oldPos.PosY == bearsCurrent[i].PosY)         // Bear walking in Left Direction
			{
				bearsCurrent[i].BearDirection = BearDirection.left;
			}
			else if (oldPos.PosX < bearsCurrent[i].PosX && oldPos.PosY == bearsCurrent[i].PosY)   // Bear walking in the Right Direction
			{
				bearsCurrent[i].BearDirection = BearDirection.right;
			}
			else if (oldPos.PosX == bearsCurrent[i].PosX && oldPos.PosY > bearsCurrent[i].PosY)   // Bear walking Up Direction
			{
				bearsCurrent[i].BearDirection = BearDirection.up;
			}
			else if (oldPos.PosX == bearsCurrent[i].PosX && oldPos.PosY < bearsCurrent[i].PosY)   // Bear walking Down Direction
			{
				bearsCurrent[i].BearDirection = BearDirection.down;
			}
			else if (oldPos.PosX > bearsCurrent[i].PosX && oldPos.PosY > bearsCurrent[i].PosY)   // Bear is walking Up-Left
			{
				bearsCurrent[i].BearDirection = BearDirection.upleft;
			}
			else if (oldPos.PosX < bearsCurrent[i].PosX && oldPos.PosY > bearsCurrent[i].PosY)   // Bear is walking Up-Right
			{
				bearsCurrent[i].BearDirection = BearDirection.upright;
			}
			else if (oldPos.PosX > bearsCurrent[i].PosX && oldPos.PosY < bearsCurrent[i].PosY)   // Bear is walking Down-Left
			{
				bearsCurrent[i].BearDirection = BearDirection.downleft;
			}
			else if (oldPos.PosX < bearsCurrent[i].PosX && oldPos.PosY < bearsCurrent[i].PosY)   // Bear is walking Down-Right
			{
				bearsCurrent[i].BearDirection = BearDirection.downright;
			}
			else                                                                    // Bear Stood Still
			{
				bearsCurrent[i].BearDirection = oldPos.BearDirection;
			}
		}

		/// <summary>
		/// Determines how the bear is currently looking (wandering) around in a random way
		/// </summary>
		public void Looking(int i)
		{
			// Formula: Adds speed to the velocity manager thats is BETWEEN! -1/2 of it's potential max speed and 1/2 is potential max speed
			// in both the x and y direction of the bear, randomly creating movement for the bear that could be super fast, or super slow.
			// Tip: Changing the (/2) is the easiest way to adjust this value.
			velocityManager.AddVelocity(rng.Next(-1 * bearsCurrent[i].MaxSpeed, bearsCurrent[i].MaxSpeed) / 6, rng.Next(-1 * bearsCurrent[i].MaxSpeed, bearsCurrent[i].MaxSpeed) / 6);

			// Sets the new Sprite Location & Player Field of Vision
			bearsCurrent[i].ObjectCollisionBox = velocityManager.UpdatePosition(bearsCurrent[i].ObjectCollisionBox);
			bearsCurrent[i].BearVision = velocityManager.UpdatePosition(bearsCurrent[i].BearVision);
			bearsCurrent[i].FieldOfAttack = velocityManager.UpdatePosition(bearsCurrent[i].FieldOfAttack);

			// Updates the Current Speed of the Player within the Player from the Calculated speed in Players VM
			bearsCurrent[i].SpeedX = velocityManager.VelocityX;
			bearsCurrent[i].SpeedY = velocityManager.VelocityY;
		}
		/// <summary>
		/// Used for the bear to follow the player
		/// </summary>
		public void FollowPlayer(int i)
		{
            pM.ResetForNewBear();
            pM.FindCurrent(bearsCurrent[i]);
            pM.Following();
            bearsCurrent[i] = pM.GetDirection(bearsCurrent[i]);

            switch (bearsCurrent[i].BearDirection)
            {
                case BearDirection.down:
                    velocityManager.AddVelocity(0, bearsCurrent[i].MaxSpeed / 6);
                    break;
                case BearDirection.up:
                    velocityManager.AddVelocity(0, -1 * bearsCurrent[i].MaxSpeed / 6);
                    break;
                case BearDirection.left:
                    velocityManager.AddVelocity(-1 * bearsCurrent[i].MaxSpeed / 6, 0);
                    break;
                case BearDirection.right:
                    velocityManager.AddVelocity(bearsCurrent[i].MaxSpeed / 6, 0);
                    break;
                case BearDirection.upleft:
                    velocityManager.AddVelocity(-1 * bearsCurrent[i].MaxSpeed / 6, -1 * bearsCurrent[i].MaxSpeed / 6);
                    break;
                case BearDirection.upright:
                    velocityManager.AddVelocity(bearsCurrent[i].MaxSpeed / 6, -1 * bearsCurrent[i].MaxSpeed / 6);
                    break;
                case BearDirection.downleft:
                    velocityManager.AddVelocity(-1 * bearsCurrent[i].MaxSpeed / 6, bearsCurrent[i].MaxSpeed / 6);
                    break;
                case BearDirection.downright:
                    velocityManager.AddVelocity(bearsCurrent[i].MaxSpeed / 6, bearsCurrent[i].MaxSpeed / 6);
                    break;
            }

            bearsCurrent[i].ObjectCollisionBox = velocityManager.UpdatePosition(bearsCurrent[i].ObjectCollisionBox);
            bearsCurrent[i].BearVision = velocityManager.UpdatePosition(bearsCurrent[i].BearVision);
            bearsCurrent[i].FieldOfAttack = velocityManager.UpdatePosition(bearsCurrent[i].FieldOfAttack);

            // Updates the Current Speed of the Player within the Player from the Calculated speed in Players VM
            bearsCurrent[i].SpeedX = velocityManager.VelocityX;
            bearsCurrent[i].SpeedY = velocityManager.VelocityY;
        }

        /// <summary>
        /// Determines the new State of Movement and how the bear should Move.
        /// </summary>
        /// <param name="oldBear"></param>
        /// <param name="i"></param>
		public void BearMovement(Bear oldBear, int i)
		{

			//Determines the bear's state
			if (pCurrent.PlayerVision.Intersects(bearsCurrent[i].BearVision) == true)
			{
				bearsCurrent[i].BearState = BearState.following;
			}
			else
			{
				if (oldBear.BearState == BearState.stationary)
				{
					bearsCurrent[i].WhenToMoveCounter += gameTime.ElapsedGameTime.TotalSeconds;
				}
				else if (oldBear.BearState == BearState.looking)
				{
					bearsCurrent[i].TimeOfMovementCounter += gameTime.ElapsedGameTime.TotalSeconds;
				}


				if (oldBear.BearState == BearState.stationary && bearsCurrent[i].WhenToMoveCounter >= bearsCurrent[i].WhenToMoveLimiter)
				{
					bearsCurrent[i].BearState = BearState.looking;
				}
				else if (oldBear.BearState == BearState.stationary && bearsCurrent[i].WhenToMoveCounter < bearsCurrent[i].WhenToMoveLimiter)
				{
					bearsCurrent[i].BearState = BearState.stationary;
				}
				else if (oldBear.BearState == BearState.looking && bearsCurrent[i].TimeOfMovementCounter >= bearsCurrent[i].TimeOfMovementLimiter)
				{
					bearsCurrent[i].BearState = BearState.stationary;
				}
				else if (oldBear.BearState == BearState.looking && bearsCurrent[i].TimeOfMovementCounter < bearsCurrent[i].TimeOfMovementLimiter)
				{
					bearsCurrent[i].BearState = BearState.looking;
				}
				else if (oldBear.BearState == BearState.following)
				{
					bearsCurrent[i].BearState = BearState.stationary;
				}



			}


			if (oldBear.BearState == BearState.stationary && bearsCurrent[i].BearState == BearState.looking)
			{
				Looking(i);
			}
			else if (oldBear.BearState == BearState.looking && bearsCurrent[i].BearState == BearState.looking)
			{
				Looking(i);
			}
			else if (oldBear.BearState == BearState.looking && bearsCurrent[i].BearState == BearState.stationary)
			{
				// Sets speed of bear to 0 since movement has now reset. 
				bearsCurrent[i].SpeedX = 0;
				bearsCurrent[i].SpeedY = 0;

				bearsCurrent[i].ResetBearTimers(rng);
			}
			else if (oldBear.BearState == BearState.following && bearsCurrent[i].BearState == BearState.following)
			{
				FollowPlayer(i);
			}
			else if (oldBear.BearState == BearState.stationary && bearsCurrent[i].BearState == BearState.following)
			{
				// Sets speed of velocity manager back to 
				bearsCurrent[i].ResetBearTimers(rng);
				FollowPlayer(i);
			}
			else if (oldBear.BearState == BearState.looking && bearsCurrent[i].BearState == BearState.following)
			{
				bearsCurrent[i].ResetBearTimers(rng);
				FollowPlayer(i);
			}



		}

        #endregion Updating Bear
        #region Updating Player & Bear Interactions

        // ----------------------------------------------------------------- PLAYER / BEAR INTERACTION --------------------------------------------------------------------

        /// <summary>
        /// Player runs into bear & attacking
        /// </summary>
        public void UpdatePlayerBearInteraction()
        {
            WalkIntoBear();
            
            if(pCurrent.IsAttacking == true)
            {
                UpdatePlayerAttacks();
            }
            

            //UpdateBearAttacks()

        }

        /// <summary>
        /// Determines if Player should lose health if running into a bear or if player is invincible. 
        /// </summary>
        public void WalkIntoBear()
        {
            if (bearsCurrent != null && pCurrent.Invincible == false)
            {
                foreach (Bear b in bearsCurrent)
                {
                    if(b.UPScreen == true)
                    {
                        if (b.ObjectCollisionBox.Intersects(pCurrent.ObjectCollisionBox))
                        {
                            pCurrent.ResetTimer();
                            pCurrent.Invincible = true;
                            pCurrent.Health--;
                            break;                        }
                    }
                }
            }

            if(pCurrent.Invincible == true)
            {
                pCurrent.InvincibleTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (pCurrent.InvincibleTimer <= 0)
                {
                    pCurrent.Invincible = false;
                }
            }


        }

        // ---------------------------------------------------------------------------- Attack Stuff ----------------------------------------------------------------------

        /// Pass in 1 or 0 to determine bear or player
        /// If bear, pass a 0 for weapon type
        /// If plr, pass 0 for axe and 1 for shotgun
        /// </summary>
        /// <param name=""></param>
        public void UpdatePlayerAttacks()
        {
            
            Rectangle attackArea;

            if (pCurrent.Attack == AttackVariation.axe)
            {
                attackArea = new Rectangle(pCurrent.Location.X, pCurrent.Location.Y, 100, 100);
                // changes the direction of the box based on player orientation
                /*
				 *    < ^ ^
				 *    < * >
				 *    \/\/>
				 * (diagram of how the layout is setup)
				 */
                switch (pCurrent.AttackDirection)
                {
                    case PlayerAttackDirection.up:
                        attackArea.Y -= pCurrent.Height;
                        break;
                    case PlayerAttackDirection.down:
                        attackArea.Y += pCurrent.Height;
                        break;
                    case PlayerAttackDirection.left:
                        attackArea.X -= pCurrent.Width;
                        break;
                    case PlayerAttackDirection.right:
                        attackArea.X += pCurrent.Width;
                        break;
                }


                //calls collision on Bears
                bool[] bearHits = collisionManager.GenericAttack(attackArea, pCurrent, bearsCurrent);
                //checks and deals with the heath of the bears
                for (int i = 0; i < bearHits.Length; i++)
                {
                    if (bearHits[i])
                    {
                        bearsCurrent[i].Health--;
                    }
                }

                //calls collision on Trees
                bool[] treeHits = collisionManager.GenericAttack(attackArea, pCurrent, treesCurrent);
                //checks and deals with the health of trees.
                for (int i = 0; i < treeHits.Length; i++)
                {
                    if (treeHits[i])
                    {
                        treesCurrent[i].Health--;
                    }
                }

            }
            else if (pCurrent.Attack == AttackVariation.shotgun)
            {
                //sizes 4:3 - May need to Scale Down LOL
                attackArea = new Rectangle(pCurrent.Location.X, pCurrent.Location.Y, 100, 100);

                //expands the area based on direction, uses same layout as above
                switch (pCurrent.AttackDirection)
                {
                    case PlayerAttackDirection.up:
                        attackArea.Y -= (pCurrent.Height * 2);
                        attackArea.Height = 350;
                        break;
                    case PlayerAttackDirection.down:
                        attackArea.Y += pCurrent.Height;
                        attackArea.Height = 350;
                        break;
                    case PlayerAttackDirection.left:
                        attackArea.X -= (pCurrent.Width * 2);
                        attackArea.Width = 350;
                        break;
                    case PlayerAttackDirection.right:
                        attackArea.X += pCurrent.Width;
                        attackArea.Width = 350;
                        break;
                }

                //calls collision on Bears
                bool[] bearHits = collisionManager.GenericAttack(attackArea, pCurrent, bearsCurrent);
                //checks and deals with the heath of the bears
                for (int i = 0; i < bearHits.Length; i++)
                {
                    if (bearHits[i])
                    {
                        bearsCurrent[i].Health--;
                    }
                }

                //calls collision on Trees
                bool[] treeHits = collisionManager.GenericAttack(attackArea, pCurrent, treesCurrent);
                //checks and deals with the health of trees.
                for (int i = 0; i < treeHits.Length; i++)
                {
                    if (treeHits[i])
                    {
                        treesCurrent[i].Health--;
                    }
                }
            }

            // Increment the Attack Animation Frame since the player has attacked!
            pCurrent.AttackAnimationF++;
        }
        
        public void UpdateBearsAttack()
        {
            foreach (Bear b in bearsCurrent)
            {
                // If Bears field of attack intersect Player 
                if (b.FieldOfAttack.Intersects(pCurrent.ObjectCollisionBox))
                {
                    Rectangle attackArea = new Rectangle(b.Location.X, b.Location.Y, 100, 100); // May be able to change to foa standard. idk
                    // May need to reconfigure to state Bears pos relative to Players pos to  decide its area
                    switch (b.BearDirection)
                    {
                        case BearDirection.up:
                            attackArea.Y -= b.Height;
                            break;
                        case BearDirection.upleft:
                            attackArea.X -= b.Width;
                            break;
                        case BearDirection.upright:
                            attackArea.Y -= b.Height;
                            break;
                        case BearDirection.down:
                            attackArea.Y += b.Height;
                            break;
                        case BearDirection.downleft:
                            attackArea.Y += b.Height;
                            break;
                        case BearDirection.downright:
                            attackArea.X += b.Width;
                            break;
                        case BearDirection.left:
                            attackArea.X -= b.Width;
                            break;
                        case BearDirection.right:
                            attackArea.X += b.Width;
                            break;

                    }
                    // Calls for attack on the player
                    // May need to rewrite method for one bear, not the whole bear list  Attacking Here
                    bool[] playerHit = collisionManager.GenericAttack( attackArea, pCurrent, bearsCurrent);
                    if (playerHit[0])
                    {
                        pCurrent.Health--;
                    }
                }
            }
        }
        

            /*
            //bear attacks (fight me, puny human)
            else
            {
                //when the bear gets in a certain radius, it attacks where it is facing
                //if (bearbox colides with player, play attack)
                
                */

        #endregion Updating Player & Bear Interactions
        #region Updating Pickups & Open Season
        // -------------------------------------------------------------Pickup Specific Methods && O.S. ---------------------------------------------------------------------

        /// <summary>
        /// Loops through list of all pickups, if pickup collides checks type and handles code
        /// </summary>
        public void UpdatePickUps()
		{
				foreach (PickUp thisPickup in pickUpsCurrent)
				{
					if (thisPickup.ObjectCollisionBox.Intersects(pCurrent.ObjectCollisionBox))
					{
						thisPickup.ItemState = ItemState.Retrieved;
						switch (thisPickup.PickupType)
						{
							case PickupType.Apple:
								pCurrent.Health += 1;
								break;

							case PickupType.MapleSyrup:
								pCurrent.TotalScore += 45;
								break;

							case PickupType.Shotgun:
                            CheckOpenSeason();
								break;
						}
					}
				}
			
		}

        /// <summary>
        /// Determines if the game State should be set or not.
        /// </summary>
        public void CheckOpenSeason()
        {
            // If open season was not occuring before, it now needs to be set.
            if (runOpenSeason == false)
            {
                runOpenSeason = true;
                SetOpenSeason();
            }
            // If open season was occuring before, then just the timer needs to be reset since all the values have already been modified. 
            else 
            {
                oS.ResetOpenSeasonTimer();
            }
        }

        /// <summary>
        /// Set's Open Season values where needed
        /// </summary>
        public void SetOpenSeason()
        {
            bearsCurrent = oS.SetOpenSeasonList(bearsCurrent);
            pCurrent = oS.SetOpenSeasonPlayer(pCurrent);
            camera = oS.OpenSeasonCamera(camera);
        }

        /// <summary>
        /// Determines if Open Season should end or not.
        /// </summary>
        public void DetermineOpenSeason()
        {
            if (oS.OpenSeason() == true)
            {
                oS.UpdateTimer(gameTime);
            }
            else
            {
                EndOpenSeason();

            }
        }

        /// <summary>
        /// Resets Values of Open Season
        /// </summary>
        public void EndOpenSeason()
        {
            bearsCurrent = oS.EndOpenSeasonList(bearsCurrent);
            pCurrent = oS.EndOpenSeasonPlayer(pCurrent);
            camera = oS.OpenSeasonCamera(camera);
            runOpenSeason = false;
        }

        #endregion Updating Pickups & Open Season
        #region Updating Camera
        // -------------------------------------------------------------------------- Camera Logic ---------------------------------------------------------------------------

        /// <summary>
        /// Updates the Camera and all variables in the Camera
        /// </summary>
        public void UpdateCamera()
        {
            camera.UpdatePosition(pCurrent.ObjectCollisionBox);
			if (treesCurrent != null)
			{
				foreach (Tree thisTree in treesCurrent)
				{
					if (camera.IsDrawn(thisTree.ObjectCollisionBox)) //checks if needs to be drawn
					{
						thisTree.OnScreen = true;
					}
					else
					{
						thisTree.OnScreen = false;
					}

					if (camera.IsUpdating(thisTree.ObjectCollisionBox)) //checks if needs to be updated
					{
						thisTree.UPScreen = true;
					}
					else
					{
						thisTree.UPScreen = false;
					}
					
					if (camera.IsColliding(thisTree.ObjectCollisionBox)) //checks if things can collide with it
					{
						thisTree.Colliding = true;
					}
					else
					{
						thisTree.Colliding = false;
					}
				}
			}
			if (bearsCurrent != null)
			{
				foreach (Bear thisBear in bearsCurrent)
				{
					if (camera.IsDrawn(thisBear.ObjectCollisionBox)) //checks if needs to be drawn
					{
						thisBear.OnScreen = true;
					}
					else
					{
						thisBear.OnScreen = false;
					}

					if (camera.IsUpdating(thisBear.ObjectCollisionBox)) //checks if needs to be updated
					{
						thisBear.UPScreen = true;
					}
					else
					{
						thisBear.UPScreen = false;
					}
				}
			}
			if (pickUpsCurrent != null)
			{
				foreach (PickUp thisPickup in pickUpsCurrent)
				{
					if (camera.IsDrawn(thisPickup.ObjectCollisionBox)) //checks if needs to be drawn
					{
						thisPickup.OnScreen = true;
					}
					else
					{
						thisPickup.OnScreen = false;
					}

					if (camera.IsUpdating(thisPickup.ObjectCollisionBox)) //checks if needs to be updated
					{
						thisPickup.UPScreen = true;
					}
					else
					{
						thisPickup.UPScreen = false;
					}
				}
			}
			pM.UPScreen.ClearList();
			foreach(Location thisTile in pM.UPScreen.Tiles)
			{
				
				if (camera.IsUpdating(thisTile.Pos))
				{
					thisTile.ToUpdate = true;
					pM.UPScreen.UpdateTiles(thisTile);
				}
				else
				{
					thisTile.ToUpdate = false;
				}
			}
        }

        #endregion Updating Camera
    }
}

