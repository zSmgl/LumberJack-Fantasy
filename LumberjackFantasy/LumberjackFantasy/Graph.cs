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
        // Logic - The Graph takes up 1792 by 1792 space whos midpoint is the same as the camera. Each Location (Tile) is 16 by 16.
        // 112 16 pixel tiles encompasses the entire map.

         
        private int graphSize = 112;
        private int tileConstant = 16;                   // The width and Height of a tile
        private Location[,] graph = new Location[112,112]; // Creates a 112 by 112 Array of Location Data Types
		private List<Location> tilesToCheck;			   // List of important tiles

		//properties
		public Location[,] Tiles { get { return graph; } }
        public Graph()
        {
            SetGraph();
            SetGraphLocAdjacents();
        }
		public Graph(string unimportant)
		{
			//leaves an empty graph
		}
        /// <summary>
        /// Creates empty LocationPlaces within the graph
        /// </summary>
		/// 

		public void ClearList() //Clears list of to Update Tiles
		{
			tilesToCheck.Clear();
		}
		public void UpdateTiles(Location toAdd) //Adds location to list of to update tiles
		{
			tilesToCheck.Add(toAdd);
		}
        public void SetGraph()
        {
            int x = 0;
            int y = 0;

            // Creates all the tiles needed to fill a 1024 x 1024 space

            for (int i = 0; i < graphSize; i++) // Columns
            {
                for (int j = 0; j < graphSize; j++) // Rows
                {
                    graph[i, j] = new Location(x, y, tileConstant); // Creates a new Location Tile that is 16x16 at the correct x or y position.
                    y += tileConstant;
                }
                x += tileConstant;
            }
        }
        /// <summary>
        /// Set's each Graph Location to it's adjecent Graph Location's Accordingly to be checked later on.
        /// </summary>
        public void SetGraphLocAdjacents()
        {
            // Sets each tiles proper adjacent tiles. 
            for (int i = 0; i < graphSize; i++)
            {
                for (int j = 0; j < graphSize; j++)
                {
                    Location current = graph[i, j];

                    // Left Column i = 0, Right Column i = 111, 
                    // Top Row j = 0, Bot Row j = 111;
                    // Corners = TL 0,0 | TR 111, 0 | BL 0,111 | BL 111,111

                    // Top Left Corner Logic
                    if (i == 0 && j == 0)
                    {
                        graph[i, j].AdjacentLocations = new Location[] { null, null, null, graph[i + 1, j], graph[i + 1, j + 1], graph[i, j + 1], null, null };
                    }
                    // Top Right Corner
                    else if (i == 111 && j == 0)
                    {
                        graph[i, j].AdjacentLocations = new Location[] { null, null, null, null, null, graph[i, j + 1], graph[i - 1, j + 1], graph[i - 1, j] };
                    }
                    // Bottom Left Corner
                    else if (i == 0 && j == 111)
                    {
                        graph[i, j].AdjacentLocations = new Location[] { null, graph[i, j - 1], graph[i + 1, j - 1], graph[i + 1, j], null, null, null, null };
                    }
                    // Bottom Right Corner
                    else if (i == 111 && j == 111)
                    {
                        graph[i, j].AdjacentLocations = new Location[] { graph[i - 1, j - 1], graph[i, j - 1], null, null, null, null, null, graph[i - 1, j] };
                    }
                    // Left Outside (No Corners)
                    else if (i == 0 && (j > 0 && j < 111))
                    {
                        graph[i, j].AdjacentLocations = new Location[] { null, graph[i, j - 1], graph[i + 1, j - 1], graph[i + 1, j], graph[i + 1, j + 1], graph[i, j + 1], null, null };
                    }
                    // Right Outside (No Corners)
                    else if (i == 111 && (j > 0 && j < 111))
                    {
                        graph[i, j].AdjacentLocations = new Location[] { graph[i - 1, j - 1], graph[i, j - 1], null, null, null, graph[i, j + 1], graph[i - 1, j + 1], graph[i - 1, j] };
                    }
                    // Top Outside (No Corners)
                    else if ((i > 0 && i < 111) && (j == 0))
                    {
                        graph[i, j].AdjacentLocations = new Location[] { null, null, null, graph[i + 1, j], graph[i + 1, j + 1], graph[i, j + 1], graph[i - 1, j + 1], graph[i - 1, j] };
                    }
                    // Bot Outside (No Corners)
                    else if ((i > 0 && i < 111) && (j == 0))
                    {
                        graph[i, j].AdjacentLocations = new Location[] { graph[i - 1, j - 1], graph[i, j - 1], graph[i + 1, j - 1], graph[i + 1, j], null, null, null, graph[i - 1, j] };
                    }
                    // Any Other Case on the inside!
                    else
                    {
                        graph[i, j].AdjacentLocations = new Location[] { graph[i - 1, j - 1], graph[i, j - 1], graph[i + 1, j - 1], graph[i + 1, j], graph[i + 1, j + 1], graph[i, j + 1], graph[i - 1, j + 1], graph[i - 1, j] };
                    }
                }
            }

            // The idea is that whenever we check adjacent tile nodes, if the adj node checked is null or it's there but it's bool passable is false, it will mean
            // That locatoin isn't avaliable to be checked and should be skipped. If a node was there, it should be auto-added to the closed list in manager.
            // If it was null, just ignore it. Your at the edge of the graph obviously. 
        }

        /// <summary>
        /// Adj the x and y of every tile in the graph with the screen. This should function like the camera does. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void GraphAdj(int x, int y)
        {

        }
    }
}
