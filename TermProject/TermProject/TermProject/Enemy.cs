﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TermProject
{
    public class Enemy : AnimatedObject
    {
        public const int THRESHHOLD = 50;
        public const int MAX_SPEED = 4;
        public EnemyState State;
        public EnemyDirection Direction;
        public Player Target;

        private const int MAX_GRAVITY = 3;
        private Action<Enemy> Ai;

        public enum EnemyState
        {
            Idle,
            Attack
        }

        public enum EnemyDirection
        {
            Left,
            Right
        }

        public Enemy(Texture2D loadedTexture, Vector2 position, int frameCount, int framesPerSec, Action<Enemy> ai)
            : base(loadedTexture, position, 0f, 1f, 1f, frameCount, framesPerSec)
        {
            this.Ai = ai;
            this.State = EnemyState.Idle;
            this.Direction = EnemyDirection.Left;
        }

        public void Update(List<GameObject> levelObjects)
        {
            ApplyGravity(levelObjects);
            this.Target = (Player)levelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));
            this.Ai(this);
        }
    }
}
