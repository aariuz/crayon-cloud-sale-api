using WriteFacade.Crayon.DTO;

namespace ReadFacade.Crayon
{
    public interface ICrayonReadFacade
    {
        Task<List<AccountDto>> GetAllAccountsForCustomerAsync(int customerId, CancellationToken cancellationToken);
        Task<CustomerDto> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken);
        Task<CustomerDto> GetCustomerByTokenAsync(string token, CancellationToken cancellationToken);
    }
}
