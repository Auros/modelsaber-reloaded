using GraphQL.Server;
using System.Net.Http;
using ModelSaber.API.Models;
using ModelSaber.API.Security;
using ModelSaber.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ModelSaber.API.Models.GraphQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddHttpContextAccessor();

            services.AddSingleton<UserType>();
            services.AddSingleton<VisibilityType>();
            services.AddSingleton<CollectionType>();
            services.AddSingleton<DiscordUserType>();
            services.AddSingleton<ModelSaberQuery>();
            services.AddSingleton<ModelSaberSchema>();
            services.AddSingleton<ApprovalStatusType>();
            services.AddGraphQL().AddSystemTextJson().AddGraphTypes(typeof(ModelSaberSchema));

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
            app.UseGraphQL<ModelSaberSchema>("/graphql");
            app.UseGraphQLAltair();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}