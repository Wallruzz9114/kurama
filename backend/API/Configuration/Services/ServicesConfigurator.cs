using System.Text;
using Core.Actions.Activities;
using Core.Implementations;
using Core.Interfaces;
using Data.Contexts;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models;

namespace API.Configuration.Services
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Veram API", Version = "v1" });
            });

            services.AddControllers(options =>
            {
                var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(authorizationPolicy));
            }).AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Create>());

            services.AddDbContext<DatabaseContext>(ob => ob.UseNpgsql(configuration.GetConnectionString("DatabaseConnection")));
            services.AddMediatR(typeof(GetAll.Handler).Assembly);

            var builder = services.AddIdentityCore<AppUser>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);

            identityBuilder.AddEntityFrameworkStores<DatabaseContext>();
            identityBuilder.AddSignInManager<SignInManager<AppUser>>();

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmetricSecurityKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                };
            });

            services.AddScoped<IJWTGenerator, JWTGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
        }
    }
}