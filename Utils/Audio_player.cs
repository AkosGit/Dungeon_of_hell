﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Utils
{
    public static class Audio_player{
        static Dictionary<string, Audio> tracks;
        static Audio_player()
        {
            tracks = new Dictionary<string, Audio>();
        }
        public static void AddTrack(string name, string path, int distance = 0,bool removeWhenEnded=false)
        {
            //removeWhenEnded will remove the track from Dictionary when its finished playing
            //The max distance will determine how loud the sound is going to be, 0 will be the loudest
            //If distance is -1 volume will be mute 
            //max distance can specified below as a const
            Audio audio = new Audio(name,path,removeWhenEnded,distance);
            tracks.Add(name, audio);
        }
        public static void RemoveTrack(string name)
        {
            tracks[name].StopPlayback();
            tracks.Remove(name);
        }
        public static void RemoveAll()
        {
            //raised when exiting
            foreach (string key in tracks.Keys)
            {
                tracks[key].StopPlayback();
                tracks.Remove(key);
            }
        }

        public static void StopPlayback(string name)
        {
            tracks[name].StopPlayback();
        }
        public static void Play(string name)
        {
            tracks[name].Play();
        }
        public static void UpdateDistance(string name,float distance)
        {
            //Update distance of object
            tracks[name].UpdateDistance(distance);
        }


    }
    public class Audio
    {
        MediaPlayer mediaplayer;
        const float MAXDISTANCE=10f;
        string path;
        float distance;
        bool removeWhenEnded;
        string name;

        public Audio(string name,string path, bool removeWhenEnded, int distance)
        {
            this.name = name;
            this.removeWhenEnded = removeWhenEnded;
            this.path = path;
            this.distance = distance;
            mediaplayer = new MediaPlayer();
        }
        public void Play()
        {
            mediaplayer.Open(new Uri(GlobalSettings.Settings.AssetsPath + path));
            mediaplayer.Volume = CalculateVolume(distance);
            mediaplayer.Play();
            if (removeWhenEnded)
            {
                mediaplayer.MediaEnded += Remove;
            }
        }
        private async void Remove(object sender, EventArgs e)
        {
            Audio_player.RemoveTrack(name);
        }
        float CalculateVolume(float distance)
        {
            if (distance == -1) { return 0f; }
            if(distance > MAXDISTANCE)
            {
                throw new InvalidOperationException($"Distance is bigger than the maximum of {MAXDISTANCE}!");
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
