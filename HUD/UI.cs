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
            UpdateHealth(100);
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
        private void Text(double x, double y, string text, Color color, Color back)
        {
            TextBlock textBlock = new TextBlock();
            StackPanel stack = new StackPanel();
            textBlock.Text = text;
            stack.Width = hud.Width;
            stack.Height = hud.Height / SLOTS;
            stack.Background= new SolidColorBrush(back);
            textBlock.FontSize = 30;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Foreground = new SolidColorBrush(color);
            textBlock.Background = new SolidColorBrush(back);
            stack.Children.Add(textBlock);
            hud.Children.Add(stack);
            Canvas.SetLeft(stack, x);
            Canvas.SetTop(stack, y);
        }
        public void UpdateHealth(int h)
        {
			Health = h;
            Text(0, ((hud.Height / SLOTS) * 5)-5, h.ToString(), Colors.Yellow,Colors.DarkRed );
            //DrawingBrush healthBrush = new DrawingBrush(RUtils.DrawMyText(Health.ToString()));
            //RGeometry.DrawRectangle(hud, 0, (hud.Height / SLOTS) * 6, 0, (hud.Height / SLOTS) * 5, hud.Width, (hud.Height / SLOTS) * 5, hud.Width, (hud.Height / SLOTS) * 6, healthBrush, Brushes.Black, 2);
        }
        public void UpdateAmmo()
        {
            Text(0, (hud.Height / SLOTS) * 6, $"{((FireArm)Inventory.SelectedItem).Ammo}/{((FireArm)Inventory.SelectedItem).Rounds}", Colors.Yellow, Colors.DarkRed);
            //DrawingBrush healthBrush = new DrawingBrush(RUtils.DrawMyText($"{((FireArm)Inventory.SelectedItem).Ammo}/{((FireArm)Inventory.SelectedItem).Rounds}")); 
            //RGeometry.DrawRectangle(hud, 0, hud.Height, 0, (hud.Height / SLOTS) * 6, hud.ActualWidth, (hud.Height / SLOTS) * 6, hud.ActualWidth, hud.Height, healthBrush, Brushes.Black, 2);
        }
	}
}
