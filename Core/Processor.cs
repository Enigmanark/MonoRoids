using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		private bool _canFire = true;
		private float _fireTimer = 0f;
		private float _fireDownTime = 0.35f;
		private int _laserSpeed = 220;

		private void Input(Ship ship, Texture2D laserTex, Bag<Laser> lasers,  GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			//countdown fire timer if _canFire is false
			if (!_canFire)
			{
				_fireTimer += delta;
				if(_fireTimer >= _fireDownTime)
				{
					_canFire = true;
					_fireTimer = 0;
				}
			}
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
				ship.Rotation -= ship.TurnSpeed * delta;
				ship.Rotation = ship.Rotation % circle;
			}
			//Rotate to the right
			if(Keyboard.GetState().IsKeyDown(Keys.D))
			{
				ship.Rotation += ship.TurnSpeed * delta;
				ship.Rotation = ship.Rotation % circle;
			}

			//Fire
			if(Keyboard.GetState().IsKeyDown(Keys.Space) && _canFire)
			{
				var xVelocity = (float)(Math.Cos(ship.Rotation) * _laserSpeed);
				var yVelocity = (float)(Math.Sin(ship.Rotation) * _laserSpeed);
				var laser = new Laser(ship.Position, ship.Rotation, new Vector2(xVelocity, yVelocity));
				laser.LoadContent(laserTex);
				lasers.Add(laser);
				_canFire = false;
			}
				
		}

		private void ProcessCollisions(GameCore game)
		{
			//Loop through lasers
			foreach(Laser laser in game.Lasers)
			{
				//Get the laser's hitbox
				var laserHitBox = laser.GetHitBox();
				//Now loop through asteroids
				foreach(Asteroid asteroid in game.Asteroids)
				{
					//Get the asteroids hitbox
					var asteroidHitBox = asteroid.GetHitBox();
					//If the laser hit the asteroid
					if(laserHitBox.Intersects(asteroidHitBox))
					{
						if (asteroid.Type == 1)
						{
							game.Asteroids.Remove(asteroid);
							game.Lasers.Remove(laser);
							var smallAsteroid1 = new Asteroid();
							smallAsteroid1.InitSmallAsteroid(game.SmallAsteroidTextures, game.Random, asteroid.Position);
							var smallAsteroid2 = new Asteroid();
							smallAsteroid2.InitSmallAsteroid(game.SmallAsteroidTextures, game.Random, asteroid.Position);
							game.Asteroids.Add(smallAsteroid1);
							game.Asteroids.Add(smallAsteroid2);
							break;
						}
						else if(asteroid.Type == 2)
						{
							game.Asteroids.Remove(asteroid);
							game.Lasers.Remove(laser);
							break;
						}
					}
				}
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

		private void UpdateLasers(Bag<Laser> lasers, GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			foreach (Laser laser in lasers)
			{
				laser.Position += (laser.Velocity * delta);
			}
		}

		public void Update(GameCore game, GameTime gameTime)
		{
			Input(game.Ship, game.LaserTex, game.Lasers, gameTime);
			UpdateShip(game.Ship, gameTime);
			UpdateLasers(game.Lasers, gameTime);
			UpdateAsteroids(game.SCREEN_WIDTH, game.SCREEN_HEIGHT, game.Asteroids, gameTime);
			ProcessCollisions(game);
		}
		
	}
}
