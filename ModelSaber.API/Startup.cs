using ModelSaber.API.Models;
using ModelSaber.API.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelSaber.API.Services;
using System.Net.Http;
using Microsoft.AspNetCore.Server.IISIntegration;

namespace ModelSaber.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSingleton(_configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>());
            services.AddSingleton(_configuration.GetSection(nameof(DiscordSettings)).Get<DiscordSettings>());
            services.AddSingleton(_configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>());

            services.AddSingleton<HttpClient>();
            services.AddSingleton<DiscordService>();
            services.AddScoped<IAuditor, Auditor>();
            services.AddSingleton<IJWTService, JWTService>();
            services.AddDbContext<ModelSaberContext>();
            services.AddControllers();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseMiddleware<JWTMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}