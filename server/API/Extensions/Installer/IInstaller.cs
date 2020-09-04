using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions.Installer
{
    public interface IInstaller
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}