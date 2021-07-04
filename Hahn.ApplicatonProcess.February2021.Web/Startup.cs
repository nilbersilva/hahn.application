using Hahn.ApplicatonProcess.February2021.Data.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.February2021.Data.Interfaces;
using Hahn.ApplicatonProcess.February2021.Data.Repositories;
using Hahn.ApplicatonProcess.February2021.Domain.Models.Mapping;
using System.Globalization;
using Serilog;
using Hahn.ApplicatonProcess.February2021.Data.Services;
using System.IO;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Swashbuckle.AspNetCore.Filters;

namespace Hahn.ApplicatonProcess.February2021.Web
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
            services.AddMemoryCache();
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddDbContext<DataContext>(options =>
            {
                options.UseInMemoryDatabase("database");
            });

            services.AddHttpClient<ICountryRepository, CountryRepository>();
            services.AddHttpClient<ITopLevelDomainRepository, TopLevelDomainRepository>();

            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<ISharedValuesService, SharedValuesService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hahn.ApplicatonProcess.February2021.Web", Version = "v1" });

                c.ExampleFilters();



                var webFilePath = Path.Combine(AppContext.BaseDirectory, "Hahn.ApplicatonProcess.February2021.Web.xml");
                if (File.Exists(webFilePath))
                {
                    c.IncludeXmlComments(webFilePath);
                }

                var domainFilePath = Path.Combine(AppContext.BaseDirectory, "Hahn.ApplicatonProcess.February2021.Domain.xml");
                if (File.Exists(domainFilePath))
                {
                    c.IncludeXmlComments(domainFilePath);
                }
            });

            services.AddSwaggerExamplesFromAssemblyOf<SwaggerExamples.AssetDtoExampleNew>();

            services.AddHttpContextAccessor();

            services.AddControllers();

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
            });


            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Initialize(serviceProvider, env);

            app.UseSerilogRequestLogging();

            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
                app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseCors();
            }


            app.UseSwagger(c =>
            {
               
            });
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hahn.ApplicatonProcess.February2021.Web v1"));
            
            app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }

        public void Initialize(IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            var logger = serviceProvider.GetService<ILogger<Startup>>();
            logger.LogInformation("Initializing...");

            var listOfValuesService = serviceProvider.GetService<ISharedValuesService>();
            if (listOfValuesService is not null)
            {
                listOfValuesService.PutAllCountriesInCache();
                listOfValuesService.PutAllAllTopLevelDomainsInCache();
            }

            if (env.IsDevelopment())
            {
                var dataContext = serviceProvider.GetService<DataContext>();
                Classes.DataSeeder.SeedCountries(dataContext);
            }
        }
    }
}
