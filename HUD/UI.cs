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
        private void Text(double x, double y, string text, Color color, Color back, string text2)
        {
            
            StackPanel stack = new StackPanel();

            stack.Width = hud.Width-6;
            stack.Height = hud.Height / SLOTS;

            stack.Background= new SolidColorBrush(back);
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = 25;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Foreground = new SolidColorBrush(color);
            textBlock.Background = new SolidColorBrush(back);
            TextBlock textBlock2 = new TextBlock();
            textBlock2.Text = text2;
            textBlock2.FontSize = 25;
            textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock2.VerticalAlignment = VerticalAlignment.Center;
            textBlock2.Foreground = new SolidColorBrush(color);
            textBlock2.Background = new SolidColorBrush(back);
            stack.Children.Add(textBlock);
            stack.Children.Add(textBlock2);
            hud.Children.Add(stack);
            Canvas.SetLeft(stack, x);
            Canvas.SetTop(stack, y);
        }
        public void UpdateHealth(int h)
        {
			Health = h;
            Text(0, (((hud.Height / SLOTS) * 4) + 5), $"Health", Colors.Yellow, Colors.DarkRed, Health.ToString());
        }
        public void UpdateAmmo()
        {
            if (Inventory.SelectedItem is FireArm)
            {
                Text(0, (hud.Height / SLOTS) * 5 + 5, $"Ammo", Colors.Yellow, Colors.DarkRed, $"{((FireArm)Inventory.SelectedItem).Ammo}/{ ((FireArm)Inventory.SelectedItem).Rounds}");

            }
        }
        public void UpdateCredit(int Credit)
        {

            Text(0, ((hud.Height / SLOTS) * 6) + 5, "Credit", Colors.Yellow, Colors.DarkRed, Credit.ToString());
        }
    }
}
