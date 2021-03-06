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
using Raycasting_Engine;

namespace Dungeon_of_hell
{
    public class WinViewModel : ViewModel
    {
        const int CredittoWin = 20;
        public WinViewModel(int Credits)
        {
            Name = "Win";
            this.Credits = Credits;
            Respawn = new RelayCommand(() =>
            {
                Audio_player.Play("menuSelect");
                AddView(new SinglePlayerViewModel(false), typeof(SinglePlayerView));
                AddView(new SingleplayerInGameMenuViewModel(), typeof(SingleplayerInGameMenuView));
                ChangePrimaryView("Singleplayer");
                RemoveView("Win");

            });
            Quit = new RelayCommand(() =>
            {

                Audio_player.Play("menuSelect");
                ChangePrimaryView("MainMenu");
                RemoveView("Win");

            });
        }
        public ICommand Quit { get; set; }
        public ICommand Respawn { get; set; }
        public int Credits { get; set; }
        public override void KeyDown(object sender, KeyEventArgs e)
        {

        }
        public override void WhenSwitchedTo()
        {
            RemoveView("Singleplayer");
            RemoveView("SingleplayerInGameMenu");
        }
    }
}
