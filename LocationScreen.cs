using System;
using System.Collections.Generic;
using TextRPGWindow;

public class LocationScreen : Screen
{
    public string LocationName { get; set; }
    public string Description { get; set; }
    public int LocationId { get; set; }
    public List<string> ConnectedLocations { get; set; }
    private GameManager _gameManager;
    private MonsterFactory _monsterFactory;
    private DatabaseManager _dbManager;

    private bool addedDragon;

    public LocationScreen(string locationName, string description, int locationId, GameManager gameManager)
    {
        Console.WriteLine($"Initializing LocationScreen with: {locationName}, {description}, {locationId}");

        LocationName = locationName;
        Description = description;
        LocationId = locationId;
        _gameManager = gameManager;
        _monsterFactory = new MonsterFactory();
        _dbManager = new DatabaseManager();
        ConnectedLocations = _dbManager.GetConnectedLocations(LocationId);
    }

    public override void Initialize()
    {
        Console.WriteLine("Initializing the LocationScreen display...");

        var displayList = new List<string>
        {
            $"          {LocationName}",
            "----------------------------------------------------------------------------",
            $"{Description}",
            "----------------------------------------------------------------------------",
            "                      Connected Locations"
        };

        for (int i = 0; i < ConnectedLocations.Count; i++)
        {
            displayList.Add($"{i + 1}. {ConnectedLocations[i]}");
        }

        displayList.Add("----------------------------------------------------------------------------");
        displayList.Add("LFT  - Look for Trouble          TRAVEL [location #] - Travel to location");
        displayList.Add("CHAR - View Character Menu");

        Display = displayList.ToArray();
    }

    public override void HandleInput(string input)
    {
        if (input.ToLower() == "lft")
        {
            Monster spawnedMonster = _monsterFactory.SpawnMonsterFromLocation(LocationId);
            if (spawnedMonster != null)
            {
                GameManager.SystemMessage = $"You encountered a {spawnedMonster.Name}!\n" +
                                            $"HP: {spawnedMonster.Health}, " +
                                            $"Brawniness: {spawnedMonster.Brawniness}, " +
                                            $"Nimbility: {spawnedMonster.Nimbility}, " +
                                            $"Mysticigence: {spawnedMonster.Mysticigence}, " +
                                            $"Enduritution: {spawnedMonster.Enduritution}";
            }
            else
            {
                GameManager.SystemMessage = "No monsters appeared this time.";
            }
            _gameManager.UpdateDisplay();
            return;
        }

        if (input.ToLower().StartsWith("travel"))
        {
            var parts = input.Split(' ');
            if (parts.Length == 2 && int.TryParse(parts[1], out int locationIndex) &&
                locationIndex >= 1 && locationIndex <= ConnectedLocations.Count)
            {
                string destination = ConnectedLocations[locationIndex - 1];
                GameManager.SystemMessage = $"Traveling to {destination}...";

                var locationScreen = _gameManager.GetLocationScreen(destination);
                _gameManager.SetScreen(locationScreen);
                return;
            }
            else
            {
                GameManager.SystemMessage = "Invalid location number. Please try again.";
                return;
            }
        }

        if (input.ToLower() == "char")
        {
            GameManager.SystemMessage = "Opening character menu...";
            _gameManager.SetScreen(new CharacterMenuScreen());
            return;
        }

        // 🔹 Hidden Commands for Testing Database Modifications
        if (input.ToLower() == "add_dragon")
        {
            // Insert the Dragon and get its ID
            int dragonId = _dbManager.InsertMonster("Dragon", 500, 10, 20, 10, 50, 30, 40, 100);

            if (dragonId != -1)
            {
                // Add the monster to the location
                _dbManager.AddMonsterToLocation(dragonId, LocationId, 150.00m);
                GameManager.SystemMessage = "Dragon added to this location!";
            }
            else
            {
                GameManager.SystemMessage = "Error adding Dragon.";
            }
            addedDragon = true;
            _gameManager.UpdateDisplay();
            return;
        }

        if (input.ToLower() == "modify_dragon")
        {
            Monster dragon = _dbManager.GetMonsterByName("Dragon");
            if (dragon != null)
            {
                _dbManager.UpdateMonster(dragon.MonsterId, "Elder Dragon", 800, 15, 25, 12, 60, 35, 50, 120);
                _dbManager.UpdateMonsterSpawnChance(dragon.MonsterId, LocationId, 200.00m);
                GameManager.SystemMessage = "Dragon modified!";
            }
            else
            {
                GameManager.SystemMessage = "No Dragon found to modify.";
            }
            _gameManager.UpdateDisplay();
            return;
        }

        if (input.ToLower() == "delete_dragon")
        {
            Monster dragon;
            if (addedDragon)
            {
                dragon = _dbManager.GetMonsterByName("Elder Dragon");
            }
            else
            {
                dragon = _dbManager.GetMonsterByName("Dragon");
            }
            Console.WriteLine($"deleting dragon with id: {dragon.MonsterId}");
            if (dragon != null)
            {
                _dbManager.DeleteMonster(dragon.MonsterId);
                GameManager.SystemMessage = "Dragon removed from the game!";
            }
            else
            {
                GameManager.SystemMessage = "No Dragon found to delete.";
            }
            _gameManager.UpdateDisplay();
            return;
        }

        GameManager.SystemMessage = "Invalid command. Please try again.";
    }
}
