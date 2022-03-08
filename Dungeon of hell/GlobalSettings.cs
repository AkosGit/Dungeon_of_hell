using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace Dungeon_of_hell
{
    public static class GlobalSettings
    {
        public static globalSettings Settings { get; set; }
    }
    public class globalSettings
    {
        public int Volume { get; set; }
        public string AssetsPath { get; set; }
        public globalSettings()
        {
            //global sound
            Volume = 40;
            AssetsPath = $"{Directory.GetCurrentDirectory()}\\Assets\\";
        }
    }
}
