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
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        Rectangle ViewPort { get; set; }
        MapMaker MapMaker;

        List<GameObject> LevelObjects;

        Player Player
        {
            get
            {
                return (Player)this.LevelObjects.FirstOrDefault(i => i.GetType() == typeof(Player));
            }
        }

        Queue<Action> _WinActions;
        Queue<Action> WinActions
        {
            get
            {
                if (_WinActions == null)
                {
                    _WinActions = new Queue<Action>();
                    _WinActions.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level1");
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door));
                        door.WinAction = WinActions.Dequeue();
                        UpdateViewport(0);
                    });
                    _WinActions.Enqueue(() =>
                    {
                        LevelObjects = MapMaker.ReadMap("maps/level-selection");
                        LevelObjects
                            .Where(i => i.Designator == 1)
                            .ToList()
                            .ForEach(i => i.Alive = false);
                        // optionally, add kaboom after a delay
                        Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 2);
                        door.WinAction = WinActions.Dequeue();
                        UpdateViewport(0);
                    });
                    _WinActions.Enqueue(() =>
                    {
                        // Level 2
                    });
                }
                return _WinActions;
            }
        }

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            MapMaker = new MapMaker(Content);
            LevelObjects = MapMaker.ReadMap("maps/level-selection");
            Door door = (Door)this.LevelObjects.First(i => i.GetType() == typeof(Door) && i.Designator == 1);
            door.WinAction = WinActions.Dequeue();
            UpdateViewport(0);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            double elapsed = gameTime.TotalGameTime.TotalMilliseconds;

            if (LevelObjects.Any())
            {
                Update_AnimatedObjects(elapsed);
                Update_Enemies();
                Update_Player(elapsed);
                Update_Positions();
                Update_Camera();
            }
            base.Update(gameTime);
        }

        #region Update
        private void Update_Enemies()
        {
            this.LevelObjects
                .Where(i => i.Alive && i is Enemy)
                .Select(i => (Enemy)i).ToList()
                .ForEach(i => i.Update(LevelObjects));
        }

        private void Update_AnimatedObjects(double elapsed)
        {
            LevelObjects
                .Where(i => i.Alive && i is AnimatedObject && !(i is Player))
                .Select(i => (AnimatedObject)i).ToList()
                .ForEach(i => i.Update(LevelObjects, elapsed));
        }

        private void Update_Player(double elapsed)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Player.Update(LevelObjects, keyboardState.GetPressedKeys(), this.ViewPort, elapsed);
        }

        private void Update_Positions()
        {
            LevelObjects.ForEach(i =>
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
            SpriteBatch.Begin();

            LevelObjects.Where(i => i.Rectangle.Intersects(this.ViewPort)).ToList().ForEach(i =>
            {
                i.Draw(SpriteBatch, this.ViewPort, SpriteEffects.None);
            });

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
