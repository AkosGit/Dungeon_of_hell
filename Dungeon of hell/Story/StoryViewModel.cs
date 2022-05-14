using HUD;
using Raycasting_Engine;
using SinglePlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Utils;
using Rendering;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Media;
using Dungeon_of_hell.SinglePlayer;

namespace Dungeon_of_hell.Story
{
    public class StoryViewModel : ViewModel
    {
        int lettercount;
        int sentencecount;
        string[] source;
        bool extra;
        Random r;
        DispatcherTimer timer;
        public string StoryText { get { return stoytext; } set { SetProperty(ref stoytext, value); } }
        string stoytext;
        public StoryViewModel()
        {
            r = new Random();

            Name = "Story";
            extra = true;
            lettercount = 0;
            sentencecount = 0;
            source = new string[]{
                "My name is Adam.",
                "It's been a week since I dropped out of college.",
                "After 10 semesters of suffering\nI failed near to the end.",
                "My life is in pieces!",
                "The worse thing is that  I'm keep dreaming about it every night.",
                "I go back every night to that place where the subjects\nare chasing me and where I have to defeat them.",
                "The place is called the DUNGEON OF HELL!"
            };
        }
        public void StartGame()
        {
            timer.Stop();
            AddView(new SinglePlayerViewModel(false), typeof(SinglePlayerView));
            AddView(new SingleplayerInGameMenuViewModel(), typeof(SingleplayerInGameMenuView));
            ChangePrimaryView("Singleplayer");
            RemoveView("Story");
        }
        public override void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                timer.Stop();
                StartGame();
            }
        }

        public override void WhenSwitchedTo()
        {
            string[] names = new string[] { $"{GlobalSettings.Settings.AssetsPath}sound\\story\\write_1.mp3", $"{GlobalSettings.Settings.AssetsPath}sound\\story\\write_2.mp3", $"{GlobalSettings.Settings.AssetsPath}sound\\story\\write_3.mp3", $"{GlobalSettings.Settings.AssetsPath}sound\\story\\write_4.mp3"};
            //Audio_player.AddTrack("write_1", $"{GlobalSettings.Settings.AssetsPath}sound\\story\\write_1.mp3");
            //Audio_player.AddTrack("write_2", $"{GlobalSettings.Settings.AssetsPath}sound\\story\\write_2.mp3");
            //Audio_player.AddTrack("write_3", $"{GlobalSettings.Settings.AssetsPath}sound\\story\\write_3.mp3");
            //Audio_player.AddTrack("write_4", $"{GlobalSettings.Settings.AssetsPath}sound\\story\\write_4.mp3");
            //Audio_player.AddTrack("test", $"{GlobalSettings.Settings.AssetsPath}sound\\Dead.waw");
            TimeSpan time = TimeSpan.FromDays(0);
            timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 150), DispatcherPriority.Normal, delegate
            {
                if (sentencecount == source.Length)
                {
                    StartGame();
                }
                else
                {
                    string s = "";
                    for (int i = 0; i < lettercount; i++)
                    {
                        s = s + source[sentencecount][i];
                    }
                    if (extra) { s = s + "  |";extra = false; }
                    else
                    {
                        extra = true;
                    }
                    StoryText = s;
                    lettercount++;
                    if (lettercount == source[sentencecount].Length+1)
                    {
                        sentencecount++;
                        lettercount = 0;
                    }
                    //Audio_player.Play($"write_{r.Next(1, 5)}");
                    MediaPlayer mediaplayer = new MediaPlayer();
                    mediaplayer.Open(new Uri(names[r.Next(0,4)]));
                    mediaplayer.Volume = GlobalSettings.Settings.Volume*0.6;
                    mediaplayer.Play();

                }
                time = time.Add(TimeSpan.FromMilliseconds(1));
            }, Application.Current.Dispatcher);
            timer.Start();
        }
    }
}
