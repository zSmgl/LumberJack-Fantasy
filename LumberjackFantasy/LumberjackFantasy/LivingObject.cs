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
    class LivingObject : GameObject
    {
        /// <summary>
        /// Jacob LeBerth
        /// Blue Print for Living Objects, which encompass objects that are active based on whether they have health
        /// Inherits from gameobject, as well as having fields and properties for health, isAlive, and can have velocity
        /// </summary>

        // fields -----------------------------------------------------------------------
        private int health;     // An Objects current Health
        private int maxHealth;  // An Objects maximum Health
        private int speedX;     // An Objects current Speed in the X direction
        private int speedY;     // An Objects current Speed in the Y direction
        private int maxSpeed;   // An Objects max amount of Speed
        

        //properties --------------------------------------------------------------------
        public int Health { get { return health; } set { health = value; if (health > maxHealth) { health = maxHealth; } } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; }  }
        public int SpeedX { get { return speedX; } set { speedX = value; } }
        public int SpeedY { get { return speedX; } set { speedX = value; } }
        public int MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

        //IsAlive references the health value and if is greater then 0 returns true
        public bool IsAlive
        { get
            {
                if (health > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        //constructor -------------------------------------------------------------------
        public LivingObject(int x, int y, int width, int height, Texture2D objectTexture, int maxH, int maxS)
            : base(x, y, width, height, objectTexture)
        {
            health = maxH;
            maxHealth = maxH;
        }
        //methods -----------------------------------------------------------------------

        //inherited draw method
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

    }
}
