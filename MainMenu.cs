using System;
using System.Diagnostics;
using Locations; 

public class MainMenuScreen : Screen
{
    public MainMenuScreen()
    {
        Initialize();
    }

    public override void Initialize()
    {
        Display = new string[]
        {
            "> Welcome to the game!",
            "1. Start a new game",
            "2. Load a game",
            "3. Exit"
        };

        GameManager.SystemMessage = "";
    }

    public override void HandleInput(string input)
    {
        switch (input)
        {
            case "1":
                GameManager.SystemMessage = "Starting a new game...";
                GameManager.Instance.SetScreen(new TownSquare(GameManager.Instance)); 
                break;

            case "2":
                GameManager.SystemMessage = "Loading a game...";
                break;

            case "3":
                GameManager.SystemMessage = "Exiting the game. Goodbye!";
                Application.Exit();
                break;

            default:
                GameManager.SystemMessage = "Invalid input. Please try again.";
                break;
        }
    }
}
