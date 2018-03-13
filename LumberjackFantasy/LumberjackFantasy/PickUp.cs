using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace LumberjackFantasy
{
    /// <summary>
    /// Declares What kind of pick up
    /// </summary>
    enum PickupType
    {
        Apple,
        Shotgun,
        MapleSyrup
    }

    /// <summary>
    /// Enum that determines if the object is currently visible or should be removed as its been retrieved
    /// </summary>
    enum ItemState
    {
        OnGround,
        Retrieved
    }

    class PickUp : GameObject
    {
        // Fields

        private int scoreValue; // The score the item is worth 


        //temp space
        public PickUp(int x, int y, int width, int height, Texture2D objectTexture) 
            : base(x, y, width, height, objectTexture)
        {

        }
    }
}
