using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace Utils
{
    
    public static class ObjectManager
    {
        public static void Write<T>(string path, T contents)
        {
            path = path + ".json";
            if (!GlobalSettings.Settings.DisableSaving)
            {
                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(contents, typeof(T), Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            Culture = System.Globalization.CultureInfo.InvariantCulture,
                        }));
            }
        }
        public static object Read(string path, Type type) 
        {
            path = path + ".json";
            if (!GlobalSettings.Settings.DisableSaving)
            {
                string file = File.ReadAllText(path).Replace(".0", "");
                return Newtonsoft.Json.JsonConvert.DeserializeObject(file, type, new JsonSerializerSettings()
                {
                    Culture = System.Globalization.CultureInfo.InvariantCulture
                });
            }
            return null;

        }
    }
}
