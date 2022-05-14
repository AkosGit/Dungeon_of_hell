using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Rendering;
using Utils;

namespace HUD
{
    
    public class Inventory
    {
        const int HEALTHBARSLOTS = 3;
        public Key[] InvKeys = { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5 };
        public List<Item> items { get; set; }
        public List<Item> Items { get=> items; }
        public int Slots { get; set; }
        Canvas hud;
        public Item SelectedItem { get; private set; }
        public Inventory(Canvas hud,int slots, Item defitem)
        {
            items = new List<Item>();
            items.Add(defitem);
            SelectedItem = defitem;
            this.hud = hud;
            Slots = slots - HEALTHBARSLOTS; //minus health bar space
            render();
        }
        public void AddItem(Item item)
        {
            if (items.Count <= Slots)
            {
                items.Add(item);
                render();
            }
        }
        public Item GetItemByIndex(int i)
        {
            return items[i];
        }
        public bool IsitemInInventory(Item item)
        {
            return items.Contains(item);
        }

        public bool IsitemInInventory(int i)
        {
            if (items.Count>= i)
            {
                return true;
            }
            return false;
        }
        public void RemoveItem(Item item)
        {
            if (IsitemInInventory(item))
            {
                items.Remove(item);
                render();
            }
        }
        public List<Item> GetItems()
        {
            return items;
        }
        public void SelectItem(Item item)
        {
            SelectedItem = item;
            render();
        }
        public void render()
        {
            hud.Children.Clear();
            //rerender inventory slots
            double slotHeight = hud.Height / (Slots + HEALTHBARSLOTS);
            //MessageBox.Show(slotHeight.ToString());
            double height = slotHeight * Slots;
            Brush outline;
            Brush Texture;
            for (int i = 0; i < Slots; i++)
            {
                if (IsitemInInventory(i+1) && items[i] == SelectedItem)
                {
                    outline = Brushes.Black;
                }
                else
                {
                    outline = Brushes.Gray;
                }
                if (IsitemInInventory(i+1))
                {
                    Texture = items[i].Icon;
                }
                else { Texture = Brushes.Transparent; }
                double margin = 4;
                if (i == Slots-1)
                {
                    margin = 0;
                }
                RGeometry.DrawRectangleNoShadow(hud, 0, height - slotHeight +margin, 0, height, hud.Width, height, hud.Width, height - slotHeight+margin, Texture, outline, 5);
                height = height - slotHeight;
            }
        }
    }

    public enum ItemActions
    {
        Shoot,Reload
    }
    public class Shotgun: FireArm
    {
        bool ammo_prepare;
        public Shotgun(string name, int ammo, int rounds, int damage): base(name, ammo, rounds, damage)
        {
            Icon = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\Weapon\\Weapon_3_icon.png")));
            Holding = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\Weapon\\Weapon_3.png")));
            InUse = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\Weapon\\Weapon_3_shoting.png")));
            Sounds[Audio_player.WeaponSound.reloading].Add("shotgun_reload_1");
            Audio_player.AddTrack("shotgun_reload_1", "sound\\shotgun\\shotgun_reload.mp3");

            Sounds[Audio_player.WeaponSound.shooting].Add("shotgun_shoot_1");
            Audio_player.AddTrack("shotgun_shoot_1", "sound\\shotgun\\shotgun_shooting.mp3");
            Sounds[Audio_player.WeaponSound.shooting].Add("shotgun_prepare_ammo");
            Audio_player.AddTrack("shotgun_prepare_ammo", "sound\\shotgun\\shotgun_pammo.mp3");
            
        }
        public override void Shoot()
        {
            
            if (Rounds > 0)
            {

                if (!IsReloading && !shotIsOngoing)
                {
                    //reqerd for rendering item in hand
                    IsShooting = true;
                    if (Rounds == 1)
                    {
                    }
                    else
                    {
                        string soundname = Sounds[Audio_player.WeaponSound.shooting][r.Next(0, Sounds[Audio_player.WeaponSound.shooting].Count - 1)];
                        Audio_player.Play(soundname);
                    }
                    Rounds=Rounds-2;
                }
            }
        }
        public override void Walking()
        {

        }
        public override void Reload()
        {
            if (Ammo - (maxrounds - Rounds)>=0 && Rounds!=maxrounds)
            {
                //wait for reload to complete
                if (!IsReloading)
                {
                    IsReloading = true;
                    Ammo = Ammo - (maxrounds - Rounds);
                    Rounds = maxrounds;
                    Audio_player.Play("shotgun_reload_1");
                }
            }
        }

