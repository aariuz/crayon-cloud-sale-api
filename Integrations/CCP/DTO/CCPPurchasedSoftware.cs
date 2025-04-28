namespace Integrations.CCP.DTO
{
    public class CCPPurchasedSoftware
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Keys { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
