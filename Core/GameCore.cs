using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.NuclexGui;
using MonoGame.Extended.NuclexGui.Controls;
using MonoGame.Extended.NuclexGui.Controls.Desktop;
using MonoGame.Extended.ViewportAdapters;
using MonoRoids.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
		public static int STATE_GAME = 1;
		public static int STATE_TITLE = 0;
		public static int STATE_SCORES = 2;
		public static int STATE_INPUTSCORE = 3;
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
		GuiManager inputHighScoreGui;
		InputListenerComponent inputManager;
		public List<HighScore> HighScoreData;
		private int transitionTo = -1;

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
			base.Initialize();

			//Init video adapter
			videoAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, SCREEN_WIDTH, SCREEN_HEIGHT);

			//Set mouse to visible
			IsMouseVisible = true;

			//Get high scores
			HighScoreData = GetHighScores();

			//Create input manager for GUI
			inputManager = new InputListenerComponent(this);

			//Set gamestate to 0 for titlescreen
			ChangeState(GameCore.STATE_TITLE);

        }

		//Unload game
		public void UnloadGame()
		{
			World = null;
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

		public void ChangeState(int state)
		{
			if(state == GameCore.STATE_TITLE)
			{
				UnloadGame();
				GameState = GameCore.STATE_TITLE;
				if(highScoresGui != null)highScoresGui.Dispose();
				titleGui = GuiFactory.CreateTitleGui(this, titleGui, inputManager);
			}
			else if(state == GameCore.STATE_GAME)
			{
				LoadGame();
				GameState = GameCore.STATE_GAME;
				titleGui.Dispose();
			}
			else if(state == GameCore.STATE_SCORES)
			{
				UnloadGame();
				GameState = GameCore.STATE_SCORES;
				titleGui.Dispose();
				highScoresGui = GuiFactory.CreateHighScoresGui(this, highScoresGui, inputManager);
				//SortHighScores();
			}
		}

        protected override void Update(GameTime gameTime)
        {
			//Update game if ingame
			if (GameState == GameCore.STATE_GAME) World.Update(this, gameTime);
			//Update GUI if on title screen
			else if (GameState == GameCore.STATE_TITLE)
			{
				inputManager.Update(gameTime);
				titleGui.Update(gameTime);
				if(transitionTo == GameCore.STATE_SCORES)
				{
					ChangeState(GameCore.STATE_SCORES);
					transitionTo = -1;
				}
			}
			//Update high scores if in high scores
			else if(GameState == GameCore.STATE_SCORES)
			{
				inputManager.Update(gameTime);
				highScoresGui.Update(gameTime);
				if(transitionTo == GameCore.STATE_TITLE)
				{
					ChangeState(GameCore.STATE_TITLE);
					transitionTo = -1;
				}
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
			else if (GameState == GameCore.STATE_TITLE)
			{
				spriteBatch.Begin();
				spriteBatch.Draw(titleTexture, Vector2.Zero, Color.White);
				spriteBatch.DrawString(titleFont, titleString, new Vector2(titleX, titleY), Color.White);
				spriteBatch.End();
				titleGui.Draw(gameTime);
			}

			else if(GameState == GameCore.STATE_SCORES)
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
				highScoresGui.Draw(gameTime);
			}

            base.Draw(gameTime);
        }

		public void BackButtonPressed(object Sender, EventArgs e)
		{
			transitionTo = GameCore.STATE_TITLE;
		}

		public void StartButtonPressed(object Sender, EventArgs e)
		{
			ChangeState(GameCore.STATE_GAME);
		}

		public void HighScoresButtonPressed(object Sender, EventArgs e)
		{
			transitionTo = GameCore.STATE_SCORES;
		}

		public void SubmitButtonPressed(object Sender, EventArgs e)
		{
			foreach(GuiControl guiC in inputHighScoreGui.Screen.Desktop.Children)
			{
				if(guiC.Name == "Text Input")
				{
					var highScoreData = new HighScore(guiC.Name, World.Score);
					HighScoreData.Add(highScoreData);
					SortHighScores();
				}
			}
		}

		public void SortHighScores()
		{
			HighScoreData.Sort(delegate (HighScore x, HighScore y)
			{
				return y.Score.CompareTo(x.Score);
			});
		}

		//Exit code, writes high scores back to highscore file and then closes application
		public void ExitButtonPressed(object Sender, EventArgs e)
		{
			SortHighScores();
			string json = JsonConvert.SerializeObject(HighScoreData.ToArray());
			System.IO.File.WriteAllText("Content/highscores.json", json);
			Exit();
		}
    }
}
