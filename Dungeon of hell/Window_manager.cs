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

namespace Dungeon_of_hell
{
    public class Window_manager : ObservableObject, IWindowManager
    {
        const bool ENABLE_SAVING = false;
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
        public Window_manager()
        {
            viewModels = new List<IViewModel>();
            GlobalSettings.Settings = new globalSettings();
            if (ENABLE_SAVING)
            {
                //Location: %APPDATA%\DungeonOfHell
                FILEPATH = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\DungeonOfHell";
                Directory.CreateDirectory(FILEPATH);
                LoadStates();
            }       
        }
        private IViewModel primaryviewmodel;
        public IViewModel PrimaryViewModel { get { return primaryviewmodel; } set { SetProperty(ref primaryviewmodel, value); } }
        private IViewModel secondaryviewmodel;
        public IViewModel SecondaryViewModel { get { return secondaryviewmodel; } set { SetProperty(ref secondaryviewmodel, value); } }
        public List<IViewModel> viewModels { get; set; }
        public string FILEPATH { get; set; }

        public void AddView(IViewModel view ,Type viewType)
        {
            Type viewModelType = view.GetType();
            view.clearsecondviewEvent += () => { ClearSecondaryView(); };
            view.changeprimaryviewEvent += (string name) => { ChangePrimaryView(name); };
            view.changesecondaryviewEvent += (string name) => { ChangeSecondaryView(name); };
            view.getviewpropertyEvent += (string viewname, string propertyname) => { return GetViewProperty<object>(viewname, propertyname); };
            view.updateviewpropertyEvent += (string viewname, string propertyname, object value) => { UpdateViewProperty(viewname, propertyname, value); };
            viewModels.Add(view);
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
        }
        int GetindexByName(string name)
        {

            if(viewModels.Any(n => n.Name == name))
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
        public void UpdateViewProperty<T>(string viewname, string propertyname,T value)
        {
            int index = GetindexByName(viewname);
            PropertyInfo propertyInfo = viewModels[index].GetType().GetProperty(propertyname);
            if (propertyInfo == null) { throw new ArgumentNullException("The property doesn't exists!"); }
            propertyInfo.SetValue(viewModels[index], Convert.ChangeType(value, propertyInfo.PropertyType), null);
        }
        void Write<T>(string path, T contents) {
            File.WriteAllText(path, JsonSerializer.Serialize(contents));
        }
        public void SaveStates()
        {
            string path = $"{FILEPATH}\\GlobalSettings.json";
            Write(path, GlobalSettings.Settings);
            foreach (IViewModel model  in viewModels)
            {
                path = $"{FILEPATH}\\{model.Name}.json";
                if(model is ISingleplayer) { Write(path, (ISingleplayer)model); }
                if (model is IMinigame) { Write(path, (IMinigame)model); }
            }
        }
        object Read(string path, Type type) { return JsonSerializer.Deserialize(File.ReadAllText(path),type); }
        public void LoadStates()
        {
            string path = $"{FILEPATH}\\GlobalSettings.json";
            if (File.Exists(path))
            {
                GlobalSettings.Settings = (globalSettings)Read(path,typeof(globalSettings));
            }
            for (int i = 0; i < viewModels.Count; i++)
            {
                path = $"{FILEPATH}\\{viewModels[i].Name}.json";
                if (File.Exists(path))
                {
                    if (viewModels[i] is ISingleplayer || viewModels[i] is IMinigame) 
                    { 
                        viewModels[i]=(IViewModel) Read(path, viewModels[i].GetType()); 
                    }
                }
            }
        }
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (ENABLE_SAVING)
            {
                SaveStates();
            }
        }
    }  
}
