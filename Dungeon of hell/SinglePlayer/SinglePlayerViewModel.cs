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
using System.IO;

namespace Dungeon_of_hell.SinglePlayer
{
	public class SinglePlayerViewModel : ViewModel
	{
		const int InventorySLOST = 7;

		//for saving
		public bool LoadSave { get; set; }
		public Player Player { get { return game.Player; }}
		public List<Item> Items { get { return game.HUD.Inventory.items; }}
		public List<Enemy> Enemys { get { return game.entities.Where(z=> !(z is Props)).Select(y=>((Enemy)y)).ToList(); } }
		public List<Props> Props { get { return game.entities.Where(z => z is Props).Select(y => ((Props)y)).ToList(); } }

		protected SPMain game;
		public DispatcherTimer timer1;
		TimeSpan time;
		Boolean StopTimer;
		private Canvas canvas;
		private Canvas hud;
		public ObservableCollection<Binding> binds;
		public Canvas HUD { get { return hud; } set { SetProperty(ref hud, value); } }
		public Canvas Canvas { get { return canvas; } set { SetProperty(ref canvas, value); } } 
		public SinglePlayerViewModel(bool load)
		{
			Name = "Singleplayer";
			LoadSave = load;
			SetDefaults();
			StartGame();
		}
		public override void KeyDown(object sender, KeyEventArgs e)
		{
			ObservableCollection<Binding> sb = GetViewProperty<ObservableCollection<Binding>>("Settings", "SingleplayerBindings");
			if (e.Key == Key.Escape) {
				timer1.Stop();
				ChangeSecondaryView("SingleplayerInGameMenu"); 
			}
			else if (game.HUD.Inventory.InvKeys.Contains(e.Key))
			{
				game.HUD.Input(e.Key);
			}
		}
		private void StartGame()
		{
			StopTimer = false;
			time = TimeSpan.FromDays(0);
			timer1 = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 80), DispatcherPriority.Normal, delegate
			{
				if (StopTimer)
				{
					//MessageBox.Show("Vége");
				}
				time = time.Add(TimeSpan.FromMilliseconds(1));
				//handle multiple keydowns
				game.HUD.UpdateAmmo();
				game.HUD.UpdateHealth(game.Player.Health);
				game.HUD.UpdateCredit(game.Player.Credit);
				binds = GetViewProperty<ObservableCollection<Binding>>("Settings", "SingleplayerBindings");
				foreach (Binding k in binds)
				{
					if (Keyboard.IsKeyDown(k.key) && game.IsReady)
					{
						if (k.Usecase is EntityActions)
						{
							game.Player.Move(k.key, game.map, game.mapX, game.mapY, (EntityActions)binds.FirstOrDefault(x => x.key == k.key).Usecase);
							
							if (game.HUD.Inventory.SelectedItem is FireArm)
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
				if (game.Player.Health <= 0)
				{
					game.Player.Health = 10000;
					timer1.Stop();
					Audio_player.StopAll();
					AddView(new DeathViewModel(), typeof(DeathView));
					ChangePrimaryView("Death");
					RemoveView("Singleplayer");
					RemoveView("SingleplayerInGameMenu");
				}
				if (game.IsWin)
				{
					game.IsWin = false;
					timer1.Stop();
					Audio_player.StopAll();
					if (game.Player.Credit < 62)
					{
						MessageBox.Show($"You have only {game.Player.Credit}, you need 62 credits to complete this semester!");
						ResetView("Singleplayer");
						ChangePrimaryView("Singleplayer");
					}
                    else
                    {
						AddView(new WinViewModel(game.Player.Credit), typeof(WinView));
						ChangePrimaryView("Win"); ;
					}
				}
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
			string map = "map1";
			Player p=null;
			List<EntityObject> entities = new List<EntityObject>();
			if (LoadSave)
            {
				if (File.Exists(GlobalSettings.Settings.AssetsPath + "\\save\\" + "Player.json"))
				{
					p = (Player)ObjectManager.Read(GlobalSettings.Settings.AssetsPath + "\\save\\" + "Player", typeof(Player));
					map = p.Place;
				}
				string[] files = Directory.GetFiles(GlobalSettings.Settings.AssetsPath + "\\save\\Props");
				foreach (string item in files)
				{
					string path = item.Replace(".json", "");
					entities.Add((Props)ObjectManager.Read(path, typeof(Props)));
				}
				files = Directory.GetFiles(GlobalSettings.Settings.AssetsPath + "\\save\\Enemys");
				foreach (string item in files)
				{
					string path = item.Replace(".json", "");
					entities.Add((Enemy)ObjectManager.Read(path, typeof(Enemy)));
				}
			}
			List<Binding> bindings = ((SettingsViewModel)ObjectManager.Read(GlobalSettings.Settings.AssetsPath + "save\\Settings", typeof(SettingsViewModel))).SingleplayerBindings.ToList();
			game = new SPMain(canvas, hud, InventorySLOST, new Pistol("pistol", 100, 13, 15), map, bindings, p, entities);
			game.HUD.Inventory.AddItem(new Shotgun("shotgun", 80, 8, 40));
			Canvas.Width = 722;
			Canvas.Height = 500;
			Canvas.Background = Brushes.Gray;
            if (LoadSave)
            {
				game.HUD.Inventory.items.Clear();
				string[] files = Directory.GetFiles(GlobalSettings.Settings.AssetsPath + "\\save\\Items");
				foreach (string item in files)
				{
					string path = item.Replace(".json", "");
					if(item.Contains("pistol") || item.Contains("shotgun"))
                    {
						if (item.Contains("pistol"))
						{
							Pistol pistol = (Pistol)ObjectManager.Read(path, typeof(Pistol));
							game.HUD.Inventory.AddItem(pistol);

						}
						if (item.Contains("shotgun"))
						{
							game.HUD.Inventory.AddItem((Shotgun)ObjectManager.Read(path, typeof(Shotgun)));

						}
					}
                    else
                    {
						Item i = (Item)ObjectManager.Read(path, typeof(Item));
						i.UpdateBrushes();
						game.HUD.Inventory.AddItem(i);
					}
				}
				game.HUD.Inventory.SelectItem(game.HUD.Inventory.GetItemByIndex(0));
			}
			game.HUD.UpdateAmmo();
		}
		public override void WhenSwitchedTo()
		{
			timer1.Start();
		}

	}
}
