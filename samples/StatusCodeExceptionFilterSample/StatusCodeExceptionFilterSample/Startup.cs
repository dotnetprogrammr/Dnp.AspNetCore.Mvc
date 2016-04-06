using Dnp.AspNetCore.Mvc;
using Dnp.AspNetCore.Mvc.Filters;

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace StatusCodeExceptionFilterSample
{
    public class Startup
    {
        /// <summary>
        /// Configures the services (dependencies) required by the application.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var transformations = new TransformationCollectionBuilder()
                .Return(404).For<NotFoundException>()
                .Return(400).For<BadRequestException>()
                .Transformations;

            services.AddMvc(options =>
            {
                options.Filters.Add(new StatusCodeExceptionFilterAttribute(transformations));
            });
        }

        /// <summary>
        /// Configures the application pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();

            app.UseMvcWithDefaultRoute();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
