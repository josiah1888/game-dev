﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TermProject.Particles;

namespace TermProject
{
    public class GameObject
    {
        public static Random Rand = new Random();
        public Texture2D Sprite;
        public Vector2 Position;
        public float Rotation = 0f;
        public Vector2 Center;
        public Vector2 Velocity;
        public Action<GameObject> DeathAction = (GameObject gameObject) => { };

        private bool _Alive = true;
        public bool Alive
        {
            get
            {
                return _Alive;
            }
            set
            {
                if (value == false)
                {
                    DeathAction(this);
                }
                _Alive = value;
            }
        }
        public float Scale = 1.0f;
        public int Designator;
        public List<GameObject> LevelObjects;

        [Flags]
        public enum Directions
        {
            None = 0,
            Up = 1,
            Right = 2,
            Down = 4,
            Left = 8
        }

        public Directions Direction;
        protected bool ObeysGravity = true;

        private const float MIN_BOUNCE_BACK = .8f;
        private const float VISION_FIELD = .02f;
        private const int VISION_LENGTH = 350;
        private const int MAX_GRAVITY = 3;

        private Vector2 FlightSpeeds = new Vector2(10, 10);

        public GameObject()
        {

        }

        public GameObject(Texture2D loadedTexture)
        {
            Setup(loadedTexture, Vector2.Zero);
        }

        public GameObject(Texture2D loadedTexture, Vector2 position)
        {
            Setup(loadedTexture, position);
        }

        public void Setup(Texture2D loadedTexture, Vector2 position)
        {
            this.Position = position;
            this.Sprite = loadedTexture;
            this.Center = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
            this.Velocity = Vector2.Zero;
        }

        public virtual void Draw(SpriteBatch batch, Vector2 position, SpriteEffects spriteEffects, Rectangle? spriteFrame = null)
        {
            if (this.Alive)
            {
                if (this is Player && ((Player)this).IsInvincible)
                {
                    batch.Draw(this.Sprite, position, spriteFrame, Color.White * 0.5f, this.Rotation, Vector2.Zero, 1.0f, spriteEffects, 0);
                }
                else if (this is SodaCan && (!this.IsOnGround() || ((SodaCan)this).JumpHeight > 0 || this.Rotation != 0f))
                    batch.Draw(this.Sprite, position, spriteFrame, Color.White, this.Rotation, this.Center, 1.0f, spriteEffects, 0);
                else
                    batch.Draw(this.Sprite, position, spriteFrame, Color.White, this.Rotation, Vector2.Zero, 1.0f, spriteEffects, 0);
            }
        }

        public void Update()
        {
            this.Position.X += this.Velocity.X;
            this.Position.Y += this.Velocity.Y;
            ApplyGravity();
        }

        public void Rotate(float rotation = .05f)
        {
            this.Rotation += rotation;
        }

        public void Fly(Keys[] keys)
        {
            if (keys.Contains(Keys.Tab))
            {
                if (keys.Contains(Keys.Up) && keys.Contains(Keys.Right))
                {
                    this.Position.Y -= this.FlightSpeeds.Y / 2;
                    this.Position.X += this.FlightSpeeds.X / 2;
                }
                else if (keys.Contains(Keys.Right) && keys.Contains(Keys.Down))
                {
                    this.Position.Y += this.FlightSpeeds.Y / 2;
                    this.Position.X += this.FlightSpeeds.X / 2;
                }
                else if (keys.Contains(Keys.Down) && keys.Contains(Keys.Left))
                {
                    this.Position.Y += this.FlightSpeeds.Y / 2;
                    this.Position.X -= this.FlightSpeeds.X / 2;
                }
                else if (keys.Contains(Keys.Left) && keys.Contains(Keys.Up))
                {
                    this.Position.Y -= this.FlightSpeeds.Y / 2;
                    this.Position.X -= this.FlightSpeeds.X / 2;
                }
                else if (keys.Contains(Keys.Left))
                {
                    this.Position.X -= this.FlightSpeeds.X;
                }
                else if (keys.Contains(Keys.Up))
                {
                    this.Position.Y -= this.FlightSpeeds.Y;
                }
                else if (keys.Contains(Keys.Down))
                {
                    this.Position.Y += this.FlightSpeeds.Y;
                }
                else if (keys.Contains(Keys.Right))
                {
                    this.Position.X += this.FlightSpeeds.X;
                }
            }
        }

