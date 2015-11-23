using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class SodaCan : Enemy
    {
        private float JumpHeight = -10;

        private const float ACCERLATION = .05f;
        private const float FRICTION = .02f;
        private const float BOUNCE = 1.3f;

        private float Speed = MAX_SPEED;

        public SodaCan(SodaGuy sodaGuy)
            : base(sodaGuy.Content.Load<Texture2D>("sprites/can"), sodaGuy.Position + sodaGuy.Center, 1, 1, SodaCan.Ai)
        {
            this.AttackSprite = this.IdleSprite = sodaGuy.Content.Load<Texture2D>("sprites/can");
            this.Velocity.Y = JumpHeight;
            this.Direction = sodaGuy.Direction;
            this.LevelObjects = sodaGuy.LevelObjects;
            sodaGuy.LevelObjects.Add(this);
        }

        private static Action<Enemy, double> Ai
        {
            get
            {
                return (Enemy sodaCanEnemy, double elapsed) =>
                {
                    SodaCan sodaCan = (SodaCan)sodaCanEnemy;

                    if (sodaCan.IsOnGround())
                    {
                        if (sodaCan.JumpHeight < 0)
                        {
                            sodaCan.JumpHeight = (int)Math.Ceiling(sodaCan.JumpHeight / BOUNCE);
                            sodaCan.Velocity.Y = (sodaCan).JumpHeight;
                        }

                        if (sodaCan.Velocity.Y == MAX_GRAVITY && sodaCan.JumpHeight > -10)
                        {
                            sodaCan.JumpHeight -= ACCERLATION;
                        }

                        sodaCan.Speed -= FRICTION;
                    }

                    if (sodaCan.Speed <= 0)
                    {
                        sodaCan.Alive = false;
                    }

                    if (sodaCan.Alive)
                    {
                        sodaCan.Velocity.X = sodaCan.Speed * sodaCan.Direction.GetLateralDirectionSign();
                    }
                };
            }
        }
    }
}