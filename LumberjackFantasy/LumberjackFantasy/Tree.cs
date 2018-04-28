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
    class Tree : LivingObject
    {
        ///Tree class
        ///game object similar to other objects, has no follow code as trees are stationary
        ///has its own sub class to distinguish from other objects
        #region Fields

        private bool colliding;
        private bool wasAttacked;

        #endregion Fields
        #region Properties
        public bool Colliding {get {return colliding;} set {colliding = value;}}
        /// <summary>
        /// Determines if a tree can lose health
        /// </summary>
        public bool WasAttacked
        {
            get { return wasAttacked; }
            set { wasAttacked = value; }
        }
        #endregion Properties
        #region Constructor

        public Tree(int x, int y, int width, int height, Texture2D objectTexture, int maxH, int maxS) 
            : base(x, y, width, height, objectTexture, maxH, maxS)
        {
			colliding = false;
            wasAttacked = false;
        }
        #endregion Constructor
        #region Methods
        public override void Draw(SpriteBatch sb, Vector2 camera)
        {
                        //Base Draw Method for Base Build
            if (onScreen)
            {
                sb.Draw(objectTexture, 
                    new Vector2(objectCollisionBox.X - Convert.ToInt32(camera.X), objectCollisionBox.Y - Convert.ToInt32(camera.Y)), 
                    new Rectangle(animationMoveFrame * objectCollisionBox.Width, 0 * objectCollisionBox.Height , objectCollisionBox.Width, objectCollisionBox.Height), Color.White);
            }

        }
        #endregion Methods
    }
}
