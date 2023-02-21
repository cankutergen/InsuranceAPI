using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Insurance.Api.Configurations.AutoMapper;
using Insurance.Business.Abstract;
using Insurance.Business.Builder;
using Insurance.Business.Concrete;
using Insurance.Core.Logging;
using Insurance.DataAccess.Abstract;
using Insurance.DataAccess.Concrete;
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

namespace Insurance.Api
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Insurance.API", Version = "v1" });
            });

            services.AddScoped<IProductApi>(provider => new ProductApi(new RestClient(Configuration.GetValue<string>("ApiBase:ProductApi")), new LogBuilder()));
            services.AddScoped<IProductTypeApi>(provider => new ProductTypeApi(new RestClient(Configuration.GetValue<string>("ApiBase:ProductTypeApi")), new LogBuilder()));

            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductTypeService, ProductTypeManager>();
            services.AddScoped<IInsuranceService, InsuranceManager>();
            services.AddScoped<IOrderInsuranceService, OrderInsuranceManager>();

            services.AddScoped<ILogBuilder, LogBuilder>();

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Insurance.API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
