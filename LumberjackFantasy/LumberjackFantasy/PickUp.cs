using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace LumberjackFantasy
{
    class PickUp : GameObject
    {
        //temp space
        public PickUp(int x, int y, int width, int height, Texture2D objectTexture) 
            : base(x, y, width, height, objectTexture)
        {

        }
    }
}
