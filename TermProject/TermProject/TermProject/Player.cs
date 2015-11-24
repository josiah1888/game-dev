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
        public const int MAX_HEALTH = 3;
        public int Health = MAX_HEALTH;
        public bool IsInvincible = false;
        public const int MERCY_INVINCIBILITY_TIME = 2000;

        public GameObject[] HealthIcons;
        private bool iconsAdded = false;

        private const int MAX_SPEED = 6;
        private const float WALKING_TOLERANCE = .3f;
        private const float FRICTION = .25F;
        private const float ACCELERATION = .4f;

        private SoundEffect JumpSound;

        public Player(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/player-walk"), position, 0f, 1f, 1f, 4, 200f)
        {
            this.TimePerFrame = 200f;
            this.JumpSound = content.Load<SoundEffect>("sounds/jump");
            HealthIcons = new GameObject[MAX_HEALTH];

            for (int i = 0; i < MAX_HEALTH; i++)
            {
                HealthIcons[i] = new GameObject(content.Load<Texture2D>("sprites/health-icon"), new Vector2(32 + 32 * i, 32));
                HealthIcons[i].Alive = true;
            }
        }

        public void Update(List<GameObject> levelObjects, Keys[] keys, double elapsed)
        {
            UpdateSprite();
            Move(keys);
            CheckEnemyCollisions(levelObjects);
            CheckLateralCollisions(levelObjects);
            CheckVerticalCollisions(levelObjects);
            CheckBoundsCollisions();
            UpdateHealthIcons(levelObjects);
            UpdateInvincibilty(elapsed);
            Fly(keys);
            SlowDown();

            base.Update(levelObjects, elapsed);
        }

        private void UpdateSprite()
        {
            if (Math.Abs(this.Velocity.X) > WALKING_TOLERANCE && this.IsPaused)
            {
                this.Restart();
            }
            else if (Math.Abs(this.Velocity.X) <= WALKING_TOLERANCE)
            {
                this.Stop();
            }
        }

        private void Move(Keys[] keys)
        {
            if (IsOnGround() && (keys.Contains(Keys.Space) || keys.Contains(Keys.Up) || keys.Contains(Keys.W)))
            {
                Jump();
            }

            Directions lateralDirection = Directions.None;

            if (keys.Contains(Keys.Right) || keys.Contains(Keys.D))
            {
                lateralDirection |= Directions.Right;
            }

            if (keys.Contains(Keys.Left) || keys.Contains(Keys.A))
            {
                lateralDirection |= Directions.Left;
            }

            Move(lateralDirection);
        }

        private void Jump()
        {
            this.Velocity.Y = -10;
            JumpSound.Play();
        }

        private void Move(Directions direction)
        {
            this.Velocity.X = Math.Max(MAX_SPEED * -1, Math.Min(this.Velocity.X + (direction.GetLateralDirectionSign() * ACCELERATION), MAX_SPEED));
        }

        private void CheckEnemyCollisions(List<GameObject> levelObjects)
        {
            KillEnemies(levelObjects);

            if (HasEnemyCollision(levelObjects))
            {
                LoseLife();
            }
        }

        private void KillEnemies(List<GameObject> levelObjects)
        {
            levelObjects
                .Where(i => i is Enemy && i.Alive && i.TopRectangle.Intersects(this.BottomRectangle))
                .ToList()
                .ForEach(i => i.Alive = false);
        }

        private void LoseLife()
        {
            this.Health--;
            this.IsInvincible = true;
        }

        private void UpdateHealthIcons(List<GameObject> levelObjects)
        {
            for (int i = 0; i < HealthIcons.Length; ++i)
            {
                if (!iconsAdded)
                {
                    HealthIcons[i].LevelObjects = levelObjects;
                    levelObjects.Add(HealthIcons[i]);
                }

                if (Health <= i)
                    HealthIcons[i].Alive = false;
            }
            iconsAdded = true;
        }

        private bool HasEnemyCollision(List<GameObject> levelObjects)
        {
            return levelObjects.Any(i => i.Alive && i is Enemy && i.Rectangle.Intersects(this.Rectangle) && !this.IsInvincible);
        }

        private void UpdateInvincibilty(double elapsed)
        {
            if (this.IsInvincible && Timer.IsTimeYet(this, elapsed, MERCY_INVINCIBILITY_TIME))
            {
                this.IsInvincible = false;
            }
        }

        private void SlowDown()
        {
            if (this.Velocity.X > 0)
            {
                this.Velocity.X = Math.Max(this.Velocity.X - FRICTION, 0f);
            }
            else
            {
                this.Velocity.X = Math.Min(this.Velocity.X + FRICTION, 0f);
            }
        }
    }
}
