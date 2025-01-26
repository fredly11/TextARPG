using NPCs;
namespace Locations
{
    public class MountainPass : LocationScreen
    {
        public MountainPass(GameManager gameManager) : base("Mountain Pass", gameManager)
        {
            Description = "A narrow pass through the mountains, with steep cliffs on either side and a dangerous reputation.";

            // Link to Town Square by name
            ConnectedLocations.Add("Town Square");
            NPCs.Add(new SarahElms());
        }
    }
}