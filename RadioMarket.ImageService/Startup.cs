using AutoMapper;
using DataLayer.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Models.Interfaces;
using Services;
using System;

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
            services.AddControllers();
            //.AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //});

            services.AddAutoMapper(typeof(Startup));

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 10;
                options.ValueLengthLimit = 5000000;
                options.MultipartBodyLengthLimit = 50000000;
                options.MemoryBufferThreshold = 50000000;
            });
            
            services.AddDbContext<ImageContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ImageConnection"));
            });
           
            services.TryAddScoped<IImageRepository, ImageRepository>();
           
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowSpecificOrigins,
                    builder =>
                    builder.WithOrigins("localhost").AllowAnyMethod().AllowAnyHeader());
            });
           
            services.AddHealthChecks()
                .AddCheck("Default", () =>
                HealthCheckResult.Healthy("Healthy"), tags: new[] { "default" });
            
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

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(AllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
