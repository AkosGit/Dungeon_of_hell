using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Utils
{
    public interface ISettings
    {
        public ObservableCollection<Binding> SingleplayerBindings { get; set; }
    }
    public class Binding
    {
        public Key key { get; set; }
        public string Message { get; set; }
        public string Usecase { get; set; }
    }
}
