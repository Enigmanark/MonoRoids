using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

		public Texture2D Texture { get; set; }

		public Asteroid(int id)
		{
			ID = id;
		}

		public void LoadContent(Game1 game)
		{
			Texture = game.Content.Load<Texture2D>("asteroid");
			SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
			Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
		}

		public void Draw(SpriteBatch batch, GameTime gameTime)
		{
			batch.Draw(Texture, Position, SourceRect, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 1);
		}
	}
}
