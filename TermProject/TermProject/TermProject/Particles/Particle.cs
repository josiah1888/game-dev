using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TermProject
{
    namespace Particles
    {
        public class Particle
        {
            public Vector2 position;
            public Vector2 velocity;
            public Vector2 acceleration;

            public float lifetime;

            public float timeSinceStart;

            public float scale;

            public float rotation;

            public float rotationSpeed;

            public bool Active
            {
                get { return timeSinceStart < lifetime; }
            }

            public void Initialize(Vector2 position, Vector2 velocity, Vector2 acceleration,
                float lifetime, float scale, float rotationSpeed)
            {
                this.position = position;
                this.velocity = velocity;
                this.acceleration = acceleration;
                this.lifetime = lifetime;
                this.scale = scale;
                this.rotationSpeed = rotationSpeed;

                this.timeSinceStart = 0.0f;

                this.rotation = ParticleSystem.RandomBetween(0, MathHelper.TwoPi);
            }

            public void Update(float dt)
            {
                velocity += acceleration * dt;
                position += velocity * dt;

                rotation += rotationSpeed * dt;

                timeSinceStart += dt;
            }
        }
    }
}
