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
        Random r;
        bool isActive;
        bool isAlive;
        int shootTimeCounter;
        int minTimeToShoot;

        int creditForKill;

        int move;

        public int Credit { get => creditForKill; }

        public bool IsActive { get => isActive; }
        public bool IsEnemyDead { get { return health < 1; } }
        public bool IsAlive { get => isAlive; }

        public bool CanShoot 
        { 
            get
            {
                if (!isAlive) return false;
                shootTimeCounter++;
                bool back = shootTimeCounter > minTimeToShoot && IsActive;
                if (back) { shootTimeCounter = 0; actualTexture = 3; IsShooting = true; }
                return back;
            } 
        }


        public Enemy(int gridX, int gridY, int mapS, string name, double he = 0, double wi = 0, int credit = 4, int minTimeToShoot = 30, bool isSolid = false, int a = 0) 
            : base(gridX, gridY, mapS, name, he, wi, isSolid, a)
        {
            r = new Random();
            Sounds = new Dictionary<Audio_player.EnitySound, List<string>>();

            Audio_player.AddTrack("enemy_moving1", "sound\\walking\\walking_wood_2.mp3");
            Sounds[Audio_player.EnitySound.walking] = new List<string>();
            Sounds[Audio_player.EnitySound.walking].Add("enemy_moving1");

            Sounds[Audio_player.EnitySound.shooting] = new List<string>();
            Sounds[Audio_player.EnitySound.shooting].Add("pistol_shoot_1");
            Sounds[Audio_player.EnitySound.shooting].Add("pistol_shoot_2");
            Sounds[Audio_player.EnitySound.shooting].Add("pistol_shoot_3");
            Sounds[Audio_player.EnitySound.shooting].Add("pistol_shoot_4");

            //Sounds[Audio_player.EnitySound.hurting] = new List<string>();
            //Audio_player.AddTrack("enemy_hurting1", "sound\\hurt1.wav");
            //Sounds[Audio_player.EnitySound.walking].Add("enemy_hurting1");
            //Audio_player.AddTrack("enemy_hurting2", "sound\\hurt1.wav");
            //Sounds[Audio_player.EnitySound.walking].Add("enemy_hurting2");

            isActive = false;
            this.minTimeToShoot = minTimeToShoot;
            shootTimeCounter = 0;
            isAlive = true;
            creditForKill = credit;
            move = 0;
        }

        public void EnemyIsDead()
		{
            this.height = 149;
            this.width = 596;
            if(r.Next(10) > 8) actualTexture = 5;
            else actualTexture = 4;

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

            actualTexture = this.move;
            this.move++; 
            if (this.move == 3) this.move = 0;
            
        }
    }
}
