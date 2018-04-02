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
		private Random rng;                             // Random Number Generator used for bears and bear speeds.
		private ScreenManager menu;
		private GameTime gameTime;                      // Holds the current GameTime
		private Player pCurrent;                        // Holds the player's values
		private List<Bear> bearsCurrent;                // Holds all of the bears in the game
		private Dictionary<int, Bear> attackingBears;   // Simplified dictionary that holds attacking bears & their int in the list
														// Key = i && Value = Bear  So... attackingBears[3].BearProperty is = to the 3rd bear that should be in Current Bears but
														// is held in the dictionary. This will then need to be returned to the proper place in the bearsCurrent List.
		private List<Tree> treesCurrent;                // Holds all of the treesInTheGame
		private List<PickUp> pickUpsCurrent;            // Holds all the pickups in the game
		private KeyboardState currentKB;                // Holds the current Kb State
		private KeyboardState previousKB;               // Holds the previous Kb State (if needed)
		private MouseState currentMS;                   // Holds the current Mouse State
		private MouseState previousMS;                  // Holds the previous Mouse State (if needed)

		VelocityManager velocityManager = new VelocityManager(0);
		CollisionManager collisionManager = new CollisionManager();

		/// <summary>
		/// Constructor - Leave Blank. Update Manager should recieve data based on it's data retrieving methods 
		/// </summary>
		public UpdateManager(Texture2D start, Texture2D exit)
		{
			LoadMenus(start, exit);
			Random rng = new Random();
		}

		// --------------------------------------------------------------------- Universal Updates and Draws for Screen State ------------------------------------------------------

		/// <summary>
		/// Update method called when the game is actually being played
		/// </summary>
		public void UpdateGameScreen()
		{

			UpdatePlayer();
			UpdateAllBears();
			//UpdateAttacks();
			RemoveStuffFromStoredLists();

		}

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
					toReturn = GameState.gameLoop;
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

		/// <summary>
		/// Update Method called when the game is at a Pause Screen
		/// </summary>
		public void UpdatePauseScreen()
		{

		}

		/// <summary>
		/// Update Method called when the game is at a Game Over Screen
		/// </summary>
		public void UpdateEndGameScreen()
		{

		}

		// ----------------------------------------------------------------------- Add / Remove Stuff from Lists ---------------------------------------------------------

		/// <summary>
		/// Sets the neccessary fields for when the game is in Update Game Screen
		/// </summary>
		/// <param name="p"> the player</param>
		/// <param name="bears"> the bears List</param>
		/// <param name="trees"> the trees List</param>
		/// <param name="pickUps"> the pickUps List</param>
		public void UpdateGameScreenFields(Player p, List<Tree> treesCurrent, KeyboardState currentKB, KeyboardState previousKB)
		{
            this.treesCurrent = treesCurrent;
            this.currentKB = currentKB;
            this.previousKB = previousKB;
            pCurrent = p;
		}

		public void UpdateTitleScreenFields()
		{

		}

		public void UpdatePauseScreenFields()
		{

		}

		public void UpdateEndGameScreenFields()
		{

		}

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
				}
			}

			//Removes all bears from the list that have less than or equal to 0 health
			for (int i = 0; i < bearsCurrent.Count; i++)
			{
				if (bearsCurrent[i].Health < 0 || bearsCurrent[i].Health == 0)
				{
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

			// 2 - Check for Collisions with Trees in Game. Adjust Speed and Pos Accordingly if needed.

			UpdatePlayerPosition(oldPos);

			// 3 - Updates the Animations of the player

			UpdatePlayerAnimations(oldPos);

            // 4 - Update PickUps

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

		/// <summary>
		/// Updates the Player's direction enum to be properly set. Used to determine what animation of Player should be drawn
		/// </summary>
		/// <param name="oldPos">The old player</param>
		public void UpdatePlayerAnimations(Player oldPos)
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
		}


		// -------------------------------------------------------------------------- Bear Specific Methods ------------------------------------------------------------


		/// <summary>
		/// Update Responsible for updating all parts of every bear for each bear.
		/// </summary>
		public void UpdateAllBears()
		{
			for (int i = 0; i < bearsCurrent.Count; i++)
			{
				// Creates a Bear Velocity Manager
				velocityManager = new VelocityManager(bearsCurrent[i].MaxSpeed)
				{
					// Sets the current X and Y speed
					VelocityX = bearsCurrent[i].SpeedX,
					VelocityY = bearsCurrent[i].SpeedY
				};


				// 0 - Saves bears UnAdjusted State to be compared to through-out UpdateAllBears

				Bear oldPos = bearsCurrent[i];

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

		/// <summary>
		/// Adjusts Bear's Rectangle Fields and Pos if a collision with a tree has occured.
		/// </summary>
		/// <param name="oldPos">The old bear</param>
		/// <param name="i">The # of bear from it's list</param>
		public void UpdateBearsPosition(Bear oldPos, int i)
		{
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
		}

		/// <summary>
		/// Updates the Bear's direction enum to be properly set. Used to determine what animation of Bear should be drawn
		/// </summary>
		/// <param name="oldPos">The old bear</param>
		/// <param name="i">The # of bear from it's list</param>
		public void UpdateBearAnimation(Bear oldPos, int i)
		{
			if (oldPos.PosX > pCurrent.PosX && oldPos.PosY == pCurrent.PosY)         // Bear walking in Left Direction
			{
				bearsCurrent[i].BearDirection = BearDirection.left;
			}
			else if (oldPos.PosX < pCurrent.PosX && oldPos.PosY == pCurrent.PosY)   // Bear walking in the Right Direction
			{
				bearsCurrent[i].BearDirection = BearDirection.right;
			}
			else if (oldPos.PosX == pCurrent.PosX && oldPos.PosY > pCurrent.PosY)   // Bear walking Up Direction
			{
				bearsCurrent[i].BearDirection = BearDirection.up;
			}
			else if (oldPos.PosX == pCurrent.PosX && oldPos.PosY < pCurrent.PosY)   // Bear walking Down Direction
			{
				bearsCurrent[i].BearDirection = BearDirection.down;
			}
			else if (oldPos.PosX > pCurrent.PosX && oldPos.PosY > pCurrent.PosY)   // Bear is walking Up-Left
			{
				bearsCurrent[i].BearDirection = BearDirection.upleft;
			}
			else if (oldPos.PosX < pCurrent.PosX && oldPos.PosY > pCurrent.PosY)   // Bear is walking Up-Right
			{
				bearsCurrent[i].BearDirection = BearDirection.upright;
			}
			else if (oldPos.PosX > pCurrent.PosX && oldPos.PosY < pCurrent.PosY)   // Bear is walking Down-Left
			{
				bearsCurrent[i].BearDirection = BearDirection.downleft;
			}
			else if (oldPos.PosX < pCurrent.PosX && oldPos.PosY < pCurrent.PosY)   // Bear is walking Down-Right
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
			velocityManager.AddVelocity(rng.Next(-1 * bearsCurrent[i].MaxSpeed, bearsCurrent[i].MaxSpeed) / 2, rng.Next(-1 * bearsCurrent[i].MaxSpeed, bearsCurrent[i].MaxSpeed) / 2);

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
		public void FollowPlayer()
		{

		}

		public void BearMovement(Bear oldBear, int i)
		{

			//Determines the bear's state
			if (pCurrent.PlayerVision.Intersects(bearsCurrent[i].BearVision) == true)
			{
				bearsCurrent[i].BearState = BearState.following;
				// SINCE THE BEAR @ I IN THE LIST IS FOLLOWING THE PLAYER, THIS MEANS THE BEAR COULD POTENTIALLY ATTACK THE PLAYER. 
				attackingBears[i] = bearsCurrent[i];
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


				if (oldBear.BearState == BearState.stationary && bearsCurrent[i].WhenToMoveCounter == bearsCurrent[i].WhenToMoveLimiter)
				{
					bearsCurrent[i].BearState = BearState.looking;
				}
				else if (oldBear.BearState == BearState.stationary && bearsCurrent[i].WhenToMoveCounter != bearsCurrent[i].WhenToMoveLimiter)
				{
					bearsCurrent[i].BearState = BearState.stationary;
				}
				else if (oldBear.BearState == BearState.looking && bearsCurrent[i].TimeOfMovementCounter == bearsCurrent[i].TimeOfMovementCounter)
				{
					bearsCurrent[i].BearState = BearState.stationary;
				}
				else if (oldBear.BearState == BearState.looking && bearsCurrent[i].TimeOfMovementCounter != bearsCurrent[i].TimeOfMovementCounter)
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
				FollowPlayer();
			}
			else if (oldBear.BearState == BearState.stationary && bearsCurrent[i].BearState == BearState.following)
			{
				// Sets speed of velocity manager back to 
				bearsCurrent[i].ResetBearTimers(rng);
				FollowPlayer();
			}
			else if (oldBear.BearState == BearState.looking && bearsCurrent[i].BearState == BearState.following)
			{
				bearsCurrent[i].ResetBearTimers(rng);
				FollowPlayer();
			}



		}

		// ------------------------------------------------------------------- Pickup Specific Methods ---------------------------------------------------------------------

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
							pCurrent.LevelScore += 45;
							break;

						case PickupType.Shotgun:
							//put open season code here
							break;
					}
				}
			}
		}


		// ---------------------------------------------------------------------------- Attack Stuff ----------------------------------------------------------------------

		/// Pass in 1 or 0 to determine bear or player
		/// If bear, pass a 0 for weapon type
		/// If plr, pass 0 for axe and 1 for shotgun
		/// </summary>
		/// <param name=""></param>
		public void UpdateAttacks(AttackVariation attackType, Point location, PlayerDirection playerDirection)
		{
			Rectangle attackArea;

			if (attackType == AttackVariation.axe)
			{
				attackArea = new Rectangle(location.X, location.Y, 100, 100);
				// changes the direction of the box based on player orientation
				/*
				 *    < ^ ^
				 *    < * >
				 *    \/\/>
				 * (diagram of how the layout is setup)
				 */
				switch (playerDirection)
				{
					case PlayerDirection.up:
						attackArea.Y -= pCurrent.Height;
						break;
					case PlayerDirection.upleft:
						attackArea.X -= pCurrent.Width;
						break;
					case PlayerDirection.upright:
						attackArea.Y -= pCurrent.Height;
						break;
					case PlayerDirection.down:
						attackArea.Y += pCurrent.Height;
						break;
					case PlayerDirection.downleft:
						attackArea.Y += pCurrent.Height;
						break;
					case PlayerDirection.downright:
						attackArea.X += pCurrent.Width;
						break;
					case PlayerDirection.left:
						attackArea.X -= pCurrent.Width;
						break;
					case PlayerDirection.right:
						attackArea.X += pCurrent.Width;
						break;
				}


				//calls collision on Bears
				bool[] bearHits = collisionManager.GenericAttack(attackType, attackArea, pCurrent, bearsCurrent);
				//checks and deals with the heath of the bears
				for (int i = 0; i < bearHits.Length; i++)
				{
					if (bearHits[i])
					{
						bearsCurrent[i].Health--;
					}
				}

				//calls collision on Trees
				bool[] treeHits = collisionManager.GenericAttack(attackType, attackArea, pCurrent, treesCurrent);
				//checks and deals with the health of trees.
				for (int i = 0; i < treeHits.Length; i++)
				{
					if (treeHits[i])
					{
						treesCurrent[i].Health--;
					}
				}

			}
			else if (attackType == AttackVariation.shotgun)
			{
				//sizes 4:3
				attackArea = new Rectangle(location.X, location.Y, 100, 100);

				//expands the area based on direction, uses same layout as above
				switch (playerDirection)
				{
					case PlayerDirection.up:
						attackArea.Y -= (pCurrent.Height * 2);
						attackArea.Height = 350;
						break;
					case PlayerDirection.upleft:
						attackArea.X -= (pCurrent.Width * 2);
						attackArea.Width = 350;
						break;
					case PlayerDirection.upright:
						attackArea.Y -= (pCurrent.Height * 2);
						attackArea.Height = 350;
						break;
					case PlayerDirection.down:
						attackArea.Y += pCurrent.Height;
						attackArea.Height = 350;
						break;
					case PlayerDirection.downleft:
						attackArea.Y += pCurrent.Height;
						attackArea.Height = 350;
						break;
					case PlayerDirection.downright:
						attackArea.X += pCurrent.Width;
						attackArea.Width = 350;
						break;
					case PlayerDirection.left:
						attackArea.X -= (pCurrent.Width * 2);
						attackArea.Width = 350;
						break;
					case PlayerDirection.right:
						attackArea.X += pCurrent.Width;
						attackArea.Width = 350;
						break;
				}

				//calls collision on Bears
				bool[] bearHits = collisionManager.GenericAttack(attackType, attackArea, pCurrent, bearsCurrent);
				//checks and deals with the heath of the bears
				for (int i = 0; i < bearHits.Length; i++)
				{
					if (bearHits[i])
					{
						bearsCurrent[i].Health--;
					}
				}

				//calls collision on Trees
				bool[] treeHits = collisionManager.GenericAttack(attackType, attackArea, pCurrent, treesCurrent);
				//checks and deals with the health of trees.
				for (int i = 0; i < treeHits.Length; i++)
				{
					if (treeHits[i])
					{
						treesCurrent[i].Health--;
					}
				}


			}
			//bear attacks (fight me, puny human)
			else
			{
				//when the bear gets in a certain radius, it attacks where it is facing
				//if (bearbox colides with player, play attack)
				foreach (Bear b in bearsCurrent)
				{
					if (collisionManager.BearboxCollider(b, pCurrent))
					{
						attackArea = new Rectangle(location.X, location.Y, 100, 100);
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
						bool[] playerHit = collisionManager.GenericAttack(attackType, attackArea, pCurrent, bearsCurrent);
						if (playerHit[0])
						{
							pCurrent.Health--;
						}
					}
				}
			}
		}

		// ---------------------------------------------------------------------------- Menus Logic -----------------------------------------------------------------------
		//loadmenu command to create the screenmanager to be called in loadContent, method is in its test state as not all textures are created
		public void LoadMenus(Texture2D startButton, Texture2D exitButton)
		{
			menu = new ScreenManager(startButton, exitButton);
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
	}
}

