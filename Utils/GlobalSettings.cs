using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace Utils
{
    public static class GlobalSettings
    {
        public static globalSettings Settings { get; set; }
    }
    public class globalSettings: IGlobalSettings
    {
        public int Volume { get; set; }
        public string AssetsPath { get; set; }
        public globalSettings()
        {
            //global sound
            Volume = 40;
            AssetsPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\DungeonOfHell\\Assets\\";
        }
    }
}
