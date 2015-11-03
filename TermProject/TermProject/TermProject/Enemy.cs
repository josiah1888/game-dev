using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TermProject
{
    public class Enemy : AnimatedObject
    {
        private Func<Vector2> Ai;

        public Enemy(Texture2D loadedTexture, Vector2 position, int frameCount, int framesPerSec, Func<Vector2> ai)
            : base(loadedTexture, position, 0f, 1f, 1f, frameCount, framesPerSec)
        {
            this.Ai = ai;
        }

        public void Update()
        {
            this.position = this.Ai();
        }

    }
}
