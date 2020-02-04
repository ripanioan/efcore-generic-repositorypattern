using EFCore.RepositoryPattern.Sample.Configuration;
using EFCore.RepositoryPattern.Sample.Data;
using EFCore.RepositoryPattern.Sample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace EFCore.RepositoryPattern.Sample
{
    public sealed class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SampleDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("Local")),
                ServiceLifetime.Scoped);

            services.AddScoped<ICarService, CarService>();

            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddApiVersioning();
            services.AddSingleton<IConfigureOptions<ApiVersioningOptions>, ConfigureApiVersioningOptions>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(builder => builder.MapControllers());
        }
    }
}
