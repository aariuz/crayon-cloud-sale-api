using System.ComponentModel.DataAnnotations;

namespace API.Contracts
{
    public class ExtendSubscriptionRequest
    {
        [Required]
        public int SubscriptionId { get; set; }
        [Required]
        public DateTime NewDate { get; set; }
    }
}
