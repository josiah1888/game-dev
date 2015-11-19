using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TermProject
{
    public class Player : AnimatedObject
    {
        private const int MAX_SPEED = 4;
        private const float WALKING_TOLERANCE = .3f;
        public const int MAX_HEALTH = 3;
        public const int MERCY_INVINCIBILITY_TIME = 2000;

        public int currentHealth = MAX_HEALTH;
        public bool invincible = false;

        private enum Direction
        {
            Left = -1,
            Right = 1
        }

        private Texture2D IdleSprite, WalkingSprite;

        private SoundEffect jumpSound;


        public Player(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/player-idle"), position, 0f, 1f, 1f, 1, 1)
        {
            this.IdleSprite = content.Load<Texture2D>("sprites/player-idle");
            this.WalkingSprite = content.Load<Texture2D>("sprites/player-walk");
            this.TimePerFrame = 200f;
            this.jumpSound = content.Load<SoundEffect>("sounds/jump");
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
            CheckInvincibilty();
            CheckHealth();
        }

        #region Move
        private void Jump()
        {
            this.Velocity.Y = -10;
            jumpSound.Play();
        }

        private void CheckEnemyCollisions(List<GameObject> levelObjects)
        {
            levelObjects
                .Where(i => i is Enemy && i.TopRectangle.Intersects(this.BottomRectangle))
                .ToList()
                .ForEach(i => i.Alive = false);

            for (int i = 0; i < levelObjects.Count; ++i)
            {
                if (levelObjects[i] is Enemy && levelObjects[i].Rectangle.Intersects(this.Rectangle) && !invincible && levelObjects[i].Alive)
                {
                    if (!levelObjects[i].TopRectangle.Intersects(this.BottomRectangle))
                    {
                        invincible = true;
                        currentHealth -= 1;
                        this.elapsed = DateTime.Now;
                    }
                }
            }

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

        private void CheckInvincibilty()
        {
            if (DateTime.Now > this.elapsed.AddMilliseconds(MERCY_INVINCIBILITY_TIME))
                invincible = false;
        }

        private void CheckHealth()
        {
            if (currentHealth <= 0)
                this.Alive = false;
        }
    }
}
