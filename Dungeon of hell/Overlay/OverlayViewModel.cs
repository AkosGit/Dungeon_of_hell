using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Dungeon_of_hell
{
    public class OverlayViewModel : ViewModel
    {
    
        public OverlayViewModel()
        {
            Name = "Overlay";
            changeView = new RelayCommand(() => {
                ClearSecondView();
            });
            PropertyTest = new RelayCommand(() =>
            {
                MessageBox.Show(GetViewProperty<string>("home", "Textbox"));
                UpdateViewProperty("home", "Textbox", "nem józsi");
            });

        }

        public ICommand changeView { get; set; }
        public ICommand PropertyTest { get; set; }

        public override void KeyDown(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
