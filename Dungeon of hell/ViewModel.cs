using Microsoft.Toolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Utils;
namespace Dungeon_of_hell
{
    public abstract class ViewModel : ObservableObject,IViewModel
    {
        [JsonIgnore]
        public Type viewType { get; set; }
        [JsonIgnore]
        public string Name { get; set; }


        [JsonIgnore]
        public object ViewId { get; set; }
        public event Action closeapp;
        public event Action<string> changeprimaryviewEvent;
        public event Action<string> changesecondaryviewEvent;
        public event Action clearsecondviewEvent;
        public event Func<string, string, object> getviewpropertyEvent;
        public event Action<string, string, object> updateviewpropertyEvent;
        public event Action<IViewModel, Type> addview;
        public event Action<string> removeview;
        public event Func<string,bool> viewexists;
        public event Func<string, IViewModel> getview;
        public event Action<string> resetview;

        public void CloseApp()
        {
            closeapp?.Invoke();
        }
        public IViewModel GetView(string viewname)
        {
            return getview?.Invoke(viewname);
        }
        public void AddView(IViewModel model, Type typeofview)
        {
            addview?.Invoke(model, typeofview);
        }
        public void RemoveView(string viewname)
        {
            removeview?.Invoke(viewname);
        }
        public bool ViewExists(string viewname)
        {
            return (bool)(viewexists?.Invoke(viewname));
        }
        public void ChangePrimaryView(string viewname)
        {
            changeprimaryviewEvent?.Invoke(viewname);
        }

        public void ChangeSecondaryView(string viewname)
        {
            changesecondaryviewEvent?.Invoke(viewname);
        }

        public void ClearSecondView()
        {
            clearsecondviewEvent?.Invoke();
        }

        public T GetViewProperty<T>(string viewname, string property)
        {
            return (T)getviewpropertyEvent?.Invoke(viewname, property);
        }

        public abstract void KeyDown(object sender, KeyEventArgs e);
        public abstract void WhenSwitchedTo();
        public void UpdateViewProperty(string viewname, string property, object value)
        {
            updateviewpropertyEvent?.Invoke(viewname, property,value);
        }

        public void ResetView(string viewname)
        {
            resetview?.Invoke(viewname);
        }
    }
}
