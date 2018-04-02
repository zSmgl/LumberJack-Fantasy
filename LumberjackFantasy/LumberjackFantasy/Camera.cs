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
    class Camera
    {
        ///summary
        ///Jacob LeBerth
        ///Camera object holds two rectangles, one is screen sized, one is slightly larger.  
        ///Its location is based on the players location, by taking the players centerpoint and positioning it so it is in the center 
        ///or at the furthest point along if the player is near the edge.
        ///Retruns bools for objects to see if they need to be drawn or need to be updated.
        ///Gameobject uses the position of the camera box to determine where to draw objects

        //fields ------------------------------------------------------------------------
        private Rectangle onScreen; //rectangle hitbox to determine if something needs to be drawn
        private Rectangle upScreen; //rectangle hitbox to determine if something needs to be updated
        private int visionStandard; //offset integer for how large the upScreen should be

        //properties --------------------------------------------------------------------
        private Rectangle CameraPosition { get { return onScreen; } }
        //constructor -------------------------------------------------------------------
        public Camera(int standard)
        {
            visionStandard = standard;
            onScreen = new Rectangle(0, 0, 896, 896);
            upScreen = new Rectangle(0 - visionStandard, 0 - visionStandard, 896 + (2 * visionStandard), 896 + (2 * visionStandard));
        }
        //methods -----------------------------------------------------------------------
        public bool isDrawn(Rectangle position) //method to check if collides with upScreen
        {
            if (onScreen.Contains(position))
            {
                return true;
            }        
                return false;           
        }
        public bool isUpdating(Rectangle position) //method to check if collides with upScreen
        {
            if(upScreen.Contains(position))
            {
                return true;
            }
            return false;
        }

        public void updatePosition(Rectangle player)//updates the cameras position
        {

            //sets onscreen location based on player
            onScreen.X = player.Center.X - 448;
            onScreen.Y = player.Center.Y - 448;

            //prevents onscreen from leaving the game map
            if (onScreen.X < 0)
            {
                onScreen.X = 0;
            }
            if (onScreen.X > 896)
            {
                onScreen.X = 896;
            }
            if (onScreen.Y < 0)
            {
                onScreen.Y = 0;
            }
            if (onScreen.Y > 896)
            {
                onScreen.Y = 896;
            }

            //places the update hitbox
            upScreen.X = onScreen.X - visionStandard;
            upScreen.Y = onScreen.Y - visionStandard;
        }

    }
}
