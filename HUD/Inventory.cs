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
    public class FireArm : Item
    {
        Random r;
        public int Ammo { get; set; }
        public int Damage { get; set; }
        public int Rounds { get; set; }
        private int maxrounds;
        public bool IsShooting { get; set; }

        public Dictionary<Audio_player.WeaponSound,List<string>> Sounds { get; set; }

        public FireArm(string name, Brush Icon, Brush Holding, Brush InUse, int ammo,int rounds,int damage) :base(name, Icon, Holding, InUse)
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
        void Play(Audio_player.WeaponSound key)
        {
            //check if one of the sound is playing and update distance based on player pos
            bool isPlaying = false;
            foreach (string name in Sounds[key])
            {
                if (Audio_player.IsPlaying(name))
                {
                    isPlaying = true;
                }
            }
            if (!isPlaying)
            {
                //play random sound if non is playing
                string soundname = Sounds[key][r.Next(0, Sounds[key].Count)];
                Audio_player.Play(soundname);
            }
        }
        public virtual void Shoot()
        {
            if (Rounds >= 0)
            {
                
            }
            else
            {
                Rounds--;
                //reqerd for rendering item in hand
                IsShooting = true;
                Play(Audio_player.WeaponSound.shooting);
            }

        }
        public virtual void Walking()
        {
            Play(Audio_player.WeaponSound.walking);

        }
        public virtual void Reload()
        {
            if (Ammo != 0)
            {
                Ammo--;
                Rounds = maxrounds;
                Play(Audio_player.WeaponSound.reloading);
            }
        }
    }
    public class Item
    {
        public string Name { get; set; }
        public Brush Icon { get; set; }
        public Brush Holding { get; set; }
        public Brush InUse { get; set; }
        public Item(string name, Brush Icon,Brush Holding, Brush InUse)
        {
            Name = name;
            this.Icon = Icon;
            this.Holding = Holding;
            this.InUse = InUse;
        }
    }

}
