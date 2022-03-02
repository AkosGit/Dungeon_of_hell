using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Window_Manager
{
    public class HomeViewModel : ViewModel
    {

        private string textbox;
        public string Textbox { get { return textbox; } set => SetProperty(ref textbox, value); }
        public ICommand changeView { get; set; }

        public HomeViewModel()
        {
            Name = "home";
            textbox = "józsiiii";
            changeView = new RelayCommand(() => ChangeSecondaryView("home2"));
        }

    }
}
