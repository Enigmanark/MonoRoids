using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRoids.Core
{
	public class Controller_Draw
	{
		public SpriteFont Mono10 { get; set; }
		private Camera2D camera;

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, World world)
		{

			spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

			//Draw ship
			world.Ship.Draw(spriteBatch, gameTime);

			//Draw asteroids
			foreach (Asteroid asteroid in world.Asteroids)
			{
				asteroid.Draw(spriteBatch, gameTime);
			}

			//Draw lasers
			foreach (Laser laser in world.Lasers)
			{
				laser.Draw(spriteBatch, gameTime);
			}

			//Draw Score
			spriteBatch.DrawString(Mono10, "Hello World!", new Vector2(0, 0), Color.White);

			spriteBatch.End();
		}

		public void Init(BoxingViewportAdapter adapter)
		{
			//Init camera
			camera = new Camera2D(adapter);
		}

		public void Draw(World world, SpriteBatch batch, GameTime gameTime)
		{
			batch.Begin(transformMatrix: camera.GetViewMatrix());

			//Draw background
			
			//Draw asteroids
			foreach(Asteroid asteroid in world.Asteroids)
			{
				asteroid.Draw(batch, gameTime);
			}

			//Draw Lasers
			foreach(Laser laser in world.Lasers)
			{
				laser.Draw(batch, gameTime);
			}

			//Draw ship
			world.Ship.Draw(batch, gameTime);

			batch.End();
		}

		public void LoadContent(GameCore game)
		{

			Mono10 = game.Content.Load<SpriteFont>("FreeMono_10");
		}
	}
}
