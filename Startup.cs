using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookSite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace BookSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //adds database
            services.AddDbContext<BookShelfDBContext>(options =>
             {
                 options.UseSqlite(Configuration["ConnectionStrings:BookConnection"]);

             });

            services.AddScoped<IBookRepository, EFBookRepository>();

            services.AddRazorPages();

            //services for session
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

            //add session
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //category pages
                endpoints.MapControllerRoute(
                    "catpage",
                    "{category}/{pageNum:int}",
                    new { Controller = "Home", action = "Index" });

                //page number pages
                endpoints.MapControllerRoute(
                    "pageNum",
                    "Bookshelf/{page:int}",
                    new { Controller = "Home", action = "Index" });

                //only category
                endpoints.MapControllerRoute(
                    "category",
                    "{category}",
                    new { Controller = "Home", action = "Index" , pageNum = 1});

                endpoints.MapControllerRoute(
                    "pagination",
                    "Bookshelf/{pageNum}",
                    new { Controller = "Home", action="Index" });

                endpoints.MapDefaultControllerRoute();

                endpoints.MapRazorPages();
            });

            //add in seeddata and connect to app
            SeedData.EnsurePopulated(app);
        }
    }
}
