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
	enum ScoreState { loading, getName, viewScore, saving } // Enum determines what should be happening in the gameover screen
	class ScoreboardManager
	{
		/// <summary>
		/// Jacob LeBerth
		/// New class introduced to handle loading in and rewriting high score document
		/// made to keep updatemanager clean
		/// </summary> 

		//fields ------------------------------------------------------------------------
		private int currentScore;           // Holds the players high score for this run
		private int index;                  // Holds where to insert code
		private string playerName;          // Holds The Players Name
		private ScoreState scoreState;      // Holds the current scoreState
		private List<int> highScores;       // Holds a list of previous high scores
		private List<string> scoreNames;    // Holds a list of names tied to high scores
		private KeyboardState currentKB;    // Holds the current keyboard state
		private KeyboardState previousKB;   // Holds the last frames keeyboard state
		bool[] prevPressed;                 // Holda the true false state for each key
		Vector2[] characterLocations;

		//properties --------------------------------------------------------------------
		public int CurrentScore { get { return currentScore; } set { currentScore = value; } }
		//constructor -------------------------------------------------------------------
		public ScoreboardManager()
		{
			scoreNames = new List<string>();
			highScores = new List<int>();
			scoreState = ScoreState.loading;
			prevPressed = new bool[256];
			characterLocations = new Vector2[] { new Vector2(298, 448), new Vector2(298, 448), new Vector2(448, 448) };
			playerName = "--";
		}
			//methods -----------------------------------------------------------------------
			public GameState UpdateGameover() //method for updating the gameover screen
			{
				GameState toReturn = GameState.gameOver;
				currentKB = Keyboard.GetState();
			switch (scoreState)
			{
				case ScoreState.loading:
					LoadScores();
					index = TestHighScore();
					if (index == 10)
					{
						scoreState = ScoreState.viewScore;
					}
					else
					{
						scoreState = ScoreState.getName;
					}

					break;

				case ScoreState.getName:
					if (playerName.Length != 3)
					{ 
					GetName();
					}
					else
					{
						scoreNames.Insert(index, playerName);
						highScores.Insert(index, currentScore);
						scoreNames.RemoveAt(10);
						highScores.RemoveAt(10);
						scoreState = ScoreState.saving;
					}
						break;

					case ScoreState.saving:
						SaveScores();
						scoreState = ScoreState.viewScore;
						break;

					case ScoreState.viewScore:
						if (currentKB.IsKeyDown(Keys.Enter))
						{
							toReturn = GameState.start;
						}
						break;

				}
				previousKB = currentKB;
				return toReturn;
			}

			public void DrawGameOver(SpriteBatch spriteBatch, SpriteFont spriteFont) //method that draws the gameover screen
			{
			switch (scoreState)
			{
				case ScoreState.loading:
					break;

				case ScoreState.getName:
					break;

				case ScoreState.saving:
					break;

				case ScoreState.viewScore:
					break;
			}
			}

			public void LoadScores() //method to load in the current high score list
			{
				StreamReader toLoad = new StreamReader("High_Scores.txt");
				try
				{
					string line = null;
					while ((line = toLoad.ReadLine()) != null)
					{
						string[] split = line.Split(',');
						scoreNames.Add(split[0]);
						highScores.Add(Int32.Parse(split[1]));
					}
				}

				catch (Exception e)
				{
					Console.WriteLine("Error loading: " + e.Message + "\n");
				}
				finally
				{
					toLoad.Close();
				}
			}

			public void SaveScores()
			{
				StreamWriter saveScore = new StreamWriter("High_Scores.txt");
				try
				{
					for (int count = 0; count < 10; count++)
					{
						saveScore.WriteLine(scoreNames[count] + "," + highScores[count]);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("Error saving: " + e.Message + "\n");
				}
				finally
				{
					saveScore.Close();
				}
			}

			public int TestHighScore() //method to see where to place the new score in on a return of 10 it is not high enough.
			{
				int count = 0;
			if (highScores != null)
			{
				foreach (int thisScore in highScores)
				{
					if (currentScore > thisScore)
					{
						break;
					}
					count++;
				}
			}
				return count;
			}

			public void GetName() //method that saves players name
			{
				Keys[] currentPressed = currentKB.GetPressedKeys();
				foreach (Keys key in currentPressed)
				{
						if (prevPressed[(int)key] == false && key >= Keys.A && key <= Keys.Z)
						{
							char inputKey = (char)key;
							if (playerName == "--")
							{
								playerName = inputKey.ToString();
							}
							else
							{
								playerName = playerName + inputKey.ToString();
							}
						}
				}
				System.Array.Clear(prevPressed, 0, prevPressed.Length);
				foreach(Keys key in currentPressed)
				{
					prevPressed[(int)key] = true;
				}
				

			}

		}
	}


