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
	public class Game1 : Game
	{
		public int WINDOW_WIDTH { get; }
		public int WINDOW_HEIGHT { get; }
		public int SCREEN_WIDTH { get; }
		public int SCREEN_HEIGHT { get; }
		private Camera2D camera;
        public GraphicsDeviceManager Graphics { get; }
		SpriteBatch spriteBatch;
		Ship ship;
		public Bag<Asteroid> Asteroids { get; set; }
		int maxAsteroids = 5;
		Processor processor;
		Random random = new Random();

		public Game1()
        {
			WINDOW_WIDTH = 800;
			WINDOW_HEIGHT = 600;
			SCREEN_WIDTH = 480;
			SCREEN_HEIGHT = 300;

            Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			Graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Content.RootDirectory = "Content";

			//Init asteroid bag
			Asteroids = new Bag<Asteroid>();

			//Init processor
			processor = new Processor();

			//Init ship
			ship = new Ship();

		}

		protected override void Initialize()
        {
			//Init camera
			var adapter = new BoxingViewportAdapter(Window, GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);
			camera = new Camera2D(adapter);

			//Init asteroids
			for (int i = 0; i < maxAsteroids; i++)
			{
				Asteroid asteroid = new Asteroid(i);
				Asteroids.Add(asteroid);
			}

			base.Initialize();
        }

		protected void PostInit()
		{

			//Place asteroids and set rotation velocity
			foreach(Asteroid asteroid in Asteroids)
			{
				asteroid.PostInit(random, SCREEN_WIDTH, SCREEN_HEIGHT);
			}

		}

		protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

			//Load ship texture
			ship.LoadContent(this);
			

			//Load asteroid textures
			foreach(Asteroid asteroid in Asteroids)
			{
				asteroid.LoadContent(this);
			}

			//Run post initialization
			PostInit();
        }

        protected override void UnloadContent()
        {
			Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
			processor.Update(SCREEN_WIDTH, SCREEN_HEIGHT, ship, Asteroids, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

			//Draw ship
			ship.Draw(spriteBatch, gameTime);

			//Draw asteroids
			foreach(Asteroid asteroid in Asteroids)
			{
				asteroid.Draw(spriteBatch, gameTime);
			}

			spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
