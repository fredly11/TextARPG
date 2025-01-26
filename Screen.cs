using System;
using System.Collections.Generic;

//Base class used to hold information of what to display and what actions to take based on user input
public abstract class Screen
{
    // Display: Array of strings representing the main game display
    public string[] Display { get; protected set; }

    // Dictionary to map user input to actions
    protected Dictionary<string, Action> Actions;

    public Screen()
    {
        Display = new string[20];
        Actions = new Dictionary<string, Action>();
    }

    // Virtual function to process the user input, unique for various Screen child objects.
    public virtual void HandleInput(string input)
    {
        if (Actions.ContainsKey(input))
        {
            Actions[input]?.Invoke();
        }
        else
        {
            Console.WriteLine("> Invalid input. Try again.");
        }
    }

    // Abstract method for initializing screen-specific logic
    public abstract void Initialize();
}
