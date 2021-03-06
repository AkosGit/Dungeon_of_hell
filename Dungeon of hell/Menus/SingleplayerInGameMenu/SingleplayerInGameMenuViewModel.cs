using Dungeon_of_hell.SinglePlayer;
using HUD;
using Microsoft.Toolkit.Mvvm.Input;
using Raycasting_Engine;
using System;
using System.Collections.Generic;
using System.IO;
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
                Audio_player.StopAll();
                if (Directory.Exists(GlobalSettings.Settings.AssetsPath + "save\\" + "Props"))
                {
                    Directory.Delete(GlobalSettings.Settings.AssetsPath + "save\\" + "Props", true);
                }
                if (Directory.Exists(GlobalSettings.Settings.AssetsPath + "save\\" + "Items"))
                {
                    Directory.Delete(GlobalSettings.Settings.AssetsPath + "save\\" + "Items", true);
                }
                if (Directory.Exists(GlobalSettings.Settings.AssetsPath + "save\\" + "Enemys"))
                {
                    Directory.Delete(GlobalSettings.Settings.AssetsPath + "save\\" + "Enemys", true);
                }
                Directory.CreateDirectory(GlobalSettings.Settings.AssetsPath + "save\\" + "Props");
                Directory.CreateDirectory(GlobalSettings.Settings.AssetsPath + "save\\" + "Items");
                Directory.CreateDirectory(GlobalSettings.Settings.AssetsPath + "save\\" + "Enemys");
                foreach (Item item in GetViewProperty<List<Item>>("Singleplayer", "Items"))
                {

                    ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "save\\Items\\" + item.Name, item);
                }
                foreach (Enemy item in GetViewProperty<List<Enemy>>("Singleplayer", "Enemys"))
                {

                    ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "save\\Enemys\\" + item.Name, item);
                }
                foreach (Props item in GetViewProperty<List<Props>>("Singleplayer", "Props"))
                {

                    ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "save\\Props\\" + item.Name, item);
                }
                ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "\\save\\" + "Player", GetViewProperty<Player>("Singleplayer", "Player"));
                ChangePrimaryView("MainMenu");
                ClearSecondView();
                RemoveView("Singleplayer");
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
        public override void WhenSwitchedTo()
        {
        }
    }
}
