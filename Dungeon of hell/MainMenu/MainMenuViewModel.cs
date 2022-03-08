using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Dungeon_of_hell.SinglePlayer;
namespace Dungeon_of_hell
{
    public class MainMenuViewModel : ViewModel
    {

        public MainMenuViewModel()
        {
            Name = "MainMenu";
            SinglePlayerView = new RelayCommand(() =>
            {
                if (!ViewExists("Singleplayer"))
                {
                    AddView(new SinglePlayerViewModel(), typeof(SinglePlayerView));
                    AddView(new InGameMenuViewModel(), typeof(InGameMenuView));
                }
                ChangePrimaryView("Singleplayer");
            });
            MultiplayerView = new RelayCommand(() =>
            {
                //TODO
                if (!ViewExists("Multiplayer"))
                {
                    //AddView(new MultiplayerViewModel(), typeof(MultiplayerView));
                    //AddView(new InGameMenuViewModel(), typeof(InGameMenuView));
                }
                ChangePrimaryView("MultiPlayer");

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
