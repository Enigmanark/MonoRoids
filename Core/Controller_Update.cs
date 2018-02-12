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
	public class Controller_Update
	{
		private bool _canFire = true;
		private float _fireTimer = 0f;
		private float _fireDownTime = 0.35f;
		private int _laserSpeed = 220;

		public void Update(World world, GameTime gameTime)
		{
			Input(world, gameTime);
			UpdateShip(world.Ship, gameTime);
			UpdateLasers(world.Lasers, gameTime);
			UpdateAsteroids(GameCore.SCREEN_WIDTH, GameCore.SCREEN_HEIGHT, world.Asteroids, gameTime);
			ProcessCollisions(world);
		}

		private void Input(World world, GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			//countdown fire timer if _canFire is false
			if (!_canFire)
			{
				_fireTimer += delta;
				if (_fireTimer >= _fireDownTime)
				{
					_canFire = true;
					_fireTimer = 0;
				}
			}
			float circle = MathHelper.Pi * 2;

			//Thrust
			if (Keyboard.GetState().IsKeyDown(Keys.W))
			{
				var xVelocity = (float)(Math.Cos(world.Ship.Rotation) * world.Ship.Speed);
				var yVelocity = (float)(Math.Sin(world.Ship.Rotation) * world.Ship.Speed);

				world.Ship.Velocity = new Vector2(xVelocity, yVelocity);
			}

			//Brake
			if (Keyboard.GetState().IsKeyDown(Keys.Q))
			{
				world.Ship.Velocity = Vector2.Zero;
			}

			//Rotate to the left
			if (Keyboard.GetState().IsKeyDown(Keys.A))
			{
				world.Ship.Rotation -= world.Ship.TurnSpeed * delta;
				world.Ship.Rotation = world.Ship.Rotation % circle;
			}
			//Rotate to the right
			if (Keyboard.GetState().IsKeyDown(Keys.D))
			{
				world.Ship.Rotation += world.Ship.TurnSpeed * delta;
				world.Ship.Rotation = world.Ship.Rotation % circle;
			}

			//Fire
			if (Keyboard.GetState().IsKeyDown(Keys.Space) && _canFire)
			{
				var xVelocity = (float)(Math.Cos(world.Ship.Rotation) * _laserSpeed);
				var yVelocity = (float)(Math.Sin(world.Ship.Rotation) * _laserSpeed);
				var laser = new Laser(world.Ship.Position, world.Ship.Rotation, new Vector2(xVelocity, yVelocity));
				laser.LoadContent(world);
				world.Lasers.Add(laser);
				_canFire = false;
			}

		}

		private void ProcessCollisions(World world)
		{
			//Loop through lasers
			foreach (Laser laser in world.Lasers)
			{
				//Get the laser's hitbox
				var laserHitBox = laser.GetHitBox();
				//Now loop through asteroids
				foreach (Asteroid asteroid in world.Asteroids)
				{
					//Get the asteroids hitbox
					var asteroidHitBox = asteroid.GetHitBox();
					//If the laser hit the asteroid
					if (laserHitBox.Intersects(asteroidHitBox))
					{
						if (asteroid.Type == 1)
						{
							world.Asteroids.Remove(asteroid);
							world.Lasers.Remove(laser);
							var smallAsteroid1 = new Asteroid();
							smallAsteroid1.InitSmallAsteroid(world.SmallAsteroidTextures, world.Random, asteroid.Position);
							var smallAsteroid2 = new Asteroid();
							smallAsteroid2.InitSmallAsteroid(world.SmallAsteroidTextures, world.Random, asteroid.Position);
							world.Asteroids.Add(smallAsteroid1);
							world.Asteroids.Add(smallAsteroid2);
							world.Score += world.BigAsteroidWorth;
							break;
						}
						else if (asteroid.Type == 2)
						{
							world.Asteroids.Remove(asteroid);
							world.Lasers.Remove(laser);
							world.Score += world.SmallAsteroidWorth;
							break;
						}
					}
				}
			}

			//Loop through asteroids to see if ship is hit
			var shipHitBox = world.Ship.GetHitBox();
			foreach (Asteroid asteroid in world.Asteroids)
			{
				if (shipHitBox.Intersects(asteroid.GetHitBox()))
				{
					world.Lives -= 1;
					if (world.Lives > 0)
					{
						world.GameOver = true;
					}
					else
					{
						world.Ship.Respawn();
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
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
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
	}
}
