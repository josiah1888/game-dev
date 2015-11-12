using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TermProject
{
    public class Player : AnimatedObject
    {
        private const int MAX_SPEED = 4;
        private const float WALKING_TOLERANCE = .3f;

        private enum Direction
        {
            Left = -1,
            Right = 1
        }

        private Texture2D IdleSprite, WalkingSprite;

        public Player(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/player-idle"), position, 0f, 1f, 1f, 1, 1)
        {
            this.IdleSprite = content.Load<Texture2D>("sprites/player-idle");
            this.WalkingSprite = content.Load<Texture2D>("sprites/player-walk");
            this.TimePerFrame = 200f;
        }

        public void Update(List<GameObject> levelObjects, Keys[] keys, Rectangle viewPort, double elapsed)
        {
            UpdateSprite();
            ApplyGravity();
            Move(keys, levelObjects, viewPort);
            SlowDown();

            base.Update(levelObjects, elapsed);
        }

        private void UpdateSprite()
        {
            if (Math.Abs(this.Velocity.X) > WALKING_TOLERANCE)
            {
                this.FrameCount = 4;
                this.Sprite = WalkingSprite;
            }
            else
            {
                this.FrameCount = 1;
                this.Sprite = IdleSprite;
            }
        }

        private void Move(Keys[] keys, List<GameObject> levelObjects, Rectangle viewPort)
        {
            if (!this.IsPaused)
            {

                if (IsOnGround() && (keys.Contains(Keys.Space) || keys.Contains(Keys.Up) || keys.Contains(Keys.W)))
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

            CheckEnemyCollisions(levelObjects);
            CheckViewportCollision(viewPort);
            CheckLateralCollisions(levelObjects);
            CheckVerticalCollisions(levelObjects);
        }

        #region Move
        private void Jump()
        {
            this.Velocity.Y = -10;
        }

        private void CheckEnemyCollisions(List<GameObject> levelObjects)
        {
            levelObjects
                .Where(i => i is Enemy && i.TopRectangle.Intersects(this.BottomRectangle))
                .ToList()
                .ForEach(i => i.Alive = false);

            if ( ShouldDie(levelObjects) ^ this.Alive)
            {
                this.Alive = ShouldDie(levelObjects);
            }
        }

        private bool ShouldDie(List<GameObject> levelObjects)
        {
            return !levelObjects.Any(i => i.Alive && i is Enemy && i.Rectangle.Intersects(this.Rectangle));
        }

        private void CheckViewportCollision(Rectangle viewPort)
        {
            if (this.Position.X < viewPort.Left)
            {
                CollideLeft();
            }
        }
        #endregion

        private void SlowDown()
        {
            if (this.Velocity.X > 0)
            {
                this.Velocity.X = Math.Max(this.Velocity.X - .1f, 0f);
            }
            else
            {
                this.Velocity.X = Math.Min(this.Velocity.X + .1f, 0f);
            }
        }

        private void Move(Direction direction)
        {
            this.Velocity.X = Math.Max(MAX_SPEED * -1, Math.Min(this.Velocity.X + (int)direction, MAX_SPEED));
        }
    }
}
