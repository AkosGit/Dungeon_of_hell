using HUD;
using Raycasting_Engine;
using SinglePlayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Utils;
using Rendering;
namespace Dungeon_of_hell.SinglePlayer
{
	public class SinglePlayerViewModel : ViewModel, ISingleplayer
	{
		const int InventorySLOST = 7;
		
		public bool InGame { get; set; }
		public DispatcherTimer timer1;
		TimeSpan time;
		Boolean StopTimer;
		SPMain game;
		private Canvas canvas;
		private Canvas hud;
		public Canvas HUD { get { return hud; } set { SetProperty(ref hud, value); } }
		public Canvas Canvas { get { return canvas; } set { SetProperty(ref canvas, value); } }
		public SinglePlayerViewModel()
		{
			Name = "Singleplayer";
			SetDefaults();
			StartGame();
			//TODO SA: Párhuzamositani a bemeneteket
			//KeydownCommand = new RelayCommand<KeyEventArgs>(Keydown);
		}
		public override void KeyDown(object sender, KeyEventArgs e)
		{
			ObservableCollection<Binding> sb = GetViewProperty<ObservableCollection<Binding>>("Settings", "SingleplayerBindings");
			if (e.Key == Key.Escape){ChangeSecondaryView("SingleplayerInGameMenu");}
			else if (e.Key == Key.E) { game.LoadNextMap(); }
            else if (game.HUD.Inventory.InvKeys.Contains(e.Key)) { 
				game.HUD.Input(e.Key); }	
		}
		private void StartGame()
		{
			StopTimer = false;
			InGame = true;
			time = TimeSpan.FromDays(0);
			timer1 = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 10), DispatcherPriority.Normal, delegate
			{
				if (StopTimer)
				{
					//MessageBox.Show("Vége");
				}
				time = time.Add(TimeSpan.FromMilliseconds(1));
				//handle multiple keydowns
				game.HUD.UpdateAmmo();
				game.HUD.UpdateHealth(game.Player.Health);
				var binds = GetViewProperty<ObservableCollection<Binding>>("Settings", "SingleplayerBindings");
				foreach (Binding k in binds)
				{
					if (Keyboard.IsKeyDown(k.key))
					{
						if(k.Usecase is EntityActions)
                        {
							game.Player.Move(k.key, game.map, game.mapX, game.mapY, (EntityActions)binds.FirstOrDefault(x => x.key == k.key).Usecase);
							if(game.HUD.Inventory.SelectedItem is FireArm)
                            {
								((FireArm)game.HUD.Inventory.SelectedItem).Walking();
							}
						}
						if (k.Usecase is ItemActions)
						{
							if ((ItemActions)k.Usecase == ItemActions.Shoot)
                            {
								if(game.HUD.Inventory.SelectedItem is FireArm) 
								{
									((FireArm)game.HUD.Inventory.SelectedItem).Shoot();
								}
                            }
							if ((ItemActions)k.Usecase == ItemActions.Reload)
							{
								if (game.HUD.Inventory.SelectedItem is FireArm)
								{
									((FireArm)game.HUD.Inventory.SelectedItem).Reload();
								}
							}

						}
					}
				}
				game.DrawTurn();
			}, Application.Current.Dispatcher);
			timer1.Start();
		}
		private void SetDefaults()
		{
			canvas = new Canvas();
			hud = new Canvas();
			hud.Width = 100;
			hud.Height = 722;
			hud.Background = Brushes.DarkRed;

			game = new SPMain(canvas,hud,InventorySLOST,new Pistol("pistol", 100, 13, 15));
			game.HUD.Inventory.AddItem(new Shotgun("shotgun", 80, 8, 50));
			Canvas.Width = 722;
			Canvas.Height = 500;
			Canvas.Background = Brushes.Gray;

		}

	}
}
