using System.ComponentModel.DataAnnotations;

namespace API.Contracts
{
    public class CreateNewCustomerRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Password { get; set; }
    }
}
