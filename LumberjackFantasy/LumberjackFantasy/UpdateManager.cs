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
    class UpdateManager
    {

        int checkablePlayerX;
        int checkablePlayerY;
        VelocityManager velocityManager = new VelocityManager(0);
        CollisionManager collisionManager = new CollisionManager();

        public UpdateManager()
        {

        }

        public void UpdateAll()
        {
            //UpdatePlayer()
            //UpdateAllBears()
            //UpdateAttacks()
        }
        // Player Intended Methods and Helper Methods 

        public void UpdatePlayer(Player p, KeyboardState currentKB, KeyboardState previousKB)
        {
            // Sets up the player X and Y values that should be modified.
            int playerX = p.PosX;
            int playerY = p.PosY;

            // Creates a Player Velocity Manager
            velocityManager = new VelocityManager(p.MaxSpeed)
            {
                // Sets the current X and Y speed that should be in pl
                VelocityX = p.SpeedX,
                VelocityY = p.SpeedY
            };

            PlayerMovement(p, currentKB);
            //p = collisionManager.MapCheck(p, treeList, itemList); ?

            
            

        }

        /// <summary>
        /// Calculates the new position of the Player & Field of Vision, and updates the Players current Speed in X and Y Directions
        /// </summary>
        /// <param name="p">The Player that will be changed</param>
        /// <param name="kb">Keyboard State of Current Keys</param>
        public void PlayerMovement(Player p, KeyboardState kb)
        {
            // Adds Speed to the Velocity Manager Based on the Current Keys Pressed
            if (kb.IsKeyDown(Keys.W) == true)
            {
                velocityManager.addVelocity(0, -1 * (p.MaxSpeed / 4));
            }
            if (kb.IsKeyDown(Keys.S) == true)
            {
                velocityManager.addVelocity(0, (p.MaxSpeed / 4));
            }
            if (kb.IsKeyDown(Keys.A) == true)
            {
                velocityManager.addVelocity(-1 * (p.MaxSpeed / 4), 0);
            }
            if (kb.IsKeyDown(Keys.D) == true)
            {
                velocityManager.addVelocity((p.MaxSpeed / 4), 0);
            }

            // Sets the new Sprite Location & Player Field of Vision
            p.ObjectCollisionBox = velocityManager.UpdatePosition(p.ObjectCollisionBox);
            p.PlayerVision = velocityManager.UpdatePosition(p.PlayerVision);

            // Updates the Current Speed of the Player within the Player from the Calculated speed in Players VM
            p.SpeedX = velocityManager.VelocityX;
            p.SpeedY = velocityManager.VelocityY;
            
        }
    }
}
