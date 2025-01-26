using System;
using System.Collections.Generic;
using System.Linq;

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
        // Helper function to wrap text
        static List<string> WrapText(string text, int maxWidth)
        {
            var wrappedLines = new List<string>();
            var words = text.Split(' ');

            string currentLine = "";
            foreach (var word in words)
            {
                if (currentLine.Length + word.Length + 1 > maxWidth)
                {
                    wrappedLines.Add(currentLine.Trim());
                    currentLine = "";
                }
                currentLine += word + " ";
            }

            if (!string.IsNullOrWhiteSpace(currentLine))
                wrappedLines.Add(currentLine.Trim());

            return wrappedLines;
        }

        var displayList = new List<string>
        {
            $"          {NPCName}",
            "----------------------------------------------------------------------------".PadRight(73).Substring(0, 73)
        };

        // Wrap and add the description
        displayList.AddRange(WrapText(Description, 73));
        displayList.Add("----------------------------------------------------------------------------".PadRight(73).Substring(0, 73));

        // Add current message
        displayList.Add("                    MESSAGE");
        displayList.AddRange(WrapText(CurrentMessage, 73));
        displayList.Add("----------------------------------------------------------------------------".PadRight(73).Substring(0, 73));

        // Add dialogue options
        displayList.Add("                DIALOGUE OPTIONS");
        for (int i = 0; i < DialogueOptions.Count; i++)
        {
            displayList.Add($"{i + 1}. {DialogueOptions[i]}");
        }

        displayList.Add("----------------------------------------------------------------------------".PadRight(73).Substring(0, 73));
        displayList.Add("LEAVE - Return to location");

        Display = displayList.ToArray();
    }

    public override void HandleInput(string input)
    {
        if (input.ToLower() == "leave")
        {
            GameManager.SystemMessage = "Returning to the location...";
            GameManager.Instance.SetScreen(GameManager.Instance.previousScreen);
            return;
        }

        if (int.TryParse(input, out int dialogueChoice) && dialogueChoice >= 1 && dialogueChoice <= DialogueOptions.Count)
        {
            CurrentMessage = DialogueResponses[dialogueChoice - 1];
            Initialize();

            // Refresh the display
            GameManager.Instance.UpdateDisplay();
            return;
        }

        GameManager.SystemMessage = "Invalid dialogue option. Please try again.";
    }
}
