using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Utils
{
    
    public static class ObjectManager
    {
        public static void Write<T>(string path, T contents)
        {
            if (!GlobalSettings.Settings.DisableSaving)
            {
                File.WriteAllText(path, System.Text.Json.JsonSerializer.Serialize(contents));
            }
        }
        public static object Read(string path, Type type) 
        {
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
