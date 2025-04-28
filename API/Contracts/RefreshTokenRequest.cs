using System.ComponentModel.DataAnnotations;

namespace API.Contracts
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
