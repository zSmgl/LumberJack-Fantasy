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
    enum BearState                      // Determines the state of which the the bear is in
    {
        looking,
        following
    }

    enum BearDirection
    {
        left,
        right,
        up,
        down
    }
    class Bear : LivingObject 
    {
        // Fields ----------------------------------------------------------------------

        private int scoreValue;                 // The amount of points that a bear is worth.

        private BearState bearState;            // Var that checks the state of the bear [Looking / Following]
        private BearDirection bearDirection;    // Var that checks that holds the direction the bear is facing. Useful for Animations

        private int visionStandard;             // the number that determines how far out the BearVision should be from the actual objectTexture.
        private Rectangle bearVision;           // The Bears field of vision that helps the bears determine what kind of state they are in.

        private int fieldOfAttackStandard;      // Determines how far the Field of Attack to trigger a bear swipping should be from the object   
        private Rectangle fieldOfAttack;        // Rectangle that determines if the bear should trigger an attack animation

        private int whenToMoveMax;      // Determines how long a bear should max wait before making a random movement in a direction.
        private int whenToMoveMin;      // Determines how long a bear should min wait before making a random movement in a direction.
        private double whenToMoveTimer; // Counter that updates with GameTime that tells the bear when it should move.
        private int whenToMove;         // Randomly Generated time between max and min that tells how long a bear waits before moving.

        private bool moveInLook;                // Allows the Bear to Move while in a looking State
        private int timeOfMovementMax;          // Determines how long a bear should max move in a random movement in a direction.
        private int timeOfMovementMin;          // Determines how long a bear should min move in a random movement in a direction.
        private double timeOfMovementCounter;   // Counter that updates with GameTime that tells the bear how long it should move.
        private int timeOfMovement;             // Randomly Generated time between max and min that tells how long a bear can move for.

        private Random rng;                     // Random object used to randomly update how long the bear's next when to move time is. 

        // Properties ------------------------------------------------------------------

       public BearState BearState
        {
            get { return BearState; }
            set { bearState = value; }
        }

       public Rectangle PlayerVision
       {
           get { return bearVision; }
       }





        public Bear(int x, int y, int width, int height, Texture2D objectTexture, int maxH, int maxS, 
            int visionStandard, int fieldOfAttackStandard, int scoreValue, Random rng)
            : base(x, y, width, height, objectTexture, maxH, maxS)
        {
            this.scoreValue = scoreValue * -1;                  // Sets the Score of the Bear to Negative since its in a looking state

            this.visionStandard = visionStandard;
            bearVision = new Rectangle(x - visionStandard, y - visionStandard, width + (visionStandard * 2), height + (visionStandard * 2));

            this.fieldOfAttackStandard = fieldOfAttackStandard;
            fieldOfAttack = new Rectangle(x - fieldOfAttackStandard, y - visionStandard, 
                width + (visionStandard * 2), height + (visionStandard * 2));

            this.rng = rng;

            whenToMoveTimer = 0;        
            whenToMoveMax = 15 + 1;                              // Bears Wait a max of 15 Seconds before making a movement;
            whenToMoveMin = 5;                                   // Bears Wait a min of 5 Seconds before making a movement;
            whenToMove = rng.Next(whenToMoveMin, whenToMoveMax); // Random int between 5-15 Seconds of when bear first can Move

            moveInLook = false;    

            timeOfMovementCounter = 0;
            timeOfMovementMax = 5 + 1;                                          // Bear can move a max 5 seconds in a direction;
            timeOfMovementMin = 1;                                              // Bear can move a min 1 seconds in a direction;
            timeOfMovement = rng.Next(timeOfMovementMin, timeOfMovementMax);    // Can Move in a random direction for 1-5 seconds

            bearDirection = BearDirection.down; // Bears spawn in looking down

        }

        public void BearMovement(GameTime gameTime)
        {
            if (bearState == BearState.looking)
            {
                /*Add Collision code between field of vision of player and field of vision of bear
                If the Collision of Fields Occurs....
                if()
                {
                    ResetWhenToMove();
                    bearScore = bearScore * -1; // Srts Bear Score from Negative to Positive
                    bearState = BearState.following;
                    break this if loop immediatly to run code for when the bear is in a following state

                */

                // Adds x amount of seconds to the movement timer
                whenToMoveTimer += gameTime.ElapsedGameTime.TotalSeconds;

                // Will begin to move whenever the Timer = to the Set time as to when to move
                if (whenToMoveTimer == whenToMove)
                {
                    // Tells the Bear it can now move in looking state and the direction it will move
                    moveInLook = true;

                    // LOOK AT THIS AND FIX IF NEEDED
                    bearDirection.Equals(rng.Next(0, 4)); // Code a random direction the bear should now move... ? May be Wrong.
                    
                }

                if (moveInLook == true) // If the object should be in a random movement state
                {
                    timeOfMovementCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    // The object then moves a random speed in a random direction
                    // objectMovement.addVelocity(?,?)

                }

                if (timeOfMovementCounter == timeOfMovement) // When the time of Movement has been hit
                {
                    ResetWhenToMove();
                }

            }

            if (bearState == BearState.following)
            {
                /*Add Collision code between field of vision of player and field of vision of bear
                    If the Collision of Fields Occurs....
                if()
                { 
                bearScore = bearScore * -1; // Sets Bear Score From Positive to Negative
                bearState = BearState.looking;
                break this if loop immediatly to run code for when the bear is in a following state

                */
                
                // write code for following state. 

            }

            objectMovement.UpdatePosition(objectCollisionBox);
            objectMovement.UpdatePosition(bearVision);
            objectMovement.UpdatePosition(fieldOfAttack);
        }

        /// <summary>
        /// Resets the Timer and comes up with a new time as to when the bear should move
        /// </summary>
        public void ResetWhenToMove()
        {
            whenToMove = rng.Next(whenToMoveMin, whenToMoveMax);            // Comes up with a new time as to when to move
            whenToMoveTimer = 0;                                            // Resets the movementTimer back to 0
            timeOfMovement = rng.Next(timeOfMovementMin, timeOfMovementMax);// Comes up with a random total time of movement.
            timeOfMovementCounter = 0;                                      // Resets the timeOfMovement counter to 0
            moveInLook = false;                                             // Bear Can No Longer move while Looking
        }
    }
}
