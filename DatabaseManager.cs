using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace TextRPGWindow
{
    internal class DatabaseManager
    {
        private string connectionString = "server=localhost;user=root;password=;database=TXTARPG;";

        public LocationScreen GetLocationByName(string locationName, GameManager gameManager)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT location_id, name, description FROM locations WHERE name = @name";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", locationName);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new LocationScreen(
                                reader.GetString("name"),
                                reader.GetString("description"),
                                reader.GetInt32("location_id"),
                                gameManager
                            );
                        }
                    }
                }
            }
            return null;
        }

        public List<string> GetConnectedLocations(int locationId)
        {
            List<string> connectedLocations = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT l.name 
                                 FROM locations l
                                 JOIN location_connections lc ON l.location_id = lc.connected_location_id
                                 WHERE lc.location_id = @locationId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@locationId", locationId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            connectedLocations.Add(reader.GetString("name"));
                        }
                    }
                }
            }
            return connectedLocations;
        }

        public List<Monster> GetMonstersForLocation(int locationId)
        {
            List<Monster> monsters = new List<Monster>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT m.monster_id, m.name, m.base_health, m.min_level, m.max_level, 
                                        m.movement_points, m.brawniness, m.nimbility, 
                                        m.mysticigence, m.enduritution 
                                 FROM monsters m
                                 JOIN location_monsters lm ON m.monster_id = lm.monster_id
                                 WHERE lm.location_id = @locationId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@locationId", locationId);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Monster testMon = new Monster(
                                reader.GetInt32("monster_id"),
                                reader.GetString("name"),
                                reader.GetInt32("base_health"),
                                reader.GetInt32("min_level"),
                                reader.GetInt32("max_level"),
                                reader.GetInt32("movement_points"),
                                reader.GetInt32("brawniness"),
                                reader.GetInt32("nimbility"),
                                reader.GetInt32("mysticigence"),
                                reader.GetInt32("enduritution")
                            );
                            monsters.Add(testMon);
                        }
                    }
                }
            }
            return monsters;
        }

        public decimal GetSpawnChanceForMonster(int locationId, int monsterId)
        {


            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT spawn_chance FROM location_monsters 
                                 WHERE location_id = @locationId AND monster_id = @monsterId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@locationId", locationId);
                    cmd.Parameters.AddWithValue("@monsterId", monsterId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetDecimal("spawn_chance");
                        }
                    }
                }
            }
            return 0;
        }

        public Monster GetMonsterByName(string name)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM monsters WHERE name = @name";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Monster(
                                reader.GetInt32("monster_id"),
                                reader.GetString("name"),
                                reader.GetInt32("base_health"),
                                reader.GetInt32("min_level"),
                                reader.GetInt32("max_level"),
                                reader.GetInt32("movement_points"),
                                reader.GetInt32("brawniness"),
                                reader.GetInt32("nimbility"),
                                reader.GetInt32("mysticigence"),
                                reader.GetInt32("enduritution")
                            );
                        }
                    }
                }
            }
            return null;
        }
        public int InsertMonster(string name, int baseHealth, int minLevel, int maxLevel, int movementPoints, int brawniness, int nimbility, int mysticigence, int enduritution)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO monsters (name, base_health, min_level, max_level, movement_points, brawniness, nimbility, mysticigence, enduritution) 
                         VALUES (@name, @baseHealth, @minLevel, @maxLevel, @movementPoints, @brawniness, @nimbility, @mysticigence, @enduritution)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@baseHealth", baseHealth);
                    cmd.Parameters.AddWithValue("@minLevel", minLevel);
                    cmd.Parameters.AddWithValue("@maxLevel", maxLevel);
                    cmd.Parameters.AddWithValue("@movementPoints", movementPoints);
                    cmd.Parameters.AddWithValue("@brawniness", brawniness);
                    cmd.Parameters.AddWithValue("@nimbility", nimbility);
                    cmd.Parameters.AddWithValue("@mysticigence", mysticigence);
                    cmd.Parameters.AddWithValue("@enduritution", enduritution);
                    cmd.ExecuteNonQuery();
                }

                // Retrieve the last inserted monster id
                string idQuery = "SELECT LAST_INSERT_ID()";
                using (MySqlCommand idCmd = new MySqlCommand(idQuery, conn))
                {
                    int monsterId = Convert.ToInt32(idCmd.ExecuteScalar());
                    return monsterId;  // Return the auto-generated monster ID
                }
            }
        }

        public void AddMonsterToLocation(int monsterId, int locationId, decimal spawnChance)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO location_monsters (location_id, monster_id, spawn_chance) 
                                 VALUES (@locationId, @monsterId, @spawnChance)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    Console.WriteLine($"monster ID: {monsterId} to location: {locationId}");
                    cmd.Parameters.AddWithValue("@locationId", locationId);
                    cmd.Parameters.AddWithValue("@monsterId", monsterId);
                    cmd.Parameters.AddWithValue("@spawnChance", spawnChance);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteMonster(int monsterId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // First, delete the reference from location_monsters table
                string deleteLocationMonsterQuery = "DELETE FROM location_monsters WHERE monster_id = @monsterId";
                using (MySqlCommand cmd = new MySqlCommand(deleteLocationMonsterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@monsterId", monsterId);
                    cmd.ExecuteNonQuery(); // Removes the connection from the location_monsters table
                    Console.WriteLine($"Monster with ID {monsterId} removed from location_monsters.");
                }

                // Then, delete the monster from the monsters table
                string deleteMonsterQuery = "DELETE FROM monsters WHERE monster_id = @monsterId";
                using (MySqlCommand cmd = new MySqlCommand(deleteMonsterQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@monsterId", monsterId);
                    cmd.ExecuteNonQuery(); // Deletes the monster from the monsters table
                    Console.WriteLine($"Monster with ID {monsterId} deleted from monsters.");
                }
            }
        }

        public void UpdateMonster(int monsterId, string name, int baseHealth, int minLevel, int maxLevel, int movementPoints, int brawniness, int nimbility, int mysticigence, int enduritution)
        {
            Console.WriteLine($"Updating monster ID {monsterId} with new stats...");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE monsters 
                                 SET name = @name, base_health = @baseHealth, min_level = @minLevel, max_level = @maxLevel, 
                                     movement_points = @movementPoints, brawniness = @brawniness, nimbility = @nimbility, 
                                     mysticigence = @mysticigence, enduritution = @enduritution 
                                 WHERE monster_id = @monsterId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@monsterId", monsterId);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@baseHealth", baseHealth);
                    cmd.Parameters.AddWithValue("@minLevel", minLevel);
                    cmd.Parameters.AddWithValue("@maxLevel", maxLevel);
                    cmd.Parameters.AddWithValue("@movementPoints", movementPoints);
                    cmd.Parameters.AddWithValue("@brawniness", brawniness);
                    cmd.Parameters.AddWithValue("@nimbility", nimbility);
                    cmd.Parameters.AddWithValue("@mysticigence", mysticigence);
                    cmd.Parameters.AddWithValue("@enduritution", enduritution);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Update the spawn chance of a monster in a specific location
        public void UpdateMonsterSpawnChance(int monsterId, int locationId, decimal newSpawnChance)
        {
            Console.WriteLine($"Updating spawn chance of monster ID {monsterId} in location ID {locationId} to {newSpawnChance}%...");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"UPDATE location_monsters 
                                 SET spawn_chance = @newSpawnChance 
                                 WHERE monster_id = @monsterId AND location_id = @locationId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@monsterId", monsterId);
                    cmd.Parameters.AddWithValue("@locationId", locationId);
                    cmd.Parameters.AddWithValue("@newSpawnChance", newSpawnChance);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
