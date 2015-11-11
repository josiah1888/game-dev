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

                    if (Vector2.Distance(frog.Position, frog.Target.Position) > Enemy.THRESHHOLD)
                    {
                        frog.State = Enemy.EnemyState.Idle;
                    }

                    else
                    {
                        frog.State = Enemy.EnemyState.Attack;
                    }


                    if (frog.State == Enemy.EnemyState.Idle)
                    {
                        if (frog.IsOnGround())
                        {
                            frog.Velocity.X = 0;
                            if (DateTime.Now > frog.elapsed)
                            {
                                frog.Velocity.Y = -10;
                                frog.Direction = (Enemy.EnemyDirection) ((int)Math.Ceiling(random.NextDouble() * 2) - 1);
                                frog.elapsed = DateTime.Now.AddMilliseconds(delay);
                            }
                        }

                        if (!frog.IsOnGround())
                        {
                            if (frog.Direction == Enemy.EnemyDirection.Left)
                                frog.Velocity.X = (Enemy.MAX_SPEED / 2) * -1;
                            else if (frog.Direction == Enemy.EnemyDirection.Right)
                                frog.Velocity.X = (Enemy.MAX_SPEED / 2);
                            else
                                frog.Velocity.X = 0;
                        }
                    }

                    else if (frog.State == Enemy.EnemyState.Attack)
                    {
                        Vector2 closingVelocity = frog.Target.Velocity - frog.Velocity;
                        Vector2 closingRange = frog.Target.Position - frog.Position;
                        Vector2 closingTime = new Vector2(Math.Abs(closingVelocity.X) / Math.Abs(closingRange.X), Math.Abs(closingVelocity.Y) / Math.Abs(closingRange.Y));

                        Vector2 frogFuturePosition = frog.Target.Position + (frog.Target.Velocity * closingTime);
                        
                        float angle = frog.GetAngle(frog.Position, frogFuturePosition);

                        if (frog.IsOnGround())
                        {
                            frog.Velocity.X = 0;
                            frog.Velocity.Y = MathHelper.Clamp((float)Math.Cos(angle) * 10.0f, -10, 0);
                        }

                        if (!frog.IsOnGround())
                            frog.Velocity.X = (float)Math.Sin(angle) * Enemy.MAX_SPEED;
                    }
                };
            }
        }
    }
}
