using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.NuclexGui;
using MonoGame.Extended.NuclexGui.Controls.Desktop;
using MonoGame.Extended.ViewportAdapters;
using MonoRoids.Core;
using System;
using System.Diagnostics;

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
		private readonly GraphicsDeviceManager _graphics;
		SpriteBatch spriteBatch;
		public int GameState { get; set; }
		World World;
		SpriteFont titleFont;
		Texture2D titleTexture;
		GuiManager _gui;
		InputListenerComponent _inputManager;

		public GameCore()
        {

            _graphics = new GraphicsDeviceManager(this);
			_graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			_graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Content.RootDirectory = "Content";

		}

		protected override void Initialize()
        {

			//Init video adapter
			videoAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);

			//Set mouse to visible
			IsMouseVisible = true;

			//Init GUI
			CreateGUI();

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
			titleTexture = Content.Load<Texture2D>("star_background1");
			titleFont = Content.Load<SpriteFont>("TitleFont");
        }

        protected override void UnloadContent()
        {
			Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
			if (GameState == 1) World.Update(gameTime);
			//Update GUI if on title screen
			else if (GameState == 0)
			{
				_inputManager.Update(gameTime);
				_gui.Update(gameTime);
			}
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(Color.Black);
			var titleString = "MonoRoids";
			var titleX = (GameCore.WINDOW_WIDTH / 2) - (titleFont.MeasureString(titleString).X / 2);
			var titleY = 100;
			if (GameState == 1) World.Draw(spriteBatch, gameTime);
			else if (GameState == 0)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(titleTexture, Vector2.Zero, Color.White);
				spriteBatch.DrawString(titleFont, titleString, new Vector2(titleX, titleY), Color.White);
				spriteBatch.End();
				_gui.Draw(gameTime);
			}

            base.Draw(gameTime);
        }

		private void CreateGUI()
		{
			//Create input manager for GUI
			_inputManager = new InputListenerComponent(this);

			//Create GUI
			var guiInputService = new GuiInputService(_inputManager);
			_gui = new GuiManager(Services, guiInputService);

			_gui.Screen = new GuiScreen(GameCore.SCREEN_WIDTH, GameCore.SCREEN_HEIGHT);

			_gui.Screen.Desktop.Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), new UniScalar(1f, 0), new UniScalar(1f, 0));

			_gui.Initialize();

			//Create buttons
			var buttonWidth = 150;
			var buttonHeight = 50;
			var buttonX = (GameCore.WINDOW_WIDTH / 2) - (buttonWidth / 2);
			var buttonY = 200;
			var buttonDivider = 100;
			var StartButton = new GuiButtonControl
			{
				Name = "Start",
				Bounds = new UniRectangle(new UniScalar(buttonX), new UniScalar(buttonY), new UniScalar(buttonWidth), new UniScalar(buttonHeight)),
				Text = "Start"
			};

			var HighScoresButton = new GuiButtonControl
			{
				Name = "HighScores",
				Bounds = new UniRectangle(new UniScalar(buttonX), new UniScalar(buttonY + buttonDivider),
					new UniScalar(buttonWidth), new UniScalar(buttonHeight)),
				Text = "High Scores"
			};

			var ExitButton = new GuiButtonControl
			{
				Name = "Exit",
				Bounds = new UniRectangle(new UniScalar(buttonX), new UniScalar(buttonY + (buttonDivider * 2)),
					new UniScalar(buttonWidth), new UniScalar(buttonHeight)),
				Text = "Exit"
			};

			//Add functon to pressed
			StartButton.Pressed += StartButtonPressed;
			HighScoresButton.Pressed += HighScoresButtonPressed;
			ExitButton.Pressed += ExitButtonPressed;

			//Add buttons to gui
			_gui.Screen.Desktop.Children.Add(StartButton);
			_gui.Screen.Desktop.Children.Add(HighScoresButton);
			_gui.Screen.Desktop.Children.Add(ExitButton);
		}

		private void StartButtonPressed(object Sender, EventArgs e)
		{
			LoadGame();
		}

		private void HighScoresButtonPressed(object Sender, EventArgs e)
		{
			Debug.WriteLine("High scores button pressed!");
		}

		private void ExitButtonPressed(object Sender, EventArgs e)
		{
			Exit();
		}
    }
}
