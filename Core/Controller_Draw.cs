using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace MonoRoids.Core
{
	public class Controller_Draw
	{
		public SpriteFont Mono10 { get; set; }
		private Camera2D camera;

		public void Init(BoxingViewportAdapter adapter)
		{
			//Init camera
			camera = new Camera2D(adapter);
		}

		public void Draw(World world, SpriteBatch batch, GameTime gameTime)
		{
			batch.Begin(transformMatrix: camera.GetViewMatrix());

			//Draw background
			batch.Draw(world.Background, Vector2.Zero, Color.White);

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

			//draw score
			batch.DrawString(Mono10, "Score: " + world.Score, new Vector2(10, 0), Color.White);

			//draw lives
			batch.DrawString(Mono10, "Lives: " + world.Lives, new Vector2(200, 0), Color.White);

			//Draw level
			batch.DrawString(Mono10, "Level: " + world.Level, new Vector2(300, 0), Color.White);

			batch.End();
		}

		public void LoadContent(GameCore game)
		{

			Mono10 = game.Content.Load<SpriteFont>("FreeMono_10");

		}
	}
}
