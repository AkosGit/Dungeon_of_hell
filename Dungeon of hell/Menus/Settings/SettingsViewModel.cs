using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using Utils;
namespace Dungeon_of_hell
{
    public class SettingsViewModel : ViewModel,ISettings
    {
        [JsonIgnore]
        List<Key> forbiddenKeys = new List<Key>() { Key.Space,Key.Enter };
        [JsonIgnore]
        public int volume { get { return GlobalSettings.Settings.Volume; } set { GlobalSettings.Settings.Volume = value; } }
        public ObservableCollection<Binding> SingleplayerBindings { get; set; }
        [JsonIgnore]
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
            if (currentBind == -1)
            {
                    string skey = (string)key;
                    Binding index = SingleplayerBindings.Where(i => i.Message == skey).First();
                    currentBind = SingleplayerBindings.IndexOf(index);
                    index.Message = "Press a key..";
                    //force ui to update
                    SingleplayerBindings[currentBind] = null;
                    SingleplayerBindings[currentBind] = index;
            }
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
        [JsonIgnore]
        public ICommand InGameMenuView { get; set; }
        [JsonIgnore]
        public ICommand ChangeBind { get; set; }

        public override void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Switch();
            }
            else
            {
                if(currentBind!= -1 && !forbiddenKeys.Contains(e.Key))
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
        public override void WhenSwitchedTo()
        {
        }
    }

}
