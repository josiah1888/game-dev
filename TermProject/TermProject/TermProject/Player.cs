using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TermProject
{
    class Player : AnimatedObject
    {
        public Player(Texture2D loadedTexture, Vector2 position, int frameCount, int framesPerSec)
            : base(loadedTexture, position, 0f, 1f, 1f, frameCount, framesPerSec)
        {

        }

        public new void Move(Keys[] keys)
        {
            bool PlayerIsOnGround = true; // needs to be implemented

            if (PlayerIsOnGround && (keys.Contains(Keys.Space) || keys.Contains(Keys.Up)))
            {
                Jump();
            }
        }

        public void Jump()
        {
            this.velocity.Y = -10;
        }
    }
}
