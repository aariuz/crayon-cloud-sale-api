using Common.Requests;
using Integrations.CCP.DTO;
using Integrations.Crayon.Database.Models;

namespace Integrations.Crayon
{
    public interface ICrayonClient
    {
        Task<List<Account>> GetAllAccountsForCustomerAsync(int customerId, CancellationToken cancellationToken);
        Task<Subscription> UpdateNumberOfSubscriptionLicensesAsync(int customerId, int subscriptionId, int newAmountOfLicenses, CancellationToken cancellationToken);
        Task CancelSubscriptionForAccountAsync(int customerId, int subscriptionId, CancellationToken cancellationToken);
        Task<Subscription> ExtendSubscriptionDateAsync(int customerId, int subscriptionId, DateTime newDate, CancellationToken cancellationToken);
        Task<List<Subscription>> CreateSubscriptionsAsync(int customerId, int accountId, List<CCPPurchasedSoftware> purchasedSoftware, CancellationToken cancellationToken);
        Task<Customer> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Customer> GetCustomerByTokenAsync(string token, CancellationToken cancellationToken);
        Task UpdateCustomerTokenAsync(int customerId, string refreshToken, CancellationToken cancellationToken);
        Task<Customer> CreateNewCustomer(NewCustomerRequest request, CancellationToken cancellationToken);
        Task<Account> CreateNewAccount(NewAccountRequest request, CancellationToken cancellationToken);
    }
}
