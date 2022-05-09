using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Raycasting_Engine
{
    public class Player : MovableEntityObject, IPlayer
    {
        public int Credit { get; set; }
        Random r;
        int armor;
        public Player(int gridX, int gridY, int mapS, int armor = 14, bool isSolid = false, int a = 0) 
            : base(gridX, gridY, mapS, "Player", 0, 0, isSolid, a)
        {
            x = gridX * mapS + mapS / 2;
            y = gridY * mapS + mapS / 2;
            this.a = a;
            dx = Math.Cos((PI / 180) * this.a);
            dy = Math.Sin((PI / 180) * this.a);

            Sounds = new Dictionary<Audio_player.EnitySound, List<string>>();
            Sounds[Audio_player.EnitySound.walking] = new List<string>();
            Sounds[Audio_player.EnitySound.walking].Add("player_moving2");
            Audio_player.AddTrack("player_moving2", "sound\\walking\\walking_wood_2.mp3");
            Sounds[Audio_player.EnitySound.walking].Add("player_moving3");
            Audio_player.AddTrack("player_moving3", "sound\\walking\\walking_wood_3.mp3");
            Sounds[Audio_player.EnitySound.walking].Add("player_moving4");
            Audio_player.AddTrack("player_moving4", "sound\\walking\\walking_wood_4.mp3");
            Sounds[Audio_player.EnitySound.walking].Add("player_moving5");
            Audio_player.AddTrack("player_moving5", "sound\\walking\\walking_wood_5.mp3");
            Sounds[Audio_player.EnitySound.walking].Add("player_moving6");
            Audio_player.AddTrack("player_moving6", "sound\\walking\\walking_wood_6.mp3");

            r = new Random();
            this.armor = armor;
        }
        
        public void Hit()
		{
            if(r.Next(0,20) > armor)
			{
                health -= r.Next(5, 15);
			}
		}
        public void Heal()
		{
            health = 100;
		}
    }
}
