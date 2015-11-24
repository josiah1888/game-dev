using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TermProject
{
    public class Enemy : AnimatedObject
    {
        public const int THRESHHOLD = 100;
        public const int MAX_SPEED = 4;
        public EnemyState State;
        public Player Target;

        protected Texture2D IdleSprite, AttackSprite;

        public const int MAX_GRAVITY = 3;
        private Action<Enemy, double> Ai;

        public enum EnemyState
        {
            Idle,
            Attack
        }

        public Enemy(Texture2D loadedTexture, Vector2 position, int frameCount, int framesPerSec, Action<Enemy, double> ai)
            : base(loadedTexture, position, 0f, 1f, 1f, frameCount, framesPerSec)
        {
            this.Ai = ai;
            this.State = EnemyState.Idle;
            this.Direction = Directions.Left;
        }

        public override void Update(List<GameObject> levelObjects, double elapsed)
        {
            UpdateSprite();
            if (CheckLateralCollisions(levelObjects))
            {
                this.Velocity.X = -this.Velocity.X;
                this.Direction = this.Direction.HasFlag(Directions.Left) ? Directions.Right : Directions.Left;
            }
            CheckVerticalCollisions(levelObjects);
            this.Target = (Player)levelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));
            this.Ai(this, elapsed);

            base.Update(levelObjects, elapsed);
        }

        private void UpdateSprite()
        {
            switch (this.State)
            {
                case EnemyState.Idle:
                    this.Sprite = IdleSprite;
                    break;
                case EnemyState.Attack:
                    this.Sprite = AttackSprite;
                    break;
            }
        }
    }
}