using System;
using System.Collections.Generic;

public class LocationScreen : Screen
{
    public string LocationName { get; set; }
    public string Description { get; set; }
    public List<NPCScreen> NPCs { get; set; }
    public List<string> ConnectedLocations { get; set; } 

    private GameManager _gameManager;

    public LocationScreen(string locationName, GameManager gameManager)
    {
        LocationName = locationName;
        Description = "Description of " + locationName;
        NPCs = new List<NPCScreen>();
        ConnectedLocations = new List<string>();
        _gameManager = gameManager; 
    }

    public override void Initialize()
    {
        var displayList = new List<string>
        {
            $"          {LocationName}",
        "----------------------------------------------------------------------------".PadRight(73).Substring(0, 73),
            $"{Description}",
        "----------------------------------------------------------------------------".PadRight(73).Substring(0, 73),
            "                      NPCs"
        };

        // Add NPC names
        for (int i = 0; i < NPCs.Count; i++)
        {
            displayList.Add($"{i + 1}. {NPCs[i].NPCName}");
        }

        displayList.Add("----------------------------------------------------------------------------".PadRight(73).Substring(0, 73));
        displayList.Add("                    Locations");

        // Add location names from the ConnectedLocations list
        for (int i = 0; i < ConnectedLocations.Count; i++)
        {
            displayList.Add($"{i + 1}. {ConnectedLocations[i]}");
        }

        displayList.Add("----------------------------------------------------------------------------".PadRight(73).Substring(0, 73));
        displayList.Add("LFT  - Look for Trouble          TRAVEL [location #] - Travel to location");
        displayList.Add("CHAR - View Character Menu       NPC [NPC #] - Speak to NPC");

        
        Display = displayList.ToArray();
    }

    public override void HandleInput(string input)
    {
        if (input.ToLower() == "lft")
        {
            GameManager.SystemMessage = "Looking for trouble...";
            // TODO: Combat start logic
            return;
        }

        if (input.ToLower() == "char")
        {
            GameManager.SystemMessage = "Opening character menu...";
            // Transition to the character menu screen
            _gameManager.SetScreen(new CharacterMenuScreen());
            return;
        }

        // Handle Travel command
        if (input.ToLower().StartsWith("travel"))
        {
            var parts = input.Split(' ');
            if (parts.Length == 2 && int.TryParse(parts[1], out int locationIndex) && locationIndex >= 1 && locationIndex <= ConnectedLocations.Count)
            {
                var locationName = ConnectedLocations[locationIndex - 1];
                GameManager.SystemMessage = $"Traveling to {locationName}...";
                var locationScreen = _gameManager.GetLocationScreen(locationName);
                _gameManager.SetScreen(locationScreen);

                return;
            }
            else
            {
                GameManager.SystemMessage = "Invalid location number. Please try again.";
                return;
            }
        }

        // Handle NPC command
        if (input.ToLower().StartsWith("npc"))
        {
            var parts = input.Split(' ');
            if (parts.Length == 2 && int.TryParse(parts[1], out int npcIndex) && npcIndex >= 1 && npcIndex <= NPCs.Count)
            {
                GameManager.SystemMessage = $"Speaking to {NPCs[npcIndex - 1].NPCName}...";
                _gameManager.SetScreen(NPCs[npcIndex - 1]);
                return;
            }
            else
            {
                GameManager.SystemMessage = "Invalid NPC number. Please try again.";
                return;
            }
        }
        GameManager.SystemMessage = "Invalid command. Please try again.";
    }
}
