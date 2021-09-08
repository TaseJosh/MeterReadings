using MeterReadings.Core.Application.CsvHandlers;
using MeterReadings.Core.Application.Processor;
using MeterReadings.Core.Data;
using MeterReadings.Core.Data.Interfaces;
using MeterReadings.Core.Data.Repositories;
using MeterReadings.Core.Domain.Models;
using MeterReadings.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using MeterReadings.Core.Domain.Interface;

namespace MeterReadings.Core.Web
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbContext<MeterReadingDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });
            services.AddControllers();
            services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IMeterReadingsCsvProcessor, MeterReadingsCsvProcessor>();
            services.AddScoped<IMeterReadingsToSave, MeterReadingsToSave>();
            services.AddScoped<IMeterReadingsCsv, MeterReadingsCsv>();
            services.AddScoped<ICsvCustomErrorHandler, CsvCustomErrorHandler<int>>();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            options.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
