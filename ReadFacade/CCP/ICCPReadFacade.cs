using ReadFacade.CCP.DTO;

namespace ReadFacade.CCP
{
    public interface ICCPReadFacade
    {
        Task<List<PurchasableSoftware>> GetSoftwareListAsync(CancellationToken cancellationToken);
    }
}
