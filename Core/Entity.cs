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
		public float Velocity { get; set; }
		public Rectangle SourceRect { get; set; }
		public Vector2 Origin { get; set; }

	}
}
