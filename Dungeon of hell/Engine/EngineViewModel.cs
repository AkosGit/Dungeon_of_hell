using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Raycasting_Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Dungeon_of_hell.Engine
{
    public class EngineViewModel : ViewModel,IEngine
    {
		public Dictionary<string, Key> KeyBindings { get; set; }
		public bool InGame { get; set; }
		DispatcherTimer timer1;
        TimeSpan time;
        Boolean StopTimer;
        Game game;
		private Canvas canvas;
		public Canvas Canvas { get { return canvas; } set { SetProperty(ref canvas, value); } }

		public EngineViewModel()
        {
            Name = "Engine";
			SetDafaults();
			StartGame();
		}
		private void StartGame()
		{
			Canvas canvas  = new Canvas();
			game = new Game(Canvas);

			StopTimer = false;

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
			canvas.Width = 650;
			canvas.Height = 550;
			canvas.Background = Brushes.Gray;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//TODO SA: Párhuzamositani a bemeneteket
			//this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
		}

		void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			game.Player.Move(e.Key, game.map, game.mapX, game.mapY);
		}


	}
}
