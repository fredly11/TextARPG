using System;
using System.Collections.Generic;
using System.Diagnostics;

// Class that holds all of the essential game data and holds the current screen for display
public class GameManager
{
    public Screen currentScreen;
    public Screen previousScreen;
    public string lastUserInput = "";
    public static string SystemMessage = "";

    private static GameManager _instance;
    public static GameManager Instance => _instance ??= new GameManager();

    private TextBox _gameOutput; 
    private Dictionary<string, LocationScreen> locationCache;

    public Player player;

    public void SetGameOutput(TextBox gameOutput)
    {
        _gameOutput = gameOutput;
        UpdateDisplay();
    }

    public GameManager()
    {
        currentScreen = new MainMenuScreen();
        locationCache = new Dictionary<string, LocationScreen>();
        player = new Player();
    }


// Function that creates new location objects when loaded
//TODO: adjust to function dynamilcally with any number of location screens
public LocationScreen GetLocationScreen(string locationName)
{
    if (!locationCache.ContainsKey(locationName))
    {
        LocationScreen locationScreen = locationName switch
        {
            "Forest Glade" => new Locations.ForestGlade(this),
            "Mountain Pass" => new Locations.MountainPass(this), 
            "Town Square" => new Locations.TownSquare(this),
            _ => throw new Exception($"Location '{locationName}' not found.")
        };
        locationCache[locationName] = locationScreen;
    }
    return locationCache[locationName];
}


// loads a new screen and updates the display.
    public void SetScreen(Screen screen)
    {
        Debug.WriteLine("setting screen...");
        if (screen == null)
        {
            throw new ArgumentNullException(nameof(screen), "Screen cannot be null");
        }

        previousScreen = currentScreen;
        currentScreen = screen;
        currentScreen.Initialize();

        if (_gameOutput != null)
        {
            UpdateDisplay();
        }
        else
        {
            Console.WriteLine("Warning: gameOutput is null.");
        }
    }

// Processes the user's input by sending input to the screen for unique handling
    public void ProcessInput(string input)
    {
        if (currentScreen == null)
        {
            MessageBox.Show("Error: No current screen is set!", "Null Reference", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        lastUserInput = input;
        currentScreen.HandleInput(input);
    }

    // Updates the display and checks there is a screen and gameoutput object
    public void UpdateDisplay()
    {
        if (_gameOutput == null)
        {
            throw new ArgumentNullException(nameof(_gameOutput), "Game output TextBox cannot be null");
        }

        if (currentScreen?.Display == null)
        {
            throw new InvalidOperationException("Current screen or its display is not initialized");
        }

        Display.UpdateUI(_gameOutput, currentScreen.Display, lastUserInput, SystemMessage);
    }
}
