using Core.Activities;
using FluentValidation.AspNetCore;
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
            services.AddCors();
            services.AddControllers()
                .AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Create>());
            services.AddDbContext<DataContext>(
                ob => ob.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddMediatR(typeof(ListAll.Handler).Assembly);
        }
    }
}