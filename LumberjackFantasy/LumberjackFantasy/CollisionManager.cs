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

        public GameObject MapCheck(GameObject obj, List<Tree> treeList)
        {
            for (int i = 0; i < treeList.Count; i++)
            {
                while (obj.ObjectCollisionBox.Intersects(treeList[i].ObjectCollisionBox) == true)
                {
                    Rectangle collision = Rectangle.Intersect(obj.ObjectCollisionBox, treeList[i].ObjectCollisionBox);

                    // how far the x value needs to be adjusted by int xAdjustBy = collision.Height;
                    // how far the y value needs to be adjusted by int yAdjustBy = collision.Width;

                }
            }

            return obj;
        }
    }
}
