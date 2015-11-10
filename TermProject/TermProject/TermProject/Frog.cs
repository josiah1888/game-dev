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
        public Frog(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/place-holder"), position, 1, 1, Frog.Ai)
        {

        }

        private static Action<Enemy> Ai
        {
            get
            {
                return (Enemy frog) =>
                {
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

                        if (frog.Velocity.Y == 0)
                        {
                            frog.Velocity.X = 0;
                            frog.Velocity.Y = -10;
                        }

                        if (frog.Velocity.Y != 0)
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

                        if (frog.Velocity.Y == 0)
                        {
                            frog.Velocity.X = 0;
                            frog.Velocity.Y = (float)Math.Cos(angle) * 10.0f;
                        }

                        if (frog.Velocity.Y != 0)
                            frog.Velocity.X = (float)Math.Sin(angle) * Enemy.MAX_SPEED;
                    }
                };
            }
        }
    }
}
