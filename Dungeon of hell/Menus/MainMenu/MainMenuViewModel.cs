using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Dungeon_of_hell.SinglePlayer;
using Dungeon_of_hell.MultiPlayer;
using Utils;
using System.Threading;

namespace Dungeon_of_hell
{
    public class MainMenuViewModel : ViewModel
    {
        public Audio_player audio;
        public MainMenuViewModel()
        {
            Name = "MainMenu";
            Audio_player audio = new Audio_player("sound\\menu_select_1.mp3");
            SinglePlayerView = new RelayCommand(() =>
            {
                audio.Play();
                if (File.Exists(GlobalSettings.Settings.AssetsPath + "save\\Singleplayer.json"))
                {
                    AddView((IViewModel)ObjectManager.Read(GlobalSettings.Settings.AssetsPath + "save\\Singleplayer.json", typeof(SinglePlayerViewModel)), typeof(SinglePlayerView));
                }
                else
                {
                    AddView(new SinglePlayerViewModel(), typeof(SinglePlayerView));
                }
                AddView(new SingleplayerInGameMenuViewModel(), typeof(SingleplayerInGameMenuView));
                ChangePrimaryView("Singleplayer");
            });
            MultiplayerView = new RelayCommand(() =>
            {
                audio.Play();
                if (File.Exists(GlobalSettings.Settings.AssetsPath + "Save\\Multiplayer.json"))
                {
                        AddView((IViewModel)ObjectManager.Read(GlobalSettings.Settings.AssetsPath + "save\\Multiplayer.json", typeof(MultiPlayerViewModel)), typeof(MultiPlayerView));
                }
                else
                {
                        AddView(new MultiPlayerViewModel(), typeof(MultiPlayerView));
                }                
                AddView(new MultiplayerInGameMenuViewModel(), typeof(MultiplayerInGameMenuView));
                ChangePrimaryView("Multiplayer");
            });
            SettingsView = new RelayCommand(() =>
            {
                audio.Play();
                ChangePrimaryView("Settings");

            });
        }
        public ICommand SinglePlayerView { get; set; }
        public ICommand MultiplayerView { get; set; }
        public ICommand SettingsView { get; set; }

        public override void KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
