using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
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

		private void UpdateAsteroids(Bag<Asteroid> asteroids, GameTime gameTime)
		{
			foreach (Asteroid asteroid in asteroids)
			{
				var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
				asteroid.Rotation += asteroid.RotationVelocity * delta;
				if (asteroid.Rotation > 360) asteroid.Rotation = 0;
			}
		}

		private void UpdateShip(Ship ship, GameTime gameTime)
		{

		}

		private void UpdateLasers(GameTime gameTime)
		{

		}

		public void Update(Game1 game, GameTime gameTime)
		{
			UpdateAsteroids(game.Asteroids, gameTime);
		}
		
	}
}
