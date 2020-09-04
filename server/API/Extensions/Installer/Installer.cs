using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Middleware;

namespace API.Extensions.Installer
{
    public class Installer : IInstaller
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Controllers
            services.AddControllers();
            // Add PostgreSQL locally
            services.AddDbContext<DataContext>(
                ob => ob.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );
            // Add CORS
            services.AddCors(options => options.AddPolicy(
                "CORSPolicy",
                policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"))
            );
        }
    }
}