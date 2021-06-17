using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using ODataFilters.Model.Data;
using System.Collections.Generic;
using System.Linq;

namespace ODataFilters.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NorthwindDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddControllers(options =>
                {
                    options.Conventions.Add(new GroupingByNamespaceConvention());
                })
                .AddNewtonsoftJson();

            //services.AddApiVersioning(config =>
            //{
            //    config.DefaultApiVersion = new ApiVersion(1, 0);
            //    config.AssumeDefaultVersionWhenUnspecified = true;
            //    config.ReportApiVersions = true;
            //    config.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("x-api-version"), new QueryStringApiVersionReader("api-version"));
            //});

            services.AddOData();

            services.AddSwaggerGen(c =>
            {
                var titleBase = "ODataFilters";

                c.SwaggerDoc("v1", new OpenApiInfo { Title = titleBase + " v1", Version = "v1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = titleBase + " v2", Version = "v2" });
            });

            SetOutputFormatters(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODataFilters v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "ODataFilters v2");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.EnableDependencyInjection();
                endpoints.Select().Filter().Expand().OrderBy().Count().MaxTop(50);
            });
        }

        private static void SetOutputFormatters(IServiceCollection services)
        {
            services.AddMvc(op =>
            {
                foreach (var formatter in op.OutputFormatters
                    .OfType<ODataOutputFormatter>()
                    .Where(it => !it.SupportedMediaTypes.Any()))
                {
                    formatter.SupportedMediaTypes.Add(
                        new MediaTypeHeaderValue("application/prs.mock-odata"));
                }

                foreach (var formatter in op.InputFormatters
                    .OfType<ODataInputFormatter>()
                    .Where(it => !it.SupportedMediaTypes.Any()))
                {
                    formatter.SupportedMediaTypes.Add(
                        new MediaTypeHeaderValue("application/prs.mock-odata"));
                }
            });
        }
    }
    public class GroupingByNamespaceConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNamespace = controller.ControllerType.Namespace;
            var apiVersion = controllerNamespace.Split(".").Last().ToLower();
            if (!apiVersion.StartsWith("v")) { apiVersion = "v1"; }
            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}
