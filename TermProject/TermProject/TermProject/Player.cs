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

        private const int MAX_SPEED = 4;
        private const int MAX_GRAVITY = 3;
        private const float MIN_BOUNCE_BACK = .8f;

        private enum Direction
        {
            Left = -1,
            Right = 1
        }

        public void Update(List<GameObject> levelObjects, Keys[] keys)
        {
            ApplyGravity(levelObjects);
            Move(keys, levelObjects);
            SlowDown();
        }

        private void ApplyGravity(List<GameObject> levelObjects)
        {
            if (!IsOnGround(levelObjects))
            {
                this.velocity.Y = Math.Min(MAX_GRAVITY, this.velocity.Y + 1);
            }
            else if (this.velocity.Y > 0)
            {
                this.velocity.Y = 0;
            }
        }

        public bool IsOnGround(List<GameObject> levelObjects)
        {
            return this.velocity.Y >= 0 && levelObjects.Where(i => i is SemiSolidTile || i is SolidTile).Any(i => i.TopRectangle.Intersects(this.BottomRectangle));
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
            
            CheckLateralCollisions(levelObjects);
            CheckVerticalCollisions(levelObjects);
        }

        private void CheckVerticalCollisions(List<GameObject> levelObjects)
        {
            GameObject problemTile = levelObjects.FirstOrDefault(i => i is SolidTile && this.TopRectangle.Intersects(i.Rectangle));

            if (problemTile != null)
            {
                this.position.Y = problemTile.BottomRectangle.Bottom;
                this.velocity.Y = 0;
            }
        }

        private void CheckLateralCollisions(List<GameObject> levelObjects)
        {
            if (levelObjects.Any(i => this.Rectangle.Intersects(i.Rectangle) && i is SolidTile))
            {
                this.position.X -= this.velocity.X;
                if (this.velocity.X > 0)
                {
                    this.velocity.X = Math.Max(this.velocity.X - 2, MIN_BOUNCE_BACK) * -1;
                }
                else if (this.velocity.X < 0)
                {
                    this.velocity.X = Math.Min(this.velocity.X + 2, -MIN_BOUNCE_BACK) * -1;
                }
            }
        }

        private void SlowDown()
        {
            if (this.velocity.X > 0)
            {
                this.velocity.X = Math.Max(this.velocity.X - .1f, 0f);
            }
            else
            {
                this.velocity.X = Math.Min(this.velocity.X + .1f, 0f);
            }
        }

        public void Jump()
        {
            this.velocity.Y = -10;
        }

        private void Move(Direction direction)
        {
            this.velocity.X = Math.Max(MAX_SPEED * -1, Math.Min(this.velocity.X + (int)direction, MAX_SPEED));
        }
    }
}
