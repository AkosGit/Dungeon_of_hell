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
    public class SettingsViewModel : ViewModel,ISettings
    {
    
        public int volume { get { return GlobalSettings.Settings.Volume; } set { GlobalSettings.Settings.Volume = value; } }
        Dictionary<string, Key> singleplayerBindings;
        public Dictionary<string, Key> SingleplayerBindings {get { return singleplayerBindings; } set { SetProperty(ref singleplayerBindings, value); } }
        string currentBind;
        public SettingsViewModel()
        {
            Name = "Settings";
            ChangeBind = new RelayCommand<object>(Change);
            InGameMenuView = new RelayCommand(() => {
                Audio_player.Play("menuSelect");
                Switch();
            });
            currentBind = "nothing";
            singleplayerBindings = new Dictionary<string, Key>();
            singleplayerBindings.Add("Forward", Key.W);
            singleplayerBindings.Add("Backwards", Key.S);
            singleplayerBindings.Add("Left", Key.A);
            singleplayerBindings.Add("Right", Key.D);
            singleplayerBindings.Add("Use", Key.Space);
        }
        void Change(object key)
        {
            currentBind = (string)key;
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
        public ICommand ChangeBind { get; set; }

        public override void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Switch();
            }
            else
            {
                if(currentBind!= "nothing")
                {
                    Dictionary<string, Key> b;
                    singleplayerBindings[currentBind] = e.Key;
                    currentBind = "nothing";
                }
                
            }
        }
    }
}
