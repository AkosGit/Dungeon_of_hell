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
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Dungeon_of_hell.SinglePlayer
{
	public class SinglePlayerViewModel : ViewModel
	{
		const int InventorySLOST = 7;

		//for saving
		public Player Player { get { return game.Player; } set { player = value; } }
		[JsonIgnore]
		private Player player;
		public string Mapname { get { return game.Mapname; } set { mapname = value; } }
		//[JsonIgnore]
		public List<Item> Items { get { return game.HUD.Inventory.items; } set { items = value; } }
		List<Item> items;
		[JsonIgnore]
		protected SPMain game;
		private string mapname;
		[JsonIgnore]
		public DispatcherTimer timer1;
		TimeSpan time;
		Boolean StopTimer;
		private Canvas canvas;
		private Canvas hud;
		[JsonIgnore]
		public Canvas HUD { get { return hud; } set { SetProperty(ref hud, value); } }
		[JsonIgnore]
		public Canvas Canvas { get { return canvas; } set { SetProperty(ref canvas, value); } }
		public SinglePlayerViewModel()
		{
			Name = "Singleplayer";
			SetDefaults();
			StartGame();
		}
		public override void KeyDown(object sender, KeyEventArgs e)
		{
			ObservableCollection<Binding> sb = GetViewProperty<ObservableCollection<Binding>>("Settings", "SingleplayerBindings");
			if (e.Key == Key.Escape){ChangeSecondaryView("SingleplayerInGameMenu");}
            else if (game.HUD.Inventory.InvKeys.Contains(e.Key)) { 
				game.HUD.Input(e.Key); 
			}	
		}
		private void StartGame()
		{
			StopTimer = false;
			time = TimeSpan.FromDays(0);
			timer1 = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 90), DispatcherPriority.Normal, delegate
			{
				if (StopTimer)
				{
					//MessageBox.Show("Vége");
				}
				time = time.Add(TimeSpan.FromMilliseconds(1));
				//handle multiple keydowns
				game.HUD.UpdateAmmo();
				game.HUD.UpdateHealth(game.Player.Health);
				game.HUD.UpdateCredit(0);
				var binds = GetViewProperty<ObservableCollection<Binding>>("Settings", "SingleplayerBindings");
				foreach (Binding k in binds)
				{
					if (Keyboard.IsKeyDown(k.key) && game.IsReady)
					{
						if(k.Usecase is EntityActions)
                        {
							game.Player.Move(k.key, game.map, game.mapX, game.mapY, (EntityActions)binds.FirstOrDefault(x => x.key == k.key).Usecase);
							if(game.HUD.Inventory.SelectedItem is FireArm)
                            {
								((FireArm)game.HUD.Inventory.SelectedItem).Walking();
							}
							if ((EntityActions)k.Usecase == EntityActions.Shoot)
							{
								if (game.HUD.Inventory.SelectedItem is FireArm)
								{
									((FireArm)game.HUD.Inventory.SelectedItem).Shoot();
								}
							}
							if ((EntityActions)k.Usecase == EntityActions.Reload)
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
            if (mapname== null)
            {
				mapname = "Main";
			}
			game = new SPMain(canvas,hud,InventorySLOST,new Pistol("pistol", 100, 13, 15),mapname);
			game.HUD.Inventory.AddItem(new Shotgun("shotgun", 80, 8, 40));
			Canvas.Width = 722;
			Canvas.Height = 500;
			Canvas.Background = Brushes.Gray;
			if (player != null)
            {
				//load saved player
				game.Player = (Player)player;
            }
            if (items != null)
            {
				game.HUD.Inventory.items = items;
            }

		}

	}
}
