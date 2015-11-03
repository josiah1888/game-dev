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
        Rectangle groundChecker;

        public Player(Texture2D loadedTexture, Vector2 position, int frameCount, int framesPerSec)
            : base(loadedTexture, position, 0f, 1f, 1f, frameCount, framesPerSec)
        {

        }

        private const int MAX_SPEED = 8;

        private enum Direction
        {
            Left = -1,
            Right = 1
        }

        private Rectangle PlayerFeet
        {
            get
            {
                return new Rectangle(this.Rectangle.X, this.Rectangle.Y + this.Rectangle.Height, this.Rectangle.Width, 1);
            }
        }

        public bool IsOnGround(List<GameObject> levelObjects)
        {
            return levelObjects.Any(i => i.Rectangle.Intersects(this.PlayerFeet));
        }

        public void Move(Keys[] keys, List<GameObject> levelObjects)
        {
            if (IsOnGround(levelObjects) && (keys.Contains(Keys.Space) || keys.Contains(Keys.Up) || keys.Contains(Keys.W)))
             {
                Jump();
            }

            if (keys.Contains(Keys.Right) || keys.Contains(Keys.D))
            {
                Move(Direction.Right);
            }

            if (keys.Contains(Keys.Left) || keys.Contains(Keys.A))
            {
                Move(Direction.Left);
            }
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
		
        private void Move(Direction direction)
        {
            this.velocity.X = Math.Max(MAX_SPEED * -1, Math.Min(this.velocity.X + (int)direction, MAX_SPEED));
        }
    }
}
