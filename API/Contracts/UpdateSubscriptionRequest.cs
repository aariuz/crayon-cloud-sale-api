using System.ComponentModel.DataAnnotations;

namespace API.Contracts
{
    public class UpdateSubscriptionRequest
    {
        [Required]
        public int SubscriptionId { get; set; }
        [Required]
        public int AmountOfLicenses { get; set; }
    }
}
