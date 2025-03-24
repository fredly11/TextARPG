using System;
using System.Collections.Generic;
using TextRPGWindow;

class MonsterFactory
{
    private DatabaseManager _dbManager = new DatabaseManager();
    private Random _rand = new Random();

    // Spawns a monster based on proportional spawn chances
    public Monster SpawnMonsterFromLocation(int locationId)
    {
        List<Monster> availableMonsters = _dbManager.GetMonstersForLocation(locationId);
        Console.WriteLine($"Monsters fetched for location {locationId}: {availableMonsters.Count}");

        if (availableMonsters.Count == 0) return null;  // No monsters available

        Dictionary<Monster, decimal> monsterChances = new Dictionary<Monster, decimal>();
        decimal totalSpawnChance = 0m;

        // Retrieve spawn chances and calculate total spawn chance
        foreach (var monster in availableMonsters)
        {
            decimal spawnChance = _dbManager.GetSpawnChanceForMonster(locationId, monster.MonsterId);
            if (spawnChance > 0)
            {
                monsterChances[monster] = spawnChance;
                totalSpawnChance += spawnChance;
            }
        }

        if (totalSpawnChance == 0) return null;  // No valid spawn chances

        // Generate a random number within the totalSpawnChance range
        decimal roll = (decimal)_rand.NextDouble() * totalSpawnChance;
        decimal cumulative = 0m;

        // Iterate over the monsters to determine which one gets selected
        foreach (var entry in monsterChances)
        {
            cumulative += entry.Value;
            if (roll <= cumulative)
            {
                return entry.Key;  // Select the monster based on weighted probability
            }
        }

        return null;  // Failsafe, shouldn't happen unless an error occurs
    }
}
