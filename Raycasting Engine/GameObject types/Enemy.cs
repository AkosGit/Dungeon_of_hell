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
		}

        public void Activate()
		{
            isActive = true;
		}
    }
}
