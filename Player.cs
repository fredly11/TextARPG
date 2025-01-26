using System;
using System.Collections.Generic;
using Abilities;

public class Player
{
    public int Level { get; set; }
    public int Experience { get; set; }
    public int ExperienceForNextLevel { get; set; }
    public int Health { get; set; }
    public int Energy { get; set; }
    public int MovementPoints { get; set; }
    public int Brawniness { get; set; }
    public int Nimbility { get; set; }
    public int Mysticigence { get; set; }
    public int Enduritution { get; set; }
    public int AvailableStatPoints { get; set; }

    public List<Ability> BasicManeuverAbilities { get; set; }
    public List<Ability> BasicStrikeAbilities { get; set; }

    public Player()
    {
        Level = 1;
        Experience = 0;
        ExperienceForNextLevel = 500;
        Brawniness = 10;
        Nimbility = 10;
        Mysticigence = 10;
        Enduritution = 10;
        AvailableStatPoints = 5;

        UpdateStats();

        BasicManeuverAbilities = new List<Ability> { new Guard(), new Circle(), new Flee(), new Circle(), new Flee(), new Circle(), new Flee() };
        BasicStrikeAbilities = new List<Ability> { new Strike(), new Cleave() };
    }

    // Function to update derived stats
    public void UpdateStats()
    {
        Health = 50 + 4 * Enduritution;
        MovementPoints = 5 + Nimbility / 2;
        Energy = 20 + 2 * Mysticigence;
    }
}
