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
        private Camera camera;

        //Constructor -------------------------------------------
        private OpenSeasonManager()
        {
            openSeasonTimer = 60;
            openSeasonLimiter = 0;
            camera = new Camera(0,camera.CamTexture);
        }

        //Methods ------------------------------------------------
        public void SetOpenSeason(List<Bear> bearscurrent)
        {
            for(int i=0;i<bearscurrent.Count;i++)
            {
                bearscurrent[i].MaxSpeed = bearscurrent[i].MaxSpeed * 2;
                bearscurrent[i].VisionStandard = bearscurrent[i].VisionStandard + 10;
                camera.BearMax = bearscurrent[i].MaxSpeed;
            }
        }

        public void EndOpenSeason(List<Bear> bearscurrent, Player pcurrent)
        {
            for (int i = 0; i < bearscurrent.Count; i++)
            {
                bearscurrent[i].MaxSpeed = bearscurrent[i].MaxSpeed * 1 / 2;
                bearscurrent[i].VisionStandard = bearscurrent[i].VisionStandard - 10;
                camera.BearMax = bearscurrent[i].MaxSpeed;
                pcurrent.Attack = AttackVariation.axe;
            }
        }

        public void UpdateTimer(GameTime gameTime)
        {
            UpdateTimer(gameTime);
        }

        public bool OpenSeason()
        {
            if(openSeasonTimer == openSeasonLimiter)
            {
                openSeason = false;
            }
            return openSeason;
        }

        
    }
}
