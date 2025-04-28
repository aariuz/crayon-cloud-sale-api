using System.ComponentModel.DataAnnotations;

namespace Common.Requests
{
    public class SoftwareOrderRequest
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public List<SoftwareOrder> SoftwareOrders { get; set; }
    }
}
