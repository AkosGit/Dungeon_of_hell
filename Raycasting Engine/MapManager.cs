using Raycasting_Engine.GameObject_types;
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
            Maps = new List<Map>();

            Map main = GenerateMain();
            Map map1 = GenerateMap1();
            Map map2 = GenerateMap2();
            Map map3 = GenerateMap3();

            AddMap(main);
            AddMap(map1);
            AddMap(map2);
            AddMap(map3);
        }
        #region maps
        Map GenerateMain()
        {
            Map main = new Map()
            {
                map = new MapObject[]
    {
                    new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Air(), new Brick(), new Air(), new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Brick(), new Air(), new Wood(),
                    new Wood(), new Air(), new Wood(), new Wood(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Wood(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Wood(), new Air(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Air(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                    new Wood(), new Air(), new Air(), new Wood(), new Air(), new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Wood(), new Air(), new Brick(),
                    new Wood(), new Air(), new Wood(), new Wood(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Wood(), new Air(), new Air(), new Air(), new Brick(),
                    new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                    new Wood(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Wood(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Wood(),
                    new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(), new Wood(),
    },
                EntityMap = new EntityObject[16],
                MapName = "Main",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(2, 2, 64)
            };

            List<EntityObject> entities = new List<EntityObject>();
            Enemy test = new Enemy(1, 1, main.MapS, "Józsi", 360, 240);
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            test.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(test);
            Enemy test2 = new Enemy(4, 4, main.MapS, "Béla", 360, 240);
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            test2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(test2);

            Props health1 = new Props(2, 2, main.MapS, "Medkit", 120, 230, PropType.heal);
            health1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\medkit.png");
            entities.Add(health1);

            Props key = new Props(4, 4, main.MapS, "BlueKey", 100, 150, PropType.key);
            key.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\keyCard.png");
            entities.Add(key);

            main.EntityMap = entities.ToArray();
            main.SetDefaults();

            return main;
        }
        Map GenerateMap1()
        {
            Map map1 = new Map()
            {
                map = new MapObject[]
            {
                new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Rock(),  new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Rock(), new Door(), new Rock(), new Rock(), new Air(), new Air(), new Rock(),  new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Air(), new Rock(), new Brick(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(),
            },
                MapName = "map1",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(1, 14, 64)
            };
            List<EntityObject> entities = new List<EntityObject>();

            Enemy enemy1 = new Enemy(14, 1, map1.MapS, "Analízis", 360, 240);
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy1);

            Enemy enemy2 = new Enemy(6, 5, map1.MapS, "Dimat", 360, 240);
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy2);

            Enemy enemy3 = new Enemy(1, 5, map1.MapS, "Bev Infó", 360, 240);
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy3);

            Enemy enemy4 = new Enemy(7, 10, map1.MapS, "Villanytan", 360, 240);
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy4);

            Enemy enemy5 = new Enemy(12, 5, map1.MapS, "Makró", 360, 240);
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy5);

            Enemy enemy6 = new Enemy(14, 10, map1.MapS, "Szoftver fejlesztés", 360, 240);
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy6);

            Props health2 = new Props(7, 2, map1.MapS, "Medkit", 120, 230, PropType.heal);
            health2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\medkit_1.png");
            entities.Add(health2);

            Props ammoBox = new Props(11, 13, map1.MapS, "AmmoBox", 120, 230, PropType.ammo);
            ammoBox.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\AmmoBox.png");
            entities.Add(ammoBox);

            Props redKey = new Props(9, 14, map1.MapS, "RedKey", 100, 150, PropType.key);
            redKey.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\KeyCard_1.png");
            entities.Add(redKey);

            Props kredit = new Props(13, 13, map1.MapS, "PlusKredit", 100, 150, PropType.kredit);
            kredit.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Coin.png");
            entities.Add(kredit);

            map1.EntityMap = entities.ToArray();
            map1.SetDefaults();

            return map1;
        }
        Map GenerateMap2()
        {
            Map map1 = new Map()
            {
                map = new MapObject[]
            {
                new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Air(), new Air(),  new Rock(), new Air(), new Air(), new Rock(), new Rock(), new Rock(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),  new Rock(), new Door(), new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Rock(),
                new Rock(), new Rock(), new Rock(), new Rock(), new Air(), new Air(), new Rock(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Rock(), new Rock(), new Rock(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Rock(),  new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Rock(),  new Rock(), new Rock(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Air(), new Rock(), new Air(), new Air(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(),
                new Rock(), new Air(), new Air(), new Rock(), new Rock(), new Rock(), new Rock(),  new Rock(), new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Air(), new Rock(), new Air(), new Air(), new Rock(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Air(), new Rock(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Rock(),
                new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(), new Rock(),
            },
                MapName = "map2",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(7, 8, 64)
            };
            List<EntityObject> entities = new List<EntityObject>();

            Enemy enemy1 = new Enemy(1, 1, map1.MapS, "Analízis", 360, 240);
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy1);

            Enemy enemy2 = new Enemy(1, 4, map1.MapS, "Dimat", 360, 240);
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy2);

            Enemy enemy3 = new Enemy(1, 8, map1.MapS, "Bev Infó", 360, 240);
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy3);

            Enemy enemy4 = new Enemy(6, 13, map1.MapS, "Villanytan", 360, 240);
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy4);

            Enemy enemy5 = new Enemy(12, 9, map1.MapS, "Makró", 360, 240);
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy5);

            Enemy enemy6 = new Enemy(13, 13, map1.MapS, "Szoftver fejlesztés", 360, 240);
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy6);

            Enemy enemy7 = new Enemy(14, 5, map1.MapS, "Szoftver fejlesztés", 360, 240);
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy7);

            Props health2 = new Props(1, 2, map1.MapS, "Medkit", 120, 230, PropType.heal);
            health2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\medkit.png");
            entities.Add(health2);

            Props redKey = new Props(14, 12, map1.MapS, "YellowKey", 100, 150, PropType.key);
            redKey.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\keyCard.png");
            entities.Add(redKey);

            map1.EntityMap = entities.ToArray();
            map1.SetDefaults();

            return map1;
        }
        Map GenerateMap3()
        {
            Map map1 = new Map()
            {
                map = new MapObject[]
            {
                new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(),
                new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(),  new Brick(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(),  new Door(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Door(), new Air(), new Air(), new Air(),  new Brick(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(),  new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(),
                new Brick(), new Brick(), new Brick(), new Brick(), new Air(), new Air(), new Air(),  new Door(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(),  new Brick(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(),  new Brick(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(),  new Brick(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Door(), new Air(), new Air(), new Air(),  new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(),
                new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(),  new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Door(), new Brick(),  new Brick(), new Brick(), new Door(), new Brick(), new Brick(), new Brick(), new Brick(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Air(), new Air(), new Air(), new Air(), new Air(), new Air(),  new Brick(), new Air(), new Air(), new Brick(), new Air(), new Air(), new Air(), new Air(), new Brick(),
                new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(), new Brick(),
            },
                MapName = "map3",
                MapX = 16,
                MapY = 16,
                MapS = 64,
                MaxL = 16,
                Player = new Player(8, 13, 64)
            };
            List<EntityObject> entities = new List<EntityObject>();

            Enemy enemy1 = new Enemy(1, 1, map1.MapS, "Analízis", 360, 240);
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy1);

            Enemy enemy2 = new Enemy(1, 6, map1.MapS, "Dimat", 360, 240);
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy2);

            Enemy enemy3 = new Enemy(1, 10, map1.MapS, "Bev Infó", 360, 240);
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy3.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy3);

            Enemy enemy4 = new Enemy(1, 13, map1.MapS, "Villanytan", 360, 240);
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy4.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy4);

            Enemy enemy5 = new Enemy(13, 2, map1.MapS, "Makró", 360, 240);
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy5.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy5);

            Enemy enemy6 = new Enemy(12, 4, map1.MapS, "Szoftver fejlesztés", 360, 240);
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy6.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy6);

            Enemy enemy7 = new Enemy(10, 8, map1.MapS, "Szoftver fejlesztés", 360, 240);
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy7.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy7);

            Enemy enemy8 = new Enemy(14, 13, map1.MapS, "Szoftver fejlesztés", 360, 240);
            enemy8.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_stand.png");
            enemy8.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_1.png");
            enemy8.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Walk_2.png");
            enemy8.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_shoting.png");
            enemy8.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\Enemy\\Enemy_Dead.png");
            enemy8.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\enemyDead.png");
            entities.Add(enemy8);

            Props health1 = new Props(1, 13, map1.MapS, "Medkit1", 120, 230, PropType.heal);
            health1.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\medkit.png");
            entities.Add(health1);

            Props health2 = new Props(14, 14, map1.MapS, "Medkit2", 120, 230, PropType.heal);
            health2.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\medkit.png");
            entities.Add(health2);


            Props redKey = new Props(1, 3, map1.MapS, "YellowKey", 100, 150, PropType.key);
            redKey.textures.Add($"{GlobalSettings.Settings.AssetsPath}img\\keyCard.png");
            entities.Add(redKey);

            map1.EntityMap = entities.ToArray();
            map1.SetDefaults();

            return map1;
        }

        #endregion


        public Map GetMap(string mapname)
        {
            return Maps.Where(n => n.MapName == mapname).First();
        }
        public void AddMap(Map map)
        {
            Maps.Add(map);
        }

    }
    //hell walls
    public class Rock : MapObject
    {

        public Rock() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 1.jpg";
        }
    }
    //between map
    //between 1-4 when multiple material texture +lava
    public class Between1 : MapObject
    {

        public Between1() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 3.jpg";
        }
    }
    public class Between2 : MapObject
    {

        public Between2() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 4.jpg";
        }
    }
    public class Between3 : MapObject
    {

        public Between3() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 5 .jpg";
        }
    }
    public class Between4 : MapObject
    {

        public Between4() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 8.jpg";
        }
    }
    public class BrickBetween : MapObject
    {

        public BrickBetween() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 8.jpg";
        }
    }
    public class WoodBetween : MapObject
    {

        public WoodBetween() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 8.jpg";
        }
    }
    //uni walls
    //brick + glass + wood
    public class InBetweenUni : MapObject
    {

        public InBetweenUni() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wall 2.jpg";
        }
    }
    public class Window : MapObject
    {

        public Window() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Window Wall .jpg";
        }
    }
    public class Wood : MapObject
    {

        public Wood() : base(0, 0, Color.FromArgb(255, 130, 160, 255), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Wood Wall.jpg";
        }
    }
    public class Brick : MapObject
    {
        public Brick() : base(0, 0, Color.FromArgb(255, 226, 107, 139), true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Walls\\Brick Wall.jpg";
        }
    }
    public class Air : MapObject
    {
        public Air() : base(false) { }
    }
    public class Door : MapObject
    {
        public Door() : base(0, 0, Color.FromArgb(255, 123, 70, 23), true, true)
        {
            this.image = $"{GlobalSettings.Settings.AssetsPath}img\\Door.jpg";
        }

    }
}
