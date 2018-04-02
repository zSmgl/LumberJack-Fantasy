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

    enum AttackVariation               // If we have a sprite sheet class that holds all the varior animations and textures to our game,
	{                                  // Axe and Shotgun should be set to the x or y by which the sheet is in, and then be the 2nd num
		axe = 10,                      // be incremented
		shotgun = 11,
		bear = 12
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
		// Violence by Bobby McKenna

        // Fields -----------------------
        private int levelScore;         // Lvl Score'
        private int totalScore;         // Player's total score
        // ------------------------------

        
        private int visionStandard;     // the number that determines how far out the playerVision should be from the actual objectTexture.
        private Rectangle playerVision; // The Players field of vision that helps the bears determine what kind of State they are in.
        private bool attackStuff;       // Determines if the player can attack something. 
        AttackVariation attack;         // Enum that determines if the Player should use the shotgun or axe animation when attacking
        PlayerDirection playerDirection;      // Enum that determines the direction the player should be facing. 

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
            set { playerVision = value; }
        }

        public AttackVariation Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        public PlayerDirection PlayerDirection
        {
            get { return playerDirection; }
            set { playerDirection = value; }
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
        }

        //old Player
        public Player (Player p)
            : base(p.PosX, p.PosY, p.Width, p.Height, p.objectTexture, p.MaxHealth, p.MaxSpeed)
        {
            this.visionStandard = p.visionStandard;
            playerVision = new Rectangle(p.PosX - visionStandard, p.PosY - visionStandard, width + (visionStandard * 2), height + (visionStandard * 2));
        }



        /// <summary>
        /// Draw's the player to the screen. Can also draw the visionBox if set correctly
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb, Vector2 camera)
        {
            //Base Draw Method for Base Build
            base.Draw(sb, camera);

            // Draw method for vision field incase we need to see it
            // This includes sending a texture for the vision texutre. Essentially just a black box.
            // sb.Draw(blackTexture, playerVision, Color.White)
        }

    }
}
