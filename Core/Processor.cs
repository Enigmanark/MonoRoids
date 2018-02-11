using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRoids.Core
{

	public class Processor
	{

		private void Input(Game1 game, GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				game.Exit();
		}

		private void UpdateAsteroids(int ScreenWidth, int ScreenHeight, Bag<Asteroid> asteroids, GameTime gameTime)
		{
			foreach (Asteroid asteroid in asteroids)
			{
				var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

				//Spin asteroid
				asteroid.Rotation += asteroid.RotationVelocity * delta;
				if (asteroid.Rotation > 360) asteroid.Rotation = 0;

				//Move asteroid
				asteroid.Position += (asteroid.Velocity * asteroid.Speed) * delta;

				//Wrap asteroid
				if (asteroid.Position.X > ScreenWidth) asteroid.Position = new Vector2(0, asteroid.Position.Y);
				else if (asteroid.Position.X < 0) asteroid.Position = new Vector2(ScreenWidth, asteroid.Position.Y);
				if (asteroid.Position.Y > ScreenHeight) asteroid.Position = new Vector2(asteroid.Position.X, 0);
				else if (asteroid.Position.Y < 0) asteroid.Position = new Vector2(asteroid.Position.X, ScreenHeight);
			}
		}

		private void UpdateShip(Ship ship, GameTime gameTime)
		{

		}

		private void UpdateLasers(GameTime gameTime)
		{

		}

		public void Update(int ScreenWidth, int ScreenHeight, Bag<Asteroid> asteroids, GameTime gameTime)
		{
			UpdateAsteroids(ScreenWidth, ScreenHeight, asteroids, gameTime);
		}
		
	}
}
