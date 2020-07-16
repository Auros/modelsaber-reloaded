using System.Text;
using System.Net.Http;
using ModelSaber.Services;
using ModelSaber.Database;
using Microsoft.AspNetCore.Http;
using ModelSaber.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ModelSaber
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
            => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = _configuration.GetSection(nameof(JWTSettings)).Get<JWTSettings>();
            var deploymentSettings = _configuration.GetSection(nameof(DeploymentSettings)).Get<DeploymentSettings>();
            
            var jwtConfig = _configuration.GetSection(nameof(JWTSettings));
            var discordConfig = _configuration.GetSection(nameof(DiscordSettings));
            var databaseConfig = _configuration.GetSection(nameof(DatabaseSettings));

            services.Configure<JWTSettings>(jwtConfig);
            services.Configure<DiscordSettings>(discordConfig);
            services.Configure<DatabaseSettings>(databaseConfig);

            services.AddSingleton<IJWTSettings>(ii => ii.GetRequiredService<IOptions<JWTSettings>>().Value);
            services.AddSingleton<IDiscordSettings>(ii => ii.GetRequiredService<IOptions<DiscordSettings>>().Value);
            services.AddSingleton<IDatabaseSettings>(ii => ii.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddDbContext<ModelSaberContext>();

            services.AddSingleton<HttpClient>();
            services.AddSingleton<JWTService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<DiscordService>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "_allowModelSaberOrigins", opt =>
                {
                    opt.WithOrigins(deploymentSettings.CORS)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
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
            app.UseCors("_allowModelSaberOrigins");
            app.UseAuthentication();

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