        public override void Tick()
        {
            //for long events that takes multiple ticks
            if (!Audio_player.IsPlaying("shotgun_reload_1"))
            {
                IsReloading = false;
            }
            bool temp = false;
            foreach (var item in Sounds[Audio_player.WeaponSound.shooting])
            {
                if (Audio_player.IsPlaying(item)) { temp = true; }
            }
            if (temp) { 
                shotIsOngoing = true;
            }
            else {
                //directly after shot
                if (Rounds>=2&& shotIsOngoing && !ammo_prepare)
                {
                    Audio_player.Play("shotgun_prepare_ammo");
                    ammo_prepare = true;
                }
                if (ammo_prepare && !Audio_player.IsPlaying("shotgun_prepare_ammo"))
                {
                    shotIsOngoing = false;
                    ammo_prepare = false;
                }
            }
        }
    }
    public class Pistol: FireArm
    {
        public Pistol(string name,int ammo, int rounds, int damage): base(name, ammo, rounds, damage)
        {
            Icon = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\Weapon\\Weapon_1_icon.png")));
            Holding = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\Weapon\\Weapon_1.png")));
            InUse = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\Weapon\\Weapon_1_shoting.png")));
            Sounds[Audio_player.WeaponSound.reloading].Add("pistol_reload_1");
            Audio_player.AddTrack("pistol_reload_1", "sound\\pistol\\reload_pistol_1.mp3");

            Sounds[Audio_player.WeaponSound.shooting].Add("pistol_shoot_1");
            Audio_player.AddTrack("pistol_shoot_1", "sound\\pistol\\shooting_pistol_1.mp3");
            Sounds[Audio_player.WeaponSound.shooting].Add("pistol_shoot_2");
            Audio_player.AddTrack("pistol_shoot_2", "sound\\pistol\\shooting_pistol_2.mp3");
            Sounds[Audio_player.WeaponSound.shooting].Add("pistol_shoot_3");
            Audio_player.AddTrack("pistol_shoot_3", "sound\\pistol\\shooting_pistol_3.mp3");
            Sounds[Audio_player.WeaponSound.shooting].Add("pistol_shoot_4");
            Audio_player.AddTrack("pistol_shoot_4", "sound\\pistol\\shooting_pistol_4.mp3");

            Sounds[Audio_player.WeaponSound.shooting].Add("pistol_last_round");
            Audio_player.AddTrack("pistol_last_round", "sound\\pistol\\empty_shooting_pistol.mp3");

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
        }
        public override void Shoot()
        {
            if (Rounds > 0)
            {

                if (!IsReloading && !shotIsOngoing)
                {
                    //reqerd for rendering item in hand
                    IsShooting = true;
                    if (Rounds == 1)
                    {
                        Audio_player.Play("pistol_last_round");
                    }
                    else
                    {
                        string soundname = Sounds[Audio_player.WeaponSound.shooting][r.Next(0, Sounds[Audio_player.WeaponSound.shooting].Count - 1)];
                        Audio_player.Play(soundname);
                    }
                    Rounds--;
                }
            }
        }
        public override void Walking()
        {

        }
        public override void Reload()
        {
            if (Ammo - (maxrounds - Rounds) >= 0 && Rounds != maxrounds)
            {
                //wait for reload to complete
                if (!IsReloading)
                {
                    IsReloading = true;
                    Ammo = Ammo-(maxrounds - Rounds);
                    Rounds = maxrounds;
                    Audio_player.Play("pistol_reload_1");
                }
            }
        }

        public override void Tick()
        {
            //for long events that takes multiple ticks
            if (!Audio_player.IsPlaying("pistol_reload_1"))
            {
                IsReloading = false;
            }
            bool temp = false;
            foreach (var item in Sounds[Audio_player.WeaponSound.shooting])
            {
                if (Audio_player.IsPlaying(item)) { temp = true; }
            }
            if (temp) { shotIsOngoing = true; }
            else { shotIsOngoing = false; }
        }
    }
    public class FireArm : Item
    {
        [JsonIgnore]
        protected Random r;
        public int Ammo { get; set; }
        public int Damage { get; set; }
        public int Rounds { get; set; }
        public int maxrounds { get; set; }
        public bool shotIsOngoing { get; set; }
        public bool IsShooting { get; set; }
        public bool IsReloading { get; set; }
        [JsonIgnore]
        public Dictionary<Audio_player.WeaponSound, List<string>> Sounds { get; set; }

        public FireArm(string name, int ammo, int rounds, int damage) : base(name)
        {

            Ammo = ammo;
            Damage = damage;
            this.Sounds = new Dictionary<Audio_player.WeaponSound, List<string>>();
            Sounds[Audio_player.WeaponSound.reloading] = new List<string>();
            Sounds[Audio_player.WeaponSound.shooting] = new List<string>();
            Sounds[Audio_player.WeaponSound.walking] = new List<string>();
            Sounds[Audio_player.WeaponSound.other] = new List<string>();
            this.Rounds = rounds;
            maxrounds = rounds;
            r = new Random();
        }
        public virtual void Shoot() { }
        public virtual void Walking() { }
        public virtual void Reload() { }
        public virtual void Tick() { }
    }
    public class Item
    {
        public void UpdateBrushes()
        {
            if (Icon_path != null)
            {
                Icon = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap(Icon_path)));

            }
            if (Holding_path != null)
            {
                Holding = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap(Holding_path)));

            }
            if (InUse_path != null)
            {
                InUse = new ImageBrush(RUtils.ImageSourceFromBitmap(new System.Drawing.Bitmap(InUse_path)));

            }
        }
        public string Name { get; set; }

        public string Icon_path { get; set; }
        [JsonIgnore]
        public Brush Icon { get; set; }
        public string Holding_path { get; set; }
        [JsonIgnore]
        public Brush Holding { get; set; }
        [JsonIgnore]
        public Brush InUse { get; set; }
        public string InUse_path { get; set; }

        public Item(string name)
        {
            Name = name;
        }
    }


}
