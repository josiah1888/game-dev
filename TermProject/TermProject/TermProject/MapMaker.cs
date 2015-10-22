using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TermProject
{
    public class MapMaker
    {
        Dictionary<char, GameObject> Legend = new Dictionary<char,GameObject>();

        List<GameObject> ReadMap(string asset)
        {
            List<GameObject> mapObjects = new List<GameObject>();
            try
            {
                List<string> lines = new List<string>(System.IO.File.ReadAllLines(asset));
                for (int x = 0; x < lines.Min(i => i.Length); x++)
                {
                    for (int y = 0; y < lines.Count; y++)
                    {
                        mapObjects.Add(this.Legend[lines[x][y]]);
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
