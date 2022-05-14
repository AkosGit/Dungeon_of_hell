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
using Dungeon_of_hell.SinglePlayer;

namespace Dungeon_of_hell.Story
{
    public class StoryViewModel : ViewModel
    {
        int lettercount;
        int sentencecount;
        string[] source;
        bool extra;
        DispatcherTimer timer;
        public string StoryText { get { return stoytext; } set { SetProperty(ref stoytext, value); } }
        string stoytext;
        public StoryViewModel()
        {
            Name = "Story";
            extra = true;
            lettercount = 0;
            sentencecount = 0;
            source = new string[]{
                "My name is Adam.",
                "It's been a week since I dropped out of college.",
                "After 10 semesters of suffering\nI failed near to the end.",
                "My life is in pieces!",
                "The worse thing is that  I'm dreaming about it every night.",
                "I go back every night to that place where the subjects\nare chasing me and where I have to defeat them.",
                "The place is called DUNGEON OF HELL!"
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
                    if (extra) { s = s + " |";extra = false; }
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
                }
                time = time.Add(TimeSpan.FromMilliseconds(1));
            }, Application.Current.Dispatcher);
            timer.Start();
        }
    }
}
