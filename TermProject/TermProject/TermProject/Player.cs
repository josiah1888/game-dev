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
        public const int MERCY_INVINCIBILITY_TIME = 2000;
        public int Health = MAX_HEALTH;
        public bool IsInvincible = false;
        public Texture2D HealthIconSprite;

        private bool iconsAdded = false;
        private Timer Timer = new Timer();

        private const int MAX_SPEED = 6;
        private const float WALKING_TOLERANCE = .3f;
        private const float FRICTION = .25F;
        private const float ACCELERATION = .4f;

        private SoundEffect JumpSound;

        public Player(ContentManager content, Vector2 position, List<GameObject> levelObjects = null)
            : base(content.Load<Texture2D>("sprites/player-walk"), position, 0f, 1f, 1f, 4, 200f)
        {
            this.TimePerFrame = 200f;
            this.JumpSound = content.Load<SoundEffect>("sounds/jump");
            this.LevelObjects = levelObjects ?? new List<GameObject>();
            this.HealthIconSprite = content.Load<Texture2D>("sprites/health-icon");
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

        public override void Draw(SpriteBatch batch, Vector2 position, SpriteEffects spriteEffects, Rectangle? spriteFrame = null, Color? color = null)
        {
            color = this.IsInvincible ? Color.White * 0.5f : Color.White;
            base.Draw(batch, position, spriteEffects, spriteFrame, color);
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

            if (this.Health <= 0)
                this.Alive = false;
        }

        private void UpdateHealthIcons(List<GameObject> levelObjects)
        {
            for (int i = 0; i < MAX_HEALTH; ++i)
            {
                PlayerLife lifeIcon;

                if (levelObjects.FirstOrDefault(j => j.GetType() == typeof(PlayerLife) && ((PlayerLife)j).HealthDesignator == i) == null)
                    lifeIcon = new PlayerLife(this, i);
                else
                    lifeIcon = (PlayerLife)levelObjects.FirstOrDefault(j => j.GetType() == typeof(PlayerLife) && ((PlayerLife)j).HealthDesignator == i);

                if (!iconsAdded)
                {
                    lifeIcon.LevelObjects = levelObjects;
                    levelObjects.Add(lifeIcon);
                }

                lifeIcon.UpdateHealthIcons();
            }
            iconsAdded = true;
        }

        private bool HasEnemyCollision(List<GameObject> levelObjects)
        {
            return levelObjects.Any(i => i.Alive && i is Enemy && i.Rectangle.Intersects(this.Rectangle) && !this.IsInvincible);
        }

        private void UpdateInvincibilty(double elapsed)
        {
            if (this.IsInvincible && this.Timer.IsTimeYet(elapsed, MERCY_INVINCIBILITY_TIME))
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
