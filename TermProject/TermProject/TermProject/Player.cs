using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TermProject
{
    class Player : AnimatedObject
    {
        public Player(Texture2D loadedTexture, Vector2 position, int frameCount, int framesPerSec)
            : base(loadedTexture, position, 0f, 1f, 1f, frameCount, framesPerSec)
        {

        }

        public void Jump()
        {
            this.velocity.Y = -10;
        }
    }
}
