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
        public bool thrownCan = false;

        public SodaGuy(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/sodaguy-idle"), position, 1, 1, SodaGuy.Ai)
        {
            this.IdleSprite = content.Load<Texture2D>("sprites/sodaguy-idle");
            this.elapsed = DateTime.Now.AddMilliseconds(drinkingTime);
            this.Content = content;
            this.AttackSprite = content.Load<Texture2D>("sprites/sodaguy-throw");
        }

        private static Action<Enemy> Ai
        {
            get
            {
                return (Enemy sodaGuy) =>
                {
                    if (sodaGuy.State == EnemyState.Idle && DateTime.Now > sodaGuy.elapsed)
                        sodaGuy.State = EnemyState.Attack;

                    if (sodaGuy.State == EnemyState.Attack)
                    {
                        if (((SodaGuy)sodaGuy).thrownCan == false)
                        {
                            SodaCan can = new SodaCan(((SodaGuy)sodaGuy).Content, sodaGuy.Position + sodaGuy.Center);
                            can.Velocity.Y = -10;
                            can.Direction = sodaGuy.Direction;
                            can.LevelObjects = sodaGuy.LevelObjects;

                            sodaGuy.LevelObjects.Add(can);
                            ((SodaGuy)sodaGuy).thrownCan = true;
                        }

                        if (DateTime.Now > sodaGuy.elapsed.AddMilliseconds(drinkingTime * 5 / 8))
                        {
                            sodaGuy.elapsed = DateTime.Now.AddMilliseconds(drinkingTime);
                            sodaGuy.State = EnemyState.Idle;
                        }
                    }

                    else
                    {
                        ((SodaGuy)sodaGuy).thrownCan = false;
                    }
                };
            }
        }
    }
}
