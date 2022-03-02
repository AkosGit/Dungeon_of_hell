using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dungeon_of_hell
{
    public interface IEngine
    {
        //used in settings for deciding to show the main menu or not
        public bool InGame { get; set; }
        //store keybindings the function as key
    }
}