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
	class Location
	{
		///<summary>
		/// Jacob LeBerth Jacob Marcovechio 
		/// class that holds mini tiles on camera
		/// tiles are either passable or impassible
		/// used to determine pathing
		///</summary>

		//fields ------------------------------------------------------------------------
		private bool passable;			// Holds whether or not the node can be passed
		private bool toUpdate;          // Holds whether or not the node needs to be checked based on Camera's UpScreen Rectangle
		private bool locked;			// Tells the updateManager whether adjacent nodes need to be checked
		private Rectangle pos;	// Holds the position of the node
		private Location[] adjacentLocations;	// Array of nodes, beginning at top left to clockwise in naming.
        private Location parent;            // The node of which this one should be pathed too. 
        private int gScore;             // The movement cost (in # of Nodess) from start point to curret square
                                        // G is calculated from taking it from it's parent and adding 1.
        private int hScore;             // Estimated cost from the current square to the destination square.
        private int fScore;             // g+h;
		//properties --------------------------------------------------------------------
		public bool Passable { get { return passable; } set { passable = value; } }
		public bool ToUpdate { get { return ToUpdate; } set { ToUpdate = value; } }
		public bool Locked { get { return locked; }  set { locked = value; } }
        public Location Parent { get { return parent; } set { parent = value; } }
		public Rectangle Pos { get { return pos; } }
		public Location[] AdjacentLocations { get { return adjacentLocations; } set { adjacentLocations = value; } }

		//constructor -------------------------------------------------------------------
		public Location(Rectangle pos)
		{
			this.pos = pos;
			IsPassable();
			toUpdate = false;
			locked = false;
			adjacentLocations = new Location[8]; //array of nodes, beginning at top left to clockwise in naming.
		}
		public Location(int x, int y, int heightAndWidth)
		{
			pos = new Rectangle(x,y,heightAndWidth,heightAndWidth);
			IsPassable();
			toUpdate = false;
			locked = false;
			adjacentLocations = new Location[8]; //array of nodes, beginning at top left to clockwise in naming.
        }

		//methods -----------------------------------------------------------------------
		public void IsPassable() //method to see if the Node can be passed
		{
			//insert code here
		}

        static int ComputeHScore(int x, int y, int targetX, int targetY)
        {
             return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }
	}
}
