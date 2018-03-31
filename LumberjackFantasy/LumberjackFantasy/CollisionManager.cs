using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

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

			foreach (LivingObject tree in treeList)
			{

				if (objCurrent.ObjectCollisionBox.Intersects(tree.ObjectCollisionBox) == true)
				{

                    // Discovers Intersection Rectangle between object and the Tree
                    Rectangle intersectionRect = Rectangle.Intersect(objCurrent.ObjectCollisionBox, tree.ObjectCollisionBox);

                    // Speed Y = units moved over Y space of time. Speed X = units moved over X space time. 
                    // tan(theta) = Units Y / Units X

                    // This is in Radians
                    if (objCurrent.SpeedY != 0 && objCurrent.SpeedX != 0)
                    {
                        double tanAngle = Math.Atan((double)(objCurrent.SpeedY / objCurrent.SpeedX));

                        // Think proportion logic here. The Proportion of the object moving X over Y should be moved back out of the
                        // Tree based on the same angle. By finding the x2 or y2 proportion with the intersection Rect, then you can figure
                        // out the unknown proprtion. 
                        // Speed Y / Speed X = tan(theta) | tan(theta) == ( y2/ x2) | y2 or x2 is solved for based on the lesser value of intRect
                        // Solve for the unknown value is all this is doing. 

                        double x2 = 0;
                        double y2 = 0;

                        if (objCurrent.PosX < objOldPos.PosX && objCurrent.PosY < objOldPos.PosY)  // Obj Move Left and Up
                        {
                            if (intersectionRect.Height > intersectionRect.Width)
                            {
                                x2 = intersectionRect.Width;
                                y2 = x2 / tanAngle;

                            }
                            else if (intersectionRect.Width > intersectionRect.Height)
                            {
                                y2 = intersectionRect.Height;
                                x2 = y2 / tanAngle;
                            }
                            else
                            {
                                x2 = intersectionRect.Width;
                                y2 = intersectionRect.Height;
                            }

                        }
                        else if (objCurrent.PosX > objOldPos.PosX && objCurrent.PosY < objOldPos.PosY) // Obj Move Right and Up
                        {
                            if (intersectionRect.Height > intersectionRect.Width) //
                            {
                                x2 = intersectionRect.Width * (-1);
                                y2 = x2 / tanAngle;

                            }
                            else if (intersectionRect.Width > intersectionRect.Height)
                            {
                                y2 = intersectionRect.Height;
                                x2 = y2 / tanAngle;
                            }
                            else
                            {
                                x2 = intersectionRect.Width * (-1);
                                y2 = intersectionRect.Height;
                            }
                        }
                        else if (objCurrent.PosX < objOldPos.PosX && objCurrent.PosY > objOldPos.PosY) // Obj Move Left and Down
                        {
                            if (intersectionRect.Height > intersectionRect.Width)
                            {
                                x2 = intersectionRect.Width;
                                y2 = x2 / tanAngle;

                            }
                            else if (intersectionRect.Width > intersectionRect.Height)
                            {
                                y2 = intersectionRect.Height * (-1);
                                x2 = y2 / tanAngle;
                            }
                            else
                            {
                                x2 = intersectionRect.Width;
                                y2 = intersectionRect.Height * (-1);
                            }
                        }
                        else if (objCurrent.PosX > objOldPos.PosX && objCurrent.PosY > objOldPos.PosY) // Obj Move Right and Down
                        {
                            if (intersectionRect.Height > intersectionRect.Width)
                            {
                                x2 = intersectionRect.Width * (-1);
                                y2 = x2 / tanAngle;

                            }
                            else if (intersectionRect.Width > intersectionRect.Height)
                            {
                                y2 = intersectionRect.Height * (-1);
                                x2 = y2 / tanAngle;
                            }
                            else
                            {
                                x2 = intersectionRect.Width * (-1);
                                y2 = intersectionRect.Height * (-1);
                            }
                        }

                        adjustByX = (int)x2;
                        adjustByY = (int)y2;
                    }
                    // Object moved into the right of the wall without changing along Y Axis
                    if (objCurrent.PosX > objOldPos.PosX && (objCurrent.PosY - objOldPos.PosY) == 0)
					{
						adjustByX-= intersectionRect.Width; 
					}

					// Object moved into the LEFT of the Wall without changing along the Y Axis
					else if (objCurrent.PosX < objOldPos.PosX && (objCurrent.PosY - objOldPos.PosY) == 0)
					{
						adjustByX+= intersectionRect.Width;
					}

					// Object moved into the top of the Wall without changing on the X axis
					else if (objCurrent.PosY > objOldPos.PosY && (objCurrent.PosX - objOldPos.PosX) == 0)
					{
						adjustByY-= intersectionRect.Height;
					}

					// Object moved into the bottom of the Wall without changing along the X axis
					else if (objCurrent.PosY < objOldPos.PosY && (objCurrent.PosX - objOldPos.PosX) == 0)
					{
						adjustByY+= intersectionRect.Height;
					}
				}
			}

			int[] posValues = new int[] { adjustByX, adjustByY };

			return posValues;
		}


		//bearbox colider
		//Not meant to be used for anything other than checking if bears should attack.
		public bool BearboxCollider(Bear bear, Player player)
		{
			Rectangle bearBox = new Rectangle(bear.Location, bear.Size); // Size and Location have been added to GameObjects to ease creation
			if (bearBox.Intersects(player.ObjectCollisionBox))
			{
				return true;
			}
			return false;
		}

		//sees which things are hit by attacks.
		//can use either for player
		//MUST BE CALLED ONCE FOR EACH LIST OF LIVING OBJECTS
		/// <summary>
		/// Override for BEARS or Player
		/// </summary>
		/// <param name="attackVariation"></param>
		/// <param name="target"></param>
		/// <param name="player"></param>
		/// <param name="Bear List"></param>
		/// <returns></returns>
		public virtual bool[] GenericAttack(AttackVariation attackVariation, Rectangle target, Player player, List<Bear> livingList)
		{
			bool[] hitList;
			if (attackVariation == AttackVariation.axe || attackVariation == AttackVariation.shotgun)
			{
				hitList = new bool[livingList.Count];
				for (int i = 0; i < livingList.Count; i++)
				{
					if (target.Intersects(livingList[i].ObjectCollisionBox))
					{
						hitList[i] = true;
					}
					else
					{
						hitList[i] = false;
					}
				}
			}
			else
			{
				hitList = new bool[1];
				if (target.Intersects(player.ObjectCollisionBox))
				{
					hitList[0] = true;
				}
			}


			return hitList;
		}

		/// <summary>
		/// Override for TREES or Player
		/// </summary>
		/// <param name="attackVariation"></param>
		/// <param name="target"></param>
		/// <param name="player"></param>
		/// <param name="Tree List"></param>
		/// <returns></returns>
		public virtual bool[] GenericAttack(AttackVariation attackVariation, Rectangle target, Player player, List<Tree> livingList)
		{
			bool[] hitList;
			if (attackVariation == AttackVariation.axe || attackVariation == AttackVariation.shotgun)
			{
				hitList = new bool[livingList.Count];
				for (int i = 0; i < livingList.Count; i++)
				{
					if (target.Intersects(livingList[i].ObjectCollisionBox))
					{
						hitList[i] = true;
					}
					else
					{
						hitList[i] = false;
					}
				}
			}
			else
			{
				hitList = new bool[1];
				if (target.Intersects(player.ObjectCollisionBox))
				{
					hitList[0] = true;
				}
			}


			return hitList;
		}

	}
}
