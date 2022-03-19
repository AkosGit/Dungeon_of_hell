using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Utils
{
    public class Audio_player
    {
        MediaPlayer mediaplayer;
        //The max distance will determine how loud the sound is going to be, 0 will be the loudest
        //If distance is -1 volume will be mute 
        const float MAXDISTANCE=10f;
        public Audio_player(string path, int distance = 0)
        {

            mediaplayer = new MediaPlayer();
            mediaplayer.Open(new Uri(GlobalSettings.Settings.AssetsPath+path));
            mediaplayer.Volume = CalculateVolume(distance);
        }
        public void Play()
        {
            mediaplayer.Play();
        }
        float CalculateVolume(float distance)
        {
            if (distance == -1) { return 0f; }
            if(distance > MAXDISTANCE)
            {
                throw new InvalidOperationException("Distance is bigger than the maximum!");
            }
            float percent = 1 - distance / MAXDISTANCE;
            if (percent == 0) { percent = 0.05f; }
            return GlobalSettings.Settings.Volume / 100.0f * percent;
        }
        public void UpdateDistance(float distance)
        {
            mediaplayer.Volume = CalculateVolume(distance);
        }
        public void StopPlayback()
        {
            mediaplayer.Stop();
        }
    }
}
