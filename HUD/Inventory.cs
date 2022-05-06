﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Rendering;
using Utils;

namespace HUD
{
    
    public class Inventory
    {
        const int HEALTHBARSLOTS = 2;
        public Key[] InvKeys = { Key.D1, Key.D2, Key.D3, Key.D4, Key.D5 };
        List<Item> Items { get; set; }
        public int Slots { get; set; }
        Canvas hud;
        public Item SelectedItem { get; private set; }
        public Inventory(Canvas hud,int slots, Item defitem)
        {
            Items = new List<Item>();
            Items.Add(defitem);
            SelectedItem = defitem;
            this.hud = hud;
            Slots = slots - 2; //minus health bar space
            render();
        }
        public void AddItem(Item item)
        {
            if (Items.Count <= Slots)
            {
                Items.Add(item);
                render();
            }
        }
        public Item GetItemByIndex(int i)
        {
            return Items[i];
        }
        public bool IsitemInInventory(Item item)
        {
            return Items.Contains(item);
        }
        public bool IsitemInInventory(int i)
        {
            if (Items.Count>= i)
            {
                return true;
            }
            return false;
        }
        public void RemoveItem(Item item)
        {
            if (IsitemInInventory(item))
            {
                Items.Remove(item);
                render();
            }
        }
        public List<Item> GetItems()
        {
            return Items;
        }
        public void SelectItem(Item item)
        {
            SelectedItem = item;
            render();
        }
        void render()
        {
            //rerender inventory slots
            double slotHeight = hud.Height / (Slots + HEALTHBARSLOTS);
            //MessageBox.Show(slotHeight.ToString());
            double height = slotHeight * Slots;
            Brush outline;
            Brush Texture;
            for (int i = 0; i < Slots; i++)
            {
                if (IsitemInInventory(i+1) && Items[i] == SelectedItem)
                {
                    outline = Brushes.Black;
                }
                else
                {
                    outline = Brushes.Gray;
                }
                if (IsitemInInventory(i+1))
                {
                    Texture = Items[i].Icon;
                }
                else { Texture = Brushes.Transparent; }
                RGeometry.DrawRectangleNoShadow(hud,0, height - slotHeight, 0,height, hud.Width, height, hud.Width, height-slotHeight, Texture, outline, 4);
                height = height - slotHeight;
            }
        }
    }

    public enum ItemActions
    {
        Shoot,Reload
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
            if (Ammo != 0)
            {
                //wait for reload to complete
                if (!IsReloading)
                {
                    IsReloading = true;
                    Ammo--;
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
    public abstract class FireArm : Item
    {
        protected Random r;
        public int Ammo { get; set; }
        public int Damage { get; set; }
        public int Rounds { get; set; }
        protected int maxrounds;
        protected bool shotIsOngoing;
        public bool IsShooting { get; set; }
        public bool IsReloading { get; set; }

        public Dictionary<Audio_player.WeaponSound, List<string>> Sounds { get; set; }

        public FireArm(string name, int ammo, int rounds, int damage) : base(name)
        {

            Ammo = ammo;
            Damage = damage;
            this.Sounds = new Dictionary<Audio_player.WeaponSound, List<string>>();
            Sounds[Audio_player.WeaponSound.reloading] = new List<string>();
            Sounds[Audio_player.WeaponSound.shooting] = new List<string>();
            Sounds[Audio_player.WeaponSound.walking] = new List<string>();
            this.Rounds = rounds;
            maxrounds = rounds;
            r = new Random();
        }
        public abstract void Shoot();
        public abstract void Walking();
        public abstract void Reload();
        public abstract void Tick();
    }
    public class Item
    {
        public string Name { get; set; }
        public Brush Icon { get; set; }
        public Brush Holding { get; set; }
        public Brush InUse { get; set; }
        public Item(string name)
        {
            Name = name;
        }
    }


}
