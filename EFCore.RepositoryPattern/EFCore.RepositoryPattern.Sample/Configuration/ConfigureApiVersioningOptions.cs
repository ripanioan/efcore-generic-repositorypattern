using EFCore.RepositoryPattern.Sample.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EFCore.RepositoryPattern.Sample.Configuration
{
    public class ConfigureApiVersioningOptions : IConfigureOptions<ApiVersioningOptions>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ConfigureApiVersioningOptions(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public void Configure(ApiVersioningOptions options)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var provider = scope.ServiceProvider;
            var configuration = provider.GetService<IConfiguration>();

            string apiVersion = configuration.GetSection("ApiVersion").Value;

            options.DefaultApiVersion = ApiVersion.Parse(apiVersion);

            options
                .Conventions
                .Controller<CarController>()
                .HasApiVersion(ApiVersion.Parse(apiVersion));
        }
    }
}
