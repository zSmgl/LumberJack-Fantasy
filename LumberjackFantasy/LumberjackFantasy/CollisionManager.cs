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

				if (tree.ObjectCollisionBox.Intersects(objCurrent.ObjectCollisionBox) == true)
				{

                    Rectangle intersection = Rectangle.Intersect(objCurrent.ObjectCollisionBox, tree.ObjectCollisionBox);  
                    // Determines how much X needs to be adjusted by until it is no longer colliding 

                    // Object is to the RIGHT of the wall currently
                    if (objCurrent.PosX > objOldPos.PosX)
					{
						adjustByX-= intersection.Width;        // Increments how much the x pos needs to be adjusted by for all other Rectangle fields
											// in the GameObject
						//objCurrent.PosX--;  // Increments actual collision box of the object that will eventually set the while loop to false
					}

					// Object is to the LEFT of the Wall Currently
					else if (objCurrent.PosX < objOldPos.PosX)
					{
						adjustByX+= intersection.Width;        // Increments how much the x pos needs to be adjusted by for all other Rectangle fields
											// in the GameObject
						//objCurrent.PosX++;  // Increments actual collision box of the object that will eventually set the while loop to false
					}

					// Determines how much Y needs to be adjusted by until it is no longer colliding 

					// Object is BELOW the wall currently
					if (objCurrent.PosY > objOldPos.PosY)
					{
						adjustByY-= intersection.Height;        // Increments how much the y pos needs to be adjusted by for all other Rectangle fields
											// in the GameObject
						//objCurrent.PosY--;  // Increments actual collision box of the object that will eventually set the while loop to false
					}

					// Object is ABOVE the Wall Currently
					else if (objCurrent.PosY < objOldPos.PosY)
					{
						adjustByY+= intersection.Height;        // Increments how much the y pos needs to be adjusted by for all other Rectangle fields
											// in the GameObject
						//objCurrent.PosY++;  // Increments actual collision box of the object that will eventually set the while loop to false
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
