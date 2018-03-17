﻿using System;
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
        KeyboardState currentKB;             // Holds the current Kb State
        KeyboardState previousKB;            // Holds the previous Kb State (if needed)

        VelocityManager velocityManager = new VelocityManager(0);
        CollisionManager collisionManager = new CollisionManager();

        public UpdateManager()
        {

        }

        public void UpdateAll()
        {
            
            //UpdateStoredLists()
            //UpdatePlayer()
            //UpdateAllBears()
            //UpdateAttacks()
        }
        // Player Intended Methods and Helper Methods 

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
        /// <param name="oldPos"></param>
        public void UpdatePlayerAnimations(Player oldPos)
        {

            // Updates Direction Animation 

            if(oldPos.PosX > pCurrent.PosX && oldPos.PosY == pCurrent.PosY)         // Player walking in Left Direction
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
    }
}
