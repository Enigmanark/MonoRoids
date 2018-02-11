﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRoids.Core
{
	public class Ship : Entity
	{
		Texture2D shipTexture;

		public void Initialize()
		{
		}

		public void LoadContent(Game1 game)
		{
			//Load player ship
			shipTexture = game.Content.Load<Texture2D>("ship");
			Position = new Vector2(game.Graphics.PreferredBackBufferWidth / 2, game.Graphics.PreferredBackBufferHeight / 2);
			SourceRect = new Rectangle(0, 0, shipTexture.Width, shipTexture.Height);
			Origin = new Vector2(shipTexture.Width / 2, shipTexture.Height / 2);
		}

		public void Draw(SpriteBatch batch, GameTime gameTime)
		{
			batch.Draw(shipTexture, Position, SourceRect, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 1);
		}
	}
}