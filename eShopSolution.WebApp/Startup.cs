using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopSolution.ApiIntergration;
using eShopSolution.Data.EF;
using eShopSolution.WebApp.LocalizationResources;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eShopSolution.WebApp
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
            services.AddHttpClient();

            var cultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("vi")
            };

            services.AddControllersWithViews()
                .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(options => {
                    options.UseAllCultureProviders = false;
                    options.ResourcesPath = "LocalizationResources";
                    options.RequestLocalizationOptions = o =>
                    {
                        o.SupportedCultures = cultures;
                        o.SupportedUICultures = cultures;
                        o.DefaultRequestCulture = new RequestCulture("vi");
                    };
                });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ISlideApiClient, SlideApiClient>();
            services.AddTransient<IProductApiClient, ProductApiClient>();
            services.AddTransient<ICategoryApiClient, CategoryApiClient>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Product category en",
                    pattern: "{culture}/categories/{id}",
                    new
                    {
                        controller = "Product",
                        action = "Category"
                    });

                endpoints.MapControllerRoute(
                   name: "Product category vn",
                   pattern: "{culture}/danh-muc/{id}",
                    new
                    {
                        controller = "Product",
                        action = "Category"
                    });

                endpoints.MapControllerRoute(
                   name: "Product detail en",
                   pattern: "{culture}/products/{id}",
                    new
                    {
                        controller = "Product",
                        action = "Detail"
                    });

                endpoints.MapControllerRoute(
                   name: "Product category vn",
                   pattern: "{culture}/san-pham/{id}",
                    new
                    {
                        controller = "Product",
                        action = "Detail"
                    });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=vi}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
