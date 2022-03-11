using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dungeon_of_hell
{
    
    public static class ObjectManager
    {
        public static string FILEPATH = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\DungeonOfHell\\";

        public static void Write<T>(string path, T contents)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(contents));

        }
        public static object Read(string path, Type type) 
        { 
            return JsonSerializer.Deserialize(File.ReadAllText(path), type); 
        }
    }
}
