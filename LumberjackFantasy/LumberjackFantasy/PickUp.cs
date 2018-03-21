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
		private PickupType pickuptype;
		private ItemState itemState;

		//properties
		public PickupType PickupType
		{
			get { return pickuptype; }
			set { pickuptype = value; }
		}

		public ItemState ItemState
		{
			get { return ItemState; }
			set { itemState = value; }
		}

        //temp space
        public PickUp(int x, int y, int width, int height, Texture2D objectTexture, int type) 
            : base(x, y, width, height, objectTexture)
        {
			switch (type)
			{
				
				case 1:
					pickuptype = PickupType.Apple;
					break;

				case 2:
					pickuptype = PickupType.MapleSyrup;
					break;

				case 3:
					pickuptype = PickupType.Shotgun;
					break;
			}
        }
    }
}
