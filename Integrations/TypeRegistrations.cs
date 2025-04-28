using Integrations.CCP;
using Integrations.CCP.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NetCore.AutoRegisterDi;
using System.Net;
using System.Text.Json;

namespace Integrations
{
    public static class TypeRegistrations
    {
        public static void RegisterTypes(this IServiceCollection services)
        {
            RegisterHttpClients(services);
            services.RegisterAssemblyPublicNonGenericClasses()
                .Where(c => !typeof(CCPClient).IsAssignableFrom(c))
                .AsPublicImplementedInterfaces();
        }

        private static void RegisterHttpClients(IServiceCollection services)
        {
            // Mock GET request to return a list of software
            var getResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(MockHelper.GetAllSoftware()))
            };

            // Mock POST request to return the purchased software
            var postResponse = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(JsonSerializer.Serialize(MockHelper.GetListOfPurchasedSoftware()))
            };

            services.AddScoped<ICCPClient, CCPClient>(provider =>
            {
                var mockedHandler = new Mock<HttpMessageHandler>();

                mockedHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get &&
                    r.RequestUri!.AbsolutePath == "/list-all-software"), ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(getResponse);

                mockedHandler
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Post &&
                    r.RequestUri!.AbsolutePath == "/purchase-software"), ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(postResponse);

                var mockedHttpClient = new HttpClient(mockedHandler.Object)
                {
                    BaseAddress = new Uri("https://fake-base-address"),
                    Timeout = TimeSpan.FromSeconds(5)
                };
                var mockLogger = new Mock<ILogger<CCPClient>>();
                var logger = provider.GetRequiredService<ILogger<CCPClient>>();
                var ccpClient = new CCPClient(mockedHttpClient, logger);
                return ccpClient;
            });
        }
    }
}
