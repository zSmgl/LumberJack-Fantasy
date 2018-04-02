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

        //fields ------------------------------------------------------------------------
        //properties --------------------------------------------------------------------
        
        //constructor
        public Tree(int x, int y, int width, int height, Texture2D objectTexture, int maxH, int maxS) 
            : base(x, y, width, height, objectTexture, maxH, maxS)
        {

        }

        //methods -----------------------------------------------------------------------
        public override void Draw(SpriteBatch sb, Vector2 camera)
        {
            base.Draw(sb, camera);
        }
    }
}
