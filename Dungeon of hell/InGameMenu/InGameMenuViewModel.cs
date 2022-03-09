using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Dungeon_of_hell
{
    public class InGameMenuViewModel : ViewModel
    {
    
        public InGameMenuViewModel()
        {
            Name = "InGameMenu";
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
                if (ViewExists("Singleplayer"))
                {
                    RemoveView("Singleplayer");
                }
                if (ViewExists("Multiplayer"))
                {
                    RemoveView("Multiplayer");
                }
                ChangePrimaryView("MainMenu");
                ClearSecondView();
                RemoveView("InGameMenu");
            });
        }
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
