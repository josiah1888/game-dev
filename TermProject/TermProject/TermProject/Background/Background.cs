using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace TermProject
{
    public class Background
    {
        private const int MAX_CLOUDS = 20;

        public List<GameObject> Clouds = new List<GameObject>();

        public Background(ContentManager content)
        {
            for (int i = 0; i < MAX_CLOUDS; i++)
            {
                Clouds.Add(new Cloud(content));
            }
        }

        public void Update()
        {
            this.Clouds.ForEach(i => i.Update());
        }
    }
}
