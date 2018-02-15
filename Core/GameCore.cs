using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.NuclexGui;
using MonoGame.Extended.NuclexGui.Controls.Desktop;
using MonoGame.Extended.ViewportAdapters;
using MonoRoids.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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
		private readonly GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		public int GameState { get; set; }
		World World;
		SpriteFont titleFont;
		SpriteFont defaultFont;
		Texture2D titleTexture;
		GuiManager titleGui;
		GuiManager highScoresGui;
		InputListenerComponent inputManager;
		public List<HighScore> HighScoreData;

		public GameCore()
        {

            graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Content.RootDirectory = "Content";

		}

		private List<HighScore> GetHighScores()
		{
			StreamReader r = new StreamReader("Content/highscores.json");
			var json = r.ReadToEnd();
			var data = JsonConvert.DeserializeObject<List<HighScore>>(json);
			return data;

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

			//Get high scores
			HighScoreData = GetHighScores();

			base.Initialize();
        }

		//Unload game
		public void UnloadGame()
		{
			World = null;
			GameState = 0;
			IsMouseVisible = true;
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
			IsMouseVisible = false;
		}

		protected override void LoadContent()
        {
			//intercept window exit event
			Exiting += ExitButtonPressed;

			spriteBatch = new SpriteBatch(GraphicsDevice);
			titleTexture = Content.Load<Texture2D>("star_background1");
			titleFont = Content.Load<SpriteFont>("TitleFont");
			defaultFont = Content.Load<SpriteFont>("DefaultFont");
        }

        protected override void UnloadContent()
        {
			Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
			if (GameState == 1) World.Update(this, gameTime);
			//Update GUI if on title screen
			else if (GameState == 0)
			{
				inputManager.Update(gameTime);
				titleGui.Update(gameTime);
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
				titleGui.Draw(gameTime);
			}

			else if(GameState == 2)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(titleTexture, Vector2.Zero, Color.White);
				int counter = 1;
				int divider = 50;
				int nameX = 200;
				int y = 40;
				int scoreX = 500;

				foreach (HighScore hs in HighScoreData)
				{
					spriteBatch.DrawString(defaultFont, hs.Name, new Vector2(nameX, y + (counter * divider)), Color.White);
					spriteBatch.DrawString(defaultFont, hs.Score.ToString(), new Vector2(scoreX, y + (counter * divider)), Color.White);
					counter++;
				}
				spriteBatch.End();
			}

            base.Draw(gameTime);
        }

		private void CreateGUI()
		{
			//Create input manager for GUI
			inputManager = new InputListenerComponent(this);

			//Create GUI
			var guiInputService = new GuiInputService(inputManager);
			titleGui = new GuiManager(Services, guiInputService);

			titleGui.Screen = new GuiScreen(GameCore.SCREEN_WIDTH, GameCore.SCREEN_HEIGHT);

			titleGui.Screen.Desktop.Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), new UniScalar(1f, 0), new UniScalar(1f, 0));

			titleGui.Initialize();

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
			titleGui.Screen.Desktop.Children.Add(StartButton);
			titleGui.Screen.Desktop.Children.Add(HighScoresButton);
			titleGui.Screen.Desktop.Children.Add(ExitButton);
		}

		private void StartButtonPressed(object Sender, EventArgs e)
		{
			LoadGame();
		}

		private void HighScoresButtonPressed(object Sender, EventArgs e)
		{
			GameState = 2;
		}

		private void SortHighScores()
		{
			HighScoreData.Sort(delegate (HighScore x, HighScore y)
			{
				return y.Score.CompareTo(x.Score);
			});
		}

		//Exit code, writes high scores back to highscore file and then closes application
		private void ExitButtonPressed(object Sender, EventArgs e)
		{
			SortHighScores();
			string json = JsonConvert.SerializeObject(HighScoreData.ToArray());
			System.IO.File.WriteAllText("Content/highscores.json", json);
			Exit();
		}
    }
}
