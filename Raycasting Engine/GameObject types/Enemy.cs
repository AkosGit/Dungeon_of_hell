using Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Utils;

namespace Raycasting_Engine.GameObject_types
{
    class Enemy : MovableEntityObject
    {
        bool isActive;
        bool isAlive;
        int shootTimeCounter;
        int minTimeToShoot;

        public bool IsActive { get => isActive; }
        public bool IsEnemyDead { get { return health < 1; } }
        public bool CanShoot 
        { 
            get
            {
                if (!isAlive) return false;
                shootTimeCounter++;
                bool back = shootTimeCounter > minTimeToShoot && IsActive;
                if (back) shootTimeCounter = 0;
                return back;
            } 
        }


        public Enemy(int gridX, int gridY, int mapS, string name, double he = 0, double wi = 0, int minTimeToShoot = 30, bool isSolid = false, int a = 0) 
            : base(gridX, gridY, mapS, name, he, wi, isSolid, a)
        {
            Sounds = new Dictionary<Audio_player.EnitySound, List<string>>();
            Audio_player.AddTrack("enemy_moving1", "sound\\walking\\walking_wood_2.mp3");
            Sounds[Audio_player.EnitySound.walking] = new List<string>();
            Sounds[Audio_player.EnitySound.walking].Add("enemy_moving1");
            isActive = false;
            this.minTimeToShoot = minTimeToShoot;
            shootTimeCounter = 0;
            isAlive = true;
        }

        public void EnemyIsDead()
		{
            this.height = 149;
            this.width = 596;
            actualTexture = 1;

            isAlive = false;
            isActive = false;
		}

        public void Activate()
		{
            if(isAlive) isActive = true;
		}
        
        public void Move(Vector move, MapObject[] map, int mapX, int mapY)
		{
            dx = move.X / (move.Magnitude);
            dy = move.Y / (move.Magnitude);

            int xo = 0; if (dx < 0) { xo = -20; } else xo = 20;
            int yo = 0; if (dy < 0) { yo = -20; } else yo = 20;
            int ipx = (int)X / 64; int ipx_P_xo = (int)(X + xo) / 64; int ipx_M_xo = (int)(X - xo) / 64;
            int ipy = (int)Y / 64; int ipy_P_yo = (int)(Y + yo) / 64; int ipy_M_yo = (int)(Y - yo) / 64;

            GridX = (int)X / 64;
            GridY = (int)Y / 64;

            if (!map[ipy * mapY + ipx_P_xo].IsSolid) { X += dx; IsMoving = true; }
            if (!map[ipy_P_yo * mapY + ipx].IsSolid) Y += dy;
        }
    }
}
