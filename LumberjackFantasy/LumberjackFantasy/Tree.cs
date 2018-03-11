using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace LumberjackFantasy
{
    class Tree : LivingObject
    {
        //creating a constructor here with the values we want so that the load command works, 
        public Tree(int x, int y, int width, int height, Texture2D objectTexture, int maxH, int maxS) 
            : base(x, y, width, height, objectTexture, maxH, maxS)
        {

        }
    }
}
