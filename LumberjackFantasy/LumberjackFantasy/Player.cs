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

    enum AttackVariation                // If we have a sprite sheet class that holds all the varior animations and textures to our game,
                                        // Axe and Shotgun should be set to the x or y by which the sheet is in, and then be the 2nd num
                                        // be incremented
                                        
    {
        axe = 10,                       
        shotgun = 11
    }

    enum PlayerDirection
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


    class Player : LivingObject
    {
        // Jacob Marcovecchio

        // Fields -----------------------
        private int levelScore;         // Lvl Score'
        private int totalScore;         // Player's total score
        private int visionStandard;     // the number that determines how far out the playerVision should be from the actual objectTexture.
        private Rectangle playerVision; // The Players field of vision that helps the bears determine what kind of State they are in.
        private bool attackStuff;       // Determines if the player can attack something. 
        AttackVariation attack;         // Enum that determines if the Player should use the shotgun or axe animation when attacking

        // Properties -------------------
        public int LevelScore
        {
            get { return levelScore; }
            set { levelScore = value; }
        }
        public int TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; }
        }

        public Rectangle PlayerVision
        {
            get { return playerVision; }
        }

        public AttackVariation SetAttack
        {
            get { return attack; }
            set { attack = value; }
        }

        public Player (int x, int y, int width, int height, Texture2D objectTexture, int maxH, int maxS, int visionStandard)
            : base(x, y, width, height, objectTexture, maxH, maxS)
        {
            levelScore = 0;
            this.visionStandard = visionStandard;
            playerVision = new Rectangle(x - visionStandard, y - visionStandard, width + (visionStandard * 2), height + (visionStandard * 2));
        }
        

        /// <summary>
        /// Determines both the movement and the attack style, if one, from the player.
        /// </summary>
        /// <param name="kb"></param>
        public void ProcessPlayerInput(KeyboardState kb)
        {

            // Movement & Movement Animation ------------------------------------------

            // Up - W
            if (kb.IsKeyDown(Keys.W) == true)
            {
                //objectMovement.addVelocity(0,?);
            }
            // Down - S
            if (kb.IsKeyDown(Keys.S) == true)
            {
                //objectMovement.addVelocity(0,?);
            }
            // Right - D
            if (kb.IsKeyDown(Keys.D) == true)
            {
                //objectMovement.addVelocity(?,0)
            }
            // Left - A
            if (kb.IsKeyDown(Keys.A) == true)
            {
                //objectMovement.addVelocity(?,0)
            }

            // Attack Animation ----------------------------------------

            // The Player Can Only Have One Attack Animation at a time and must complete it before triggering another one.

            if (kb.IsKeyDown(Keys.I) == true )
            {
                if (attackStuff == false)
                {
                    attackStuff = true; 
                }
                else
                {
                    // Else stuff
                }
                /* If the Attack should be over....
                if ( [Attack is over now])
                    {
                 attackStuff = false;
                }
                */
            }
            else if (kb.IsKeyDown(Keys.J) == true)
            {
                if (attackStuff == false)
                {
                    attackStuff = true;
                }
                else
                {
                    // Else stuff
                }
            }
            else if (kb.IsKeyDown(Keys.J) == true)
            {
                if (attackStuff == false)
                {
                    attackStuff = true;
                }
                else
                {
                    // Else stuff
                }
            }
            else if (kb.IsKeyDown(Keys.J) == true)
            {
                if (attackStuff == false)
                {
                    attackStuff = true;
                }
                else
                {
                    // Else stuff
                }
            }
            // Collisions get handled in the update Position method to assure stuff doesnt end up in other stuff

            objectMovement.UpdatePosition(objectCollisionBox);
            objectMovement.UpdatePosition(playerVision);


        }

        /// <summary>
        /// Draw's the player to the screen. Can also draw the visionBox if set correctly
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            // Draw method for vision field incase we need to see it
            // This includes sending a texture for the vision texutre. Essentially just a black box.
            // sb.Draw(blackTexture, playerVision, Color.White)
        }
    }
}
