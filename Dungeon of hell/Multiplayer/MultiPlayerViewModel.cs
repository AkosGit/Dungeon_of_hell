using Raycasting_Engine;
using SinglePlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Utils;
namespace Dungeon_of_hell.MultiPlayer
{
	public class MultiPlayerViewModel : ViewModel, IMultiplayer
	{
		public bool InGame { get; set; }
		DispatcherTimer timer1;
		TimeSpan time;
		Boolean StopTimer;
		SPMain game;
		private Canvas canvas;
		public Canvas Canvas { get { return canvas; } set { SetProperty(ref canvas, value); } }
		public MultiPlayerViewModel()
		{
			Name = "Multiplayer";
			SetDafaults();
			StartGame();
			//TODO SA: Párhuzamositani a bemeneteket
			//KeydownCommand = new RelayCommand<KeyEventArgs>(Keydown);
		}
		public override void KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Escape){ChangeSecondaryView("MultiplayerInGameMenu");}
			if (e.Key == Key.E) { game.LoadNextMap(); }
			game.Player.Move(e.Key, game.map, game.mapX, game.mapY);
		}
		private void StartGame()
		{
			StopTimer = false;
			InGame = true;
			//Audio_player audio = new Audio_player(GlobalSettings.Settings.AssetsPath + "sound\\test.mp3", 10);
			time = TimeSpan.FromDays(0);
			timer1 = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 15), DispatcherPriority.Normal, delegate
			{
				if (StopTimer)
				{
					//MessageBox.Show("Vége");
				}
				time = time.Add(TimeSpan.FromMilliseconds(1));
				game.DrawTurn();
			}, Application.Current.Dispatcher);

			timer1.Start();
		}
		private void SetDafaults()
		{
			canvas = new Canvas();
			game = new SPMain(canvas);

			Canvas.Width = 722;
			Canvas.Height = 500;
			Canvas.Background = Brushes.Gray;
		}
	}
}
