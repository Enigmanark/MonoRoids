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
		public bool GameOver { get; set; }
		public int Score { get; set; }
		public int Lives { get; set; }
		public int Level { get; set; }
		public Bag<Asteroid> Asteroids { get; set; }
		public Bag<Laser> Lasers { get; set; }
		public Bag<Explosion> Explosions { get; set; }
		public Random Random { get; set; }
		public Ship Ship { get; set; }

		public Controller_Draw Drawer { get; set; }
		public Controller_Update Updater { get; set; }

		public Bag<Texture2D> LargeAsteroidTextures { get; set; }
		public Bag<Texture2D> SmallAsteroidTextures { get; set; }
		public Texture2D LaserTex { get; set; }
		public Texture2D Background { get; set; }
		public Texture2D ExplosionTex { get; set; }

		public bool TransitionToNewLevel { get; set; }
		public float TransitionTime = 2f;
		private float _transitionTimer = 0f;

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

			//Init explosion bag
			Explosions = new Bag<Explosion>();

			//Init large asteroid textures bag
			LargeAsteroidTextures = new Bag<Texture2D>();

			//Init small asteroid textures bag
			SmallAsteroidTextures = new Bag<Texture2D>();

			//Init game vars
			Score = 0;
			Lives = 3;
			Level = 0;

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

			//Load explosion texture
			ExplosionTex = game.Content.Load<Texture2D>("free_explosion_1_128x128");

			Drawer.LoadContent(game);
		}

		public void TransitionLevel(float delta)
		{
			_transitionTimer += delta;
			if(_transitionTimer >= TransitionTime)
			{
				_transitionTimer = 0;
				GenerateLevel();
				TransitionToNewLevel = false;
			}
		}

		public void PostInit()
		{
			TransitionToNewLevel = true;
		}

		private void GenerateLevel()
		{
			Level += 1;
			var maxAsteroids = Level * 1.5;

			for(int i = 0; i < maxAsteroids; i++)
			{
				var asteroid = new Asteroid();
				asteroid.GenerateLargeAsteroid(LargeAsteroidTextures, Random, GameCore.SCREEN_WIDTH, GameCore.SCREEN_HEIGHT);
				Asteroids.Add(asteroid);
			}
		}

		public void Update(GameCore game, GameTime gameTime)
		{
			if (TransitionToNewLevel) TransitionLevel((float)gameTime.ElapsedGameTime.TotalSeconds);
			else Updater.Update(game, this, gameTime);
		}
		
		public void Draw(SpriteBatch batch, GameTime gameTime)
		{
			Drawer.Draw(this, batch, gameTime);
		}
	}
}
