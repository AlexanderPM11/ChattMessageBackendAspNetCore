using crudSegnalR.Infrastructure.Identity.Context;
using crudSegnalR.Infrastructure.Identity.Entities;
using crudSegnalR.Infrastructure.Identity.Service;
using crudSignalR.Core.Application.Dtos.Account;
using crudSignalR.Core.Application.Interface.Services;
using crudSignalR.Core.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace crudSegnalR.Infrastructure.Identity
{
    public static class ServicesRegistration
    {
        public static void AddLayerIdenityApi(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context
            services.AddDbContext<IdentityContext>(optionsAction =>
            {
                //optionsAction.EnableSensitiveDataLogging();
                optionsAction.UseSqlServer(
                    configuration.GetConnectionString("IdentityConnection"), 
                    m=>m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
            });
            #endregion
            var re = configuration["JwtSettings:Key"];
            #region Identity
            services.AddIdentity<ApplicantionUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer
            (options =>
            {
                options.RequireHttpsMetadata= false;
                options.SaveToken= false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer=true,
                    ValidateAudience=true,
                    ValidateLifetime=true,
                    ClockSkew=TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());

                    },
                    OnChallenge = c =>
                    {
                        c.HandleResponse();
                        c.Response.StatusCode = 401;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new JwtResponse { Error = "You are not authorized", HasError = true }); ;
                        return c.Response.WriteAsync(result);
                    },
                    OnForbidden = c =>
                    {
                        c.Response.StatusCode = 403;
                        c.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new JwtResponse { Error = "You are not authorized to access this resource", HasError = true }); ;
                        return c.Response.WriteAsync(result);
                    }
                };
            }

                
            );
            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();

            #endregion
        }
    
    public static void AddLayerIdenityWeb(this IServiceCollection services, IConfiguration configuration)
        {
            #region Context
            services.AddDbContext<IdentityContext>(optionsAction =>
            {
                //optionsAction.EnableSensitiveDataLogging();
                optionsAction.UseSqlServer(
                    configuration.GetConnectionString("IdentityConnection"),
                    m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
            });

            services.AddIdentity<ApplicantionUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();

            #endregion
        }
    
    
    
    }
}
