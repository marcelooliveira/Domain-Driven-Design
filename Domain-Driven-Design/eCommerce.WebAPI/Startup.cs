using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using eCommerce.ApplicationLayer;
using eCommerce.ApplicationLayer.Products;
using eCommerce.DomainModelLayer.Countries;
using eCommerce.DomainModelLayer.Customers;
using eCommerce.DomainModelLayer.Email;
using eCommerce.DomainModelLayer.Newsletter;
using eCommerce.DomainModelLayer.Products;
using eCommerce.DomainModelLayer.Tax;
using eCommerce.Helpers.Repository;
using eCommerce.InfrastructureLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace eCommerce.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private MapperConfiguration mapperConfiguration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Map>();
            });
            services.AddSingleton<IMapper>(sp => mapperConfiguration.CreateMapper());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddTransient<IProductService, ProductService>();
            services.AddSingleton<IEmailDispatcher, SmtpEmailDispatcher>();
            services.AddSingleton<IEmailDispatcher, SmtpEmailDispatcher>();
            services.AddSingleton<INewsletterSubscriber, WSNewsletterSubscriber>();
            services.AddSingleton<IEmailGenerator, StubEmailGenerator>();
            services.AddSingleton(typeof(MemoryRepository<>), typeof(MemoryRepository<>));
            services.AddSingleton(typeof(IRepository<>), typeof(MemoryRepository<>));
            services.AddSingleton<IUnitOfWork, MemoryUnitOfWork>();
            services.AddSingleton<IRepository<ProductCode>, StubDataProductCodeRepository>();
            services.AddSingleton<IRepository<Country>, StubDataCountryRepository>();
            services.AddSingleton<IRepository<CountryTax>, StubDataCountryTaxRepository>();
            services.AddSingleton<IRepository<Product>, StubDataProductRepository>();
            services.AddSingleton(typeof(IRepository<Product>), typeof(MemoryRepository<Product>));
            services.AddSingleton<ICustomerRepository, StubDataCustomerRepository>();
            services.AddSingleton<IDomainEventRepository, MemDomainEventRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
