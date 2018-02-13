using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;
using MonoRoids.Core;

namespace MonoRoids
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class GameCore : Game
	{
		public static int WINDOW_WIDTH = 800;
		public static int WINDOW_HEIGHT = 600;
		public static int SCREEN_WIDTH = 480;
		public static int SCREEN_HEIGHT = 300;
		BoxingViewportAdapter videoAdapter { get; set; }
		public GraphicsDeviceManager Graphics { get; }
		SpriteBatch spriteBatch;
		public int GameState { get; set; }
		World World;

		public GameCore()
        {

            Graphics = new GraphicsDeviceManager(this);
			Graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			Graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Content.RootDirectory = "Content";

		}

		protected override void Initialize()
        {

			//Init video adapter
			videoAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);

			//Set gamestate to 0 for titlescreen
			GameState = 0;

			base.Initialize();
        }

		//Only run on Start game
		protected void LoadGame()
		{
			//Init world
			World = new World();
			World.Init(videoAdapter);
			World.LoadContent(this);
			World.PostInit();
			GameState = 1;
		}

		protected override void LoadContent()
        {
			spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
			Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
			if(GameState == 1) World.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Color.Black);

			if(GameState == 1) World.Draw(spriteBatch, gameTime);

            base.Draw(gameTime);
        }
    }
}
