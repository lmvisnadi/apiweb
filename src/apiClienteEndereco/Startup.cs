using apiClienteEndereco.Extensions;
using Data.Context;
using FluentValidation;
using HealthChecks.UI.Client;
using Infraestructure.Context;
using KeL.Infrastructure.Security.Context;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Text;

namespace apiClienteEndereco
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _currentEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment currentEnvironment)
        {
            _configuration = configuration;
            _currentEnvironment = currentEnvironment;
        }

        private ILoggerFactory CreateLoggerFactory()
        {
            return LoggerFactory.Create(builder => builder
                .AddFilter((category, level)
                    => category == DbLoggerCategory.Database.Command.Name
                       && level == LogLevel.Information)
                .AddConsole());
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    };
                });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddRequestContext();

            services.AddFailFastBehavior();

            services.AddSwagger();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });


            // DbContext
            services.AddScoped<SharedContext>(provider =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<BaseContext>();

                if (!_currentEnvironment.IsProduction())
                {
                    optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
                }

                var options = optionsBuilder.UseNpgsql(_configuration.GetConnectionString("AdminDatabase"))
                    .Options;
                var userContext = provider.GetService<RequestContext>();

                return new SharedContext(options, userContext?.UserId ?? Guid.Empty);
            });

            services.AddRepositories();

            services.AddHealthChecks()
                        .AddNpgSql(npgsqlConnectionString: _configuration.GetConnectionString("AdminDatabase"), name: "PostGreSQL");

            var assembly = AppDomain.CurrentDomain.Load("Application");

            services.AddMediatR(assembly);

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            app.UseRouting();

            app.ConfigureCORS(_configuration);

            app.UseAuthorization();

            app.UseMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var serviceName = Assembly.GetEntryAssembly()!.GetName().Name;

            UpdateDatabaseSqlServer();

            app.UseSwagger(moduleName: "Admin");

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = p => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }

        private void UpdateDatabaseSqlServer()
        {
            if (_currentEnvironment.IsDevelopment())
            {
                var optionsBuilder = new DbContextOptionsBuilder<BaseContext>();
                var options = optionsBuilder.UseNpgsql(_configuration.GetConnectionString("AdminDatabase")).Options;

                using (var RecordsContext = new SharedContext(options, Guid.Empty))
                {
                    RecordsContext.Database.Migrate();
                }
            }

        }
    }



}
