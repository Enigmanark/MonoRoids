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

		private void Input(Ship ship, GameTime gameTime)
		{
			var turnSpeed = ship.TurnSpeed;
			var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
			float circle = MathHelper.Pi * 2;

			//Thrust
			if (Keyboard.GetState().IsKeyDown(Keys.W))
			{
				var xVelocity = (float)(Math.Cos(ship.Rotation) * ship.Speed);
				var yVelocity = (float)(Math.Sin(ship.Rotation) * ship.Speed);

				ship.Velocity = new Vector2(xVelocity, yVelocity);
			}

			//Brake
			if(Keyboard.GetState().IsKeyDown(Keys.Q))
			{
				ship.Velocity = Vector2.Zero;
			}

			//Rotate to the left
			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				ship.Rotation -= turnSpeed * delta;
				ship.Rotation = ship.Rotation % circle;
			}
			//Rotate to the right
			if(Keyboard.GetState().IsKeyDown(Keys.D))
			{
				ship.Rotation += turnSpeed * delta;
				ship.Rotation = ship.Rotation % circle;
			}

			//Fire
			if(Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				
			}
				
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
			var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
			ship.Position += (ship.Velocity * delta);
		}

		private void UpdateLasers(GameTime gameTime)
		{

		}

		public void Update(int ScreenWidth, int ScreenHeight, Ship ship, Bag<Asteroid> asteroids, GameTime gameTime)
		{
			Input(ship, gameTime);
			UpdateShip(ship, gameTime);
			UpdateAsteroids(ScreenWidth, ScreenHeight, asteroids, gameTime);
		}
		
	}
}
