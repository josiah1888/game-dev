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
        }

        private static Action<Enemy, double> Ai
        {
            get
            {
                return (Enemy enemyFrog, double elapsed) =>
                {
                    Frog frog = (Frog)enemyFrog;
                    bool isOnGround = frog.IsOnGround();

                    frog.State = Vector2.Distance(frog.Position, frog.Target.Position) > THRESHHOLD || frog.Target.IsOnGround() || (frog.State == EnemyState.Idle && !isOnGround)
                        ? EnemyState.Idle
                        : EnemyState.Attack;

                    switch (frog.State)
                    {
                        case EnemyState.Idle:
                            Ai_Idle(frog, elapsed);
                            break;
                        case EnemyState.Attack:
                            Ai_Attack(frog);
                            break;
                    }
                };
            }
        }

        private static void Ai_Idle(Frog frog, double elapsed)
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

        private static void Ai_Attack(Frog frog)
        {
            Vector2 closingVelocity = new Vector2(frog.Target.Velocity.X - frog.Velocity.X, 0);
            Vector2 closingRange = new Vector2(frog.Target.Position.X - frog.Position.X, 0);
            Vector2 closingTime = new Vector2(Math.Abs(closingRange.X) / Math.Abs(closingVelocity.X), 0);
            Vector2 frogFuturePosition = frog.Target.Position + (frog.Target.Velocity * closingTime);
            float angle = frog.GetAngle(frog.Position, frogFuturePosition);

            if (frog.IsOnGround())
            {
                frog.Velocity.Y = -10;
            }
            else
            {
                frog.Velocity.X = (float)Math.Cos(angle) * MAX_SPEED / 1.5f;
            }
        }
    }
}
