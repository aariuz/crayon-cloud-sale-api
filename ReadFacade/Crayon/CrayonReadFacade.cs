using AutoMapper;
using Integrations.Crayon;
using Microsoft.Extensions.Logging;
using NetCore.AutoRegisterDi;
using WriteFacade.Crayon.DTO;

namespace ReadFacade.Crayon
{
    [RegisterAsScoped]
    public class CrayonReadFacade : ICrayonReadFacade
    {
        private readonly ICrayonClient _crayonClient;
        private readonly ILogger<CrayonReadFacade> _logger;
        private readonly IMapper _mapper;

        public CrayonReadFacade(ICrayonClient crayonClient, IMapper mapper, ILogger<CrayonReadFacade> logger)
        {
            _crayonClient = crayonClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<AccountDto>> GetAllAccountsForCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _crayonClient.GetAllAccountsForCustomerAsync(customerId, cancellationToken);
                return _mapper.Map<List<AccountDto>>(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to get all acounts for customer: {0}", customerId));
                throw;
            }
        }

        public async Task<CustomerDto> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _crayonClient.GetCustomerByEmailAsync(email, cancellationToken);
                return _mapper.Map<CustomerDto>(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to get customer: {0}", email));
                throw;
            }
        }

        public async Task<CustomerDto> GetCustomerByTokenAsync(string token, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _crayonClient.GetCustomerByTokenAsync(token, cancellationToken);
                return _mapper.Map<CustomerDto>(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to get customer: {0}", token));
                throw;
            }
        }
    }
}
