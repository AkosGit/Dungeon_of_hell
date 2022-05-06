using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Rendering;
namespace HUD
{
    public class UI
    {
        int Health { get; set; }
        Canvas hud { get; set; }
        int SLOTS;
        public Inventory Inventory { get; set; }
        public UI(Canvas hud,int slots, Item defitem)
        {
            this.SLOTS = slots;
            this.hud = hud;
            hud.Height = 500;
            ChangeHealth(100);
            Inventory = new Inventory(hud,SLOTS,defitem);
        }
        public void Input(Key key)
        {
            //items can be selected by pushing the corresponding number 1-5
            int index = 0;
            switch (key)
            {
                case Key.D1:
                    index = 1;
                    break;
                case Key.D2:
                    index = 2;
                    break;
                case Key.D3:
                    index = 3;
                    break;
                case Key.D4:
                    index = 4;
                    break;
                case Key.D5:
                    index = 5;
                    break;
            }

            if (index <= SLOTS)
            {
                if (Inventory.IsitemInInventory(index))
                {
                    Item item = Inventory.GetItemByIndex(index-1);
                    Inventory.SelectItem(item);
                }
            }
        }
		public void ChangeHealth(int h)
        {
			Health = h;
			DrawingBrush healthBrush = new DrawingBrush(RUtils.DrawMyText(Health.ToString()));
            RGeometry.DrawRectangle(hud,0, (hud.Height / SLOTS) * 6, 0, (hud.Height / SLOTS) * 5, hud.Width, (hud.Height / SLOTS) * 5, hud.Width, (hud.Height / SLOTS) * 6, healthBrush, Brushes.Black, 2);			
        }
        public void UpdateAmmo()
        {
            DrawingBrush healthBrush = new DrawingBrush(RUtils.DrawMyText($"{((FireArm)Inventory.SelectedItem).Ammo}/{((FireArm)Inventory.SelectedItem).Rounds}"));
            RGeometry.DrawRectangle(hud, 0, hud.Height, 0, (hud.Height / SLOTS) * 6, hud.ActualWidth, (hud.Height / SLOTS) * 6, hud.ActualWidth, hud.Height, Brushes.DarkRed, Brushes.Black, 2);
            RGeometry.DrawRectangle(hud, 0, hud.Height, 0, (hud.Height / SLOTS) * 6, hud.ActualWidth, (hud.Height / SLOTS) * 6, hud.ActualWidth, hud.Height, healthBrush, Brushes.Black, 2);
        }
	}
}
