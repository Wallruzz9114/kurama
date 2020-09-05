using Core.Activities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Middleware.Contexts;

namespace API.Extensions.Installer
{
    public class Installer : IInstaller
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Add CORS
            services.AddCors();
            // Controllers
            services.AddControllers();
            // Add PostgreSQL locally
            services.AddDbContext<DataContext>(
                ob => ob.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );
            // Add MediatR
            services.AddMediatR(typeof(ListAll.Handler).Assembly);
        }
    }
}