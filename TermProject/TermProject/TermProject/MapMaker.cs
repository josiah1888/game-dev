using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TermProject.Particles;

namespace TermProject
{
    public class MapMaker
    {

        private Dictionary<char, GameObjectType> _Legend;
        private Dictionary<char, GameObjectType> Legend
        {
            get
            {
                if (_Legend == null)
                {
                    _Legend = new Dictionary<char, GameObjectType>();
                    _Legend.Add('*', GameObjectType.SolidTile);
                    _Legend.Add('_', GameObjectType.SemiSolidTile);
                    _Legend.Add('p', GameObjectType.Player);
                    _Legend.Add('f', GameObjectType.Frog);
                    _Legend.Add('e', GameObjectType.Emu);
                    _Legend.Add('s', GameObjectType.SodaGuy);
                    _Legend.Add('d', GameObjectType.Door);
                    _Legend.Add('h', GameObjectType.Hill);
                }
                return _Legend;
            }
        }

        private ContentManager Content;
        private Action<GameObject> DeathAction;

        private enum GameObjectType
        {
            SolidTile,
            SemiSolidTile,
            Player,
            Frog,
            Emu,
            SodaGuy,
            Door,
            Hill,
            Sun
        }

        public MapMaker(ContentManager content, Action<GameObject> deathAction)
        {
            this.Content = content;
            this.DeathAction = deathAction;
        }

        public List<GameObject> ReadMap(string asset)
        {
            List<GameObject> mapObjects = new List<GameObject>();
            try
            {
                List<string> lines = new List<string>(System.IO.File.ReadAllLines("../../../" + asset)); // asset not getting copied to bin folder
                for (int x = 0; x < lines.Count; x++)
                {
                    for (int y = 0; y < lines.Min(i => i.Length); y++)
                    {
                        char mapCode = lines[x][y];

                        if (this.Legend.ContainsKey(mapCode))
                        {
                            GameObjectType gameObjectType = this.Legend[char.ToLower(mapCode)];
                            List<GameObject> gameObjects = new List<GameObject>();
                            switch (gameObjectType)
                            {
                                default:
                                case GameObjectType.SolidTile:
                                    gameObjects.Add(new SolidTile(this.Content, GetPosition(x, y)));
                                    break;
                                case GameObjectType.SemiSolidTile:
                                    gameObjects.Add(new SemiSolidTile(this.Content, GetPosition(x, y)));
                                    break;
                                case GameObjectType.Player:
                                    gameObjects.Add(new Player(this.Content, GetPosition(x, y)));
                                    break;
                                case GameObjectType.Frog:
                                    gameObjects.Add(new Frog(this.Content, GetPosition(x, y)));
                                    break;
                                case GameObjectType.Emu:
                                    gameObjects.Add(new Emu(this.Content, GetPosition(x, y)));
                                    break;
                                case GameObjectType.SodaGuy:
                                    gameObjects.AddRange(SodaGuy.CreateSodaGuysWithCans(this.Content, GetPosition(x, y)));
                                    break;
                                case GameObjectType.Door:
                                    gameObjects.Add(new Door(this.Content, GetPosition(x, y)));
                                    break;
                                case GameObjectType.Hill:
                                    gameObjects.Add(new Hill(this.Content));
                                    break;
                                case GameObjectType.Sun:
                                    gameObjects.Add(new Sun(this.Content));
                                    break;
                            }

                            foreach(GameObject gameObject in gameObjects)
                            {
                                if (lines[x].Length > y + 1)
                                {
                                    string designator = lines[x][y + 1].ToString();
                                    int.TryParse(designator, out gameObject.Designator);
                                }
                                mapObjects.AddRange(gameObjects);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Maybe try to load a different level? reload? Maybe this will just never happen
                throw e;
            }

            mapObjects.ForEach(i => {
                i.LevelObjects = mapObjects;
                i.DeathAction = this.DeathAction;
            });

            return mapObjects;
        }

        public List<GameObject> MakeSplashScreen(string asset)
        {
            return new List<GameObject>() { new GameObject(this.Content.Load<Texture2D>(asset), Vector2.Zero) };
        }

        private Vector2 GetPosition(int x, int y)
        {
            return new Vector2(Tile.SIZE * y, Tile.SIZE * x);
        }
    }
}
