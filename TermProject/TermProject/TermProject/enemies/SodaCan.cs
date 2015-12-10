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
        public float JumpHeight = -10;

        private const float ACCERLATION = .05f;
        private const float FRICTION = .02f;
        private const float BOUNCE = 1.3f;

        private float Speed = MAX_SPEED;
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
            // todo: Debug to see if this works
            //SodaCan recycledCan = (SodaCan)sodaGuy.LevelObjects.FirstOrDefault(i => i is SodaCan && !i.Alive) ?? this;
            this.Position = sodaGuy.Position + sodaGuy.Center;
            this.Alive = false;
            this.AttackSprite = this.IdleSprite = sodaGuy.SodaCanSprite;
            this.Velocity.Y = JumpHeight;
            this.Direction = sodaGuy.Direction;
            this.Speed = MAX_SPEED;
            this.SodaGuy = sodaGuy;

            //if (!sodaGuy.LevelObjects.Contains(recycledCan))
            //{
            //    sodaGuy.LevelObjects.Add(recycledCan);
            //}
        }

        public void Recycle(SodaGuy sodaGuy)
        {
            this.Position = sodaGuy.Position + sodaGuy.Center;
            this.Alive = true;
            this.AttackSprite = this.IdleSprite = sodaGuy.SodaCanSprite;
            this.Velocity.Y = JumpHeight;
            this.Direction = sodaGuy.Direction;
            this.Speed = MAX_SPEED;
            this.SodaGuy = sodaGuy;
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

                    if (sodaCan.IsOnGround() && ((SodaCan)sodaCan).JumpHeight == 0)
                    {
                        ((SodaCan)sodaCan).Rotation = 0;
                        ((SodaCan)sodaCan).Speed -= FRICTION;
                    }
                    else
                    {
                        ((SodaCan)sodaCan).Rotate(.1f + (.1f * -((SodaCan)sodaCan).JumpHeight));
                    }
                };
            }
        }
    }
}