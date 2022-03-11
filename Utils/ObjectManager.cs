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
            //default json writer can't write maps because of cycles but newsoft can't write interfaces
            if(typeof(ISingleplayer).IsAssignableFrom(typeof(T))|| typeof(ISettings).IsAssignableFrom(typeof(T)) || typeof(IMultiplayer).IsAssignableFrom(typeof(T)) || typeof(IMultiplayer).IsAssignableFrom(typeof(T)))
            {
                
                File.WriteAllText(path, System.Text.Json.JsonSerializer.Serialize(contents));
            }
            else
            {
                File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(contents, typeof(T), Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            Culture = System.Globalization.CultureInfo.InvariantCulture,
                        }));
            }


        }
        public static object Read(string path, Type type) 
        {
            string file = File.ReadAllText(path).Replace(".0", "");
            return Newtonsoft.Json.JsonConvert.DeserializeObject(file, type, new JsonSerializerSettings() {
                Culture= System.Globalization.CultureInfo.InvariantCulture
            }); 
        }
    }
}
