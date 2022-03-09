using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dungeon_of_hell
{
    public interface IWindowManager
    {
        public List<IViewModel> viewModels { get; set; }
        //UI binds to it (UserControll)
        public IViewModel PrimaryViewModel { get; set; }
        public IViewModel SecondaryViewModel { get; set; }
        public bool ViewExists(string name);
        public void ChangePrimaryView(int index);
        public void ChangePrimaryView(string name);
        public void ChangeSecondaryView(int index);
        public void ChangeSecondaryView(string name);
        public void ClearSecondaryView();
        public void AddView(IViewModel view,Type viewType);
        public void RemoveView(string viewname);
        public T GetViewProperty<T>(string viewname, string propertyname);
        public void UpdateViewProperty<T>(string viewname, string propertyname, T value);

        //save states to filesystem: settings, player positions etc
        public string FILEPATH { get; set; }
        public void LoadStates();
        public void SaveStates();
        public void OnWindowClosing(object sender, CancelEventArgs e);
    }
}
