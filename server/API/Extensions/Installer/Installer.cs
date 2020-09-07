using System.Text;
using AutoMapper;
using Core.Actions.Activities;
using Core.Interfaces.Security;
using Data;
using FluentValidation.AspNetCore;
using Infrastructure.Requirements;
using Infrastructure.Services.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Middleware.Contexts;

namespace API.Extensions.Installer
{
    public class Installer : IInstaller
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
                options.AddPolicy("CorsPolicy", policy =>
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:3000")));

            services.AddControllers(options =>
            {
                var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(authorizationPolicy));
            })
            .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Create>());

            services.AddDbContext<DataContext>(ob =>
            {
                ob.UseLazyLoadingProxies();
                ob.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMediatR(typeof(ListAll.Handler).Assembly);
            services.AddAutoMapper(typeof(ListAll.Handler));

            var ib = services.AddIdentityCore<AppUser>();
            var identityBuilder = new IdentityBuilder(ib.UserType, ib.Services);

            identityBuilder.AddEntityFrameworkStores<DataContext>();
            identityBuilder.AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthorization(options =>
                options.AddPolicy("AppUserIsHostingActivity", pb => pb.Requirements.Add(new IsHostRequirement())));
            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();

            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symetricSecurityKey,
                    ValidateAudience = false,
                    ValidateIssuer = false
                });

            services.AddScoped<IJWTGeneratorService, JWTGeneratorService>();
            services.AddScoped<IAppUserService, AppUserService>();
        }
    }
}