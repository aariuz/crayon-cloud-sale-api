namespace WriteFacade.Crayon.DTO
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public virtual List<SubscriptionDto> Subscriptions { get; set; } = new List<SubscriptionDto>();
    }
}
