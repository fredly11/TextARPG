namespace NPCs
{
    public class NedFarley : NPCScreen
    {
        public NedFarley() : base("Ned Farley")
        {
            Description = "A seasoned traveler who seems to know a lot about the area.";
            DialogueOptions.Add("Can you tell me about the forest?");
            DialogueOptions.Add("Where can I find wolves?");
            DialogueResponses.Add("Aye, the forest is tranquil and pretty, a good place to take a walk. There might be some danger there, but nothing a strong adventurer like you couldn't deal with.");
            DialogueResponses.Add("Wolves? Well, you can find some in the forest, but I heard there are some particularly fearsome wolves down by the mountain pass.");
        }
    }
}
