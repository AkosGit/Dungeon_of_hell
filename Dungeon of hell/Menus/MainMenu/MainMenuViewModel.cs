using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Dungeon_of_hell.SinglePlayer;
using Dungeon_of_hell.Story;
using Utils;
using System.Threading;

namespace Dungeon_of_hell
{
    public class MainMenuViewModel : ViewModel
    {
        public MainMenuViewModel()
        {
            Name = "MainMenu";
            NewGameView = new RelayCommand(() =>
            {
                Audio_player.Play("menuSelect");
                AddView(new StoryViewModel(), typeof(StoryView));
                ChangePrimaryView("Story");
            });
            LoadSaveView = new RelayCommand(() =>
            {
                Audio_player.Play("menuSelect");
                AddView(new SinglePlayerViewModel(true), typeof(SinglePlayerView));
                AddView(new SingleplayerInGameMenuViewModel(), typeof(SingleplayerInGameMenuView));
                ChangePrimaryView("Singleplayer");
            });
            SettingsView = new RelayCommand(() =>
            {
                Audio_player.Play("menuSelect");
                ChangePrimaryView("Settings");

            });
            Quit = new RelayCommand(() =>
            {
                Audio_player.Play("menuSelect");
                CloseApp();

            });
        }
        public string Background { get { return GlobalSettings.Settings.AssetsPath + "img\\MainMenuBackground.png"; } }
        public string Logo { get { return GlobalSettings.Settings.AssetsPath + "img\\logo.png"; } }
        public ICommand NewGameView { get; set; }
        public ICommand Quit { get; set; }
        public ICommand LoadSaveView { get; set; }
        public ICommand MultiplayerView { get; set; }
        public ICommand SettingsView { get; set; }

        public override void KeyDown(object sender, KeyEventArgs e)
        {

        }
        public override void WhenSwitchedTo()
        {
        }
    }
}
