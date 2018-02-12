using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRoids.Core
{
	public class Entity
	{
		public Vector2 Position { get; set; }
		public float Rotation { get; set; }
		public Vector2 Velocity { get; set; }
		public Rectangle SourceRect { get; set; }
		public Vector2 Origin { get; set; }
		public float Speed { get; set; }

		public Rectangle GetHitBox()
		{
			return (new Rectangle((int)Position.X, (int)Position.Y, SourceRect.Size.X, SourceRect.Size.Y));
		}

	}
}
