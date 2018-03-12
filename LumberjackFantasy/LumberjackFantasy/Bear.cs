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
    enum BearState                      // Determines the state of which the the bear is in
    {
        looking,
        following
    }

    enum BearDirection
    {
        upleft,
        upright,
        downleft,
        downright,
        up,
        down,
        left,
        right,
    }
    class Bear : LivingObject 
    {
        // "Normal" Fields -------------------------------------------------------------------

        private int scoreValue;                 // The amount of points that a bear is worth.
        private int hitPoints;                  // The total amount of hit points the bear currently has. 

        // Attack and Attacked Based Fields --------------------------------------------------

        private int fieldOfAttackStandard;      // Determines how far the Field of Attack to trigger a bear swipping should be from the object   
        private Rectangle fieldOfAttack;        // Rectangle that determines if the bear should trigger an attack animation

        // Movement Based Fields --------------------------------------------------------------

        private BearState bearState;            // Var that checks the state of the bear [Looking / Following]

        private int visionStandard;             // the number that determines how far out the BearVision should be from the actual objectTexture.
        private Rectangle bearVision;           // The Bears field of vision that helps the bears determine what kind of state they are in.

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

       public BearDirection BearDirection
        {
            get
            {
                if(objectMovement.VelocityX > 0 && objectMovement.VelocityY > 0)
                {
                    return BearDirection.downright;
                }
                else if (objectMovement.VelocityX > 0 && objectMovement.VelocityY == 0)
                {
                    return BearDirection.right;
                }
                else if (objectMovement.VelocityX > 0 && objectMovement.VelocityY < 0)
                {
                    return BearDirection.upright;
                }
                else if (objectMovement.VelocityX == 0 && objectMovement.VelocityY > 0)
                {
                    return BearDirection.down;
                }
                else if (objectMovement.VelocityX < 0 && objectMovement.VelocityY > 0)
                {
                    return BearDirection.downleft;
                }
                else if (objectMovement.VelocityX < 0 && objectMovement.VelocityY == 0)
                {
                    return BearDirection.left;
                }
                else if (objectMovement.VelocityX < 0 && objectMovement.VelocityY < 0)
                {
                    return BearDirection.upleft;
                }
                else if (objectMovement.VelocityX == 0 && objectMovement.VelocityX < 0)
                {
                    return BearDirection.up;
                }
                else
                {
                    //default case, usually upon spawning in
                    return BearDirection.down;
                }
            }
        }

        // Constructor ------------------------------------------------------------------------------

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

                    Set the field of attack of the bear based on the position of the bear.
                    fieldOfAttack = new Rectangle(PosX - fieldOfAttackStandard, PosY - visionStandard,
                    width + (visionStandard * 2), height + (visionStandard * 2));

                    break this if loop immediatly to run code for when the bear is in a following state

                */

                // Adds x amount of seconds to the movement timer
                whenToMoveTimer += gameTime.ElapsedGameTime.TotalSeconds;

                // Will begin to move whenever the Timer = to the Set time as to when to move
                if (whenToMoveTimer == whenToMove)
                {
                    // Tells the Bear it can now move in looking state and the direction it will move
                    moveInLook = true;
                    
                }

                if (moveInLook == true) // If the object should be in a random movement state
                {
                    timeOfMovementCounter += gameTime.ElapsedGameTime.TotalSeconds;
                    // The object then moves a random speed in a random direction
                    objectMovement.addVelocity
                        (
                        rng.Next(-1 *objectMovement.MaxSpeed,objectMovement.MaxSpeed) / 2,  // X Pos increment that is 1/2 potential speed
                        rng.Next(-1 * objectMovement.MaxSpeed, objectMovement.MaxSpeed) / 2 // Y Pos that is 1/2 Potential Speed
                        );


                    // Bear Should only update it's position if it's in a state of moving

                    objectMovement.UpdatePosition(objectCollisionBox);
                    objectMovement.UpdatePosition(bearVision);

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

                //else()

                // 1, Update where the bear and it's various rectangles should be.
                // 2, check to see if there are any bear - wall/ fOA - Player collisions due to this update. 
                // 3, Adjust all the rectangles as needed to assure bear doesnt get drawn in something.
                // 3 - b, be sure to see the current of the velocity of the bear to 0 if it runs into a wall.
                // 3 - c, check to see if the bears attack animation should be triggered or is triggered.
                // 3c - a, Trigger Attacking Animation from the midpoint of the bear / Update frame of attacking animation to be called.
                // 3c - b, Decrease hp of player if certain frames of the attackin animation collide with the player.
                // 3d - a, Dont decrease player hp if it was decreased in a previous attack animation (boolean dependent)

                // [In draw method, end attackin animation boolean once the draw method for the attack is called.]
                // [This will reset the animation]



            }
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
