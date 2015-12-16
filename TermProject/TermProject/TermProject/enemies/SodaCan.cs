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
        public const float MAX_JUMP_HEIGHT = -10;
        public const float DEATH_TOLERANCE = .50f;
        public const double DEATH_DELAY = 1500;

        public float JumpHeight;
        public Timer Timer;

        private const float ACCERLATION = .05f;
        private const float FRICTION = .02f;
        private const float BOUNCE = 1.6f;

        private SodaGuy SodaGuy;

        public override List<GameObject> LevelObjects
        {
            get
            {
                return this.SodaGuy.LevelObjects;
            }
        }

        public SodaCan(SodaGuy sodaGuy)
            : base(sodaGuy.SodaCanSprite, sodaGuy.Position + sodaGuy.Center, 1, 1, SodaCan.Ai)
        {
            Initilize(sodaGuy, false);
        }

        public void Recycle(SodaGuy sodaGuy)
        {
            Initilize(sodaGuy, true);
        }

        private void Initilize(SodaGuy sodaGuy, bool isAlive)
        {
            this.Position = sodaGuy.Position + sodaGuy.Center;
            this.Alive = isAlive;
            this.AttackSprite = this.IdleSprite = sodaGuy.SodaCanSprite;
            this.Direction = sodaGuy.Direction;
            this.SodaGuy = sodaGuy;
            this.Velocity = new Vector2(Enemy.MAX_SPEED * sodaGuy.Direction.GetLateralDirectionSign(), MAX_JUMP_HEIGHT);
            this.JumpHeight = MAX_JUMP_HEIGHT;
            this.DeathAction = (GameObject gameObject) => { };
            this.Timer = new Timer();
        }

        private static Action<Enemy, double> Ai
        {
            get
            {
                return (Enemy sodaCanEnemy, double elapsed) =>
                {
                    SodaCan sodaCan = (SodaCan)sodaCanEnemy;

                    if (sodaCan.IsOnGround() && sodaCan.Velocity != Vector2.Zero)
                    {
                        sodaCan.JumpHeight = (int)(sodaCan.JumpHeight / SodaCan.BOUNCE);
                        sodaCan.Velocity.Y = sodaCan.JumpHeight;
                        sodaCan.Velocity.X -= FRICTION * sodaCan.Direction.GetLateralDirectionSign();
                    }

                    if (Math.Abs(sodaCan.Velocity.X) <= SodaCan.DEATH_TOLERANCE)
                    {
                        sodaCan.Velocity = Vector2.Zero;
                        if (sodaCan.Timer.IsTimeYet(elapsed, SodaCan.DEATH_DELAY))
                        {
                            sodaCan.Alive = false;
                        }
                    }

                    if (sodaCan.Velocity.Y == 0)
                    {
                        sodaCan.Rotation = 0;
                    }
                    else
                    {
                        sodaCan.Rotate(.1f + (.1f * -sodaCan.JumpHeight));
                    }

                    sodaCan.Origin = sodaCan.Velocity.Y > 0 || sodaCan.Rotation != 0f || !sodaCan.IsOnGround() ? sodaCan.Center : Vector2.Zero;
                };
            }
        }
    }
}