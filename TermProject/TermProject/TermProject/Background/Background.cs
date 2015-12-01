using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace TermProject
{
    public class Background
    {
        public List<GameObject> BackgroundObjects = new List<GameObject>();
        
        private const int MAX_CLOUDS = 20;

        public Background(ContentManager content)
        {
            for (int i = 0; i < MAX_CLOUDS; i++)
            {
                BackgroundObjects.Add(new Cloud(content));
            }

            BackgroundObjects.Add(new Hill(content));
        }

        public void Update()
        {
            this.BackgroundObjects.ForEach(i => i.Update());
        }
    }
}
