using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Window_Manager
{
    public class OverlayViewModel : ViewModel
    {
    
        public OverlayViewModel()
        {
            Name = "home2";
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
    }
}
