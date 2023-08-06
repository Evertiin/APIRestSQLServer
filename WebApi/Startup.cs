using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WebApi
{
    public class Startup
    {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
            Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
            services.AddControllers();
            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            


        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }

    public interface IWebHostingEnvironment
    {
    }

    public interface IStartup
    {
        IConfiguration Configuration { get; }
        void Configure(WebApplication app, IWebHostEnvironment environment);
        void ConfigureServices(IServiceCollection services);    
    }

    public static class Startupextensions
    {
        public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder WebAppBuilder) where TStartup : IStartup
        {
            var startup = Activator.CreateInstance(typeof(TStartup), WebAppBuilder.Configuration) as IStartup;

            if (startup != null) throw new ArgumentException("Classe Startup.cs inválida");

            startup.ConfigureServices(WebAppBuilder.Services);

            var app = WebAppBuilder.Build();


            startup.Configure(app, app.Environment);
            app.Run();

            return WebAppBuilder;
        }
    }
}
