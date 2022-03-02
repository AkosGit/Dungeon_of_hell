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
namespace Window_Manager
{
    public class Window_manager : ObservableObject, IWindowManager
    {
        public Window_manager()
        {
            viewModels = new List<IViewModel>();

        }
        private IViewModel primaryviewmodel;
        public IViewModel PrimaryViewModel { get { return primaryviewmodel; } set { SetProperty(ref primaryviewmodel, value); } }
        private IViewModel secondaryviewmodel;
        public IViewModel SecondaryViewModel { get { return secondaryviewmodel; } set { SetProperty(ref secondaryviewmodel, value); } }
        public List<IViewModel> viewModels { get; set; }
        const string FILEPATH = "C:\\Users\\Akos\\Documents";

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
        void Write<T>(string path, T contents) { File.WriteAllText(path, JsonSerializer.Serialize(contents)); }
        public void SaveStates()
        {
            //TODO: save global settings
            foreach (IViewModel model  in viewModels)
            {
                string path = $"{FILEPATH}/{model.Name}.json";
                if(model is IEngine) { Write(path, (IEngine)model); }
                if (model is IMinigame) { Write(path, (IMinigame)model); }
            }
        }
        T Read<T>(string path) { return JsonSerializer.Deserialize<T>(File.ReadAllText(path)); }
        public void LoadStates()
        {
            for (int i = 0; i < viewModels.Count; i++)
            {
                string path = $"{FILEPATH}/{viewModels[i].Name}.json";
                if (File.Exists(path))
                {
                    if (viewModels[i] is IEngine) { viewModels[i]=(IViewModel) Read<IEngine>(path); }
                    if (viewModels[i] is IMinigame) { viewModels[i] = (IViewModel)Read<IMinigame>(path); }
                }
            }
        }
        void Read2(string path,Type type)
        {
            string[] jsons = JsonSerializer.Deserialize<string[]>(File.ReadAllText(path));
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i].SetValue(JsonSerializer.Deserialize(File.ReadAllText(path), fields[i].FieldType),null);
            }
        }
        public void LoadStates2()
        {
            if (File.Exists(FILEPATH + "/globalSettings.json"))
            {
                Read2(FILEPATH + "/globalSettings.json",typeof(GlobalSettings));
            }
            for (int i = 0; i < viewModels.Count; i++)
            {
                string path = $"{FILEPATH}/{viewModels[i].Name}.json";
                if (File.Exists(path))
                {
                    Read2(path, viewModels[i].GetType());
                }
            }
        }
        void Write(FieldInfo[] fields, string path)
        {
            string[] jsons = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                string json = JsonSerializer.Serialize(fields[i].GetValue(null));
                jsons[i] = json;
            }
            File.WriteAllText(path, JsonSerializer.Serialize(jsons));
        }
        public void SaveState2()
        {
            Type t = typeof(GlobalSettings);
            FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);
            Write(fields, $"{FILEPATH}/globalSettings.json");
            foreach (IViewModel model in viewModels)
            {
                string path = $"{FILEPATH}/{model.Name}.json";
                t = model.GetType();
                if (model is IEngine) { Write(path, t.GetFields(BindingFlags.Public)); }
                if (model is IMinigame) { Write(path, t.GetFields(BindingFlags.Public)); }
            }

        }
    }  
}
