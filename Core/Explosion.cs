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
	public class Explosion : Entity
	{
		public Texture2D Texture;
		private int frameSize = 128;
		private float _timer = 0f;
		private int frame = 0;
		public bool destroy { get; set; }
		private float frameTime = 0.1f;
		private int frames = 8;

		public Explosion(Texture2D tex, Vector2 pos)
		{
			Texture = tex;
			Position = pos;
			destroy = false;
			Rotation = 0;
			Origin = Vector2.Zero;
		}

		public void Update(float delta)
		{
			_timer += delta;
			if(_timer >= frameTime)
			{
				_timer = 0;
				frame += 1;
				if (frame > frames) destroy = true;
			}
		}

		private Rectangle GetSourceRect()
		{
			return( new Rectangle(frame * frameSize, 0, frameSize, frameSize) );
		}

		private Vector2 GetDrawPosition()
		{
			return new Vector2(Position.X - (frameSize / 2), Position.Y - (frameSize / 2));
		}

		public void Draw(SpriteBatch batch)
		{
			batch.Draw(texture: Texture, position: GetDrawPosition(), sourceRectangle: GetSourceRect());
		}
	}
}
