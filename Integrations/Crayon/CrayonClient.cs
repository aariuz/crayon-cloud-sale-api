using Castle.Core.Resource;
using Common.Requests;
using Integrations.CCP.DTO;
using Integrations.Crayon.Database.Models;
using Microsoft.EntityFrameworkCore;
using NetCore.AutoRegisterDi;
using System.Threading;

namespace Integrations.Crayon
{
    [RegisterAsScoped]
    public class CrayonClient : ICrayonClient
    {
        private readonly CrayonDbContext _dbContext;

        public CrayonClient(CrayonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CancelSubscriptionForAccountAsync(int customerId, int subscriptionId, CancellationToken cancellationToken)
        {
            var selectedSubscription = await GetCustomerSubscription(customerId, subscriptionId, cancellationToken);

            selectedSubscription.Active = false;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Subscription>> CreateSubscriptionsAsync(int customerId, int accountId, List<CCPPurchasedSoftware> purchasedSoftware, CancellationToken cancellationToken)
        {
            var selectedCustomer = await _dbContext.Customers
                .Include(c => c.Accounts)
                .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);

            var selectedAccount = selectedCustomer
                .Accounts.FirstOrDefault(a => a.Id == accountId);

            foreach (var newSoftware in purchasedSoftware)
            {
                var newSubscription = new Subscription
                {
                    AccountId = accountId,
                    Active = true,
                    SoftwareDescription = newSoftware.Description,
                    SoftwareId = newSoftware.Id,
                    SoftwareName = newSoftware.Name,
                    ValidUntil = newSoftware.ValidUntil
                };
                await _dbContext.Subscriptions.AddAsync(newSubscription, cancellationToken);
                newSubscription.Licenses = CreateNewLicensesForSubscription(newSubscription.Id, newSoftware.Keys);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            return await _dbContext.Subscriptions.Where(s => s.AccountId == accountId).ToListAsync(cancellationToken);
        }

        public async Task<Subscription> ExtendSubscriptionDateAsync(int customerId, int subscriptionId, DateTime newDate, CancellationToken cancellationToken)
        {
            var selectedSubscription = await GetCustomerSubscription(customerId, subscriptionId, cancellationToken);

            selectedSubscription.ValidUntil = newDate;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return selectedSubscription;
        }

        public async Task<List<Account>> GetAllAccountsForCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            var customerAccounts = await _dbContext.Accounts.Where(a => a.CustomerId == customerId).ToListAsync(cancellationToken);
            return customerAccounts;
        }

        public async Task<Subscription> UpdateNumberOfSubscriptionLicensesAsync(int customerId, int subscriptionId, int newAmountOfLicenses, CancellationToken cancellationToken)
        {
            var selectedSubscription = await GetCustomerSubscription(customerId, subscriptionId, cancellationToken);

            if (selectedSubscription.Licenses.Count == newAmountOfLicenses)
            {
                return selectedSubscription;
            }
            // We should remove some
            else if (selectedSubscription.Licenses.Count > newAmountOfLicenses)
            {
                var numberOfLicensesToRemove = selectedSubscription.Licenses.Count - newAmountOfLicenses;
                var licensesToRemove = selectedSubscription.Licenses.Take(numberOfLicensesToRemove).ToList();
                _dbContext.Licenses.RemoveRange(licensesToRemove);
            }
            // We should add some
            else if (selectedSubscription.Licenses.Count < newAmountOfLicenses)
            {
                var numberOfNewLicensesNeeded = newAmountOfLicenses - selectedSubscription.Licenses.Count;
                for (int i = 0; i < numberOfNewLicensesNeeded; i++)
                {
                    await _dbContext.Licenses.AddAsync(new License { SubscriptionId = selectedSubscription.Id, Key = Guid.NewGuid().ToString() }, cancellationToken);
                }
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            return await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscriptionId, cancellationToken);
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        }

        public async Task<Customer> GetCustomerByTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(c => c.RefreshToken == token, cancellationToken);
        }

        public async Task UpdateCustomerTokenAsync(int customerId, string refreshToken, CancellationToken cancellationToken)
        {
            var selectedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
            selectedCustomer.RefreshToken = refreshToken;
            selectedCustomer.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Customer> CreateNewCustomer(NewCustomerRequest request, CancellationToken cancellationToken)
        {
            var newCustomer = new Customer
            {
                Email = request.Email,
                Name = request.Name,
                PasswordHash = request.PasswordHash,
                Salt = request.Salt
            };
            await _dbContext.Customers.AddAsync(newCustomer, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newCustomer;
        }

        public async Task<Account> CreateNewAccount(NewAccountRequest request, CancellationToken cancellationToken)
        {
            var newAccount = new Account
            {
                CustomerId = request.CustomerId,
                Description = request.Description,
                Name = request.Name
            };
            await _dbContext.Accounts.AddAsync(newAccount, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return newAccount;
        }

        private async Task<Subscription> GetCustomerSubscription(int customerId, int subscriptionId, CancellationToken cancellationToken)
        {
            var selectedCustomer = await _dbContext.Customers
            .Include(c => c.Accounts)
            .ThenInclude(a => a.Subscriptions)
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);

            var selectedSubscription = selectedCustomer
                .Accounts.FirstOrDefault(a => a.Subscriptions.Any(s => s.Id == subscriptionId))
                .Subscriptions.FirstOrDefault(s => s.Id == subscriptionId);

            return selectedSubscription;
        }

        private List<License> CreateNewLicensesForSubscription(int subscriptionId, List<string> licenses)
        {
            var newLicenses = new List<License>();
            foreach (var license in licenses)
            {
                newLicenses.Add(new License
                {
                    Key = license,
                    SubscriptionId = subscriptionId,
                });
            }
            return newLicenses;
        }
    }
}
