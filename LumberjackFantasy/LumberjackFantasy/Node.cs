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
	class Node
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
		private Rectangle nodeSpace;	// Holds the position of the node
		private Node[] adjacentNodes;	// Array of nodes, beginning at top left to clockwise in naming.
        private Node pathTo;            // The node of which this one should be pathed too. 
        private int[] distance;         // The distance between this node and another. 4 = Corner Node, 3 = N,S,E,W node
		//properties --------------------------------------------------------------------
		public bool Passable { get { return passable; } set { passable = value; } }
		public bool ToUpdate { get { return ToUpdate; } set { ToUpdate = value; } }
		public bool Locked { get { return locked; }  set { locked = value; } }
        public Node PathTo { get { return pathTo; } set { pathTo = value; } }
		public Rectangle NodeSpace { get { return nodeSpace; } }
		public Node[] AdjacentNodes { get { return AdjacentNodes; } }

		//constructor -------------------------------------------------------------------
		public Node(Rectangle nodeLocation)
		{
			nodeSpace = nodeLocation;
			IsPassable();
			toUpdate = false;
			locked = false;
			adjacentNodes = new Node[8]; //array of nodes, beginning at top left to clockwise in naming.
            distance = new int[] { 4, 3 }; // Cost 4 to go to a corner, cost three to go straight up, right, left, or down
		}
		public Node(int x, int y, int height, int width)
		{
			nodeSpace = new Rectangle(x,y,height,width);
			IsPassable();
			toUpdate = false;
			locked = false;
			adjacentNodes = new Node[8]; //array of nodes, beginning at top left to clockwise in naming.
            distance = new int[] { 4, 3 }; // Cost 4 to go to a corner, cost three to go straight up, right, left, or down
        }

		//methods -----------------------------------------------------------------------
		public void IsPassable() //method to see if the Node can be passed
		{
			//insert code here
		}
	}
}
