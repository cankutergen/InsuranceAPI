using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Insurance.Api.Configurations.AutoMapper;
using Insurance.Business.Abstract;
using Insurance.Business.Builder;
using Insurance.Business.Concrete;
using Insurance.Core.Logging;
using Insurance.DataAccess.Abstract;
using Insurance.DataAccess.Concrete.RestSharp;
using Insurance.DataAccess.Concrete.EntityFramework;
using Insurance.Entities.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RestSharp;
using Insurance.Business.ValidationRules.FluentValidation;

namespace Insurance.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            using (var client = new InsuranceContext())
            {
                var connectionString = this.Configuration.GetConnectionString("InsuranceConnection");
                InsuranceContext.SetConnectionString(connectionString);
                client.Database.EnsureCreated();
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Insurance", new OpenApiInfo { Title = "Insurance", Version = "v1" });
                c.SwaggerDoc("Surcharge", new OpenApiInfo { Title = "Surcharge", Version = "v2" });
            });

            services.AddScoped<IProductApi>(provider => new ProductApi(new RestClient(Configuration.GetValue<string>("ApiBase:ProductApi")), new LogBuilder()));
            services.AddScoped<IProductTypeApi>(provider => new ProductTypeApi(new RestClient(Configuration.GetValue<string>("ApiBase:ProductTypeApi")), new LogBuilder()));
            services.AddScoped<ISurchargeRateDal, EfSurchargeRateDal>();

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductTypeService, ProductTypeManager>();
            services.AddScoped<IInsuranceService, InsuranceManager>();
            services.AddScoped<IOrderInsuranceService, OrderInsuranceManager>();
            services.AddScoped<ISurchargeRateService, SurchargeRateManager>();

            services.AddScoped<ILogBuilder, LogBuilder>();
            services.AddScoped<IValidator<SurchargeRate>, SurchargeRateValidator>();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Insurance/swagger.json", "Insurance");
                c.SwaggerEndpoint("/swagger/Surcharge/swagger.json", "Surcharge");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
