using System.Net.Http;
using ModelSaber.Services;
using ModelSaber.Database;
using Microsoft.AspNetCore.Http;
using ModelSaber.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModelSaber
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
            => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var deploymentSettings = _configuration.GetSection(nameof(DeploymentSettings)).Get<DeploymentSettings>();
            var databaseSettings = _configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            var discordConfig = _configuration.GetSection(nameof(DiscordSettings));
            var databaseConfig = _configuration.GetSection(nameof(DatabaseSettings));
            services.Configure<DiscordSettings>(discordConfig);
            services.Configure<DatabaseSettings>(databaseConfig);

            services.AddSingleton<IDiscordSettings>(ii => ii.GetRequiredService<IOptions<DiscordSettings>>().Value);
            services.AddSingleton<IDatabaseSettings>(ii => ii.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddDbContext<ModelSaberContext>();

            services.AddSingleton<HttpClient>();
            services.AddSingleton<DiscordService>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "_allowModelSaberWhitelistedOrigins", opt =>
                {
                    opt.WithOrigins(deploymentSettings.CORS)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("_allowModelSaberWhitelistedOrigins");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("ModelSaber OK!");
                });
                endpoints.MapGet("/api", async context =>
                {
                    await context.Response.WriteAsync("ModelSaber OK!");
                });
                endpoints.MapControllers();
                endpoints.MapFallback(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Not Found");
                });
            });
        }
    }
}
