using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRoids.Core
{
	public class Ship : Entity
	{
		Texture2D shipTexture;
		public int TurnSpeed { get; }
		public bool Invulnerable = true;
		private float _invulnerableTime = 3.5f;
		private float _invulnerableTimer = 0f;
		public Ship()
		{
			Speed = 50;
			TurnSpeed = 2;
		}

		public void Update(float delta)
		{
			Position += (Velocity * delta);
			if(Invulnerable)
			{
				_invulnerableTimer += delta;
				if(_invulnerableTimer >= _invulnerableTime)
				{
					Invulnerable = false;
					_invulnerableTimer = 0f;
				}
			}
		}

		public void Respawn()
		{
			Position = new Vector2(GameCore.SCREEN_WIDTH / 2, GameCore.SCREEN_HEIGHT / 2);
			Velocity = Vector2.Zero;
			Invulnerable = true;
		}

		public void LoadContent(GameCore game)
		{
			//Load player ship
			shipTexture = game.Content.Load<Texture2D>("ship");
			Position = new Vector2(GameCore.SCREEN_WIDTH / 2, GameCore.SCREEN_HEIGHT / 2);
			SourceRect = new Rectangle(0, 0, shipTexture.Width, shipTexture.Height);
			Origin = new Vector2(shipTexture.Width / 2, shipTexture.Height / 2);
		}

		public void Draw(SpriteBatch batch, GameTime gameTime)
		{
			if(Invulnerable) batch.Draw(shipTexture, Position, SourceRect, Color.CadetBlue, Rotation, Origin, 1.0f, SpriteEffects.None, 1);
			else batch.Draw(shipTexture, Position, SourceRect, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 1);
		}
	}
}
