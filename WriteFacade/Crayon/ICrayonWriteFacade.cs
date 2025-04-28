using Common.Requests;
using WriteFacade.Crayon.DTO;

namespace WriteFacade.Crayon
{
    public interface ICrayonWriteFacade
    {
        Task<List<SubscriptionDto>> PurchaseSoftwareAsync(int customerId, int accountId, SoftwareOrderRequest order, CancellationToken cancellationToken);
        Task<SubscriptionDto> UpdateNumberOfSubscriptionLicensesAsync(int customerId, int subscriptionId, int newAmountOfLicenses, CancellationToken cancellationToken);
        Task CancelSubscriptionForAccountAsync(int customerId, int subscriptionId, CancellationToken cancellationToken);
        Task<SubscriptionDto> ExtendSubscriptionDateAsync(int customerId, int subscriptionId, DateTime newDate, CancellationToken cancellationToken);
        Task UpdateCustomerTokenAsync(int customerId, string refreshToken, CancellationToken cancellationToken);
        Task<CustomerDto> CreateNewCustomer(NewCustomerRequest request, CancellationToken cancellationToken);
        Task<AccountDto> CreateNewAccountForCustomer(NewAccountRequest request, CancellationToken cancellationToken);
    }
}
