using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Dungeon_of_hell.SinglePlayer;
using Utils;
using System.Threading;

namespace Dungeon_of_hell
{
    public class DeathViewModel : ViewModel
    {
        public DeathViewModel()
        {
            Name = "Death";
            Respawn = new RelayCommand(() =>
            {
                Audio_player.Play("menuSelect");
                AddView(new SinglePlayerViewModel(false), typeof(SinglePlayerView));
                AddView(new SingleplayerInGameMenuViewModel(), typeof(SingleplayerInGameMenuView));
                ChangePrimaryView("Singleplayer");
                RemoveView("Death");

            });
            Quit = new RelayCommand(() =>
            {
                Audio_player.Play("menuSelect");
                ChangePrimaryView("MainMenu");
                RemoveView("Death");

            });
        }
        public ICommand Quit { get; set; }
        public ICommand Respawn { get; set; }

        public override void KeyDown(object sender, KeyEventArgs e)
        {

        }

        public override void WhenSwitchedTo()
        {
        }
    }
}
