using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Dungeon_of_hell
{
    public static class Audio_player
    {
        static MediaPlayer mediaplayer;
        static Audio_player()
        {
            mediaplayer = new MediaPlayer();
        }
        public static void PlaySound(string path)
        {
            mediaplayer.Open(new Uri(path));
            mediaplayer.Volume = GlobalSettings.Volume / 100.0f;
            mediaplayer.Play();
        }
        public static void StopPlayback()
        {
            mediaplayer.Stop();
        }
    }
}
