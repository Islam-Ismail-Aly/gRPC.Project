using gRPC.API.Entites.Interfaces;

namespace gRPC.API.Entites.Services
{
    public class ApiKeyProviderService : IApiKeyProviderService
    {
        private readonly IConfiguration _configuration;

        public ApiKeyProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetApiKey()
        {
            return _configuration.GetSection("ApiKey").Value;
        }
    }
}
