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

        private Dictionary<char, Type> _MapLegend;
        public Dictionary<char, Type> MapLegend
        {
            get
            {
                if (_MapLegend == null)
                {
                    _MapLegend = new Dictionary<char, Type>();
                    _MapLegend.Add('*', (new SolidTile()).GetType());
                    _MapLegend.Add('_', (new SemiSolidTile()).GetType());
                }
                return _MapLegend;
            }
        }

        List<GameObject> levelObjects;

        Player Player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MapMaker mapMaker = new MapMaker(Content);
            mapMaker.Legend = MapLegend;
            levelObjects = mapMaker.ReadMap("maps/level1");
            Player = new Player(Content.Load<Texture2D>("Sprites/place-holder"), new Vector2(35, 400), 1, 1);
            levelObjects.Add(Player);

            for (int i = 0; i < levelObjects.Count; i++)
            {
                if (!(levelObjects[i] is Tile))
                    levelObjects[i].obeysGravity = true;
            }

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            Update_Player();
            Update_Positions();
            base.Update(gameTime);
        }

        private void Update_Player()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Player.Move(keyboardState.GetPressedKeys());
        }

        private void Update_Positions()
        {
            levelObjects.ForEach(i =>
            {
                i.position.X += i.velocity.X;
                i.position.Y += i.velocity.Y;
            });
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin();

            levelObjects.ForEach(i =>
            {
                spriteBatch.Draw(i.sprite, i.position, null, Color.White, i.rotation, i.center, 1.0f, SpriteEffects.None, 0);
            });

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
