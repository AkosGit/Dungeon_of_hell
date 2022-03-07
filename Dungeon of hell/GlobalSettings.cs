using System;
using System.Collections.Generic;
using System.Linq;
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
        public globalSettings()
        {
            //global sound
            Volume = 100;
        }
    }
}
