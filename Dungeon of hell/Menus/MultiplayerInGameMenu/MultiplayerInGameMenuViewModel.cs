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
    public class MultiplayerInGameMenuViewModel : ViewModel
    {
        
        public MultiplayerInGameMenuViewModel()
        {
            Name = "MultiplayerInGameMenu";
            Resume = new RelayCommand(() => {
                ClearSecondView();
            });
            SettingsView = new RelayCommand(() =>
            {
                ClearSecondView();
                ChangePrimaryView("Settings");
            });
            MainMenuView = new RelayCommand(() =>
            {
                ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "Save\\Multiplayer.json", (IMultiplayer)GetView("Multiplayer"));
                RemoveView("Multiplayer");                
                ChangePrimaryView("MainMenu");
                ClearSecondView();
                RemoveView("MultiplayerInGameMenu");
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
