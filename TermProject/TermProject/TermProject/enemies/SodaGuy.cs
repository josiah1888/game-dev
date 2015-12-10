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
        public ContentManager Content;
        private const int IDLE_TIME = 1000;
        private const int ATTACK_TIME = 1000;
        public object AiLock = new object();
        public Texture2D SodaCanSprite;

        public SodaGuy(ContentManager content, Vector2 position)
            : base(content.Load<Texture2D>("sprites/sodaguy-idle"), position, 1, 1, SodaGuy.Ai)
        {
            this.Content = content;
            this.IdleSprite = content.Load<Texture2D>("sprites/sodaguy-idle");
            this.AttackSprite = content.Load<Texture2D>("sprites/sodaguy-throw");
            this.SodaCanSprite = content.Load<Texture2D>("sprites/can");
        }

        public static List<Enemy> CreateSodaGuysWithCans(ContentManager content, Vector2 position, int numberOfCans = 4)
        {
            List<Enemy> enemies = new List<Enemy>();
            SodaGuy sodaGuy = new SodaGuy(content, position);
            enemies.Add(sodaGuy);
            for (int i = 0; i < numberOfCans; i++)
            {
                enemies.Add(new SodaCan(sodaGuy));
            }

            return enemies;
        }

        private static Action<Enemy, double> Ai
        {
            get
            {
                return (Enemy sodaGuyEnemy, double elapsed) =>
                {
                    SodaGuy sodaGuy = (SodaGuy)sodaGuyEnemy;

                    switch (sodaGuy.State)
                    {
                        case EnemyState.Idle:
                            Ai_Idle(sodaGuy, elapsed);
                            break;
                        case EnemyState.Attack:
                            Ai_Attack(sodaGuy, elapsed);
                            break;
                    }
                };
            }
        }

        private static void Ai_Idle(SodaGuy sodaGuy, double elapsed)
        {
            if (Timer.IsTimeYet(sodaGuy.AiLock, elapsed, IDLE_TIME))
            {
                sodaGuy.State = EnemyState.Attack;
                SodaCan sodaCan = (SodaCan)sodaGuy.LevelObjects.First(i => i is SodaCan && !i.Alive);
                sodaCan.Recycle(sodaGuy);
            }
        }

        private static void Ai_Attack(SodaGuy sodaGuy, double elapsed)
        {
            if (Timer.IsTimeYet(sodaGuy.AiLock, elapsed, ATTACK_TIME))
            {
                sodaGuy.State = EnemyState.Idle;
            }
        }
    }
}
