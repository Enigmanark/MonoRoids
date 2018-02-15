using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.NuclexGui;
using MonoGame.Extended.NuclexGui.Controls.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoRoids.Core
{
	public class GuiFactory
	{
		public static GuiManager CreateHighScoresGui(GameCore game, GuiManager highScoresGui, InputListenerComponent inputManager)
		{
			//Create gui
			var guiInputService = new GuiInputService(inputManager);
			highScoresGui = new GuiManager(game.Services, guiInputService);

			highScoresGui.Screen = new GuiScreen(GameCore.SCREEN_WIDTH, GameCore.SCREEN_HEIGHT);

			highScoresGui.Screen.Desktop.Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), new UniScalar(1f, 0), new UniScalar(1f, 0));

			//Create back button
			var buttonWidth = 150;
			var buttonHeight = 50;
			var buttonX = (GameCore.WINDOW_WIDTH / 2) - (buttonWidth / 2);
			var buttonY = (GameCore.WINDOW_HEIGHT - 70);

			var backButton = new GuiButtonControl
			{
				Name = "Back",
				Bounds = new UniRectangle(new UniScalar(buttonX), new UniScalar(buttonY), new UniScalar(buttonWidth), new UniScalar(buttonHeight)),
				Text = "Back"
			};

			backButton.Pressed += game.BackButtonPressed;
			highScoresGui.Screen.Desktop.Children.Add(backButton);
			highScoresGui.Initialize();

			return highScoresGui;
		}

		public static GuiManager CreateTitleGui(GameCore game, GuiManager titleGui, InputListenerComponent inputManager)
		{
			//Create GUI
			var guiInputService = new GuiInputService(inputManager);
			titleGui = new GuiManager(game.Services, guiInputService);

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
			StartButton.Pressed += game.StartButtonPressed;
			HighScoresButton.Pressed += game.HighScoresButtonPressed;
			ExitButton.Pressed += game.ExitButtonPressed;

			//Add buttons to gui
			titleGui.Screen.Desktop.Children.Add(StartButton);
			titleGui.Screen.Desktop.Children.Add(HighScoresButton);
			titleGui.Screen.Desktop.Children.Add(ExitButton);

			return titleGui;
		}

		public static GuiManager CreateInputHighScoreGui(GameCore game, GuiManager inputHighScoreGui, InputListenerComponent inputManager)
		{
			//Create gui
			var guiInputService = new GuiInputService(inputManager);
			inputHighScoreGui = new GuiManager(game.Services, guiInputService);

			inputHighScoreGui.Screen = new GuiScreen(GameCore.SCREEN_WIDTH, GameCore.SCREEN_HEIGHT);

			inputHighScoreGui.Screen.Desktop.Bounds = new UniRectangle(new UniScalar(0f, 0), new UniScalar(0f, 0), new UniScalar(1f, 0), new UniScalar(1f, 0));

			//Create Submit button
			var buttonWidth = 150;
			var buttonHeight = 50;
			var buttonX = (GameCore.WINDOW_WIDTH / 2) - (buttonWidth / 2);
			var buttonY = (GameCore.WINDOW_HEIGHT - 200);

			var submitButton = new GuiButtonControl
			{
				Name = "Submit",
				Bounds = new UniRectangle(new UniScalar(buttonX), new UniScalar(buttonY), new UniScalar(buttonWidth), new UniScalar(buttonHeight)),
				Text = "Submit"
			};

			var inputWidth = 200;
			var inputHeight = 20;
			var inputX = (GameCore.WINDOW_WIDTH / 2) - (inputWidth / 2);
			var inputY = (GameCore.WINDOW_HEIGHT - 400);

			var textInput = new GuiInputControl
			{
				Name = "Text Input",
				Bounds = new UniRectangle(new UniScalar(inputX), new UniScalar(inputY), new UniScalar(inputWidth), new UniScalar(inputHeight)),
				Text = "Name"
			};

			submitButton.Pressed += game.SubmitButtonPressed;

			inputHighScoreGui.Screen.Desktop.Children.Add(submitButton);
			inputHighScoreGui.Screen.Desktop.Children.Add(textInput);
			inputHighScoreGui.Initialize();

			return inputHighScoreGui;
		}
	}
}
