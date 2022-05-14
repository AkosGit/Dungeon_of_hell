using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Media;
using System.Windows.Media;
using System.Windows.Input;
using Utils;
using HUD;
using Dungeon_of_hell.SinglePlayer;

namespace Dungeon_of_hell
{
    public class Window_manager : ObservableRecipient, IWindowManager
    {
        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (SecondaryViewModel == null)
            {
                viewModels[GetindexByName(PrimaryViewModel.Name)].KeyDown(sender, e);
            }
            else
            {
                viewModels[GetindexByName(SecondaryViewModel.Name)].KeyDown(sender, e);
            }
        }
        void AddTracks()
        {
            Audio_player.AddTrack("menuSelect", "sound\\menu_select_1.mp3");
        }
        public Window_manager()
        {
            AddTracks();
            GlobalSettings.Settings = new globalSettings();
            viewModels = new List<IViewModel>();
            if(File.Exists(GlobalSettings.Settings.AssetsPath + "save\\GlobalSettings.json") && !GlobalSettings.Settings.DisableSaving)
            {
                GlobalSettings.Settings = (globalSettings)ObjectManager.Read(GlobalSettings.Settings.AssetsPath + "save\\GlobalSettings", typeof(globalSettings));
            }

        }
        private IViewModel primaryviewmodel;
        public IViewModel PrimaryViewModel { get { return primaryviewmodel; } set { SetProperty(ref primaryviewmodel, value); } }
        private IViewModel secondaryviewmodel;
        public IViewModel SecondaryViewModel { get { return secondaryviewmodel; } set { SetProperty(ref secondaryviewmodel, value); } }
        public List<IViewModel> viewModels { get; set; }
        public void RemoveView(string viewname)
        {
            IViewModel view = viewModels[GetindexByName(viewname)];
            if (PrimaryViewModel!= view && SecondaryViewModel != view)
            {
                Application.Current.Resources.Remove(view.ViewId);
                viewModels.Remove(view);
            }

        }
        public void AddView(IViewModel view, Type viewType)
        {
            Type viewModelType = view.GetType();
            view.viewType = viewType;
            if (File.Exists(GlobalSettings.Settings.AssetsPath + "save\\Settings.json") && view is ISettings && !GlobalSettings.Settings.DisableSaving)
            {
                SettingsViewModel v = (SettingsViewModel)ObjectManager.Read(GlobalSettings.Settings.AssetsPath + "save\\Settings", typeof(SettingsViewModel));
                view = (IViewModel)v;
            }
            else if(view.Name=="Settings")
            {
                //defaults
                ((ISettings)view).SingleplayerBindings.Add(new Binding() { Usecase = EntityActions.Forward, key = Key.W, Message = "W" });
                ((ISettings)view).SingleplayerBindings.Add(new Binding() { Usecase = EntityActions.Backwards, key = Key.S, Message = "S" });
                ((ISettings)view).SingleplayerBindings.Add(new Binding() { Usecase = EntityActions.Left, key = Key.A, Message = "A" });
                ((ISettings)view).SingleplayerBindings.Add(new Binding() { Usecase = EntityActions.Right, key = Key.D, Message = "D" });
                ((ISettings)view).SingleplayerBindings.Add(new Binding() { Usecase = EntityActions.Use, key = Key.E, Message = "E" });
                ((ISettings)view).SingleplayerBindings.Add(new Binding() { Usecase = EntityActions.Shoot, key = Key.K, Message = "K" });
                ((ISettings)view).SingleplayerBindings.Add(new Binding() { Usecase = EntityActions.Reload, key = Key.R, Message = "R" });

            }
            view.closeapp += () => { OnWindowClosing(this, null); Environment.Exit(0); };
            view.resetview += (string viewname) => { ResetView(viewname); };
            view.getview += (string viewname) => { return GetView(viewname); };
            view.addview += (IViewModel model, Type typeofview) => { AddView(model, typeofview); };
            view.removeview += (string viewname) => { RemoveView(viewname); };
            view.viewexists += (string viewname) => { return ViewExists(viewname); };
            view.clearsecondviewEvent += () => { ClearSecondaryView(); };
            view.changeprimaryviewEvent += (string name) => { ChangePrimaryView(name); };
            view.changesecondaryviewEvent += (string name) => { ChangeSecondaryView(name); };
            view.getviewpropertyEvent += (string viewname, string propertyname) => { return GetViewProperty<object>(viewname, propertyname); };
            view.updateviewpropertyEvent += (string viewname, string propertyname, object value) => { UpdateViewProperty(viewname, propertyname, value); };
            //add data template dynamically, required to access view
            const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
            var xaml = String.Format(xamlTemplate, viewModelType.Name, viewType.Name, viewModelType.Namespace, viewType.Namespace);

            var context = new ParserContext();

            context.XamlTypeMapper = new XamlTypeMapper(new string[0]);
            context.XamlTypeMapper.AddMappingProcessingInstruction("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
            context.XamlTypeMapper.AddMappingProcessingInstruction("v", viewType.Namespace, viewType.Assembly.FullName);

            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
            context.XmlnsDictionary.Add("vm", "vm");
            context.XmlnsDictionary.Add("v", "v");
            var template = (DataTemplate)XamlReader.Parse(xaml, context);
            var key = template.DataTemplateKey;
            Application.Current.Resources.Add(key, template);
            view.ViewId = key;
            viewModels.Add(view);
        }
        public void ResetView(string viewname)
        {
            IViewModel v = viewModels[GetindexByName(viewname)];
            viewModels.Remove(v);
            Application.Current.Resources.Remove(v.ViewId);
            AddView(v, v.viewType);
        }
        public IViewModel GetView(string viewname)
        {
            return viewModels[GetindexByName(viewname)];
        }
        int GetindexByName(string name)
        {

            if (viewModels.Any(n => n.Name == name))
            {
                return viewModels.FindIndex(n => n.Name == name);
            }
            throw new Exception("No view can be found with this name!");

        }
        public void ChangePrimaryView(string name)
        {

            PrimaryViewModel = viewModels[GetindexByName(name)];
            PrimaryViewModel.WhenSwitchedTo();
        }
        public void ChangePrimaryView(int index)
        {
            PrimaryViewModel = viewModels[index];
            PrimaryViewModel.WhenSwitchedTo();
        }
        public void ChangeSecondaryView(int index)
        {
            SecondaryViewModel = viewModels[index];
            SecondaryViewModel.WhenSwitchedTo();
        }

        public void ChangeSecondaryView(string name)
        {
            SecondaryViewModel = viewModels[GetindexByName(name)];
            SecondaryViewModel.WhenSwitchedTo();
        }
        public void ClearSecondaryView()
        {
            SecondaryViewModel = null;
            PrimaryViewModel.WhenSwitchedTo();

        }
        public T GetViewProperty<T>(string viewname, string propertyname)
        {
            int index = GetindexByName(viewname);
            PropertyInfo propertyInfo = viewModels[index].GetType().GetProperty(propertyname);
            if (propertyInfo == null) { throw new ArgumentNullException("The property doesn't exists!"); }
            return (T)propertyInfo.GetValue(viewModels[index], null);
        }
        public void UpdateViewProperty<T>(string viewname, string propertyname, T value)
        {
            int index = GetindexByName(viewname);
            PropertyInfo propertyInfo = viewModels[index].GetType().GetProperty(propertyname);
            if (propertyInfo == null) { throw new ArgumentNullException("The property doesn't exists!"); }
            propertyInfo.SetValue(viewModels[index], Convert.ChangeType(value, propertyInfo.PropertyType), null);
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Audio_player.RemoveAll();
            ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "save\\GlobalSettings", GlobalSettings.Settings);
            ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "save\\Settings", (SettingsViewModel)GetView("Settings"));
        }

        public bool ViewExists(string name)
        {
            return viewModels.Any(m => m.Name == name);
        }
    }
}
