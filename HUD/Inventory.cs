using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Rendering;
namespace HUD
{
    
    public class Inventory
    {
        const int HEALTHBARSLOTS = 2;
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
            if (Items.ElementAtOrDefault(i) != null)
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
                if (IsitemInInventory(i) && Items[i] == SelectedItem)
                {
                    outline = Brushes.Black;
                }
                else
                {
                    outline = Brushes.Gray;
                }
                if (IsitemInInventory(i))
                {
                    Texture = Items[i].Texture;
                }
                else { Texture = Brushes.Transparent; }
                RGeometry.DrawRectangleNoShadow(hud,0, height - slotHeight, 0,height, hud.Width, height, hud.Width, height-slotHeight, Texture, outline, 4);
                height = height - slotHeight;
            }
        }
    }
    public class Item
    {
        public string Name { get; set; }
        public Brush Texture { get; set; }

        public Item(string name, Brush texture)
        {
            Name = name;
            Texture = texture;
        }
    }
}
