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

        public Enemy(int gridX, int gridY, int mapS,string name, bool isSolid = false, int a = 0) : base(gridX, gridY, mapS,name, isSolid, a)
        {
            Sounds = new Dictionary<Audio_player.EnitySound, List<string>>();
            Audio_player.AddTrack("enemy_moving1", "sound\\walking\\walking_wood_2.mp3");
            Sounds[Audio_player.EnitySound.walking] = new List<string>();
            Sounds[Audio_player.EnitySound.walking].Add("enemy_moving1");
        }
    }
}
