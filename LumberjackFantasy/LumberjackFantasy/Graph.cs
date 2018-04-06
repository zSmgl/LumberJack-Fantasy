using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberjackFantasy
{
    class Graph
    {
        /// <summary>
        /// Jacob Marcovecchio and Jacob Laberth
        /// This class creates a graph of Locations (which are basically rectangles) that our camera holds to see if a bear can move into.
        /// These Locations update and adjust all of their values accordingly as the camera moves.
        /// This line above me should be done in camera once I figure this shit out.
        /// </summary>
        /// 
        // Logic - The Graph takes up 1024 by 1024 space whos midpoint is the same as the camera. Each Location (Tile) is 16 by 16.
        // The square root of 1024 is 32. So instead of having 32  32 by 32 tiles fill the screen, I'm going to have 64 16*16 tiles do the exact
        // Same thing. These values can be adjusted as needed. I just wanted ez numbers to work with. 

        private int graphSize = 64;
        private int tileConstant = 16;                   // The width and Height of a tile
        private Location[,] graph = new Location[64,64]; // Creates a 64 by 64 Array of Location Data Types

        public void SetGraph()
        {

        }
    }
}
