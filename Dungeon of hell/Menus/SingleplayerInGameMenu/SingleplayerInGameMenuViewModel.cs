using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Utils;
namespace Dungeon_of_hell
{
    public class SingleplayerInGameMenuViewModel : ViewModel
    {
    
        public SingleplayerInGameMenuViewModel()
        {
            Name = "SingleplayerInGameMenu";
            Resume = new RelayCommand(() => {
                Audio_player.Play("menuSelect");
                ClearSecondView();
            });
            SettingsView = new RelayCommand(() =>
            {
                Audio_player.Play("menuSelect");
                ClearSecondView();
                ChangePrimaryView("Settings");
            });
            MainMenuView = new RelayCommand(() =>
            {
                Audio_player.Play("menuSelect");
                ObjectManager.Write<ISingleplayer>(GlobalSettings.Settings.AssetsPath + "save\\Singleplayer.json", (ISingleplayer)GetView("Singleplayer"));
                RemoveView("Singleplayer");
                ChangePrimaryView("MainMenu");
                ClearSecondView();
                RemoveView("SingleplayerInGameMenu");
            });
        }
        public string Background { get { return GlobalSettings.Settings.AssetsPath + "img\\MenuBackground.png"; } }
        public ICommand Resume { get; set; }
        public ICommand SettingsView { get; set; }
        public ICommand MainMenuView { get; set; }
        

        public override void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ClearSecondView();
            }
        }
    }
}
