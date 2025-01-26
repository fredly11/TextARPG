namespace NPCs
{
    public class SarahElms : NPCScreen
    {
        public SarahElms() : base("Sarah Elms")
        {
            Description = "A kind-hearted healer who tends to the sick and injured in the village.";
            DialogueOptions.Add("Do you know anything about the healing herbs in the area?");
            DialogueOptions.Add("Are there any dangerous creatures nearby?");
            DialogueResponses.Add("Ah, healing herbs... You can find plenty of them in the woods. Just be cautious around the mushrooms.");
            DialogueResponses.Add("Yes, the woods are home to many wild creatures, and the mountain pass is particularly dangerous.");
        }
    }
}
