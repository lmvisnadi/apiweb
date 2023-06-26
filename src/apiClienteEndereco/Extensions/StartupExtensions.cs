using Data.Repositories;
using dominio.Repositories;
using dominio.Servico;
using FluentValidation;
using Infraestructure.Context;
using KeL.Infrastructure.Security.Context;
using KeL.Infrastructure.WebApi.Middlewares;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace apiClienteEndereco.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddFailFastBehavior(this IServiceCollection services)
        {
            var applicationAssemblyName = Assembly.GetEntryAssembly()!.GetName().Name;

            var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName!);

            AssemblyScanner
                .FindValidatorsInAssembly(assembly)
                .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

            return services;
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IMunicipioRepository, MunicipioRepository>();
            services.AddScoped<IUnidadeFederativaRepository, UnidadeFederativaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUsuarioDomainService, UsuarioDomainService>();
            return services;
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        Array.Empty<string>()
                    }
                });

                c.CustomSchemaIds(x => x.FullName);
            });

            return services;
        }
        public static IApplicationBuilder ConfigureCORS(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            applicationBuilder.UseCors(
                builder => builder
                    .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());

            return applicationBuilder;
        }
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, string moduleName)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{moduleName} API Docs");
                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }

        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
        public static IServiceCollection AddRequestContext(this IServiceCollection services)
        {
            services.AddTransient(provider =>
            {
                var service = provider.GetService<IHttpContextAccessor>();
                var extractor = new HttpContextAcessorParamExtractor(service!);
                var timeZone = extractor.GetTimeZone();

                return new RequestContext(
                    extractor.GetToken(),
                    extractor.GetUserGuid(),
                    extractor.GetUserName(),
                    Guid.NewGuid(),
                    timeZone
                );
            });
            return services;
        }

    }
}
