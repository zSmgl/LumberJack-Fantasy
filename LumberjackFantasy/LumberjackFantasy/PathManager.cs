using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumberjackFantasy
{
	class PathManager
	{
        ///<summary>
        ///Jacob LeBerth Jacob Marcovechio
        ///NodeManager class
        ///holds a list of nodes that encompass the games map
        ///tracks groupings of nodes and their neighbors for passability
        /// and handles other node based logic
        ///</summary>

        //fields ------------------------------------------------------------------------
        public List<Location> openList;     // List of unchecked nodes
        public List<Location> closedList;   // List of checked nodes
        public Location current;
        public Location start;
        public Location target;
        int g = 0;

		//properties --------------------------------------------------------------------

		//constructor -------------------------------------------------------------------
        public PathManager()
        {
        }

        /// <summary>
        /// Reset fields for a new bear.
        /// </summary>
        public void ResetForNewBear()
        {
            g = 0;
            openList.Clear();
            closedList.Clear();
            start = null;           // Should be one of the tiles the bear is currently standing on.
        }

        /// <summary>
        /// Finds new Target Tile that should be set on the graph
        /// </summary>
        public void ResetTarget()
        {
            target = null;          // Finds new Target Tile to set to
        }

        // May need to make a method that states if the current obj is on a tile that is impassible, it needs to prioritize finding an empty tile. 

        public void Following()
        {
            Location current = null;

            // start by adding the original position to the open list
            openList.Add(start);

            while (openList.Count > 0)
            {
                // get the square with the lowest F score
                var lowest = openList.Min(l => l.F);
                current = openList.First(l => l.F == lowest);

                // add the current square to the closed list
                closedList.Add(current);

                // remove it from the open list
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path
                if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                    break;
                
                // Gets Adj Tiles
                var adjacentSquares = GetWalkableAdjacentSquares();
                g++;

                foreach (var adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it
                    if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) != null)
                        continue;

                    // if it's not in the open list...
                    if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
                            && l.Y == adjacentSquare.Y) == null)
                    {
                        // compute its score, set the parent
                        adjacentSquare.G = g;
                        adjacentSquare.H = ComputeHScore();
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;

                        // and add it to the open list
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        // test if using the current G score makes the adjacent square's F score
                        // lower, if yes update the parent because it means it's a better path
                        if (g + adjacentSquare.H < adjacentSquare.F)
                        {
                            adjacentSquare.G = g;
                            adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                            adjacentSquare.Parent = current;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a list of potential locations
        /// </summary>
        /// <returns></returns>
        static List<Location> GetWalkableAdjacentSquares()
        {
            var proposedLocations = new List<Location>();
            for (int i = 0; i < current.AdjacentLocations.Length; i++)
            {
                if (current.AdjacentLocations[i] != null)
                {
                    if (current.AdjacentLocations[i].Passable == true)
                    {
                        proposedLocations.Add(current.AdjacentLocations[i]);
                    }
                }
            }

            return proposedLocations;
        }

        /// <summary>
        /// Figures out the H Score
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="targetX"></param>
        /// <param name="targetY"></param>
        /// <returns></returns>
        static int ComputeHScore()
        {
            return Math.Abs(target.X - current.X) + Math.Abs(target.Y - current.Y);
        }

        //methods -----------------------------------------------------------------------


    }
}
