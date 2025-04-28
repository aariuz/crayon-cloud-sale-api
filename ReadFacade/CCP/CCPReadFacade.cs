using AutoMapper;
using Integrations.CCP;
using Microsoft.Extensions.Logging;
using NetCore.AutoRegisterDi;
using ReadFacade.CCP.DTO;

namespace ReadFacade.CCP
{
    [RegisterAsScoped]
    public class CCPReadFacade : ICCPReadFacade
    {
        private readonly ICCPClient _ccpClient;
        private readonly ILogger<CCPReadFacade> _logger;
        private readonly IMapper _mapper;

        public CCPReadFacade(ICCPClient ccpClient, IMapper mapper, ILogger<CCPReadFacade> logger)
        {
            _ccpClient = ccpClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<PurchasableSoftware>> GetSoftwareListAsync(CancellationToken cancellationToken)
        {
            try
            {
                var ccpSoftware = await _ccpClient.GetSoftwareListAsync(cancellationToken);
                return _mapper.Map<List<PurchasableSoftware>>(ccpSoftware);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("Failed to get software from CCP: {0}", ex.Message));
                throw;
            }
        }
    }
}
