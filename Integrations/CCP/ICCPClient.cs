using Common.Requests;
using Integrations.CCP.DTO;

namespace Integrations.CCP
{
    public interface ICCPClient
    {
        Task<List<CCPSoftware>> GetSoftwareListAsync(CancellationToken cancellationToken);
        Task<List<CCPPurchasedSoftware>> PurchaseSoftwareAsync(SoftwareOrderRequest order, CancellationToken cancellationToken);
    }
}
