using System;
using System.Linq;
using System.Diagnostics;

public class CharacterMenuScreen : Screen
{
    private Player player;
    private int maneuverScrollIndex = 0;
    private int strikeScrollIndex = 0;

    public CharacterMenuScreen()
    {
        player = GameManager.Instance.player;
    }

    public override void Initialize()
    {
        Actions.Add("spend", () => SpendPoints(GameManager.Instance.lastUserInput));
        Actions.Add("tree", ViewAbilityTree);
        Actions.Add("return", ReturnToGame);
        Actions.Add("quit", QuitGame);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var maneuvers = player.BasicManeuverAbilities.Skip(maneuverScrollIndex).Take(4).Select(a => a?.Name ?? "--Empty--").ToList();
        var strikes = player.BasicStrikeAbilities.Skip(strikeScrollIndex).Take(4).Select(a => a?.Name ?? "--Empty--").ToList();

        while (maneuvers.Count < 4) maneuvers.Add("--Empty--");
        while (strikes.Count < 4) strikes.Add("--Empty--");

        Display = new string[]
        {
            $"Level: {player.Level}".PadRight(40) + $"Abilities".PadLeft(20),
            $"Experience: {player.Experience}".PadRight(40) + $"--------------",
            $"Next Level: {player.ExperienceForNextLevel}".PadRight(40) + $"Maneuvers:",
            $"Hit Points: {player.Health}".PadRight(40) + $"{maneuvers[0]}",
            $"Movement Points: {player.MovementPoints}".PadRight(40) + $"{maneuvers[1]}",
            $"Energy: {player.Energy}".PadRight(40) + $"{maneuvers[2]}",
            "---------------".PadRight(40) + $"{maneuvers[3]}",
            $"Brawniness: {player.Brawniness}".PadRight(40),
            $"Nimbility: {player.Nimbility}".PadRight(40) + $"Strikes:",
            $"Mysticigence: {player.Mysticigence}".PadRight(40) + $"{strikes[0]}",
            $"Enduritution: {player.Enduritution}".PadRight(40) + $"{strikes[1]}",
            $"Available Points: {player.AvailableStatPoints}".PadRight(40) + $"{strikes[2]}",
            "---------------".PadRight(40) + $"{strikes[3]}",
            "Spend Points - spend [first letter of stat] [amount to add]".PadRight(40),
            "View Abilities - tree".PadRight(40),
            "Scroll Ability preview - [Press down arrow or up arrow]".PadRight(40),
            "Return to game - return".PadRight(40),
            "Quit - quit".PadRight(40)
        };

        GameManager.Instance.UpdateDisplay();
    }

    public void ScrollDown()
    {
        if (maneuverScrollIndex < player.BasicManeuverAbilities.Count - 4 || strikeScrollIndex < player.BasicStrikeAbilities.Count - 4)
        {
            maneuverScrollIndex = Math.Min(maneuverScrollIndex + 1, Math.Max(0, player.BasicManeuverAbilities.Count - 4));
            strikeScrollIndex = Math.Min(strikeScrollIndex + 1, Math.Max(0, player.BasicStrikeAbilities.Count - 4));
            UpdateDisplay();
        }
        else
        {
            GameManager.SystemMessage = "You can't scroll down further.";
        }
    }

    public void ScrollUp()
    {
        if (maneuverScrollIndex > 0 || strikeScrollIndex > 0)
        {
            maneuverScrollIndex = Math.Max(0, maneuverScrollIndex - 1);
            strikeScrollIndex = Math.Max(0, strikeScrollIndex - 1);
            UpdateDisplay();
        }
        else
        {
            GameManager.SystemMessage = "You can't scroll up further.";
        }
    }

public override void HandleInput(string input)
{
    input = input.ToLower().Trim();

    if (input.StartsWith("spend "))
    {
        SpendPoints(input);
        return;
    }

    if (Actions.ContainsKey(input))
    {
        Actions[input]?.Invoke();
    }
    else
    {
        GameManager.SystemMessage = "Invalid input. Type 'spend [first letter of stat] [amount]' to spend points, 'tree' to view the ability tree, 'return' to return to the game, or 'quit' to quit.";
    }
}


private void SpendPoints(string input)
{
    // Expecting input in the form "spend [stat] [amount]"
    var split = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

    if (split.Length != 3 || split[0] != "spend")
    {
        GameManager.SystemMessage = "Invalid format for spending points. Use 'spend [first letter of stat] [amount]'.";
        return;
    }

    string stat = split[1].ToLower(); // Handle case-insensitivity
    if (!int.TryParse(split[2], out int amount))
    {
        GameManager.SystemMessage = "Invalid amount. Please enter a valid number.";
        return;
    }

    if (amount <= 0 || amount > player.AvailableStatPoints)
    {
        GameManager.SystemMessage = "You don't have enough points to spend.";
        return;
    }

    // Distribute the points based on the stat
    switch (stat)
    {
        case "b":
            player.Brawniness += amount;
            break;
        case "n":
            player.Nimbility += amount;
            break;
        case "m":
            player.Mysticigence += amount;
            break;
        case "e":
            player.Enduritution += amount;
            break;
        default:
            GameManager.SystemMessage = "Invalid stat. Use 'b', 'n', 'm', or 'e'.";
            return;
    }

    player.AvailableStatPoints -= amount;
    player.UpdateStats(); // Recalculate stats after spending points
    GameManager.SystemMessage = $"{amount} points spent on {stat}.";
    UpdateDisplay();
}



    private void ViewAbilityTree()
    {
        GameManager.SystemMessage = "Ability tree is a work in progress. Stay tuned!";
    }

    private void ReturnToGame()
    {
        GameManager.SystemMessage = "Returning to the game...";
        // Transition back to the game, potentially closing this screen or switching to another.
        GameManager.Instance.SetScreen(GameManager.Instance.previousScreen);
    }

    private void QuitGame()
    {
        GameManager.SystemMessage = "Quitting the game...";
        Application.Exit();
    }
}
