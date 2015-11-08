using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TermProject
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle ViewPort { get; set; }

        List<GameObject> levelObjects;

        Player Player
        {
            get
            {
                return (Player)this.levelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MapMaker mapMaker = new MapMaker(Content);
            levelObjects = mapMaker.ReadMap("maps/level1");
            UpdateViewport(0);
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            if (levelObjects.Any())
            {
                Update_Player();
                Update_Positions();
                Update_Camera();
            }
            base.Update(gameTime);
        }

        #region Update
        private void Update_Player()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Player.Update(levelObjects, keyboardState.GetPressedKeys(), this.ViewPort);
        }

        private void Update_Positions()
        {
            levelObjects.ForEach(i =>
            {
                i.Position.X += i.Velocity.X;
                i.Position.Y += i.Velocity.Y;
            });
        }

        private void Update_Camera()
        {
            float distancePlayerIsAhead = Player.Position.X + Player.Center.X - this.ViewPort.X;
            if (distancePlayerIsAhead > this.ViewPort.Width * (3.0 / 5.0))
            {
                UpdateViewport(this.ViewPort.X + distancePlayerIsAhead / 150f);
            }
        }

        private void UpdateViewport(double x)
        {
            this.ViewPort = new Rectangle((int)x, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);
        }
        #endregion

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            levelObjects.Where(i => i.Rectangle.Intersects(this.ViewPort)).ToList().ForEach(i =>
            {
                spriteBatch.Draw(i.Sprite, new Vector2(i.Position.X - this.ViewPort.X, i.Position.Y), null, Color.White, i.Rotation, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            });

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
