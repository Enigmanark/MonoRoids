using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRoids.Core
{
	public class HighScore
	{
		public string Name;
		public int Score;

		public HighScore(String n, int s)
		{
			Name = n;
			Score = s;
		}
	}
}
