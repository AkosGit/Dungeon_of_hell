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
		DispatcherTimer timer1;
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
			timer1 = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 8), DispatcherPriority.Normal, delegate
			{
				if (StopTimer)
				{
					//MessageBox.Show("Vége");
				}
				time = time.Add(TimeSpan.FromMilliseconds(1));
				//handle multiple keydowns
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

			game = new SPMain(canvas,hud,InventorySLOST,CreatePistol());

			Canvas.Width = 722;
			Canvas.Height = 500;
			Canvas.Background = Brushes.Gray;

		}
		FireArm CreatePistol()
        {
			ImageBrush icon = new ImageBrush(RUtils.ImageSourceFromBitmap(
				new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\Weapon\\Weapon_1_icon.png")));
			ImageBrush holding = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\Weapon\\Weapon_1.png")));
			ImageBrush shoot = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\Weapon\\Weapon_1_shoting.png")));
			FireArm Pistol = new FireArm("pistol", icon, holding, shoot, 100, 10, 30);

			Pistol.Sounds[Audio_player.WeaponSound.reloading].Add("pistol_reload_1");
			Audio_player.AddTrack("pistol_reload_1", "sound\\pistol\\reload_pistol_1.mp3");
			Pistol.Sounds[Audio_player.WeaponSound.reloading].Add("pistol_reload_2");
			Audio_player.AddTrack("pistol_reload_2", "sound\\pistol\\reload_pistol_2.mp3");
			Pistol.Sounds[Audio_player.WeaponSound.shooting].Add("pistol_shoot_1");
			Audio_player.AddTrack("pistol_shoot_1", "sound\\pistol\\shooting_pistol_1.mp3");
			Pistol.Sounds[Audio_player.WeaponSound.shooting].Add("pistol_shoot_2");
			Audio_player.AddTrack("pistol_shoot_2", "sound\\pistol\\shooting_pistol_2.mp3");
			Pistol.Sounds[Audio_player.WeaponSound.shooting].Add("pistol_shoot_3");
			Audio_player.AddTrack("pistol_shoot_3", "sound\\pistol\\shooting_pistol_3.mp3");
			Pistol.Sounds[Audio_player.WeaponSound.shooting].Add("pistol_shoot_4");
			Audio_player.AddTrack("pistol_shoot_4", "sound\\pistol\\shooting_pistol_4.mp3");
			//Pistol.Sounds[Audio_player.WeaponSound.walking].Add("pistol_walking_1");
			//Audio_player.AddTrack("pistol_walking_1", "sound\\pistol\\walking_pistol_1.mp3");
			//Pistol.Sounds[Audio_player.WeaponSound.walking].Add("pistol_walking_2");
			//Audio_player.AddTrack("pistol_walking_2", "sound\\pistol\\walking_pistol_2.mp3");
			//Pistol.Sounds[Audio_player.WeaponSound.shooting].Add("pistol_walking_3");
			//Audio_player.AddTrack("pistol_walking_3", "sound\\pistol\\walking_pistol_3.mp3");
			//Pistol.Sounds[Audio_player.WeaponSound.walking].Add("pistol_walking_4");
			//Audio_player.AddTrack("pistol_walking_4", "sound\\pistol\\walking_pistol_4.mp3");
			//Pistol.Sounds[Audio_player.WeaponSound.walking].Add("pistol_walking_5");
			//Audio_player.AddTrack("pistol_walking_5", "sound\\pistol\\walking_pistol_5.mp3");
			//Audio_player.ChangeVolume("pistol_walking_1", 20);
			//Audio_player.ChangeVolume("pistol_walking_2", 20);
			//Audio_player.ChangeVolume("pistol_walking_3", 20);
			//Audio_player.ChangeVolume("pistol_walking_4", 20);
			return Pistol;
		}
	}
}
