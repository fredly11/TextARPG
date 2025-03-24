using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPGWindow
{
    internal class Monster
    {
        public int MonsterId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int MovementPoints { get; set; }
        public int Brawniness { get; set; }
        public int Nimbility { get; set; }
        public int Mysticigence { get; set; }
        public int Enduritution { get; set; }

        public Monster(int id, string name, int baseHealth, int minLevel, int maxLevel, int movementPoints, int brawniness, int nimbility, int mysticigence, int enduritution)
        {
            MonsterId = id;
            Random rand = new Random();
            Level = rand.Next(minLevel, maxLevel + 1);
            Health = baseHealth + (int)(baseHealth * 0.1 * (Level - minLevel));
            MovementPoints = movementPoints;
            Brawniness = brawniness;
            Nimbility = nimbility;
            Mysticigence = mysticigence;
            Enduritution = enduritution;
            Name = $"{name} (Lvl {Level})";
        }
    }
}
