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

		private Player pCurrent;             // Holds the player's values
		private List<Bear> bearsCurrent;     // Holds all of the bears in the game
		private List<Tree> treesCurrent;     // Holds all of the treesInTheGame
		private List<PickUp> pickUpsCurrent; // Holds all the pickups in the game
		private KeyboardState currentKB;     // Holds the current Kb State
		private KeyboardState previousKB;    // Holds the previous Kb State (if needed)

		VelocityManager velocityManager = new VelocityManager(0);
		CollisionManager collisionManager = new CollisionManager();

        /// <summary>
        /// Constructor - Leave Blank. Update Manager should recieve data based on it's data retrieving methods 
        /// </summary>
		public UpdateManager()
		{

		}

        // --------------------------------------------------------------------- Universal Updates for Screen State ------------------------------------------------------

        /// <summary>
        /// Update method called when the game is actually being played
        /// </summary>
		public void UpdateGameScreen()
		{

			//UpdateStoredLists()
			//UpdatePlayer()
			//UpdateAllBears()
			//UpdateAttacks()
		}

        /// <summary>
        /// Update method called when the game is at the Title Screen
        /// </summary>
        public void UpdateTitleScreen()
        {

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
        /// Updates all of the Lists (Tree, Bear, PickUp)
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

			Player oldPos = pCurrent;

			// 1 - Finds Players "Un-Collided" Position with new Speed 

			UpdatePlayerMovement();

			// 2 - Check for Collisions with Trees in Game. Adjust Speed and Pos Accordingly if needed.

			UpdatePlayerPosition(oldPos);

			// 3 - Updates the Animations of the player

			UpdatePlayerAnimations(oldPos);




		}

		/// <summary>
		/// Calculates the new position of the Player & Field of Vision, and updates the Players current Speed in X and Y Directions
		/// </summary>
		public void UpdatePlayerMovement()
		{
			// Adds Speed to the Velocity Manager Based on the Current Keys Pressed
			if (currentKB.IsKeyDown(Keys.W) == true)
			{
				velocityManager.addVelocity(0, -1 * (pCurrent.MaxSpeed / 4));
			}
			if (currentKB.IsKeyDown(Keys.S) == true)
			{
				velocityManager.addVelocity(0, (pCurrent.MaxSpeed / 4));
			}
			if (currentKB.IsKeyDown(Keys.A) == true)
			{
				velocityManager.addVelocity(-1 * (pCurrent.MaxSpeed / 4), 0);
			}
			if (currentKB.IsKeyDown(Keys.D) == true)
			{
				velocityManager.addVelocity((pCurrent.MaxSpeed / 4), 0);
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
				pCurrent.ObjectCollisionBox.Offset(adjustPosValues[0], adjustPosValues[1]);
				pCurrent.PlayerVision.Offset(adjustPosValues[0], adjustPosValues[1]);

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
			for(int i =0; i<bearsCurrent.Count;i++)
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

				// 1 - Finds Bears "Un-Collided" Position with new Speed 

				BearMovement();

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
                bearsCurrent[i].ObjectCollisionBox.Offset(adjustPosValues[0], adjustPosValues[1]);
                bearsCurrent[i].BearVision.Offset(adjustPosValues[0], adjustPosValues[1]);
                bearsCurrent[i].FieldOfAttack.Offset(adjustPosValues[0], adjustPosValues[1]);

                // Changes the speed to 0 in the direction of which a potential collision has now occured. 
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
            if(oldPos.PosX > pCurrent.PosX && oldPos.PosY == pCurrent.PosY)         // Bear walking in Left Direction
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
						attackArea.Y -= 96;
						break;
					case PlayerDirection.upleft:
						attackArea.X -= 96;
						break;
					case PlayerDirection.upright:
						attackArea.Y -= 96;
						break;
					case PlayerDirection.down:
						attackArea.Y += 96;
						break;
					case PlayerDirection.downleft:
						attackArea.Y += 96;
						break;
					case PlayerDirection.downright:
						attackArea.X += 96;
						break;
					case PlayerDirection.left:
						attackArea.X -= 96;
						break;
					case PlayerDirection.right:
						attackArea.X += 96;
						break;

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
						attackArea.Y -= (96*2);
						attackArea.Height = 350;
						break;
					case PlayerDirection.upleft:
						attackArea.X -= (96 * 2);
						attackArea.Width = 350;
						break;
					case PlayerDirection.upright:
						attackArea.Y -= (96 * 2);
						attackArea.Height = 350;
						break;
					case PlayerDirection.down:
						attackArea.Y += 96;
						attackArea.Height = 350;
						break;
					case PlayerDirection.downleft:
						attackArea.Y += 96;
						attackArea.Height = 350;
						break;
					case PlayerDirection.downright:
						attackArea.X += 96;
						attackArea.Width = 350;
						break;
					case PlayerDirection.left:
						attackArea.X -= (96*2);
						attackArea.Width = 350;
						break;
					case PlayerDirection.right:
						attackArea.X += 96;
						attackArea.Width = 350;
						break;

				}

			}
			//bear attacks (fight me, puny human)
			else
			{
				//should be a radius around the bear
				//not quite sure about how big I want it to be 
			}
		}

		
	}
}

