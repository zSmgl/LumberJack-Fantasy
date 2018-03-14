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
    class CollisionManager
    {

        public CollisionManager()
        {

        }

        /// <summary>
        /// Adjust the position of the object until it is no longer colliding with any trees in the game.
        /// Returns the numbers needed to increment that adjustment.
        /// </summary>
        /// <param name="objCurrent">The current state of the object, as adjusted by only velocity</param>
        /// <param name="objOldPos">The old state of the object before adding speed to it. Used to compare with the new state to determine increments
        /// in x and y for other object Rectangle Fields</param>
        /// <param name="treeList">The list of trees the current object is tested to be in collisions with</param>
        /// <returns></returns>
        public int[] PosAdjust(LivingObject objCurrent, LivingObject objOldPos, List<Tree> treeList)
        {
            // The x and y amount all other Rectangles exisiting in the living object need to be adjusted by

            int adjustByX = 0;  
            int adjustByY = 0;  

            foreach(LivingObject tree in treeList)
            {
                while (objCurrent.ObjectCollisionBox.Intersects(tree.ObjectCollisionBox) == true)
                {
                    // Determines how much X needs to be adjusted by until it is no longer colliding 

                    // Object is to the RIGHT of the wall currently
                    if (objCurrent.PosX > objOldPos.PosX)
                    {
                        adjustByX--;        // Increments how much the x pos needs to be adjusted by for all other Rectangle fields
                                            // in the GameObject
                        objCurrent.PosX--;  // Increments actual collision box of the object that will eventually set the while loop to false
                    }

                    // Object is to the LEFT of the Wall Currently
                    else if(objCurrent.PosX < objOldPos.PosX)
                    {
                        adjustByX++;        // Increments how much the x pos needs to be adjusted by for all other Rectangle fields
                                            // in the GameObject
                        objCurrent.PosX++;  // Increments actual collision box of the object that will eventually set the while loop to false
                    }

                    // Determines how much Y needs to be adjusted by until it is no longer colliding 

                    // Object is BELOW the wall currently
                    if (objCurrent.PosY > objOldPos.PosY)
                    {
                        adjustByY--;        // Increments how much the y pos needs to be adjusted by for all other Rectangle fields
                                            // in the GameObject
                        objCurrent.PosY--;  // Increments actual collision box of the object that will eventually set the while loop to false
                    }

                    // Object is ABOVE the Wall Currently
                    else if (objCurrent.PosY < objOldPos.PosY)
                    {
                        adjustByY++;        // Increments how much the y pos needs to be adjusted by for all other Rectangle fields
                                            // in the GameObject
                        objCurrent.PosY++;  // Increments actual collision box of the object that will eventually set the while loop to false
                    }
                }
            }

            int[] posValues = new int[] { adjustByX, adjustByY };

            return posValues;
        }
    }
}
