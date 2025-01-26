public class NPCScreen : Screen
{
    public string NPCName { get; set; }
    public string Description { get; set; }
    public List<string> DialogueOptions { get; set; }
    public List<string> DialogueResponses { get; set; }
    public string CurrentMessage { get; set; }

    public NPCScreen(string npcName)
    {
        NPCName = npcName;
        DialogueOptions = new List<string>();
        DialogueResponses = new List<string>();
        CurrentMessage = "Hello, adventurer! How can I help you today?";
    }

    public override void Initialize()
    {
        Display = new string[]
        {
            $"          {NPCName}",
            "----------------------------------------------------------------------------",
            $"{Description}",
            "----------------------------------------------------------------------------",
            "                    MESSAGE",
            CurrentMessage,
            "----------------------------------------------------------------------------",
            "                DIALOGUE OPTIONS"
        };

        // Add dialogue options
        for (int i = 0; i < DialogueOptions.Count; i++)
        {
            Display = Display.Concat(new[] { $"{i + 1}. {DialogueOptions[i]}" }).ToArray();
        }

        Display = Display.Concat(new[] {
            "----------------------------------------------------------------------------",
            "LEAVE - Return to location"
        }).ToArray();
    }

    public override void HandleInput(string input)
    {
        if (input.ToLower() == "leave")
        {
            GameManager.SystemMessage = "Returning to the location...";
            // Transition back to the location screen
            GameManager.Instance.SetScreen(GameManager.Instance.previousScreen);  // Change this to the correct location screen
            return;
        }

        // Handle dialogue option selection (e.g., "1", "2", etc.)
        if (int.TryParse(input, out int dialogueChoice) && dialogueChoice >= 1 && dialogueChoice <= DialogueOptions.Count)
        {
            // Update NPC's message with the corresponding response
            CurrentMessage = DialogueResponses[dialogueChoice - 1];
            GameManager.SystemMessage = $"NPC responds: {CurrentMessage}";
            Initialize();  // Re-initialize the screen to update the display
            return;
        }

        // If the input doesn't match a valid dialogue option or "leave", show an invalid input message
        GameManager.SystemMessage = "Invalid dialogue option. Please try again.";
    }
}
