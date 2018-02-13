using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace MonoRoids.Core
{

	public class World
	{
		public int BigAsteroidWorth = 100;
		public int SmallAsteroidWorth = 50;
		int _maxAsteroids = 5;
		public bool GameOver { get; set; }
		public int Score { get; set; }
		public int Lives { get; set; }
		public int Level { get; set; }
		public Bag<Asteroid> Asteroids { get; set; }
		public Bag<Laser> Lasers { get; set; }
		public Random Random { get; set; }
		public Ship Ship { get; set; }

		public Controller_Draw Drawer { get; set; }
		public Controller_Update Updater { get; set; }

		public Bag<Texture2D> LargeAsteroidTextures { get; set; }
		public Bag<Texture2D> SmallAsteroidTextures { get; set; }
		public Texture2D LaserTex { get; set; }
		public Texture2D Background { get; set; }

		public void Init(BoxingViewportAdapter adapter)
		{
			//Create Controllers
			Drawer = new Controller_Draw();
			Updater = new Controller_Update();

			//Init drawer
			Drawer.Init(adapter);

			//Init ship
			Ship = new Ship();

			//Init random
			Random = new Random();

			//Init asteroid bag
			Asteroids = new Bag<Asteroid>();

			//Init laser bag
			Lasers = new Bag<Laser>();

			//Init large asteroid textures bag
			LargeAsteroidTextures = new Bag<Texture2D>();

			//Init small asteroid textures bag
			SmallAsteroidTextures = new Bag<Texture2D>();

			//Init game vars
			Score = 0;
			Lives = 3;
			Level = 1;

			//Init asteroids
			for (int i = 0; i < _maxAsteroids; i++)
			{
				Asteroid asteroid = new Asteroid();
				Asteroids.Add(asteroid);
			}

		}

		public void LoadContent(GameCore game)
		{
			//Set ship texture
			Ship.LoadContent(game);

			//Load asteroid textures
			LargeAsteroidTextures.Add(game.Content.Load<Texture2D>("asteroid"));

			//Load Small asteroid textures
			SmallAsteroidTextures.Add(game.Content.Load<Texture2D>("smallasteroid1"));
			SmallAsteroidTextures.Add(game.Content.Load<Texture2D>("smallasteroid2"));

			//Load laser texture
			LaserTex = game.Content.Load<Texture2D>("laser");

			//Load background
			Background = game.Content.Load<Texture2D>("star_background1");

			Drawer.LoadContent(game);
		}

		public void PostInit()
		{
			//Place asteroids and set rotation velocity
			foreach (Asteroid asteroid in Asteroids)
			{
				asteroid.PostInit(LargeAsteroidTextures, Random, GameCore.SCREEN_WIDTH, GameCore.SCREEN_HEIGHT);
			}
		}



		public void Update(GameTime gameTime)
		{
			Updater.Update(this, gameTime);
		}
		
		public void Draw(SpriteBatch batch, GameTime gameTime)
		{
			Drawer.Draw(this, batch, gameTime);
		}
	}
}
