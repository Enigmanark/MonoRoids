using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRoids.Core
{
	public class Asteroid : Entity
	{
		public int ID { get; }
		public float RotationVelocity { get; set; }
		public int Health { get; set; }

		public Texture2D asteroidTexture1 { get; set; }
		public int AsteroidWidth = 32;
		public int AsteroidHeight = 32;
		public Asteroid(int id)
		{
			ID = id;
		}

		public void PostInit(Random random, int ScreenWidth, int ScreenHeight)
		{

			//Set position
			var minX = AsteroidWidth;
			var maxX = ScreenWidth - AsteroidWidth;
			var minY = AsteroidHeight;
			var maxY = ScreenHeight - AsteroidHeight;
			Position = new Vector2(random.Next(minX, maxX), random.Next(minY, maxY));

			//Set rotation velocity
			var maxVelocity = 3;
			RotationVelocity = random.Next(1, maxVelocity);

			//Set movement velocity
			Speed = random.Next(0, 20);
			int xDir = random.Next(-1, 1);
			int yDir = random.Next(-1, 1);

			Velocity = new Vector2(xDir, yDir);
		}

		public void LoadContent(Game game)
		{
			//Load asteroid texture
			asteroidTexture1 = game.Content.Load<Texture2D>("asteroid");
			SourceRect = new Rectangle(0, 0, AsteroidWidth, AsteroidHeight);
			Origin = new Vector2(AsteroidWidth / 2, AsteroidHeight / 2);
		}

		public void Draw(SpriteBatch batch, GameTime gameTime)
		{
			batch.Draw(asteroidTexture1, Position, SourceRect, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 1);
		}
	}
}
