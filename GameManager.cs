using System;
using System.Collections.Generic;
using System.Diagnostics;
using TextRPGWindow;

public class GameManager
{
    public Screen currentScreen;
    public Screen previousScreen;
    public string lastUserInput = "";
    public static string SystemMessage = "";

    private static GameManager _instance;
    public static GameManager Instance => _instance ??= new GameManager();

    private TextBox _gameOutput;
    private Dictionary<string, LocationScreen> locationCache; // Cache for Location Screens
    private DatabaseManager _dbManager;  // Added DatabaseManager for dynamic loading of locations

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
        _dbManager = new DatabaseManager();  // Initialize the DatabaseManager
        player = new Player();
    }

    // Function that loads locations dynamically from the database and caches them
    public LocationScreen GetLocationScreen(string locationName)
    {
        if (!locationCache.ContainsKey(locationName))
        {
            // Fetch location data from the database
            LocationScreen locationScreen = _dbManager.GetLocationByName(locationName, this);

            if (locationScreen == null)
            {
                throw new Exception($"Location '{locationName}' not found in the database.");
            }

            // Cache the location screen
            locationCache[locationName] = locationScreen;
        }
        return locationCache[locationName];
    }

    // Function to load a new screen and update the display
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

    // Function to process the user's input by sending input to the screen for unique handling
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

    // Updates the display and ensures the screen and game output are properly initialized
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
