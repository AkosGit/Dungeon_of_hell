using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
using Utils;

namespace Dungeon_of_hell
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Window_manager manager;
		public MainWindow()
		{

			manager = new Window_manager();
			InitializeComponent();
			DataContext = manager;
			Closing += manager.OnWindowClosing;
			manager.AddView(new MainMenuViewModel(), typeof(MainMenuView));
			manager.AddView(new SettingsViewModel(), typeof(SettingsView));
			manager.ChangePrimaryView("MainMenu");

			string imgSourceFile = $"{System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\"))}Rendering\\Assets\\img\\";
			string imgDestinationFile = $"{GlobalSettings.Settings.AssetsPath}img\\";
			string soundSourceFile = $"{System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\"))}Rendering\\Assets\\sound\\";
			string soundDestinationFile = $"{GlobalSettings.Settings.AssetsPath}sound\\";
			string saveDestination = $"{GlobalSettings.Settings.AssetsPath}save\\";
			if (System.IO.Directory.Exists(imgSourceFile) && System.IO.Directory.Exists(soundSourceFile))
			{
				if (System.IO.Directory.Exists(imgDestinationFile)) System.IO.Directory.Delete(imgDestinationFile, true);
				if (System.IO.Directory.Exists(soundDestinationFile)) System.IO.Directory.Delete(soundDestinationFile, true);

				CopyDirectory(imgSourceFile, imgDestinationFile, true);
				CopyDirectory(soundSourceFile, soundDestinationFile, true);
			}
			if (!Directory.Exists(saveDestination))
			{
				Directory.CreateDirectory(saveDestination);
			}

			if (bool.Parse(ConfigurationManager.AppSettings.Get("IsTest")) == true)
			{
				manager.AddView(new SinglePlayerViewModel(false), typeof(SinglePlayerView));
				manager.ChangePrimaryView("Singleplayer");
			}
		}

		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			manager.KeyDown(sender, e);
		}
		static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
		{
			var dir = new DirectoryInfo(sourceDir);

			DirectoryInfo[] dirs = dir.GetDirectories();

			Directory.CreateDirectory(destinationDir);

			foreach (FileInfo file in dir.GetFiles())
			{
				string targetFilePath = System.IO.Path.Combine(destinationDir, file.Name);
				file.CopyTo(targetFilePath);
			}

			if (recursive)
			{
				foreach (DirectoryInfo subDir in dirs)
				{
					string newDestinationDir = System.IO.Path.Combine(destinationDir, subDir.Name);
					CopyDirectory(subDir.FullName, newDestinationDir, true);
				}
			}
		}
	}
}
