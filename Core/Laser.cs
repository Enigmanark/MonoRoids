using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRoids.Core
{
	public class Laser : Entity
	{
		public Texture2D Texture { get; set; }

		public Laser(Vector2 position, float rotation, Vector2 velocity)
		{
			Position = position;
			Velocity = velocity;
			Rotation = rotation;
		}

		public void LoadContent(Game game)
		{
			Texture = game.Content.Load<Texture2D>("laser");
			SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
			Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
		}

		public void Draw(SpriteBatch batch, GameTime gameTime)
		{
			batch.Draw(Texture, Position, SourceRect, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 1);
		}
	}
}
