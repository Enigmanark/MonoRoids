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
		public int WINDOW_WIDTH { get; }
		public int WINDOW_HEIGHT { get; }
		public int SCREEN_WIDTH { get; }
		public int SCREEN_HEIGHT { get; }
		private Camera2D camera;
		public GraphicsDeviceManager Graphics { get; }
		SpriteBatch spriteBatch;
		public Ship Ship { get; }
		public Texture2D LaserTex { get; set; }
		public Bag<Asteroid> Asteroids { get; set; }
		public Bag<Laser> Lasers { get; set; }
		int _maxAsteroids = 5;
		Processor processor;
		public Random Random { get; }

		public Bag<Texture2D> LargeAsteroidTextures { get; set; }
		public Bag<Texture2D> SmallAsteroidTextures { get; set; }

		public GameCore()
        {
			//Init core stuff
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

			//Init laser bag
			Lasers = new Bag<Laser>();

			//Init large asteroid textures bag
			LargeAsteroidTextures = new Bag<Texture2D>();

			//Init small asteroid textures bag
			SmallAsteroidTextures = new Bag<Texture2D>();

			//Init processor
			processor = new Processor();

			//Init ship
			Ship = new Ship();

			//Init random
			Random = new Random();
		}

		protected override void Initialize()
        {
			//Init camera
			var adapter = new BoxingViewportAdapter(Window, GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);
			camera = new Camera2D(adapter);

			//Init asteroids
			for (int i = 0; i < _maxAsteroids; i++)
			{
				Asteroid asteroid = new Asteroid();
				Asteroids.Add(asteroid);
			}

			base.Initialize();
        }

		protected void PostInit()
		{

			//Place asteroids and set rotation velocity
			foreach(Asteroid asteroid in Asteroids)
			{
				asteroid.PostInit(LargeAsteroidTextures, Random, SCREEN_WIDTH, SCREEN_HEIGHT);
			}

		}

		protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

			LaserTex = Content.Load<Texture2D>("laser");

			//Set ship texture
			Ship.LoadContent(this);

			//Load asteroid textures
			LargeAsteroidTextures.Add(Content.Load<Texture2D>("asteroid"));

			//Load Small asteroid textures
			SmallAsteroidTextures.Add(Content.Load<Texture2D>("smallasteroid1"));
			SmallAsteroidTextures.Add(Content.Load<Texture2D>("smallasteroid2"));

			//Run post initialization
			PostInit();
        }

        protected override void UnloadContent()
        {
			Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
			processor.Update(this, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

			//Draw ship
			Ship.Draw(spriteBatch, gameTime);

			//Draw asteroids
			foreach(Asteroid asteroid in Asteroids)
			{
				asteroid.Draw(spriteBatch, gameTime);
			}

			//Draw lasers
			foreach(Laser laser in Lasers)
			{
				laser.Draw(spriteBatch, gameTime);
			}

			spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
