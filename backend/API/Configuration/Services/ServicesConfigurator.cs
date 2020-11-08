using Core.Actions.Activities;
using Data.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Configuration.Services
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddDbContext<DatabaseContext>(ob => ob.UseNpgsql(configuration.GetConnectionString("DatabaseConnection")));

            services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
                policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"))
            );
            services.AddMediatR(typeof(GetAll.Handler).Assembly);
        }
    }
}