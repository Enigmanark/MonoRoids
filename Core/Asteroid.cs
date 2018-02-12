using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
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
		public int Type;
		public int MaxSpeed = 40;
		public float RotationVelocity { get; set; }
		public int Health { get; set; }
		public Texture2D AsteroidTexture {get;set;}

		public void InitSmallAsteroid(Bag<Texture2D> smallAsteroidTextures, Random random, Vector2 position)
		{
			//Init type
			Type = 2;

			//Init position
			Position = position;

			//Set random texture
			var index = random.Next(0, smallAsteroidTextures.Count);

			AsteroidTexture = smallAsteroidTextures[index];

			//Set rotation velocity
			var maxVelocity = 3;
			RotationVelocity = random.Next(1, maxVelocity);

			//Set movement velocity
			Speed = random.Next(0, MaxSpeed);
			int xDir = random.Next(-1, 1);
			int yDir = random.Next(-1, 1);

			Velocity = new Vector2(xDir, yDir);

			SourceRect = new Rectangle(0, 0, AsteroidTexture.Width, AsteroidTexture.Height);
			Origin = new Vector2(AsteroidTexture.Width / 2, AsteroidTexture.Height / 2);
		}

		public void PostInit(Bag<Texture2D> largeAsteroidTextures, Random random, int ScreenWidth, int ScreenHeight)
		{
			//Init type
			Type = 1;

			//Set random texture
			var index = random.Next(0, largeAsteroidTextures.Count);

			AsteroidTexture = largeAsteroidTextures[index];

			//Set position
			var minX = AsteroidTexture.Width;
			var maxX = ScreenWidth - AsteroidTexture.Width;
			var minY = AsteroidTexture.Height;
			var maxY = ScreenHeight - AsteroidTexture.Height;
			Position = new Vector2(random.Next(minX, maxX), random.Next(minY, maxY));

			//Set rotation velocity
			var maxVelocity = 3;
			RotationVelocity = random.Next(1, maxVelocity);

			//Set movement velocity
			Speed = random.Next(0, MaxSpeed);
			int xDir = random.Next(-1, 1);
			int yDir = random.Next(-1, 1);

			Velocity = new Vector2(xDir, yDir);

			SourceRect = new Rectangle(0, 0, AsteroidTexture.Width, AsteroidTexture.Height);
			Origin = new Vector2(AsteroidTexture.Width / 2, AsteroidTexture.Height / 2);
		}

		public void Draw(SpriteBatch batch, GameTime gameTime)
		{
			batch.Draw(AsteroidTexture, Position, SourceRect, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 1);
		}
	}
}
