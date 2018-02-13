using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
		public Texture2D LaserTex { get; set; }
		public SoundEffect Effect { get; set; }

		public Laser(Texture2D texture, SoundEffect effect, Vector2 position, float rotation, Vector2 velocity)
		{
			Position = position;
			Velocity = velocity;
			Rotation = rotation;
			LaserTex = texture;
			SourceRect = new Rectangle(0, 0, LaserTex.Width, LaserTex.Height);
			Origin = new Vector2(LaserTex.Width / 2, LaserTex.Height / 2);
			effect.Play();
		}

		public void Draw(SpriteBatch batch, GameTime gameTime)
		{
			batch.Draw(LaserTex, Position, SourceRect, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 1);
		}
	}
}
