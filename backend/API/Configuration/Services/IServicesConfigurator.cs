using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configuration.Services
{
    public interface IServicesConfigurator
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}