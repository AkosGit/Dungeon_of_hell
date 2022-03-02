﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Window_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Window_manager manager = new Window_manager();
            InitializeComponent();
            DataContext = manager;
            IViewModel home = new HomeViewModel();
            IViewModel home2 = new OverlayViewModel(); 
            manager.AddView(home,typeof(HomeView));
            manager.AddView(home2,typeof(Overlay));
            manager.ChangePrimaryView("home");

        }
    }
}
