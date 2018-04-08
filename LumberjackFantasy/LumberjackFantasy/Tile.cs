 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace LumberjackFantasy
{
    //enum for loadState
    enum LoadState { tree, bear, pickUp}

    class Tile
    {
        ///Tile Class By Jacob
        ///Meant to load an individual tile
        ///holds a list of trees and pickups within it as a parameter given to it in a load command
        ///manages things within it.

        //fields ------------------------------------------------------------------------
        private List<Tree> trees;
        private List<Bear> bears;
        private List<PickUp> collectibles;
        private List<Texture2D> pickupTextures;
		private int XOffset;
		private int YOffset;
        LoadState loadState = new LoadState();

        public List<Bear> BearsFromTile
        {
            get { return bears; }
        }
        public List<Tree> TreesFromTile
        {
            get { return trees; }
        }
        public List<PickUp> PickUpsFromTile
        {
            get { return collectibles; }
        }



        //constructor -------------------------------------------------------------------
        public Tile(string toLoad, Texture2D treeTexture, Texture2D bearTexture, List<Texture2D> pickups, Random rng, Quadrent quadrent)
        {
            pickupTextures = pickups;
            LoadTile(toLoad, treeTexture, bearTexture, rng, quadrent);
        }
        //methods -----------------------------------------------------------------------

        //method to load in tile information to be called in constructor
        public void LoadTile(string toLoad, Texture2D treeTexture, Texture2D bearTexture, Random rng, Quadrent quadrent)
        {
			
			switch (quadrent)
			{
				case (Quadrent.UL):
					XOffset = 0;
					YOffset = 0;
					break;
				case (Quadrent.UR):
					XOffset = 896;
					YOffset = 0;
					break;
				case (Quadrent.BL):
					XOffset = 0;
					YOffset = 896;
					break;
				case (Quadrent.BR):
					XOffset = 896;
					YOffset = 896;
					break;
			}



			// Create the data structures
		
            StreamReader load;

            load = new StreamReader(toLoad);
            try
            {
                string line = null;
                while ((line = load.ReadLine()) != null)
                {
                    //if line lists object Type switch state to load that object
                    if(line == "Trees:")
                    {
                        loadState = LoadState.tree;
                    }
                    else if (line == "PickUps:")
                    {
                        loadState = LoadState.pickUp;
                    }
                    else if (line == "Bears:")
                    {
                        loadState = LoadState.bear;
                    }

                    //if line has load information adds an object to list
                    else
                    {
                        //split the line into information
                        string[] split = line.Split(',');

                        switch (loadState)
                        {
                            case LoadState.tree:


                                trees.Add(new Tree
                                    (
                                      Int32.Parse(split[0])+XOffset,
                                      Int32.Parse(split[1]) + YOffset,
                                      Int32.Parse(split[2]),
                                      Int32.Parse(split[3]),
                                      treeTexture,
                                      Int32.Parse(split[4]),
                                      Int32.Parse(split[5])

                                    )
                                    );
                                break;

                            case LoadState.bear:
								bears.Add(new Bear
                                    (
                                    Int32.Parse(split[0]) + XOffset,
                                    Int32.Parse(split[1]) + YOffset,
                                    Int32.Parse(split[2]),
									Int32.Parse(split[3]),
									bearTexture,
									Int32.Parse(split[4]),
									Int32.Parse(split[5]),
									Int32.Parse(split[6]),
									Int32.Parse(split[7]),
									Int32.Parse(split[8]),
									rng
                                    )
                                    );
                                break;

							case LoadState.pickUp:
                                collectibles.Add(new PickUp
                                        (
                                        Int32.Parse(split[1]) + XOffset,
                                        Int32.Parse(split[2]) + YOffset,
                                        Int32.Parse(split[3]),
                                        Int32.Parse(split[4]),
                                        pickupTextures[Int32.Parse(split[0])],
										Int32.Parse(split[0])
                                        )
                                        );
                                
                                break;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading: " + e.Message + "\n");
            }
            finally
            {
                load.Close();
            }
        }
    }
}
