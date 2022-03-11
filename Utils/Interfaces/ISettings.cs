using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Utils
{
    public interface ISettings
    {
        public Dictionary<string, Key> SingleplayerBindings { get; set; }
    }
}
