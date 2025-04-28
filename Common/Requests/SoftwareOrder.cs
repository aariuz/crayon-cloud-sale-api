using System.ComponentModel.DataAnnotations;

namespace Common.Requests
{
    public class SoftwareOrder
    {
        [Required]
        public int CCPSoftwareId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
