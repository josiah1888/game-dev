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
        Rectangle groundChecker;

        public Player(Texture2D loadedTexture, Vector2 position, int frameCount, int framesPerSec)
            : base(loadedTexture, position, 0f, 1f, 1f, frameCount, framesPerSec)
        {

        }

        public void Jump()
        {
            this.velocity.Y = -10;
        }

        public void FallOntoGround(List<GameObject> levelObjects)
        {
            for (int i = 0; i < levelObjects.Count; i++)
            {
                if(groundChecker.Intersects(levelObjects[i].Rectangle)
                {
                    if((levelObjects[i] is SemiSolidTile || levelObjects[i] is SolidTile) && this.velocity.Y > 0)
                        this.isOnGround = true;
                }
            }
        }
    }
}
