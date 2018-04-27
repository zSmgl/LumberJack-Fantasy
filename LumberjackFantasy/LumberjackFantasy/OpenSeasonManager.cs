using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LumberjackFantasy
{
    class OpenSeasonManager
    {
        //Fields -----------------------------------------------
        private double openSeasonTimer;
        private double openSeasonLimiter;
        private bool openSeason;
        private int bearMax;

        public int BearMax
        {
            set { bearMax = value; }
        }

        //Constructor -------------------------------------------
        public OpenSeasonManager()
        {
            openSeason = true;
            openSeasonTimer = 2;
            openSeasonLimiter = 0;
        }

        //Methods ------------------------------------------------
        public List<Bear> SetOpenSeasonList(List<Bear> bearscurrent)
        {
            for(int i=0;i<bearscurrent.Count;i++)
            {
                bearscurrent[i].MaxSpeed = bearscurrent[i].MaxSpeed * 2;
                bearscurrent[i].ScoreValue = bearscurrent[i].ScoreValue * -1;
                bearscurrent[i].VisionStandard = bearscurrent[i].VisionStandard + 10;
                bearscurrent[i].BearVision = new Rectangle(bearscurrent[i].BearVision.X - bearscurrent[i].VisionStandard , bearscurrent[i].BearVision.Y - bearscurrent[i].VisionStandard,
                bearscurrent[i].BearVision.Width + (bearscurrent[i].VisionStandard *2) , bearscurrent[i].BearVision.Height + (bearscurrent[i].VisionStandard * 2));
            }
            bearMax = bearscurrent[0].MaxSpeed;
            return bearscurrent;
        }

        public Camera OpenSeasonCamera(Camera camera)
        {
            camera.BearMax = bearMax;
            return camera;
        }

        public Player SetOpenSeasonPlayer(Player pcurrent)
        {
            pcurrent.Attack = AttackVariation.shotgun;
            return pcurrent;
        }

        public List<Bear> EndOpenSeasonList(List<Bear> bearscurrent)
        {
            for (int i = 0; i < bearscurrent.Count; i++)
            {
                bearscurrent[i].MaxSpeed = bearscurrent[i].MaxSpeed * 1 / 2;
                bearscurrent[i].ScoreValue = bearscurrent[i].ScoreValue * -1;
                bearscurrent[i].VisionStandard = bearscurrent[i].VisionStandard - 10;
                bearscurrent[i].BearVision = new Rectangle(bearscurrent[i].BearVision.X + bearscurrent[i].VisionStandard, bearscurrent[i].BearVision.Y + bearscurrent[i].VisionStandard,
                bearscurrent[i].BearVision.Width - (bearscurrent[i].VisionStandard * 2), bearscurrent[i].BearVision.Height - (bearscurrent[i].VisionStandard * 2));
            }
            bearMax = bearscurrent[0].MaxSpeed;
            return bearscurrent;
        }

        public Player EndOpenSeasonPlayer(Player pcurrent)
        {
            pcurrent.Attack = AttackVariation.axe;
            return pcurrent;
        }
        public void UpdateTimer(GameTime gameTime)
        {
            openSeasonTimer -= gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool OpenSeason()
        {
            openSeason = true;
            if(openSeasonTimer <= openSeasonLimiter)
            {
                openSeason = false;
                ResetOpenSeasonTimer();
            }
            return openSeason;
        }

        public void ResetOpenSeasonTimer()
        {
            openSeasonTimer = 2;
        }

        
    }
}
