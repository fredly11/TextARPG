namespace NPCs
{
    public class TomBrightwood : NPCScreen
    {
        public TomBrightwood() : base("Tom Brightwood")
        {
            Description = "A young and eager merchant, always looking to trade.";
            DialogueOptions.Add("What goods do you have for sale?");
            DialogueOptions.Add("Can you tell me about any local landmarks?");
            DialogueResponses.Add("I’ve got all sorts of things! Weapons, potions, and even some rare trinkets.");
            DialogueResponses.Add("There’s an old temple to the north and a haunted castle to the east. Both are worth visiting if you’re brave enough.");
        }
    }
}