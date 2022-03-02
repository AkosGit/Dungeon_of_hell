using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Window_Manager
{
    public interface IEngine
    {
        //used in settings for deciding to show the main menu or not
        public static bool InGame { get; set; }
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }
        public int CurrentLevel { get; set; }
        public int Score { get; set; }
        //store keybindings the function as key
        public Dictionary<string, Key> KeyBindings { get; set; }
    }
}
