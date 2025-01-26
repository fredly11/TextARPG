using NPCs;
namespace Locations
{
    public class ForestGlade : LocationScreen
    {
        public ForestGlade(GameManager gameManager) : base("Forest Glade", gameManager)
        {
            Description = "A serene glade deep in the forest, where the sunlight filters through the trees.";

            ConnectedLocations.Add("Town Square");
            NPCs.Add(new TomBrightwood());
        }
    }
}            