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

        Dictionary<char, Type> MapLegend = GetLegend();
        List<GameObject> levelObjects;

        private static Dictionary<char, Type> GetLegend()
        {
            Dictionary<char, Type> legend = new Dictionary<char, Type>();
            legend.Add('*', (new SolidTile()).GetType());
            legend.Add('_', (new SemiSolidTile()).GetType());
            return legend;
        }

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
            MapMaker mapMaker = new MapMaker();
            mapMaker.Legend = MapLegend;
            levelObjects = mapMaker.ReadMap("maps/level1");
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

            base.Update(gameTime);
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
