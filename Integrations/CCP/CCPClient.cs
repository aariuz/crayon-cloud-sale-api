using Common.Requests;
using Integrations.CCP.DTO;
using Microsoft.Extensions.Logging;
using NetCore.AutoRegisterDi;
using System.Text;
using System.Text.Json;

namespace Integrations.CCP
{
    [RegisterAsScoped]
    public class CCPClient : ICCPClient
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public CCPClient(HttpClient httpClient, ILogger<CCPClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<CCPSoftware>> GetSoftwareListAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("/list-all-software", cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<List<CCPSoftware>>(result);
        }

        public async Task<List<CCPPurchasedSoftware>> PurchaseSoftwareAsync(SoftwareOrderRequest order, CancellationToken cancellationToken)
        {
            // Serialize dictionary to JSON
            var jsonContent = JsonSerializer.Serialize(order);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/purchase-software", content, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<List<CCPPurchasedSoftware>>(result);
        }
    }
}
