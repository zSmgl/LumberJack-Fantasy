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
    class VelocityManager
    {
        /// <summary>
        /// Jacob LeBerth
        /// VelocityManager class that stores the current speed of an object and is called to update it every frame
        /// gives livingObjects back updated position based on stored information
        /// slightly barebones at the moment but as we need more control over the objects physics we can add more
        /// </summary>

        //fields ------------------------------------------------------------------------
        private int velocityX;
        private int velocityY;
        private int maxSpeed;

        //properties --------------------------------------------------------------------
        
        //velocities have a get and a set value, the set allow for values within the max speed to be set directly, adding or subtracting to velocity will be done through methods

        public int VelocityX
        {
            get { return velocityX; }
            set
            {
                if (value <= maxSpeed && value >= (maxSpeed * -1))
                {
                    velocityX = value;
                }
            }
        }

        public int VelocityY
        {
            get { return velocityY; }
            set
            {
                if (value <= maxSpeed && value >= (maxSpeed * -1))
                {
                    velocityY = value;
                }
            }
        }

        //maxSpeed has a get and a set in case we want to alter an objects move speed range in game
        public int MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }
        //constructor -------------------------------------------------------------------
        public VelocityManager(int max)
        {
            maxSpeed = max;
            velocityX = 0;
            velocityY = 0;
        }
        //methods -----------------------------------------------------------------------

        //update position method which returns a rectangle with location equal to the input rectangle plus velocities
        public Rectangle UpdatePosition(Rectangle position)
        {
            return new Rectangle(position.X + velocityX, position.Y + velocityY, position.Width, position.Height );
        }

        //addVelocity which changes the current velocities instead of setting them
        public void AddVelocity(int velX, int velY)
        {
            velocityX += velX;
            velocityY += velY;

            //if velocity has gone out of bounds of max speed, set it such that it is at its max
            if (velocityX > maxSpeed)
            {
                velocityX = maxSpeed;
            }
            if (velocityY > maxSpeed)
            {
                velocityY = maxSpeed;
            }
            if (velocityX < maxSpeed * -1)
            {
                velocityX = maxSpeed * -1;
            }
            if (velocityY < maxSpeed * -1)
            {
                velocityY = maxSpeed * -1;
            }
        }
    }
}
