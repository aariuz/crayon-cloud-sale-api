using System.ComponentModel.DataAnnotations;

namespace API.Contracts
{
    public class CreateNewAccountRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
