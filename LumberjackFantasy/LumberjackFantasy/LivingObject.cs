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
        private int health;
        private int maxHealth;
        protected VelocityManager objectMovement;
        

        //properties --------------------------------------------------------------------
        public int Health { get { return health; } }
        public int MaxHealth { get { return maxHealth; } }

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
            objectMovement = new VelocityManager(maxS);
        }
        //methods -----------------------------------------------------------------------

        //inherited draw method
        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        //method to change the position of the rectangle
        public void MoveObject()
        {
            objectCollisionBox = objectMovement.UpdatePosition(objectCollisionBox);
        }

        //method which hard sets the velocity of an object, children LivingObjects can have methods which use the addSpeed Method
        public void setVelocity(int velX, int velY)
        {
            objectMovement.VelocityX = velX;
            objectMovement.VelocityY = velY;
        }

    }
}
