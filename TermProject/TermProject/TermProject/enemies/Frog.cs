using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class Frog : Enemy
    {
        const int delay = 2000;
        const int predictFrames = 3;
        
        Random random = new Random();

        public Frog(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/frog-idle"), position, 1, 1, Frog.Ai)
        {
            this.IdleSprite = content.Load<Texture2D>("sprites/frog-idle");
            this.AttackSprite = content.Load<Texture2D>("sprites/frog-jump");
            this.TimePerFrame = 200f;
            this.elapsed = DateTime.Now;
        }

        private static Action<Enemy> Ai
        {
            get
            {
                return (Enemy frog) =>
                {
                    Random random = new Random();

                    if (Vector2.Distance(frog.Position, frog.Target.Position) > THRESHHOLD || frog.Target.IsOnGround() || (frog.State == EnemyState.Idle && !frog.IsOnGround()))
                    {
                        frog.State = EnemyState.Idle;
                    }

                    else
                    {
                        frog.State = EnemyState.Attack;
                    }


                    if (frog.State == EnemyState.Idle)
                    {
                        if (frog.IsOnGround())
                        {
                            frog.Velocity.X = 0;
                            if (DateTime.Now > frog.elapsed)
                            {
                                frog.Velocity.Y = -10;
                                frog.Direction = (EnemyDirection) ((int)Math.Ceiling(random.NextDouble() * 2) - 1);
                                frog.elapsed = DateTime.Now.AddMilliseconds(delay);
                            }
                        }

                        if (!frog.IsOnGround())
                        {
                            if (frog.Direction == EnemyDirection.Left)
                                frog.Velocity.X = (MAX_SPEED / 2) * -1;
                            else if (frog.Direction == EnemyDirection.Right)
                                frog.Velocity.X = (MAX_SPEED / 2);
                            else
                                frog.Velocity.X = 0;
                        }
                    }

                    else if (frog.State == EnemyState.Attack)
                    {
                        Vector2 closingVelocity = Vector2.Zero;
                        closingVelocity.X = frog.Target.Velocity.X - frog.Velocity.X;

                        Vector2 closingRange = Vector2.Zero;
                        closingRange.X = frog.Target.Position.X - frog.Position.X;

                        Vector2 closingTime = new Vector2(Math.Abs(closingRange.X) / Math.Abs(closingVelocity.X), 0);

                        Vector2 frogFuturePosition = frog.Target.Position + (frog.Target.Velocity * closingTime);
                        float angle = frog.GetAngle(frog.Position, frogFuturePosition);

                        if (frog.IsOnGround())
                            frog.Velocity.Y = -10;

                        if (!frog.IsOnGround())
                            frog.Velocity.X = (float)Math.Cos(angle) * MAX_SPEED / 1.5f;
                    }
                };
            }
        }
    }
}
