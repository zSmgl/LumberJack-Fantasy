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
    enum LoadState { tree, pickUp}

    class Tile
    {
        ///Tile Class By Jacob
        ///Meant to load an individual tile
        ///holds a list of trees and pickups within it as a parameter given to it in a load command
        ///manages things within it.

        //fields ------------------------------------------------------------------------
        private List<Tree> trees;
        private List<PickUp> collectibles;
        private List<Texture2D> pickupTextures;
        LoadState loadState = new LoadState();

        //properties --------------------------------------------------------------------
        public List<Tree> Trees { get { return trees; } }
        public List<PickUp> Collectibles { get { return collectibles; } }

        //constructor -------------------------------------------------------------------
        public Tile(string toLoad, Texture2D treeTexture, List<Texture2D> pickups)
        {
            pickupTextures = pickups;
            loadTile(toLoad, treeTexture);
        }
        //methods -----------------------------------------------------------------------

        //method to load in tile information to be called in constructor
        public void loadTile(string toLoad, Texture2D treeTexture)
        {
            // Create the data structures
            trees = new List<Tree>();
            collectibles = new List<PickUp>();
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
                                      Int32.Parse(split[0]),
                                      Int32.Parse(split[1]),
                                      Int32.Parse(split[2]),
                                      Int32.Parse(split[3]),
                                      treeTexture,
                                      Int32.Parse(split[4]),
                                      Int32.Parse(split[5])

                                    )
                                    );
                                break;



                            case LoadState.pickUp:

                                //branching if statement for each kind of pickup, the first value in the txt determines pickup type
                                if (split[0] == "0")
                                {
                                    //generic pickup
                                    collectibles.Add(new PickUp
                                        (
                                        Int32.Parse(split[1]),
                                        Int32.Parse(split[2]),
                                        Int32.Parse(split[3]),
                                        Int32.Parse(split[4]),
                                        pickupTextures[0]
                                        )
                                        );
                                }
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
