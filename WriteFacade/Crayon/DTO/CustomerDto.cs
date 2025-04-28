using Integrations.Crayon.Database.Models;

namespace WriteFacade.Crayon.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public virtual List<Account> Accounts { get; set; } = new List<Account>();
    }
}
