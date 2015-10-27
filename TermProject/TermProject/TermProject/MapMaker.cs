using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TermProject
{
    public class MapMaker
    {
        public Dictionary<char, Type> Legend;

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
                        if (this.Legend.ContainsKey(lines[x][y]))
                        {
                            Type gameObjectType = this.Legend[lines[x][y]];
                            GameObject gameObject = null;
                            if (gameObjectType == typeof(SemiSolidTile))
                            {
                                gameObject = new SemiSolidTile();
                            }
                            else if (gameObjectType == typeof(SolidTile))
                            {
                                gameObject = new SolidTile();
                            }

                            gameObject.position.X = Tile.SIZE * x;
                            gameObject.position.Y = Tile.SIZE * y;

                            if (gameObject != null) mapObjects.Add(gameObject);
                        }
                        else
                        {
                            //throw new Exception("Invalid character found in map");
                            // for final testing?
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Maybe try to load a different level? reload? Maybe this will just never happen
                throw e;
            }

            return mapObjects;
        }
    }
}
