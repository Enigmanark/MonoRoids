using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;
using System;
using System.Diagnostics;

namespace MonoRoids.Core
{
	public class Controller_Update
	{
		private bool _canFire = true;
		private float _fireTimer = 0f;
		private float _fireDownTime = 0.35f;
		private int _laserSpeed = 220;

		public void Update(GameCore game, World world, GameTime gameTime)
		{
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (world.GameOver == true)
			{
				if(Keyboard.GetState().IsKeyDown(Keys.Enter))
				{
					game.UnloadGame();
				}
			}
			else { 
				Input(game, world, delta);
				UpdateShip(world.Ship, delta);
				UpdateLasers(world.Lasers, delta);
				UpdateAsteroids(world.Asteroids, delta);
				ProcessCollisions(world);
				UpdateExplosions(world.Explosions, delta);
				if (AreAllAsteroidsDestroyed(world.Asteroids)) world.TransitionToNewLevel = true;
			}
		}

		private void Input(GameCore game, World world, float delta)
		{
			//Return to menu?
			if (Keyboard.GetState().IsKeyDown(Keys.Escape)) game.UnloadGame();

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
				var laser = new Laser(world.LaserTex, world.LaserSnd, world.Ship.Position, world.Ship.Rotation, new Vector2(xVelocity, yVelocity));
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
						//If it was a big asteroid
						if (asteroid.Type == 1)
						{
							//Make an explosion
							world.Explosions.Add(new Explosion(world.ExplosionTex, world.ExplosionSnd, asteroid.Position));

							//Destroy the big asteroid and the laser and create 2 small asteroids
							world.Asteroids.Remove(asteroid);
							world.Lasers.Remove(laser);
							var smallAsteroid1 = new Asteroid();
							smallAsteroid1.GenerateSmallAsteroid(world.SmallAsteroidTextures, world.Random, asteroid.Position);
							var smallAsteroid2 = new Asteroid();
							smallAsteroid2.GenerateSmallAsteroid(world.SmallAsteroidTextures, world.Random, asteroid.Position);
							world.Asteroids.Add(smallAsteroid1);
							world.Asteroids.Add(smallAsteroid2);
							world.Score += world.BigAsteroidWorth;
							break;
						}
						//If it was a small asteroid
						else if (asteroid.Type == 2)
						{
							//Make an explosion
							world.Explosions.Add(new Explosion(world.ExplosionTex, world.ExplosionSnd, asteroid.Position));

							//Destroy the small asteroid and the laser
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
					if (world.Lives < 0)
					{
						world.GameOver = true;
					}
					else
					{
						world.ReloadingLevel = true;
					}
					break;
				}
			}
		}

		private bool AreAllAsteroidsDestroyed(Bag<Asteroid> asteroids)
		{
			if (asteroids.Count == 0)
			{
				return true;
			}
			else return false;
		}

		private void UpdateExplosions(Bag<Explosion> explosions, float delta)
		{
			foreach(Explosion explosion in explosions)
			{
				explosion.Update(delta);
				if(explosion.destroy)
				{
					explosions.Remove(explosion);
				}
			}
		}

		private void UpdateAsteroids(Bag<Asteroid> asteroids, float delta)
		{
			foreach (Asteroid asteroid in asteroids)
			{
				asteroid.Update(delta);
			}
		}

		private void UpdateShip(Ship ship, float delta)
		{
			ship.Update(delta);
		}

		private void UpdateLasers(Bag<Laser> lasers, float delta)
		{
			foreach (Laser laser in lasers)
			{
				laser.Position += (laser.Velocity * delta);
			}
		}
	}
}
