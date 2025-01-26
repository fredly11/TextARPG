using NPCs;

namespace Locations
{
    public class TownSquare : LocationScreen
    {
        public TownSquare(GameManager gameManager) : base("Town Square", gameManager)
        {
            Description = "The bustling heart of the town, filled with merchants, townsfolk, and travelers.";

            // Link to other locations by name
            ConnectedLocations.Add("Forest Glade");
            ConnectedLocations.Add("Mountain Pass");
            NPCs.Add(new NedFarley());
        }
    }
}