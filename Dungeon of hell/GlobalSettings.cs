using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace Window_Manager
{
    public static class GlobalSettings
    {

        static GlobalSettings()
        {
            Volume = 100;
            
        }
        //global sound
        public static int Volume { get; set; }
    }
}