        private void ApplyGravity()
        {
            if (this.ObeysGravity)
            {
                if (!IsOnGround())
                {
                    this.Velocity.Y = Math.Min(MAX_GRAVITY, this.Velocity.Y + 1);
                }
                else if (this.Velocity.Y > 0)
                {
                    this.Velocity.Y = 0;
                }
            }
        }

        public virtual Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(this.Sprite.Width * Scale), (int)(this.Sprite.Height * Scale));
            }
        }

        public Rectangle TopRectangle
        {
            get
            {
                return new Rectangle(this.Rectangle.X, this.Rectangle.Y, this.Rectangle.Width, 8);
            }
        }

        public Rectangle BottomRectangle
        {
            get
            {
                return new Rectangle(this.Rectangle.X, this.Rectangle.Y + this.Rectangle.Height - 4, this.Rectangle.Width, 8);
            }
        }

        public bool IsOnGround()
        {
            return this.Velocity.Y >= 0 && LevelObjects.Any(i => i.Alive && i is Tile && i.TopRectangle.Intersects(this.BottomRectangle));
        }

        protected bool CheckLateralCollisions(List<GameObject> levelObjects)
        {
            bool hasCollided = false;
            if (levelObjects.Any(i => i.Alive && i is SolidTile && this.Rectangle.Intersects(i.Rectangle)))
            {
                this.Position.X -= this.Velocity.X;
                if (this.Velocity.X > 0)
                {
                    hasCollided = true;
                    CollideRight();
                }
                else if (this.Velocity.X < 0)
                {
                    hasCollided = true;
                    CollideLeft();
                }
            }
            return hasCollided;
        }

        protected bool CheckVerticalCollisions(List<GameObject> levelObjects)
        {
            bool hasCollided = false;
            GameObject problemTile = levelObjects.FirstOrDefault(i => i.Alive && i is SolidTile && this.TopRectangle.Intersects(i.Rectangle));

            if (problemTile != null)
            {
                hasCollided = true;
                CollideTop(problemTile);
            }
            return hasCollided;
        }

        protected void CheckBoundsCollisions()
        {
            if(this.Position.X < 0)
            {
                this.Velocity.X = Math.Abs(this.Velocity.X);
                this.Position.X = 0;
            }
        }

        protected void CollideRight()
        {
            this.Velocity.X = Math.Max(this.Velocity.X - 2, MIN_BOUNCE_BACK) * -1;
        }

        protected void CollideLeft()
        {
            this.Velocity.X = Math.Min(this.Velocity.X + 2, -MIN_BOUNCE_BACK) * -1;
        }

        protected void CollideTop(GameObject problemTile)
        {
            this.Position.Y = problemTile.BottomRectangle.Bottom;
            this.Velocity.Y = 0;
        }

        public float GetAngle(Vector2 a, Vector2 b)
        {
            float deltaX = b.X - a.X;
            float deltaY = b.Y - a.Y;

            return (float)Math.Atan2(deltaY, deltaX);
        }

        #region Legacy GameObject Methods
        [Obsolete("Legacy code from a past game.")]
        public double DistanceFrom(GameObject otherGuy)
        {
            double deltaX = otherGuy.Position.X - this.Position.X;
            deltaX *= deltaX;
            double deltaY = otherGuy.Position.Y - this.Position.Y;
            deltaY *= deltaY;

            return Math.Sqrt(deltaX + deltaY);
        }

        [Obsolete("Legacy code from a past game.")]
        public void MoveTowards(GameObject otherGuy)
        {
            Vector2 direction = GetDirection(otherGuy);
            this.Position.X += direction.X * this.Velocity.X;
            this.Position.Y += direction.Y * this.Velocity.Y;

            this.Rotation = GetAngle(direction, Vector2.UnitY);
        }

        [Obsolete("Legacy code from a past game.")]
        private Vector2 GetDirection(GameObject otherGuy)
        {
            Vector2 direction = new Vector2(otherGuy.Position.X - this.Position.X, otherGuy.Position.Y - this.Position.Y);
            direction.Normalize();
            return direction;
        }

        [Obsolete("Legacy code from a past game.")]
        public bool CanSee(GameObject otherGuy)
        {
            return GetAngle(GetDirection(otherGuy), Vector2.UnitY) - Rotation < VISION_FIELD
                && DistanceFrom(otherGuy) < VISION_LENGTH;
        }
        #endregion
    }
}