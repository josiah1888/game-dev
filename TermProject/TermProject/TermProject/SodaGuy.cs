using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermProject
{
    public class SodaGuy : Enemy
    {
        const int drinkingTime = 2000;
        public ContentManager Content;

        public SodaGuy(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/sodaguy-idle"), position, 1, 1, SodaGuy.Ai)
        {
            this.AttackSprite = this.IdleSprite = content.Load<Texture2D>("sprites/place-holder");
            this.elapsed = DateTime.Now.AddMilliseconds(drinkingTime);
            this.Content = content;
            this.AttackSprite = this.IdleSprite = content.Load<Texture2D>("sprites/sodaguy-throw");
        }

        private static Action<Enemy> Ai
        {
            get
            {
                return (Enemy sodaGuy) =>
                {
                    if (sodaGuy.Target.Position.X > sodaGuy.Position.X)
                        sodaGuy.Direction = EnemyDirection.Right;

                    if (sodaGuy.Target.Position.X < sodaGuy.Position.X)
                        sodaGuy.Direction = EnemyDirection.Left;

                    if (sodaGuy.State == EnemyState.Idle && DateTime.Now > sodaGuy.elapsed)
                        sodaGuy.State = EnemyState.Attack;

                    if (sodaGuy.State == EnemyState.Attack)
                    {
                        SodaCan can = new SodaCan(((SodaGuy)sodaGuy).Content, sodaGuy.Position + sodaGuy.Center);
                        can.Velocity.Y = -10;
                        can.Direction = sodaGuy.Direction;
                        can.LevelObjects = sodaGuy.LevelObjects;

                        sodaGuy.LevelObjects.Add(can);

                        sodaGuy.elapsed = DateTime.Now.AddMilliseconds(drinkingTime);
                        sodaGuy.State = EnemyState.Idle;
                    }
                };
            }
        }
    }
}
