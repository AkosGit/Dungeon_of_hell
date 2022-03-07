using System;
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
using Dungeon_of_hell.Engine;
using Dungeon_of_hell.SinglePlayer;

namespace Dungeon_of_hell
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

			IViewModel engine = new EngineViewModel();
			manager.AddView(engine, typeof(EngineView));

			IViewModel singleplayer = new SinglePlayerViewModel();
            manager.AddView(singleplayer, typeof(SinglePlayerView));
			
            
            //manager.ChangePrimaryView("Engine");

            manager.ChangePrimaryView("Singleplayer");
        }
    }
}
