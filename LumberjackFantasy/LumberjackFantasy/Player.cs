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

    enum AttackVariation               // If we have a sprite sheet class that holds all the varior animations and textures to our game,
	{                                  // Axe and Shotgun should be set to the x or y by which the sheet is in, and then be the 2nd num
		axe = 10,                      // be incremented
		shotgun = 11,
		bear = 12
	}                               
                                     
    enum PlayerDirection
    {
        upleft = 6,
        upright = 5,
        downleft = 4,
        downright = 7,
        up = 0,
        down = 1,
        left = 3,
        right = 2,
    }

    enum PlayerAttackDirection
    {
        up,
        down,
        right,
        left
    }


    class Player : LivingObject
    {
        // Jacob Marcovecchio
		// Violence by Bobby McKenna

        // Fields -----------------------
        private int levelScore;         // Lvl Score'
        private int totalScore;         // Player's total score
        // ------------------------------

        private Texture2D attackTexture;// Players Attack Texture

        private double invincibleTimer; // Invincibility Timer
        private bool invincibile;        // determines if player is invincible or not.
        
        private int visionStandard;     // the number that determines how far out the playerVision should be from the actual objectTexture.
        private Rectangle playerVision; // The Players field of vision that helps the bears determine what kind of State they are in.

        private bool attackStuff;       // Determines if the player can attack something. 
        private int attackAnimationF;   // Keeps track of the players current attacking animation frame;
        AttackVariation attack;         // Enum that determines if the Player should use the shotgun or axe animation when attacking
        PlayerAttackDirection directionOfAttack;    // Enum that determines where drawing the attack frames relative to the player should be and how the attacking collision box should be drawn;
        PlayerDirection playerDirection;      // Enum that determines the direction the player should be facing. 
		private Rectangle playerAttackBox;    // Stores the Players Attack Hitbox

        // Properties -------------------
		public Rectangle PlayerAttackBox { get { return playerAttackBox; } set { playerAttackBox = value; } }
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
            set { playerVision = value; }
        }

        public AttackVariation Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        public PlayerAttackDirection AttackDirection
        {
            get { return directionOfAttack; }
            set { directionOfAttack = value; }
        }

        public bool IsAttacking
        {
            get { return attackStuff; }
            set { attackStuff = value; }
        }

        public int AttackAnimationF
        {
            get { return attackAnimationF; }
            set { attackAnimationF = value; }
        }

        public PlayerDirection PlayerDirection
        {
            get { return playerDirection; }
            set { playerDirection = value; }
        }

        public bool Invincible
        {
            get { return invincibile; }
            set { invincibile = value; }
        }

        public double InvincibleTimer
        {
            get { return invincibleTimer; }
            set { invincibleTimer = value; }
        }

        public int VisionStandard
        {
            get { return visionStandard; }
        }

        // Constructor --------------------

        public Player (int x, int y, int width, int height, Texture2D objectTexture, int maxH, int maxS, int visionStandard)
            : base(x, y, width, height, objectTexture, maxH, maxS)
        {
            levelScore = 0;
            this.visionStandard = visionStandard;
            playerVision = new Rectangle(x - visionStandard, y - visionStandard, width + (visionStandard * 2), height + (visionStandard * 2));
            onScreen = true;
            uPScreen = true;
            attackStuff = false;
            invincibleTimer = 1;
            invincibile = false;
            attackAnimationF = 0; // Will draw the first attacking frame
            attack = AttackVariation.axe; // Player Starts with Axe
        }

        //old Player
        public Player (Player p)
            : base(p.PosX, p.PosY, p.Width, p.Height, p.objectTexture, p.MaxHealth, p.MaxSpeed)
        {
            this.visionStandard = p.visionStandard;
            playerVision = new Rectangle(p.PosX - visionStandard, p.PosY - visionStandard, width + (visionStandard * 2), height + (visionStandard * 2));

            invincibleTimer = p.invincibleTimer;
            invincibile = p.invincibile;

            attackStuff = p.attackStuff;
            attackAnimationF = p.attackAnimationF;
            playerDirection = p.playerDirection;
        }

        public void LoadAttackTexture(Texture2D attackText)
        {
            attackTexture = attackText;
        }

        /// <summary>
        /// Reset's Invincibility
        /// </summary>
        public void ResetTimer()
        {
            invincibleTimer = 1;
        }

        /// <summary>
        /// Draw's the player to the screen. Can also draw the visionBox if set correctly
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb, Vector2 camera)
        {
            //Base Draw Method for Base Build
            if (onScreen)
            {
                PlayerDirection animationDirection;
                if ((int)playerDirection > 3)
                {
                   animationDirection = (PlayerDirection)((int)playerDirection % 3);
                }
                else
                {
                    animationDirection = PlayerDirection;
                }
                sb.Draw(objectTexture, 
                    new Vector2(objectCollisionBox.X - Convert.ToInt32(camera.X), objectCollisionBox.Y - Convert.ToInt32(camera.Y)), 
                    new Rectangle(animationMoveFrame * objectCollisionBox.Width, (int)animationDirection * objectCollisionBox.Height , objectCollisionBox.Width, objectCollisionBox.Height), Color.White);
            }

            // Draw method for vision field incase we need to see it
            // This includes sending a texture for the vision texutre. Essentially just a black box.
            // sb.Draw(blackTexture, playerVision, Color.White)
        }
    }
}
