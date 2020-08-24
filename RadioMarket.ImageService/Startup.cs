using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HealthCheck;
using Microsoft.VisualStudio.Services.Location;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using HealthChecks.UI.Client;

namespace RadioMarket.ImageService
{
    public class Startup
    {
        readonly string AllowSpecificOrigins = "_allowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddScoped<Models.Interfaces.IItemService, Services.ItemService>();
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 10;
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigins,
                    builder =>
                    builder.WithOrigins("localhost").AllowAnyMethod().AllowAnyHeader());
            });
            services.AddHealthChecksUI(option =>
            {
                option.AddHealthCheckEndpoint("main", "https://localhost:44359/health");
            }).AddInMemoryStorage();

            services.AddHealthChecks()
                .AddCheck<HealthCheck.HealthChecks.MemoryHealthCheck>("memory", tags: new[] { "memory" })
                .AddCheck<HealthCheck.HealthChecks.DiskHealthCheck>("disk", tags: new[] { "disk" });

            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            services.Configure<FormOptions>(options =>
            {
                options.MemoryBufferThreshold = Int32.MaxValue;
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseHttpsRedirection();

            //app.UseHealthChecks("/health");

            app.UseCors(AllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
                endpoints.MapHealthChecks("health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
