using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class Emu : Enemy
    {
        public Emu(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/old-bird"), position, 1, 1, Emu.Ai)
        {
            this.AttackSprite = this.IdleSprite = content.Load<Texture2D>("sprites/old-bird");
        }

        private static Action<Enemy> Ai
        {
            get
            {
                return (Enemy Emu) =>
                {
                    if (Vector2.Distance(Emu.Position + Emu.Center, Emu.Target.Position + Emu.Target.Center) > Enemy.THRESHHOLD || Math.Abs(Emu.Target.Position.Y - Emu.Position.Y) > 50)
                    {
                        Emu.State = Enemy.EnemyState.Idle;
                    }

                    else if ((Emu.Target.Position.X <= Emu.Position.X && Emu.Direction == EnemyDirection.Right) || (Emu.Target.Position.X >= Emu.Position.X && Emu.Direction == EnemyDirection.Left))
                        Emu.State = Enemy.EnemyState.Idle;

                    else
                    {
                        Emu.State = Enemy.EnemyState.Attack;
                    }

                    if (Emu.State == Enemy.EnemyState.Idle)
                    {
                        if (Emu.Direction == Enemy.EnemyDirection.Left)
                            Emu.Velocity.X = (Enemy.MAX_SPEED / 2) * -1;
                        else if (Emu.Direction == Enemy.EnemyDirection.Right)
                            Emu.Velocity.X = (Enemy.MAX_SPEED / 2);
                        else
                            Emu.Velocity.X = 0;
                    }

                    else if (Emu.State == Enemy.EnemyState.Attack)
                    {
                        if (Emu.Direction == Enemy.EnemyDirection.Left)
                            Emu.Velocity.X = Enemy.MAX_SPEED * -1;
                        else if (Emu.Direction == Enemy.EnemyDirection.Right)
                            Emu.Velocity.X = Enemy.MAX_SPEED;
                        else
                            Emu.Velocity.X = 0;
                    }

                    if (Emu.Velocity.Y > 0)
                    {
                        if (Emu.Velocity.X < 0)
                        {
                            Emu.Position.Y -= 1;
                            Emu.Direction = EnemyDirection.Right;
                            Emu.Velocity.X *= -1;
                        }

                        else if (Emu.Velocity.X > 0)
                        {
                            Emu.Position.Y -= 1;
                            Emu.Direction = EnemyDirection.Left;
                            Emu.Velocity.X *= -1;
                        }
                    }
                };
            }
        }
    }
}
