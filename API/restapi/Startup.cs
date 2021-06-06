using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using restapi.Interfaces;
using restapi.Models;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace restapi
{
    public class Startup
    {
        bool rebuild;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            rebuild = Configuration.GetValue<bool>("Rebuild");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // register our services
            services.AddHttpContextAccessor();
            services.AddSingleton((ILogger)Log.Logger);

            services.AddDbContext<MetaDataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<MetaDataContext, MetaDataContext>();
            services.AddScoped<IMetaDataRepository, MetaDataRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();

            services.AddControllers();
            services
                .AddMvc()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.Converters = new List<JsonConverter>()
                    {
                        new StringEnumConverter(new CamelCaseNamingStrategy(), true)
                    };
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    o.SerializerSettings.Formatting = Formatting.Indented;
                    o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                    o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    o.SerializerSettings.DateFormatString = "u";
                    o.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
                    o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Global Addresses Team Project",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "Ben Gruher",
                        Email = "gruherb@seattleu.edu"
                    },
                    Description = "Global Addresses Team Project"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMetaDataRepository metaRepo)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // order is important here
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CPSC 5200 REST Example");
            });

            app.UseSerilogRequestLogging();

            // app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // these functions create the metadata and data tables based on the configuration file
            if(rebuild)
            {
                metaRepo.ReadConfig();             // reads in addressConfig.json and adds address formats to metadata
                metaRepo.GenCountryTables();       // sql ddl to create tables for each country, depends on rows in the metadata
            }
        }
    }
}
