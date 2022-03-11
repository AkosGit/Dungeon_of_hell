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
using Utils;
namespace Dungeon_of_hell.Engine
{
    public class EngineViewModel : ViewModel,IEngine
    {
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
			//TODO SA: Párhuzamositani a bemeneteket
			//KeydownCommand = new RelayCommand<KeyEventArgs>(Keydown);
		}
		public override void KeyDown(object sender,KeyEventArgs e)
        {
			game.Player.Move(e.Key, game.map, game.mapX, game.mapY);
		}
		private void StartGame()
		{
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
			canvas = new Canvas();
			game = new Game(Canvas);

			Canvas.Width = 722;
			Canvas.Height = 500;
			Canvas.Background = Brushes.Gray;
		}




	}
}
