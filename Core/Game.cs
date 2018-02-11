using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;
using MonoRoids.Core;
using System;

namespace MonoRoids
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; }
		SpriteBatch spriteBatch;
		Ship ship;
		public Bag<Asteroid> Asteroids { get; set; }
		int maxAsteroids = 5;
		Processor processor;

		public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = 480;
			Graphics.PreferredBackBufferHeight = 300;
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
			var random = new Random();

			//Place asteroids and set rotation velocity
			foreach(Asteroid asteroid in Asteroids)
			{
				//Set position
				var minX = asteroid.Texture.Width;
				var maxX = Graphics.PreferredBackBufferWidth - asteroid.Texture.Width;
				var minY = asteroid.Texture.Height;
				var maxY = Graphics.PreferredBackBufferHeight - asteroid.Texture.Height;
				asteroid.Position = new Vector2(random.Next(minX, maxX), random.Next(minY, maxY));

				//Set rotation velocity
				var maxVelocity = 7;
				asteroid.RotationVelocity = random.Next(1, maxVelocity);
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
			processor.Update(this, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin();

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
