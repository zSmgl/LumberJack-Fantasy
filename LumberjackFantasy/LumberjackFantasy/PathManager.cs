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

		//methods -----------------------------------------------------------------------


	}
}
