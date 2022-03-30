using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Binding> SingleplayerBindings { get; set; }
        int currentBind;
        public SettingsViewModel()
        {
            Name = "Settings";
            ChangeBind = new RelayCommand<object>(Change);
            InGameMenuView = new RelayCommand(() => {
                Audio_player.Play("menuSelect");
                Switch();
            });
            currentBind = -1;
            SingleplayerBindings = new ObservableCollection<Binding>();
        }
        void Change(object key)
        {
            Binding index = SingleplayerBindings.Where(i => i.Message == (string)key).First();
            currentBind = SingleplayerBindings.IndexOf(index);
            index.Message = "Press a key..";
            //force ui to update
            SingleplayerBindings[currentBind] = null;
            SingleplayerBindings[currentBind] = index;
            
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
                if(currentBind!= -1)
                {
                    Binding b = SingleplayerBindings[currentBind];
                    b.key = e.Key;
                    b.Message = e.Key.ToString();
                    SingleplayerBindings[currentBind] = null;
                    SingleplayerBindings[currentBind] = b;
                    currentBind = -1;
                }
                
            }
        }
    }

}
