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
        private const int JUMP_DELAY_TIME = 2000;
        private const int PREDICT_FRAMES = 3;
        public Timer Timer = new Timer();

        public Frog(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/frog-idle"), position, 1, 1, Frog.Ai)
        {
            this.IdleSprite = content.Load<Texture2D>("sprites/frog-idle");
            this.AttackSprite = content.Load<Texture2D>("sprites/frog-jump");
            this.TimePerFrame = 200f;
        }

        private static Action<Enemy, double> Ai
        {
            get
            {
                return (Enemy enemyFrog, double elapsed) =>
                {
                    Frog frog = (Frog)enemyFrog;

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
                            if (frog.Timer.IsTimeYet(elapsed, JUMP_DELAY_TIME))
                            {
                                frog.Velocity.Y = -10;
                                frog.Direction = (new List<Directions>() { Directions.Left, Directions.Right }[Rand.Next(2)]);
                            }
                        }
                        else
                        {
                            frog.Velocity.X = (MAX_SPEED / 2) * frog.Direction.GetLateralDirectionSign();
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
