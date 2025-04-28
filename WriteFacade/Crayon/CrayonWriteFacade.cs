using AutoMapper;
using Common.Requests;
using Integrations.CCP;
using Integrations.Crayon;
using Microsoft.Extensions.Logging;
using NetCore.AutoRegisterDi;
using WriteFacade.Crayon.DTO;

namespace WriteFacade.Crayon
{
    [RegisterAsScoped]
    public class CrayonWriteFacade : ICrayonWriteFacade
    {
        private readonly ICrayonClient _crayonClient;
        private readonly ICCPClient _ccpClient;
        private readonly ILogger<CrayonWriteFacade> _logger;
        private readonly IMapper _mapper;

        public CrayonWriteFacade(ICrayonClient crayonClient, ICCPClient ccpClient, IMapper mapper, ILogger<CrayonWriteFacade> logger)
        {
            _crayonClient = crayonClient;
            _ccpClient = ccpClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CancelSubscriptionForAccountAsync(int customerId, int subscriptionId, CancellationToken cancellationToken)
        {
            try
            {
                await _crayonClient.CancelSubscriptionForAccountAsync(customerId, subscriptionId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to extend for cancel subscription: {0}", subscriptionId));
                throw;
            }
        }

        public async Task<SubscriptionDto> ExtendSubscriptionDateAsync(int customerId, int subscriptionId, DateTime newDate, CancellationToken cancellationToken)
        {
            try
            {
                var updatedSubscrion = await _crayonClient.ExtendSubscriptionDateAsync(customerId, subscriptionId, newDate, cancellationToken);
                return _mapper.Map<SubscriptionDto>(updatedSubscrion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to extend for subscription time for subscription: {0}", subscriptionId));
                throw;
            }
        }

        public async Task<List<SubscriptionDto>> PurchaseSoftwareAsync(int customerId, int accountId, SoftwareOrderRequest order, CancellationToken cancellationToken)
        {
            try
            {
                var purchasedSoftware = await _ccpClient.PurchaseSoftwareAsync(order, cancellationToken);
                var newSubscriptions = await _crayonClient.CreateSubscriptionsAsync(customerId, accountId, purchasedSoftware, cancellationToken);
                return _mapper.Map<List<SubscriptionDto>>(newSubscriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to purchase software from CCP: {0}", ex.Message));
                throw;
            }
        }

        public async Task<SubscriptionDto> UpdateNumberOfSubscriptionLicensesAsync(int customerId, int subscriptionId, int newAmountOfLicenses, CancellationToken cancellationToken)
        {
            try
            {
                var updatedSubscription = await _crayonClient.UpdateNumberOfSubscriptionLicensesAsync(customerId, subscriptionId, newAmountOfLicenses, cancellationToken);
                return _mapper.Map<SubscriptionDto>(updatedSubscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to update the number of licenses for subscription: {0}", subscriptionId));
                throw;
            }
        }

        public async Task UpdateCustomerTokenAsync(int customerId, string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                await _crayonClient.UpdateCustomerTokenAsync(customerId, refreshToken, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to update the customers token: {0}", customerId));
                throw;
            }
        }

        public async Task<CustomerDto> CreateNewCustomer(NewCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var newCustomer = await _crayonClient.CreateNewCustomer(request, cancellationToken);
                return _mapper.Map<CustomerDto>(newCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to create new customer using email: {0}", request.Email));
                throw;
            }
        }

        public async Task<AccountDto> CreateNewAccountForCustomer(NewAccountRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var newAccount = await _crayonClient.CreateNewAccount(request, cancellationToken);
                return _mapper.Map<AccountDto>(newAccount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to create new account for customer: {0}", request.CustomerId));
                throw;
            }
        }
    }
}
