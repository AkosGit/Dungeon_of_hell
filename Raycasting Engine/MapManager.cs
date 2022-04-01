using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Utils;
using Color = System.Windows.Media.Color;

namespace Raycasting_Engine
{
    public class MapManager
    {
        List<Map> Maps;
        public MapManager()
        {

            LoadMaps();
            Map main = new Map()
            {
                map = new MapObject[]
                {
                    new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(),
                    new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                    new Wall(), new Air(), new Air(), new Wall2(), new Air(), new Wall(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Wall2(), new Air(), new Wall(),
                    new Wall(), new Air(), new Wall(), new Wall(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Wall(), new Wall(), new Air(), new Air(), new Air(), new Wall(),
                    new Wall(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                    new Wall(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                    new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                    new Wall(), new Wall(), new Air(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Air(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(),
                    new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                    new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall2(),
                    new Wall(), new Air(), new Air(), new Wall(), new Air(), new Wall(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Wall(), new Air(), new Wall2(),
                    new Wall(), new Air(), new Wall(), new Wall(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Wall(), new Wall(), new Air(), new Air(), new Air(), new Wall2(),
                    new Wall(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Wall2(),
                    new Wall(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Wall(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                    new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                    new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(),
                },
                EntityMap = new EntityObject[16],
                MapName = "Main",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(10, 9, 64)
            };
            AddMap(main);
            Map test = new Map()
            {
                map = new MapObject[]
            {
                new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Door(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall2(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall2(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall2(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall2(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Wall(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wall(),
                new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(),
            },
                MapName = "Test",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(2, 2, 64)
            };
            AddMap(test);
        }
        void LoadMaps()
        {
            Maps = new List<Map>();
            foreach (string file in Directory.GetFiles(GlobalSettings.Settings.AssetsPath + "map"))
            {
                Maps.Add((Map)ObjectManager.Read(file, typeof(Map)));
            }
        }
        public Map GetMap(string mapname)
        {
            return Maps.Where(n => n.MapName == mapname).First();
        }
        public void AddMap(Map map)
        {
            Maps.Add(map);
            if (!GlobalSettings.Settings.DisableSaving)
            {
                ObjectManager.Write(GlobalSettings.Settings.AssetsPath + "map\\" + map.MapName + ".json", map);
            }
        }

    }
    public class Wall : MapObject
    {

        public Wall() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = new Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\test.jpg");
        }
    }
    public class Wall2 : MapObject
    {
        public Wall2() : base(0, 0, Color.FromArgb(255, 226, 107, 139), true) 
        {
            this.image = new Bitmap($"{GlobalSettings.Settings.AssetsPath}img\\test.jpg");
        }
    }
    public class Air : MapObject
    {
        public Air() : base(false) { }
    }
    public class Door : MapObject
    {
        public Door() : base(0, 0, Color.FromArgb(255, 123, 70, 23), true, true) { }
    }
}
