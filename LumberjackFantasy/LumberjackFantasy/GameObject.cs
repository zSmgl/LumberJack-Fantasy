﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LumberjackFantasy
{
    public class GameObject
    {

        /// <summary>
        /// Jacob Marcovecchio
        /// Blue Print for In-game objects.
        /// Has fields that hold the texture of the object and builds a path of the object.
        /// The path is held in both individual int fields and a Rectangle Field that can be updated to be the same as the modified int fields.
        /// </summary>

        // Fields - Numbers
		// Could be changed in the future to make use of rectangle instead of posX and posY

        protected int posX;     // position of object [x]
        protected int posY;     // position of object [y]
        protected int width;    // scale by which object width is drawn
        protected int height;   // scale by which object height is drawn

        protected Texture2D objectTexture;      // Objects visual texture
        protected Rectangle objectCollisionBox; // Objects Pathing & Rectangle Dimensions used to detect colliding with other game objects


        // Properties

        public int PosX
        {
            set { posX = value; }
            get { return posX; }
        }

        public int PosY
        {
            set { posY = value; }
            get { return posY; }
        }

        public int Width
        {
            set { width = value; }
            get { return width; }
        }

        public int Height
        {
            set { height = value; }
            get { return height; }
        }

        public Texture2D ObjectTexture
        {
            set { objectTexture = value; }
            get { return objectTexture; }
        }

        public Rectangle ObjectCollisionBox
        {
            set { objectCollisionBox = value; }
            get { return objectCollisionBox; }
        }

        /// <summary>
        /// Constructor for GameObject
        /// </summary>
        /// <param name="x"> X Position of the Object on the Screen </param>
        /// <param name="y"> Y Position of the Object on the Screen</param>
        /// <param name="width">  Width of the Object</param>
        /// <param name="height"> Height of the Object</param>
        /// <param name="gameTexture"></param>
        public GameObject(int x, int y, int width, int height, Texture2D objectTexture)
        {
            posX = x;
            posY = y;
            this.width = width;
            this.height = height;
            this.objectTexture = objectTexture;
            objectCollisionBox = new Rectangle(x, y, width, height);

        }

        /// <summary>
        /// Updates the objectPath by creating a new Rectangle with any modified posX and posY variables that were called.
        /// Not Necessarily Needed, but added just in case
        /// </summary>
        public void PathUpdate()
        {
            objectCollisionBox = new Rectangle(posX, posY, width, height);
        }

        /// <summary>
        /// Draws the Game Object to the Screen
        /// </summary>
        /// <param name="sb"></param>
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(objectTexture, objectCollisionBox, Color.White);
        }

    }
}