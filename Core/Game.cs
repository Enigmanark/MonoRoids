using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.ViewportAdapters;
using MonoRoids.Core;
using System;

namespace MonoRoids
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class GameCore : Game
	{
		public static int WINDOW_WIDTH = 800;
		public static int WINDOW_HEIGHT = 600;
		public static int SCREEN_WIDTH = 480;
		public static int SCREEN_HEIGHT = 300;
		BoxingViewportAdapter videoAdapter { get; set; }
		public GraphicsDeviceManager Graphics { get; }
		SpriteBatch spriteBatch;
		World World;

		public GameCore()
        {

            Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			Graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Content.RootDirectory = "Content";

		}

		protected override void Initialize()
        {

			//Init video adapter
			videoAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);

			//Init world
			World = new World();
			World.Init(videoAdapter);

			base.Initialize();
        }

		protected void PostInit()
		{

			World.PostInit(SCREEN_WIDTH, SCREEN_HEIGHT);
		}

		protected override void LoadContent()
        {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			World.LoadContent(this);

			//Run post initialization
			PostInit();
        }

        protected override void UnloadContent()
        {
			Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
			World.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Color.Black);

			World.Draw(spriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
