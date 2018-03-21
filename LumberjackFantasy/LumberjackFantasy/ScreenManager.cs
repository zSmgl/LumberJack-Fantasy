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
    class ScreenManager
    {
        //No difficulty button put in yet
        
        //Fields
        //Texture Fields
        private Texture2D startScreenBackground;
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D health;
        private Texture2D hiScore;
        private Texture2D gameScreenBackground;
        private Texture2D playerPortrait;
        private Texture2D gameOverScreenBackground;
        private Texture2D menuReturnButton;
        private Texture2D pauseScreenBackground;
        
        //Vector Fields
        private Vector2 ssBGPos;
        private Vector2 sbPos;
        private Vector2 ebPos;
        private Vector2 hPos;
        private Vector2 hsPos;
        private Vector2 gsBGPos;
        private Vector2 ppPos;
        private Vector2 gosBGPos;
        private Vector2 mrbPos;
        private Vector2 psBGPos;

        //Constructor
        public ScreenManager(Texture2D ssBG, Texture2D sb, Texture2D eb,
            Texture2D h, Texture2D hs, Texture2D gsBG, Texture2D pp, 
            Texture2D gosBG, Texture2D mrb, Texture2D psBG)
        { 
            //Vector Positions are filler for now
            //Start Screen
            startScreenBackground = ssBG;
            ssBGPos.X = 500;
            ssBGPos.Y = 500;
            
            //Start Button
            startButton = sb;
            sbPos.X = 600;
            sbPos.Y = 600;
            
            //Exit Button
            exitButton = eb;
            ebPos.X = 400;
            ebPos.Y = 400;

            //Health


            //HiScore


            //Game Screen Background


            //Player Portrait


            //Game Over Screen Background


            //Menu Return Button


            //Pause Screen Background
        }
        
        //Methods
        public void DrawStartScreen(SpriteBatch spriteBatch)
        {
            //Calls the parameter spritebatch and then draws
            //the background and buttons

            spriteBatch.Begin();

            spriteBatch.Draw(background, ssBGPos, Color.White);
            spriteBatch.Draw(startButton, sbPos, Color.White);
            spriteBatch.Draw(exitButton, ebPos, Color.White);

            spriteBatch.End();
        }
    }
}
