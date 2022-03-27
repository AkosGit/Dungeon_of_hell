using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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
			Health = 100;
            Inventory = new Inventory(hud,SLOTS,defitem);
        }
        public void Input(Key key)
        {
            //items can be selected by pushing the corresponding number 1-5
            int index = (int)key;
            if (index <= SLOTS)
            {
                if (Inventory.IsitemInInventory(index-1))
                {
                    Item item = Inventory.GetItemByIndex(index-1);
                    Inventory.SelectItem(item);
                }
            }
        }
		public void ChangeHealth(int h)
        {
			Health = h;
			DrawingBrush healthBrush = new DrawingBrush(Render.DrawMyText(Health.ToString()));
			Render.DrawRectangle(hud,0, 0, hud.Width, 0, 0, hud.Height / SLOTS * 2, hud.Width, hud.Height / SLOTS * 2, healthBrush, Brushes.Black, 2);			
        }


	}
}
