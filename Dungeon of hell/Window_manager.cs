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
namespace Dungeon_of_hell
{
    public class Window_manager : ObservableObject, IWindowManager
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
            if(File.Exists(GlobalSettings.Settings.AssetsPath + "save\\GlobalSettings.json"))
            {
                GlobalSettings.Settings = (globalSettings)ObjectManager.Read(GlobalSettings.Settings.AssetsPath + "save\\GlobalSettings.json", typeof(globalSettings));
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
            Application.Current.Resources.Remove(view.ViewId);
            viewModels.Remove(view);
        }
        public void AddView(IViewModel view, Type viewType)
        {
            Type viewModelType = view.GetType();
            if (File.Exists(GlobalSettings.Settings.AssetsPath + "save\\Settings.json") && view is ISettings)
            {
                view = (IViewModel)ObjectManager.Read(GlobalSettings.Settings.AssetsPath + "save\\Settings.json", typeof(SettingsViewModel));
            }
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
        }
        public void ChangePrimaryView(int index)
        {
            PrimaryViewModel = viewModels[index];
        }
        public void ChangeSecondaryView(int index)
        {
            SecondaryViewModel = viewModels[index];
        }

        public void ChangeSecondaryView(string name)
        {
            SecondaryViewModel = viewModels[GetindexByName(name)];
        }
        public void ClearSecondaryView()
        {
            SecondaryViewModel = null;

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
            if (ViewExists("Singleplayer")){
                ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "save\\Singleplayer.json", (ISingleplayer)viewModels[GetindexByName("Singleplayer")]);
            }
            ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "save\\GlobalSettings.json", GlobalSettings.Settings);
            ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "save\\Settings.json", (ISettings)GetView("Settings"));
        }

        public bool ViewExists(string name)
        {
            return viewModels.Any(m => m.Name == name);
        }
    }
}
