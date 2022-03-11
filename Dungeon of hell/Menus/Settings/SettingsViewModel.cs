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
    public class SettingsViewModel : ViewModel
    {
    
        public SettingsViewModel()
        {
            Name = "Settings";
            InGameMenuView = new RelayCommand(() => {
                Switch();
            });
        }
        void Switch()
        {
            if (ViewExists("Singleplayer"))
            {
                ChangePrimaryView("Singleplayer");
                ChangeSecondaryView("SingleplayerInGameMenu");
            }
            else if (ViewExists("Multiplayer"))
            {
                ChangePrimaryView("Multiplayer");
                ChangeSecondaryView("MultiplayerInGameMenu");
            }
            else
            {
                ChangePrimaryView("MainMenu");
            }

        }
        public ICommand InGameMenuView { get; set; }

        public override void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Switch();
            }
        }
    }
}
