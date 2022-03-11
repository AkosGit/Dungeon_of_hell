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

namespace Dungeon_of_hell
{
    public class MainMenuViewModel : ViewModel
    {

        public MainMenuViewModel()
        {
            Name = "MainMenu";
            SinglePlayerView = new RelayCommand(() =>
            {
                if (File.Exists(ObjectManager.FILEPATH + "Singleplayer.json"))
                {
                    AddView((IViewModel)ObjectManager.Read(ObjectManager.FILEPATH + "Singleplayer.json", typeof(SinglePlayerViewModel)), typeof(SinglePlayerView));
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
                if (File.Exists(ObjectManager.FILEPATH + "Multiplayer.json"))
                {
                        AddView((IViewModel)ObjectManager.Read(ObjectManager.FILEPATH+"Multiplayer.json",typeof(MultiPlayerViewModel)), typeof(MultiPlayerView));
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
